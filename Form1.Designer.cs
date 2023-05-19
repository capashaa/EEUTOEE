namespace EEU2EEO
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblToken = new Label();
            txtbToken = new TextBox();
            lblRoom = new Label();
            txtbRoom = new TextBox();
            btnCB = new Button();
            btnConvert = new Button();
            rtxtbLog = new RichTextBox();
            lvRooms = new ListView();
            chName = new ColumnHeader();
            chPlays = new ColumnHeader();
            chVisible = new ColumnHeader();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            btnOwn = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // lblToken
            // 
            lblToken.AutoSize = true;
            lblToken.Location = new Point(12, 23);
            lblToken.Name = "lblToken";
            lblToken.Size = new Size(41, 15);
            lblToken.TabIndex = 0;
            lblToken.Text = "Token:";
            // 
            // txtbToken
            // 
            txtbToken.Location = new Point(12, 41);
            txtbToken.Name = "txtbToken";
            txtbToken.PasswordChar = '*';
            txtbToken.Size = new Size(223, 23);
            txtbToken.TabIndex = 1;
            txtbToken.UseSystemPasswordChar = true;
            // 
            // lblRoom
            // 
            lblRoom.AutoSize = true;
            lblRoom.Location = new Point(12, 106);
            lblRoom.Name = "lblRoom";
            lblRoom.Size = new Size(53, 15);
            lblRoom.TabIndex = 2;
            lblRoom.Text = "RoomID:";
            // 
            // txtbRoom
            // 
            txtbRoom.Location = new Point(12, 124);
            txtbRoom.Name = "txtbRoom";
            txtbRoom.Size = new Size(223, 23);
            txtbRoom.TabIndex = 3;
            // 
            // btnCB
            // 
            btnCB.Location = new Point(160, 70);
            btnCB.Name = "btnCB";
            btnCB.Size = new Size(75, 23);
            btnCB.TabIndex = 4;
            btnCB.Text = "Clipboard";
            btnCB.UseVisualStyleBackColor = true;
            btnCB.Click += btnCB_Click;
            // 
            // btnConvert
            // 
            btnConvert.Location = new Point(12, 153);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(75, 23);
            btnConvert.TabIndex = 5;
            btnConvert.Text = "Convert";
            btnConvert.UseVisualStyleBackColor = true;
            btnConvert.Click += btnConvert_Click;
            // 
            // rtxtbLog
            // 
            rtxtbLog.Location = new Point(11, 22);
            rtxtbLog.Name = "rtxtbLog";
            rtxtbLog.Size = new Size(281, 50);
            rtxtbLog.TabIndex = 6;
            rtxtbLog.Text = "";
            // 
            // lvRooms
            // 
            lvRooms.Columns.AddRange(new ColumnHeader[] { chName, chPlays, chVisible });
            lvRooms.FullRowSelect = true;
            lvRooms.GridLines = true;
            lvRooms.Location = new Point(11, 22);
            lvRooms.MultiSelect = false;
            lvRooms.Name = "lvRooms";
            lvRooms.Size = new Size(281, 166);
            lvRooms.TabIndex = 7;
            lvRooms.UseCompatibleStateImageBehavior = false;
            lvRooms.View = View.Details;
            lvRooms.SelectedIndexChanged += lvRooms_SelectedIndexChanged;
            // 
            // chName
            // 
            chName.Text = "Name";
            chName.Width = 120;
            // 
            // chPlays
            // 
            chPlays.Text = "Plays";
            chPlays.Width = 80;
            // 
            // chVisible
            // 
            chVisible.Text = "Visible";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lvRooms);
            groupBox1.Location = new Point(241, 23);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(305, 197);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Your Own Worlds";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rtxtbLog);
            groupBox2.Location = new Point(241, 226);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(305, 84);
            groupBox2.TabIndex = 9;
            groupBox2.TabStop = false;
            groupBox2.Text = "Logs";
            // 
            // btnOwn
            // 
            btnOwn.Location = new Point(12, 70);
            btnOwn.Name = "btnOwn";
            btnOwn.Size = new Size(75, 23);
            btnOwn.TabIndex = 10;
            btnOwn.Text = "My Info";
            btnOwn.UseVisualStyleBackColor = true;
            btnOwn.Click += btnOwn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(556, 322);
            Controls.Add(btnOwn);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(btnConvert);
            Controls.Add(btnCB);
            Controls.Add(txtbRoom);
            Controls.Add(lblRoom);
            Controls.Add(txtbToken);
            Controls.Add(lblToken);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EEU to EEO";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblToken;
        private TextBox txtbToken;
        private Label lblRoom;
        private TextBox txtbRoom;
        private Button btnCB;
        private Button btnConvert;
        private RichTextBox rtxtbLog;
        private ListView lvRooms;
        private ColumnHeader chName;
        private ColumnHeader chPlays;
        private ColumnHeader chVisible;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button btnOwn;
    }
}