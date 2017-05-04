namespace WinForm_SerialPort
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ReceivedCharRadioButton = new System.Windows.Forms.RadioButton();
            this.ReceivedValueRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PostCharRadioButton = new System.Windows.Forms.RadioButton();
            this.PostValueRadioButton = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BaudComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SerialPortComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReceivedTextBox = new System.Windows.Forms.TextBox();
            this.PostTextBox = new System.Windows.Forms.TextBox();
            this.PostButton = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.ScanButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ScanButton);
            this.groupBox1.Controls.Add(this.CloseButton);
            this.groupBox1.Controls.Add(this.OpenButton);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.BaudComboBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SerialPortComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 246);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(133, 217);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 9;
            this.CloseButton.Text = "关闭端口";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(19, 217);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 8;
            this.OpenButton.Text = "打开端口";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ReceivedCharRadioButton);
            this.panel2.Controls.Add(this.ReceivedValueRadioButton);
            this.panel2.Location = new System.Drawing.Point(76, 170);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(143, 41);
            this.panel2.TabIndex = 7;
            // 
            // ReceivedCharRadioButton
            // 
            this.ReceivedCharRadioButton.AutoSize = true;
            this.ReceivedCharRadioButton.Location = new System.Drawing.Point(85, 16);
            this.ReceivedCharRadioButton.Name = "ReceivedCharRadioButton";
            this.ReceivedCharRadioButton.Size = new System.Drawing.Size(47, 16);
            this.ReceivedCharRadioButton.TabIndex = 1;
            this.ReceivedCharRadioButton.TabStop = true;
            this.ReceivedCharRadioButton.Text = "字符";
            this.ReceivedCharRadioButton.UseVisualStyleBackColor = true;
            // 
            // ReceivedValueRadioButton
            // 
            this.ReceivedValueRadioButton.AutoSize = true;
            this.ReceivedValueRadioButton.Location = new System.Drawing.Point(17, 16);
            this.ReceivedValueRadioButton.Name = "ReceivedValueRadioButton";
            this.ReceivedValueRadioButton.Size = new System.Drawing.Size(47, 16);
            this.ReceivedValueRadioButton.TabIndex = 0;
            this.ReceivedValueRadioButton.TabStop = true;
            this.ReceivedValueRadioButton.Text = "数值";
            this.ReceivedValueRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.PostCharRadioButton);
            this.panel1.Controls.Add(this.PostValueRadioButton);
            this.panel1.Location = new System.Drawing.Point(76, 128);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(143, 36);
            this.panel1.TabIndex = 6;
            // 
            // PostCharRadioButton
            // 
            this.PostCharRadioButton.AutoSize = true;
            this.PostCharRadioButton.Location = new System.Drawing.Point(85, 14);
            this.PostCharRadioButton.Name = "PostCharRadioButton";
            this.PostCharRadioButton.Size = new System.Drawing.Size(47, 16);
            this.PostCharRadioButton.TabIndex = 1;
            this.PostCharRadioButton.TabStop = true;
            this.PostCharRadioButton.Text = "字符";
            this.PostCharRadioButton.UseVisualStyleBackColor = true;
            // 
            // PostValueRadioButton
            // 
            this.PostValueRadioButton.AutoSize = true;
            this.PostValueRadioButton.Location = new System.Drawing.Point(17, 14);
            this.PostValueRadioButton.Name = "PostValueRadioButton";
            this.PostValueRadioButton.Size = new System.Drawing.Size(47, 16);
            this.PostValueRadioButton.TabIndex = 0;
            this.PostValueRadioButton.TabStop = true;
            this.PostValueRadioButton.Text = "数值";
            this.PostValueRadioButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "接收模式";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "发送模式";
            // 
            // BaudComboBox
            // 
            this.BaudComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BaudComboBox.FormattingEnabled = true;
            this.BaudComboBox.Items.AddRange(new object[] {
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "115200"});
            this.BaudComboBox.Location = new System.Drawing.Point(93, 59);
            this.BaudComboBox.Name = "BaudComboBox";
            this.BaudComboBox.Size = new System.Drawing.Size(115, 20);
            this.BaudComboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "波特率";
            // 
            // SerialPortComboBox
            // 
            this.SerialPortComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortComboBox.FormattingEnabled = true;
            this.SerialPortComboBox.Location = new System.Drawing.Point(93, 27);
            this.SerialPortComboBox.Name = "SerialPortComboBox";
            this.SerialPortComboBox.Size = new System.Drawing.Size(115, 20);
            this.SerialPortComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口";
            // 
            // ReceivedTextBox
            // 
            this.ReceivedTextBox.Location = new System.Drawing.Point(252, 12);
            this.ReceivedTextBox.Multiline = true;
            this.ReceivedTextBox.Name = "ReceivedTextBox";
            this.ReceivedTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReceivedTextBox.Size = new System.Drawing.Size(225, 246);
            this.ReceivedTextBox.TabIndex = 1;
            // 
            // PostTextBox
            // 
            this.PostTextBox.Location = new System.Drawing.Point(12, 264);
            this.PostTextBox.Multiline = true;
            this.PostTextBox.Name = "PostTextBox";
            this.PostTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PostTextBox.Size = new System.Drawing.Size(368, 42);
            this.PostTextBox.TabIndex = 2;
            // 
            // PostButton
            // 
            this.PostButton.Enabled = false;
            this.PostButton.Location = new System.Drawing.Point(387, 264);
            this.PostButton.Name = "PostButton";
            this.PostButton.Size = new System.Drawing.Size(90, 42);
            this.PostButton.TabIndex = 3;
            this.PostButton.Text = "发送";
            this.PostButton.UseVisualStyleBackColor = true;
            this.PostButton.Click += new System.EventHandler(this.PostButton_Click);
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(66, 99);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(103, 23);
            this.ScanButton.TabIndex = 10;
            this.ScanButton.Text = "扫描可用端口";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(490, 322);
            this.Controls.Add(this.PostButton);
            this.Controls.Add(this.PostTextBox);
            this.Controls.Add(this.ReceivedTextBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox BaudComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SerialPortComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton ReceivedCharRadioButton;
        private System.Windows.Forms.RadioButton ReceivedValueRadioButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton PostCharRadioButton;
        private System.Windows.Forms.RadioButton PostValueRadioButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ReceivedTextBox;
        private System.Windows.Forms.TextBox PostTextBox;
        private System.Windows.Forms.Button PostButton;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button ScanButton;
    }
}

