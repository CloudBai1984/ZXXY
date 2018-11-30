namespace XYJClient
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
            this.tbxCMD = new System.Windows.Forms.TextBox();
            this.tbxContent = new System.Windows.Forms.TextBox();
            this.btnJJC = new System.Windows.Forms.Button();
            this.btnDFFB = new System.Windows.Forms.Button();
            this.btnMY = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbxCMD
            // 
            this.tbxCMD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxCMD.Location = new System.Drawing.Point(12, 215);
            this.tbxCMD.Name = "tbxCMD";
            this.tbxCMD.Size = new System.Drawing.Size(385, 26);
            this.tbxCMD.TabIndex = 1;
            this.tbxCMD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxCMD_KeyPress);
            // 
            // tbxContent
            // 
            this.tbxContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxContent.Location = new System.Drawing.Point(12, 13);
            this.tbxContent.Multiline = true;
            this.tbxContent.Name = "tbxContent";
            this.tbxContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxContent.Size = new System.Drawing.Size(385, 196);
            this.tbxContent.TabIndex = 2;
            // 
            // btnJJC
            // 
            this.btnJJC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJJC.Location = new System.Drawing.Point(12, 247);
            this.btnJJC.Name = "btnJJC";
            this.btnJJC.Size = new System.Drawing.Size(70, 26);
            this.btnJJC.TabIndex = 3;
            this.btnJJC.Text = "JJC";
            this.btnJJC.UseVisualStyleBackColor = true;
            this.btnJJC.Click += new System.EventHandler(this.btnJJC_Click);
            // 
            // btnDFFB
            // 
            this.btnDFFB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDFFB.Location = new System.Drawing.Point(12, 291);
            this.btnDFFB.Name = "btnDFFB";
            this.btnDFFB.Size = new System.Drawing.Size(70, 26);
            this.btnDFFB.TabIndex = 4;
            this.btnDFFB.Text = "DFFB";
            this.btnDFFB.UseVisualStyleBackColor = true;
            this.btnDFFB.Click += new System.EventHandler(this.btnDFFB_Click);
            // 
            // btnMY
            // 
            this.btnMY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMY.Location = new System.Drawing.Point(98, 247);
            this.btnMY.Name = "btnMY";
            this.btnMY.Size = new System.Drawing.Size(70, 26);
            this.btnMY.TabIndex = 5;
            this.btnMY.Text = "MY";
            this.btnMY.UseVisualStyleBackColor = true;
            this.btnMY.Click += new System.EventHandler(this.btnMY_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(327, 247);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(70, 26);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(327, 291);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(70, 26);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 329);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnMY);
            this.Controls.Add(this.btnDFFB);
            this.Controls.Add(this.btnJJC);
            this.Controls.Add(this.tbxContent);
            this.Controls.Add(this.tbxCMD);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbxCMD;
        private System.Windows.Forms.TextBox tbxContent;
        private System.Windows.Forms.Button btnJJC;
        private System.Windows.Forms.Button btnDFFB;
        private System.Windows.Forms.Button btnMY;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnStop;
    }
}

