namespace FileCompare
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
            splitContainer1 = new SplitContainer();
            panel3 = new Panel();
            lvwLeftDir = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            panel2 = new Panel();
            txtLeftDir = new TextBox();
            btnLeftDir = new Button();
            panel1 = new Panel();
            btnCopyFromLeft = new Button();
            lblAppName = new Label();
            panel6 = new Panel();
            lvwrightDir = new ListView();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            panel5 = new Panel();
            txtRightDir = new TextBox();
            btnRightDir = new Button();
            panel4 = new Panel();
            btnCopyFromRight = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel6);
            splitContainer1.Panel2.Controls.Add(panel5);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(1674, 829);
            splitContainer1.SplitterDistance = 829;
            splitContainer1.SplitterWidth = 16;
            splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(lvwLeftDir);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 300);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(16, 0, 0, 16);
            panel3.Size = new Size(829, 529);
            panel3.TabIndex = 0;
            // 
            // lvwLeftDir
            // 
            lvwLeftDir.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvwLeftDir.Dock = DockStyle.Fill;
            lvwLeftDir.FullRowSelect = true;
            lvwLeftDir.GridLines = true;
            lvwLeftDir.Location = new Point(16, 0);
            lvwLeftDir.Name = "lvwLeftDir";
            lvwLeftDir.Size = new Size(813, 513);
            lvwLeftDir.TabIndex = 2;
            lvwLeftDir.UseCompatibleStateImageBehavior = false;
            lvwLeftDir.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "이름";
            columnHeader1.Width = 442;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "크기";
            columnHeader2.Width = 129;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "수정일";
            columnHeader3.Width = 237;
            // 
            // panel2
            // 
            panel2.Controls.Add(txtLeftDir);
            panel2.Controls.Add(btnLeftDir);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 150);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(20);
            panel2.Size = new Size(829, 150);
            panel2.TabIndex = 1;
            // 
            // txtLeftDir
            // 
            txtLeftDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLeftDir.Location = new Point(23, 86);
            txtLeftDir.Name = "txtLeftDir";
            txtLeftDir.Size = new Size(622, 39);
            txtLeftDir.TabIndex = 2;
            // 
            // btnLeftDir
            // 
            btnLeftDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLeftDir.Location = new Point(656, 81);
            btnLeftDir.Name = "btnLeftDir";
            btnLeftDir.Size = new Size(150, 46);
            btnLeftDir.TabIndex = 1;
            btnLeftDir.Text = "폴더선택";
            btnLeftDir.UseVisualStyleBackColor = true;
            btnLeftDir.Click += btnLeftDir_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCopyFromLeft);
            panel1.Controls.Add(lblAppName);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(20);
            panel1.Size = new Size(829, 150);
            panel1.TabIndex = 0;
            // 
            // btnCopyFromLeft
            // 
            btnCopyFromLeft.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyFromLeft.Location = new Point(656, 81);
            btnCopyFromLeft.Name = "btnCopyFromLeft";
            btnCopyFromLeft.Size = new Size(150, 46);
            btnCopyFromLeft.TabIndex = 0;
            btnCopyFromLeft.Text = ">>>";
            btnCopyFromLeft.UseVisualStyleBackColor = true;
            btnCopyFromLeft.Click += btnCopyFromLeft_Click;
            // 
            // lblAppName
            // 
            lblAppName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("맑은 고딕", 28.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAppName.ForeColor = Color.Blue;
            lblAppName.Location = new Point(22, 19);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(512, 100);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "File Compare";
            // 
            // panel6
            // 
            panel6.Controls.Add(lvwrightDir);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 300);
            panel6.Name = "panel6";
            panel6.Padding = new Padding(0, 0, 16, 16);
            panel6.Size = new Size(829, 529);
            panel6.TabIndex = 1;
            // 
            // lvwrightDir
            // 
            lvwrightDir.Columns.AddRange(new ColumnHeader[] { columnHeader4, columnHeader5, columnHeader6 });
            lvwrightDir.Dock = DockStyle.Fill;
            lvwrightDir.FullRowSelect = true;
            lvwrightDir.GridLines = true;
            lvwrightDir.Location = new Point(0, 0);
            lvwrightDir.Name = "lvwrightDir";
            lvwrightDir.Size = new Size(813, 513);
            lvwrightDir.TabIndex = 3;
            lvwrightDir.UseCompatibleStateImageBehavior = false;
            lvwrightDir.View = View.Details;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "이름";
            columnHeader4.Width = 442;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "크기";
            columnHeader5.Width = 129;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "수정일";
            columnHeader6.Width = 237;
            // 
            // panel5
            // 
            panel5.Controls.Add(txtRightDir);
            panel5.Controls.Add(btnRightDir);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 150);
            panel5.Name = "panel5";
            panel5.Padding = new Padding(20);
            panel5.Size = new Size(829, 150);
            panel5.TabIndex = 0;
            // 
            // txtRightDir
            // 
            txtRightDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtRightDir.Location = new Point(23, 86);
            txtRightDir.Name = "txtRightDir";
            txtRightDir.Size = new Size(622, 39);
            txtRightDir.TabIndex = 4;
            // 
            // btnRightDir
            // 
            btnRightDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRightDir.Location = new Point(656, 81);
            btnRightDir.Name = "btnRightDir";
            btnRightDir.Size = new Size(150, 46);
            btnRightDir.TabIndex = 3;
            btnRightDir.Text = "폴더선택";
            btnRightDir.UseVisualStyleBackColor = true;
            btnRightDir.Click += btnRightDir_Click;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnCopyFromRight);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(20);
            panel4.Size = new Size(829, 150);
            panel4.TabIndex = 0;
            // 
            // btnCopyFromRight
            // 
            btnCopyFromRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCopyFromRight.Location = new Point(23, 81);
            btnCopyFromRight.Name = "btnCopyFromRight";
            btnCopyFromRight.Size = new Size(150, 46);
            btnCopyFromRight.TabIndex = 1;
            btnCopyFromRight.Text = "<<<";
            btnCopyFromRight.UseVisualStyleBackColor = true;
            btnCopyFromRight.Click += btnCopyFromRight_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1674, 829);
            Controls.Add(splitContainer1);
            Name = "Form1";
            StartPosition = FormStartPosition.Manual;
            Text = " File Compare v1.0";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Label lblAppName;
        private Panel panel6;
        private Panel panel5;
        private Panel panel4;
        private TextBox txtLeftDir;
        private Button btnLeftDir;
        private Button btnCopyFromLeft;
        private ListView lvwLeftDir;
        private TextBox txtRightDir;
        private Button btnRightDir;
        private Button btnCopyFromRight;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ListView lvwrightDir;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
    }
}
