namespace VisionCog
{
    partial class FrmMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startGrapOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolboxrunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teachingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_Barcode = new System.Windows.Forms.TextBox();
            this.tb_Part = new System.Windows.Forms.TextBox();
            this.tb_Cartype = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.tb_Start = new System.Windows.Forms.TextBox();
            this.tb_Conn = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tb_DataRFY_mm = new System.Windows.Forms.TextBox();
            this.tb_DataRFScale_change = new System.Windows.Forms.TextBox();
            this.tb_DataRFX_mm = new System.Windows.Forms.TextBox();
            this.tb_DataLFY_mm = new System.Windows.Forms.TextBox();
            this.tb_DataLFScale_change = new System.Windows.Forms.TextBox();
            this.FRScale_Change_label = new System.Windows.Forms.Label();
            this.tb_DataLFX_mm = new System.Windows.Forms.TextBox();
            this.FLScale_Change_label = new System.Windows.Forms.Label();
            this.RF_MM_label = new System.Windows.Forms.Label();
            this.LF_MM_label = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.LF_origin_label = new System.Windows.Forms.Label();
            this.tb_DataLFX_origin = new System.Windows.Forms.TextBox();
            this.RF_origin_label = new System.Windows.Forms.Label();
            this.tb_DataLFY_origin = new System.Windows.Forms.TextBox();
            this.tb_DataRFX_origin = new System.Windows.Forms.TextBox();
            this.tb_DataRFY_origin = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tb_DataLFH = new System.Windows.Forms.TextBox();
            this.tb_DataRFH = new System.Windows.Forms.TextBox();
            this.tb_DataRFL = new System.Windows.Forms.TextBox();
            this.tb_DataLFL = new System.Windows.Forms.TextBox();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tb_StatusRF = new System.Windows.Forms.TextBox();
            this.tb_StatusLF = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cogToolBlockEditV21 = new Cognex.VisionPro.ToolBlock.CogToolBlockEditV2();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tmr_Sequence = new System.Windows.Forms.Timer(this.components);
            this.tabStatus = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lvLog = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lvData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabStatus.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.setUpToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(942, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // setUpToolStripMenuItem
            // 
            this.setUpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalSetToolStripMenuItem,
            this.startGrapOneToolStripMenuItem,
            this.toolboxrunToolStripMenuItem,
            this.teachingToolStripMenuItem});
            this.setUpToolStripMenuItem.Name = "setUpToolStripMenuItem";
            this.setUpToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.setUpToolStripMenuItem.Text = "Set up";
            // 
            // globalSetToolStripMenuItem
            // 
            this.globalSetToolStripMenuItem.Name = "globalSetToolStripMenuItem";
            this.globalSetToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.globalSetToolStripMenuItem.Text = "Global Set";
            this.globalSetToolStripMenuItem.Click += new System.EventHandler(this.globalSetToolStripMenuItem_Click);
            // 
            // startGrapOneToolStripMenuItem
            // 
            this.startGrapOneToolStripMenuItem.Name = "startGrapOneToolStripMenuItem";
            this.startGrapOneToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.startGrapOneToolStripMenuItem.Text = "Start Grap_One";
            this.startGrapOneToolStripMenuItem.Click += new System.EventHandler(this.startGrapOneToolStripMenuItem_Click);
            // 
            // toolboxrunToolStripMenuItem
            // 
            this.toolboxrunToolStripMenuItem.Name = "toolboxrunToolStripMenuItem";
            this.toolboxrunToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.toolboxrunToolStripMenuItem.Text = "toolboxrun";
            this.toolboxrunToolStripMenuItem.Visible = false;
            this.toolboxrunToolStripMenuItem.Click += new System.EventHandler(this.toolboxrunToolStripMenuItem_Click);
            // 
            // teachingToolStripMenuItem
            // 
            this.teachingToolStripMenuItem.Name = "teachingToolStripMenuItem";
            this.teachingToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.teachingToolStripMenuItem.Text = "Teaching";
            this.teachingToolStripMenuItem.Visible = false;
            this.teachingToolStripMenuItem.Click += new System.EventHandler(this.teachingToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem1,
            this.clearToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // testToolStripMenuItem1
            // 
            this.testToolStripMenuItem1.Name = "testToolStripMenuItem1";
            this.testToolStripMenuItem1.Size = new System.Drawing.Size(101, 22);
            this.testToolStripMenuItem1.Text = "Test";
            this.testToolStripMenuItem1.Click += new System.EventHandler(this.testToolStripMenuItem1_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Visible = false;
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(906, 117);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.txt_Barcode, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_Part, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_Cartype, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tb_Start, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_Conn, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 15);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(894, 88);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // txt_Barcode
            // 
            this.txt_Barcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Barcode.BackColor = System.Drawing.Color.Beige;
            this.txt_Barcode.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Barcode.Location = new System.Drawing.Point(672, 47);
            this.txt_Barcode.Multiline = true;
            this.txt_Barcode.Name = "txt_Barcode";
            this.txt_Barcode.ReadOnly = true;
            this.txt_Barcode.Size = new System.Drawing.Size(219, 38);
            this.txt_Barcode.TabIndex = 18;
            this.txt_Barcode.Text = "1234567890";
            this.txt_Barcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Part
            // 
            this.tb_Part.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Part.BackColor = System.Drawing.Color.Beige;
            this.tb_Part.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Part.Location = new System.Drawing.Point(226, 47);
            this.tb_Part.Multiline = true;
            this.tb_Part.Name = "tb_Part";
            this.tb_Part.ReadOnly = true;
            this.tb_Part.Size = new System.Drawing.Size(217, 38);
            this.tb_Part.TabIndex = 13;
            this.tb_Part.Text = "0000";
            this.tb_Part.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Cartype
            // 
            this.tb_Cartype.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Cartype.BackColor = System.Drawing.Color.Beige;
            this.tb_Cartype.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Cartype.Location = new System.Drawing.Point(3, 47);
            this.tb_Cartype.Multiline = true;
            this.tb_Cartype.Name = "tb_Cartype";
            this.tb_Cartype.Size = new System.Drawing.Size(217, 38);
            this.tb_Cartype.TabIndex = 12;
            this.tb_Cartype.Text = "0";
            this.tb_Cartype.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BackColor = System.Drawing.Color.Khaki;
            this.textBox2.Font = new System.Drawing.Font("굴림", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox2.Location = new System.Drawing.Point(226, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(217, 38);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Meas. Part";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.Color.Khaki;
            this.textBox1.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(217, 38);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "Car Type";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.BackColor = System.Drawing.Color.Khaki;
            this.textBox4.Font = new System.Drawing.Font("굴림", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox4.Location = new System.Drawing.Point(449, 3);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(217, 38);
            this.textBox4.TabIndex = 14;
            this.textBox4.Text = "Start Sig.";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Start
            // 
            this.tb_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Start.BackColor = System.Drawing.Color.Beige;
            this.tb_Start.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Start.Location = new System.Drawing.Point(449, 47);
            this.tb_Start.Multiline = true;
            this.tb_Start.Name = "tb_Start";
            this.tb_Start.ReadOnly = true;
            this.tb_Start.Size = new System.Drawing.Size(217, 38);
            this.tb_Start.TabIndex = 16;
            this.tb_Start.Text = "0";
            this.tb_Start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Conn
            // 
            this.tb_Conn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_Conn.BackColor = System.Drawing.Color.Lime;
            this.tb_Conn.Font = new System.Drawing.Font("굴림", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Conn.Location = new System.Drawing.Point(672, 3);
            this.tb_Conn.Multiline = true;
            this.tb_Conn.Name = "tb_Conn";
            this.tb_Conn.ReadOnly = true;
            this.tb_Conn.Size = new System.Drawing.Size(219, 38);
            this.tb_Conn.TabIndex = 17;
            this.tb_Conn.Text = "Connected";
            this.tb_Conn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel5);
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Location = new System.Drawing.Point(6, 129);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(906, 533);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.tb_DataRFY_mm, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.tb_DataRFScale_change, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.tb_DataRFX_mm, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.tb_DataLFY_mm, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.tb_DataLFScale_change, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.FRScale_Change_label, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.tb_DataLFX_mm, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.FLScale_Change_label, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.RF_MM_label, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.LF_MM_label, 0, 2);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 344);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 5;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(894, 180);
            this.tableLayoutPanel5.TabIndex = 13;
            // 
            // tb_DataRFY_mm
            // 
            this.tb_DataRFY_mm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFY_mm.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFY_mm.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFY_mm.Location = new System.Drawing.Point(450, 148);
            this.tb_DataRFY_mm.Name = "tb_DataRFY_mm";
            this.tb_DataRFY_mm.ReadOnly = true;
            this.tb_DataRFY_mm.Size = new System.Drawing.Size(441, 30);
            this.tb_DataRFY_mm.TabIndex = 14;
            this.tb_DataRFY_mm.Text = "0";
            this.tb_DataRFY_mm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_DataRFY_mm.TextChanged += new System.EventHandler(this.tb_DataRFY_mm_TextChanged);
            // 
            // tb_DataRFScale_change
            // 
            this.tb_DataRFScale_change.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFScale_change.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFScale_change.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFScale_change.Location = new System.Drawing.Point(450, 40);
            this.tb_DataRFScale_change.Name = "tb_DataRFScale_change";
            this.tb_DataRFScale_change.ReadOnly = true;
            this.tb_DataRFScale_change.Size = new System.Drawing.Size(441, 30);
            this.tb_DataRFScale_change.TabIndex = 39;
            this.tb_DataRFScale_change.Text = "0";
            this.tb_DataRFScale_change.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_DataRFX_mm
            // 
            this.tb_DataRFX_mm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFX_mm.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFX_mm.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFX_mm.Location = new System.Drawing.Point(450, 113);
            this.tb_DataRFX_mm.Name = "tb_DataRFX_mm";
            this.tb_DataRFX_mm.ReadOnly = true;
            this.tb_DataRFX_mm.Size = new System.Drawing.Size(441, 30);
            this.tb_DataRFX_mm.TabIndex = 13;
            this.tb_DataRFX_mm.Text = "0";
            this.tb_DataRFX_mm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_DataRFX_mm.TextChanged += new System.EventHandler(this.tb_DataRFX_mm_TextChanged);
            // 
            // tb_DataLFY_mm
            // 
            this.tb_DataLFY_mm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFY_mm.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFY_mm.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFY_mm.Location = new System.Drawing.Point(3, 148);
            this.tb_DataLFY_mm.Name = "tb_DataLFY_mm";
            this.tb_DataLFY_mm.ReadOnly = true;
            this.tb_DataLFY_mm.Size = new System.Drawing.Size(441, 30);
            this.tb_DataLFY_mm.TabIndex = 20;
            this.tb_DataLFY_mm.Text = "0";
            this.tb_DataLFY_mm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_DataLFY_mm.TextChanged += new System.EventHandler(this.tb_DataLFY_mm_TextChanged);
            // 
            // tb_DataLFScale_change
            // 
            this.tb_DataLFScale_change.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFScale_change.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFScale_change.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFScale_change.Location = new System.Drawing.Point(3, 40);
            this.tb_DataLFScale_change.Name = "tb_DataLFScale_change";
            this.tb_DataLFScale_change.ReadOnly = true;
            this.tb_DataLFScale_change.Size = new System.Drawing.Size(441, 30);
            this.tb_DataLFScale_change.TabIndex = 40;
            this.tb_DataLFScale_change.Text = "0";
            this.tb_DataLFScale_change.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FRScale_Change_label
            // 
            this.FRScale_Change_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FRScale_Change_label.AutoSize = true;
            this.FRScale_Change_label.BackColor = System.Drawing.Color.MistyRose;
            this.FRScale_Change_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FRScale_Change_label.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FRScale_Change_label.Location = new System.Drawing.Point(450, 0);
            this.FRScale_Change_label.Name = "FRScale_Change_label";
            this.FRScale_Change_label.Size = new System.Drawing.Size(441, 37);
            this.FRScale_Change_label.TabIndex = 46;
            this.FRScale_Change_label.Text = "Scale";
            this.FRScale_Change_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_DataLFX_mm
            // 
            this.tb_DataLFX_mm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFX_mm.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFX_mm.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFX_mm.Location = new System.Drawing.Point(3, 113);
            this.tb_DataLFX_mm.Name = "tb_DataLFX_mm";
            this.tb_DataLFX_mm.ReadOnly = true;
            this.tb_DataLFX_mm.Size = new System.Drawing.Size(441, 30);
            this.tb_DataLFX_mm.TabIndex = 19;
            this.tb_DataLFX_mm.Text = "0";
            this.tb_DataLFX_mm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_DataLFX_mm.TextChanged += new System.EventHandler(this.tb_DataLFX_mm_TextChanged);
            // 
            // FLScale_Change_label
            // 
            this.FLScale_Change_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FLScale_Change_label.AutoSize = true;
            this.FLScale_Change_label.BackColor = System.Drawing.Color.MistyRose;
            this.FLScale_Change_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FLScale_Change_label.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FLScale_Change_label.Location = new System.Drawing.Point(3, 0);
            this.FLScale_Change_label.Name = "FLScale_Change_label";
            this.FLScale_Change_label.Size = new System.Drawing.Size(441, 37);
            this.FLScale_Change_label.TabIndex = 44;
            this.FLScale_Change_label.Text = "Scale";
            this.FLScale_Change_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RF_MM_label
            // 
            this.RF_MM_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RF_MM_label.AutoSize = true;
            this.RF_MM_label.BackColor = System.Drawing.Color.MistyRose;
            this.RF_MM_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RF_MM_label.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.RF_MM_label.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RF_MM_label.Location = new System.Drawing.Point(450, 74);
            this.RF_MM_label.Name = "RF_MM_label";
            this.RF_MM_label.Size = new System.Drawing.Size(441, 36);
            this.RF_MM_label.TabIndex = 53;
            this.RF_MM_label.Text = "Offset Data(mm)";
            this.RF_MM_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LF_MM_label
            // 
            this.LF_MM_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LF_MM_label.AutoSize = true;
            this.LF_MM_label.BackColor = System.Drawing.Color.MistyRose;
            this.LF_MM_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LF_MM_label.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.LF_MM_label.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LF_MM_label.Location = new System.Drawing.Point(3, 74);
            this.LF_MM_label.Name = "LF_MM_label";
            this.LF_MM_label.Size = new System.Drawing.Size(441, 36);
            this.LF_MM_label.TabIndex = 51;
            this.LF_MM_label.Text = "Offset Data(mm)";
            this.LF_MM_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.LF_origin_label, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tb_DataLFX_origin, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.RF_origin_label, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.tb_DataLFY_origin, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tb_DataRFX_origin, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.tb_DataRFY_origin, 1, 2);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 106);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(894, 106);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // LF_origin_label
            // 
            this.LF_origin_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LF_origin_label.AutoSize = true;
            this.LF_origin_label.BackColor = System.Drawing.Color.MistyRose;
            this.LF_origin_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LF_origin_label.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.LF_origin_label.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LF_origin_label.Location = new System.Drawing.Point(3, 0);
            this.LF_origin_label.Name = "LF_origin_label";
            this.LF_origin_label.Size = new System.Drawing.Size(441, 36);
            this.LF_origin_label.TabIndex = 21;
            this.LF_origin_label.Text = "LF Training Data";
            this.LF_origin_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_DataLFX_origin
            // 
            this.tb_DataLFX_origin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFX_origin.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFX_origin.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFX_origin.Location = new System.Drawing.Point(3, 39);
            this.tb_DataLFX_origin.Name = "tb_DataLFX_origin";
            this.tb_DataLFX_origin.ReadOnly = true;
            this.tb_DataLFX_origin.Size = new System.Drawing.Size(441, 30);
            this.tb_DataLFX_origin.TabIndex = 18;
            this.tb_DataLFX_origin.Text = "0";
            this.tb_DataLFX_origin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RF_origin_label
            // 
            this.RF_origin_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RF_origin_label.AutoSize = true;
            this.RF_origin_label.BackColor = System.Drawing.Color.MistyRose;
            this.RF_origin_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RF_origin_label.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.RF_origin_label.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.RF_origin_label.Location = new System.Drawing.Point(450, 0);
            this.RF_origin_label.Name = "RF_origin_label";
            this.RF_origin_label.Size = new System.Drawing.Size(441, 36);
            this.RF_origin_label.TabIndex = 15;
            this.RF_origin_label.Text = "RF Training Data";
            this.RF_origin_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_DataLFY_origin
            // 
            this.tb_DataLFY_origin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFY_origin.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFY_origin.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFY_origin.Location = new System.Drawing.Point(3, 75);
            this.tb_DataLFY_origin.Name = "tb_DataLFY_origin";
            this.tb_DataLFY_origin.ReadOnly = true;
            this.tb_DataLFY_origin.Size = new System.Drawing.Size(441, 30);
            this.tb_DataLFY_origin.TabIndex = 17;
            this.tb_DataLFY_origin.Text = "0";
            this.tb_DataLFY_origin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_DataRFX_origin
            // 
            this.tb_DataRFX_origin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFX_origin.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFX_origin.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFX_origin.Location = new System.Drawing.Point(450, 39);
            this.tb_DataRFX_origin.Name = "tb_DataRFX_origin";
            this.tb_DataRFX_origin.ReadOnly = true;
            this.tb_DataRFX_origin.Size = new System.Drawing.Size(441, 30);
            this.tb_DataRFX_origin.TabIndex = 12;
            this.tb_DataRFX_origin.Text = "0";
            this.tb_DataRFX_origin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_DataRFY_origin
            // 
            this.tb_DataRFY_origin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFY_origin.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFY_origin.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFY_origin.Location = new System.Drawing.Point(450, 75);
            this.tb_DataRFY_origin.Name = "tb_DataRFY_origin";
            this.tb_DataRFY_origin.ReadOnly = true;
            this.tb_DataRFY_origin.Size = new System.Drawing.Size(441, 30);
            this.tb_DataRFY_origin.TabIndex = 11;
            this.tb_DataRFY_origin.Text = "0";
            this.tb_DataRFY_origin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.tb_DataLFH, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tb_DataRFH, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tb_DataRFL, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.tb_DataLFL, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.textBox19, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBox20, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 215);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(894, 126);
            this.tableLayoutPanel3.TabIndex = 11;
            // 
            // tb_DataLFH
            // 
            this.tb_DataLFH.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFH.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFH.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFH.Location = new System.Drawing.Point(3, 87);
            this.tb_DataLFH.Multiline = true;
            this.tb_DataLFH.Name = "tb_DataLFH";
            this.tb_DataLFH.ReadOnly = true;
            this.tb_DataLFH.Size = new System.Drawing.Size(441, 36);
            this.tb_DataLFH.TabIndex = 20;
            this.tb_DataLFH.Text = "0";
            this.tb_DataLFH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_DataRFH
            // 
            this.tb_DataRFH.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFH.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFH.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFH.Location = new System.Drawing.Point(450, 87);
            this.tb_DataRFH.Multiline = true;
            this.tb_DataRFH.Name = "tb_DataRFH";
            this.tb_DataRFH.ReadOnly = true;
            this.tb_DataRFH.Size = new System.Drawing.Size(441, 36);
            this.tb_DataRFH.TabIndex = 19;
            this.tb_DataRFH.Text = "0";
            this.tb_DataRFH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_DataRFL
            // 
            this.tb_DataRFL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataRFL.BackColor = System.Drawing.Color.Beige;
            this.tb_DataRFL.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataRFL.Location = new System.Drawing.Point(450, 45);
            this.tb_DataRFL.Multiline = true;
            this.tb_DataRFL.Name = "tb_DataRFL";
            this.tb_DataRFL.ReadOnly = true;
            this.tb_DataRFL.Size = new System.Drawing.Size(441, 36);
            this.tb_DataRFL.TabIndex = 13;
            this.tb_DataRFL.Text = "0";
            this.tb_DataRFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_DataLFL
            // 
            this.tb_DataLFL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_DataLFL.BackColor = System.Drawing.Color.Beige;
            this.tb_DataLFL.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_DataLFL.Location = new System.Drawing.Point(3, 45);
            this.tb_DataLFL.Multiline = true;
            this.tb_DataLFL.Name = "tb_DataLFL";
            this.tb_DataLFL.ReadOnly = true;
            this.tb_DataLFL.Size = new System.Drawing.Size(441, 36);
            this.tb_DataLFL.TabIndex = 12;
            this.tb_DataLFL.Text = "0";
            this.tb_DataLFL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox19
            // 
            this.textBox19.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox19.BackColor = System.Drawing.Color.MistyRose;
            this.textBox19.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox19.Location = new System.Drawing.Point(450, 3);
            this.textBox19.Multiline = true;
            this.textBox19.Name = "textBox19";
            this.textBox19.ReadOnly = true;
            this.textBox19.Size = new System.Drawing.Size(441, 36);
            this.textBox19.TabIndex = 12;
            this.textBox19.Text = "RF Data";
            this.textBox19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox20
            // 
            this.textBox20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox20.BackColor = System.Drawing.Color.MistyRose;
            this.textBox20.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox20.Location = new System.Drawing.Point(3, 3);
            this.textBox20.Multiline = true;
            this.textBox20.Name = "textBox20";
            this.textBox20.ReadOnly = true;
            this.textBox20.Size = new System.Drawing.Size(441, 36);
            this.textBox20.TabIndex = 11;
            this.textBox20.Text = "LF Data";
            this.textBox20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.tb_StatusRF, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.tb_StatusLF, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox12, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBox13, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 15);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(894, 88);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // tb_StatusRF
            // 
            this.tb_StatusRF.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_StatusRF.BackColor = System.Drawing.Color.Beige;
            this.tb_StatusRF.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_StatusRF.Location = new System.Drawing.Point(450, 47);
            this.tb_StatusRF.Multiline = true;
            this.tb_StatusRF.Name = "tb_StatusRF";
            this.tb_StatusRF.ReadOnly = true;
            this.tb_StatusRF.Size = new System.Drawing.Size(441, 38);
            this.tb_StatusRF.TabIndex = 13;
            this.tb_StatusRF.Text = "0";
            this.tb_StatusRF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_StatusRF.TextChanged += new System.EventHandler(this.tb_StatusRF_TextChanged);
            // 
            // tb_StatusLF
            // 
            this.tb_StatusLF.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_StatusLF.BackColor = System.Drawing.Color.Beige;
            this.tb_StatusLF.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_StatusLF.Location = new System.Drawing.Point(3, 47);
            this.tb_StatusLF.Multiline = true;
            this.tb_StatusLF.Name = "tb_StatusLF";
            this.tb_StatusLF.ReadOnly = true;
            this.tb_StatusLF.Size = new System.Drawing.Size(441, 38);
            this.tb_StatusLF.TabIndex = 12;
            this.tb_StatusLF.Text = "0";
            this.tb_StatusLF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_StatusLF.TextChanged += new System.EventHandler(this.tb_StatusLF_TextChanged);
            // 
            // textBox12
            // 
            this.textBox12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox12.BackColor = System.Drawing.Color.MistyRose;
            this.textBox12.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox12.Location = new System.Drawing.Point(450, 3);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.ReadOnly = true;
            this.textBox12.Size = new System.Drawing.Size(441, 38);
            this.textBox12.TabIndex = 12;
            this.textBox12.Text = "Right Status";
            this.textBox12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox13
            // 
            this.textBox13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox13.BackColor = System.Drawing.Color.MistyRose;
            this.textBox13.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox13.Location = new System.Drawing.Point(3, 3);
            this.textBox13.Multiline = true;
            this.textBox13.Name = "textBox13";
            this.textBox13.ReadOnly = true;
            this.textBox13.Size = new System.Drawing.Size(441, 38);
            this.textBox13.TabIndex = 11;
            this.textBox13.Text = "Left Status";
            this.textBox13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.cogToolBlockEditV21);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(917, 673);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Vision Program";
            // 
            // cogToolBlockEditV21
            // 
            this.cogToolBlockEditV21.AllowDrop = true;
            this.cogToolBlockEditV21.ContextMenuCustomizer = null;
            this.cogToolBlockEditV21.EnableDelegateQueuing = true;
            this.cogToolBlockEditV21.LocalDisplayVisible = false;
            this.cogToolBlockEditV21.Location = new System.Drawing.Point(3, 3);
            this.cogToolBlockEditV21.MinimumSize = new System.Drawing.Size(489, 0);
            this.cogToolBlockEditV21.Name = "cogToolBlockEditV21";
            this.cogToolBlockEditV21.ShowNodeToolTips = true;
            this.cogToolBlockEditV21.Size = new System.Drawing.Size(911, 664);
            this.cogToolBlockEditV21.SuspendElectricRuns = false;
            this.cogToolBlockEditV21.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(917, 673);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Current Status";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(925, 699);
            this.tabControl1.TabIndex = 11;
            // 
            // tmr_Sequence
            // 
            this.tmr_Sequence.Interval = 1000;
            this.tmr_Sequence.Tick += new System.EventHandler(this.tmr_Sequence_Tick);
            // 
            // tabStatus
            // 
            this.tabStatus.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabStatus.Controls.Add(this.tabPage3);
            this.tabStatus.Controls.Add(this.tabPage4);
            this.tabStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabStatus.Location = new System.Drawing.Point(0, 728);
            this.tabStatus.Name = "tabStatus";
            this.tabStatus.SelectedIndex = 0;
            this.tabStatus.Size = new System.Drawing.Size(942, 187);
            this.tabStatus.TabIndex = 12;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lvLog);
            this.tabPage3.Controls.Add(this.listView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(934, 161);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Log";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lvLog
            // 
            this.lvLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lvLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.lvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLog.Location = new System.Drawing.Point(3, 3);
            this.lvLog.Name = "lvLog";
            this.lvLog.Size = new System.Drawing.Size(928, 155);
            this.lvLog.TabIndex = 2;
            this.lvLog.UseCompatibleStateImageBehavior = false;
            this.lvLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Time";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Type";
            this.columnHeader7.Width = 120;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Description";
            this.columnHeader8.Width = 500;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(928, 155);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.lvData);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(934, 161);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Received data";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lvData
            // 
            this.lvData.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lvData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3});
            this.lvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvData.Location = new System.Drawing.Point(3, 3);
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(928, 155);
            this.lvData.TabIndex = 3;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Received";
            this.columnHeader3.Width = 400;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 0;
            this.columnHeader2.Text = "Time";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 1;
            this.columnHeader4.Text = "Type";
            this.columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 2;
            this.columnHeader5.Text = "Description";
            this.columnHeader5.Width = 500;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 915);
            this.Controls.Add(this.tabStatus);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "Vision Program";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cogToolBlockEditV21)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabStatus.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalSetToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tb_Conn;
        private System.Windows.Forms.TextBox tb_Start;
        private System.Windows.Forms.TextBox tb_Part;
        private System.Windows.Forms.TextBox tb_Cartype;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox tb_DataRFL;
        private System.Windows.Forms.TextBox tb_DataLFL;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.TextBox textBox20;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox tb_StatusRF;
        private System.Windows.Forms.TextBox tb_StatusLF;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox tb_DataLFH;
        private System.Windows.Forms.TextBox tb_DataRFH;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private Cognex.VisionPro.ToolBlock.CogToolBlockEditV2 cogToolBlockEditV21;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Timer tmr_Sequence;
        private System.Windows.Forms.TextBox tb_DataLFX_mm;
        private System.Windows.Forms.TextBox tb_DataLFY_mm;
        private System.Windows.Forms.TextBox tb_DataRFY_mm;
        private System.Windows.Forms.TextBox tb_DataRFX_mm;
        private System.Windows.Forms.TextBox tb_DataLFScale_change;
        private System.Windows.Forms.TextBox tb_DataRFScale_change;
        private System.Windows.Forms.Label RF_MM_label;
        private System.Windows.Forms.Label LF_MM_label;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label LF_origin_label;
        private System.Windows.Forms.TextBox tb_DataLFX_origin;
        private System.Windows.Forms.Label RF_origin_label;
        private System.Windows.Forms.TextBox tb_DataLFY_origin;
        private System.Windows.Forms.TextBox tb_DataRFX_origin;
        private System.Windows.Forms.TextBox tb_DataRFY_origin;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ToolStripMenuItem startGrapOneToolStripMenuItem;
        private System.Windows.Forms.TabControl tabStatus;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ListView lvLog;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Label FRScale_Change_label;
        private System.Windows.Forms.Label FLScale_Change_label;
        private System.Windows.Forms.TextBox txt_Barcode;
        private System.Windows.Forms.ToolStripMenuItem toolboxrunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem teachingToolStripMenuItem;
    }
}

