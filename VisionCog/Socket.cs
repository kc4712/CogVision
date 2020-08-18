using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace VisionCog
{
    public class AsyncObject
    {
        public byte[] Buffer;
        public Socket WorkingSocket;
        public readonly int BufferSize;
        public AsyncObject(int bufferSize)
        {
            BufferSize = bufferSize;
            Buffer = new byte[BufferSize];
        }

        public void ClearBuffer()
        {
            Array.Clear(Buffer, 0, BufferSize);
        }
    }
    class SocketServer
    {
        private string CLASSNAME;
        private Socket mainSock;
        private Socket mClientSock;

        private const int TIMERTICK = 1000;
        private const byte STX = 0x02;      // 데이타 Frame Start
        private const byte ETX = 0x03;      // 데이타 Frame End

        private string ipaddr { get; set; }
        private int port { get; set; }
        private int buffsize { get; set; }
        
        public SocketServer(string ipaddr, int port, int buffsize)
        {
            CLASSNAME = this.GetType().Name;
            this.ipaddr = ipaddr;
            this.port = port;
            this.buffsize = buffsize;
        }

        public bool ServerOpen()
        {
            try
            {
                mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(ipaddr), port);
                mainSock.Bind(serverEP);
                mainSock.Listen(10);
                mainSock.BeginAccept(AcceptCallback, null);
                return true;
            }
            catch (SocketException ex)
            {
                //Console.WriteLine(CLASSNAME + "Server Open ERR" + ex);
                Log.LogStr("ServerOpen", "SocketException==>"+ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(CLASSNAME + "Server Open ERR Unknown reason" + ex);
                Log.LogStr("ServerOpen", ex.ToString());
                return false;
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            mClientSock = mainSock.EndAccept(ar);

            AsyncObject obj = new AsyncObject(this.buffsize);
            obj.WorkingSocket = mClientSock;

            //Console.WriteLine(CLASSNAME + "Client Conn : {0}", mClientSock.RemoteEndPoint);
            Log.LogStr("AcceptCallback",CLASSNAME + "Client Conn : "+ mClientSock.RemoteEndPoint.ToString());
            mClientSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
        }

        private void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            int received = obj.WorkingSocket.EndReceive(ar);
            string smsg = "";

            if (obj.Buffer.Length != 0)
            {
                for (int i = 0; i < obj.Buffer.Length; i++)
                {
                    smsg += String.Format("{0:x2}", obj.Buffer[i]);
                }
            }
            obj.ClearBuffer();

            obj.WorkingSocket.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
        }


        internal bool OnSendData(byte[] data)
        {
            if (!mainSock.IsBound || data.Length <= 0)
            {
                return false;
            }
            try
            {
                mClientSock.Send(data, 0, data.Length, SocketFlags.None);
                //mStream.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(CLASSNAME + "Send error" + ex.ToString());
                Log.LogStr("OnSendData", CLASSNAME + "Send error" + ex.ToString());
                return false;
            }
        }
    }

    class SocketClient
    {
        private string CLASSNAME;
        private Socket mainSock;
        private IPEndPoint ipep;
        private AsyncObject ao;

        private System.Timers.Timer keepAliveTimer;

        private const int TIMERTICK = 1000;
        private const byte STX = 0x02;      // 데이타 Frame Start
        private const byte ETX = 0x03;      // 데이타 Frame End

        private string ipaddr { get; set; }
        private int port { get; set; }
        private int buffsize { get; set; }
        private bool CONTINOUSCONN { get; set; }

        public bool CONNECTED
        {
            get
            {
                if (mainSock != null)
                {
                    return mainSock.Connected;
                }else
                {
                    return false;
                }
            }
        }

        public delegate void DataReceiveByteEventHandler(byte[] ba);
        public event DataReceiveByteEventHandler DataReceiveEventByte;

        //public delegate void DataReceiveStringEventHandler(string str);
        //public event DataReceiveStringEventHandler DataReceiveEventStr;

        public SocketClient(string ipaddr, int port, int buffsize, bool continousconn)
        {
            this.CLASSNAME = "[" + this.GetType().Name + "]";
            this.ipaddr = ipaddr;
            this.port = port;
            this.buffsize = buffsize;
            this.CONTINOUSCONN = continousconn;
            this.keepAliveTimer = new System.Timers.Timer();
        }

        private void DataReceived(IAsyncResult ar)
        {
            try
            {
                if (!mainSock.Connected || !mainSock.IsBound)
                {
                    return;
                }
                AsyncObject obj = (AsyncObject)ar.AsyncState;
                int received = 0;
           
            
                received = obj.WorkingSocket.EndReceive(ar);
                //Console.WriteLine(CLASSNAME + "Receive Byte Size:" + received);
                if (received > 0)
                {
                    byte[] buff = new byte[72];
                    buff = obj.Buffer;
                    Array.Resize<byte>(ref buff, 72);
                    string smsg = "";
                    for (int i = 0; i < obj.Buffer.Length; i++)
                    {
                        smsg += String.Format("{0:X2}", obj.Buffer[i]);
                    }
                    DataReceiveEventByte?.Invoke(buff);
                    //DataReceiveEventStr?.Invoke(smsg);

                }
                else if(received == 0)
                {

                    //Console.WriteLine(CLASSNAME + "Receive Byte Size:" + received + "Emergency Close()");
                    obj.ClearBuffer();
                    obj.WorkingSocket.Disconnect(true);
                    return;
                }
                
            
                 obj.ClearBuffer();
                if(!obj.WorkingSocket.IsBound || !obj.WorkingSocket.Connected)
                {
                    return;
                }
                 obj.WorkingSocket.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
            }
            catch (SocketException e)
            {
                //Console.WriteLine(CLASSNAME + "Receive Err" + e);
                Log.LogStr("DataReceived", CLASSNAME + "Receive Err" + e);
                return;
            }
        }

        internal bool SendData(byte[] data)
        {
            if (!mainSock.IsBound || !mainSock.Connected)
            {
                //Console.WriteLine("mainSock check");
                Log.LogStr("SendData", "mainSock check");
                return false;
            }
            try
            {
                mainSock.Send(data, 0, data.Length, SocketFlags.None);
                //mStream.Write(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogStr("SendData", CLASSNAME + "Send Err" + ex.ToString());
                //Console.WriteLine(CLASSNAME + "Send Err" + ex.ToString());
                return false;
            }
        }

        internal bool Connect()
        {
            try
            {
                bool CONN = false;
                if (mainSock == null || !mainSock.Connected || !mainSock.IsBound)
                {
                    mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    ao = new AsyncObject(this.buffsize);
                    ipep = new IPEndPoint(IPAddress.Parse(this.ipaddr), this.port);
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(this.ipaddr);

                    if (reply.Status == IPStatus.Success) //핑이 제대로 들어가고 있을 경우
                    {
                        mainSock.Connect(ipep);
                        ao.WorkingSocket = mainSock;
                        mainSock.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, DataReceived, ao);
                    }

                    
                    //OnSendData();
                    if (this.CONTINOUSCONN)
                    {
                        if (keepAliveTimer.Enabled)
                        {
                            keepAliveTimer.Stop();
                        }
                        keepAliveTimer.Elapsed -= keepAliveTimer_Tick;
                        keepAliveTimer.Interval = TIMERTICK;
                        keepAliveTimer.Elapsed += keepAliveTimer_Tick;
                        if (!keepAliveTimer.Enabled)
                        {
                            keepAliveTimer.Start();
                        }
                    }
                    CONN = true;
                }
                return CONN;
            }
            catch (SocketException ex)
            {
                
                //Log.LogStr("Connect", "ConnectError:" + ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                
                //Log.LogStr("Connect", "ConnectError:" + ex.ToString());
                return false;
            }
        }

        internal void Close()
        {
            try { 
                if (this.CONTINOUSCONN)
                {
                    keepAliveTimer.Elapsed -= keepAliveTimer_Tick;
                    if (keepAliveTimer.Enabled)
                    {
                        keepAliveTimer.Stop();
                    }
                }
                if (mainSock.Connected)
                {
                    mainSock.Close();
                }
            }catch(Exception ex)
            {
                Log.LogStr("Close", ex.ToString());
            }
        }

        private void keepAliveTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            try { 
                if (!mainSock.Connected | !mainSock.IsBound || mainSock == null)
                {
                    //Close();
                    Thread.Sleep(TIMERTICK);
                    Connect();
                }
            }catch(Exception ex)
            {
                Log.LogStr("keepAliveTimer_Tick", ex.ToString());
            }
        }

    }

}
