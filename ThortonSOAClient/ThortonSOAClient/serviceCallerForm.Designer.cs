namespace ThortonSOAClient
{
    partial class serviceCallerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serviceInfoGB = new System.Windows.Forms.GroupBox();
            this.serviceNameTB = new System.Windows.Forms.TextBox();
            this.teamNametb = new System.Windows.Forms.TextBox();
            this.serviceDescTB = new System.Windows.Forms.TextBox();
            this.serviceDescriptionLbl = new System.Windows.Forms.Label();
            this.serviceNameLbl = new System.Windows.Forms.Label();
            this.teamNameLbl = new System.Windows.Forms.Label();
            this.serviceCallerGB = new System.Windows.Forms.GroupBox();
            this.argPanel = new System.Windows.Forms.Panel();
            this.executeBtn = new System.Windows.Forms.Button();
            this.arg1Err = new System.Windows.Forms.ErrorProvider(this.components);
            this.arg2Err = new System.Windows.Forms.ErrorProvider(this.components);
            this.responseGB = new System.Windows.Forms.GroupBox();
            this.responseTB = new System.Windows.Forms.TextBox();
            this.serviceInfoGB.SuspendLayout();
            this.serviceCallerGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.arg1Err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arg2Err)).BeginInit();
            this.responseGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // serviceInfoGB
            // 
            this.serviceInfoGB.Controls.Add(this.serviceNameTB);
            this.serviceInfoGB.Controls.Add(this.teamNametb);
            this.serviceInfoGB.Controls.Add(this.serviceDescTB);
            this.serviceInfoGB.Controls.Add(this.serviceDescriptionLbl);
            this.serviceInfoGB.Controls.Add(this.serviceNameLbl);
            this.serviceInfoGB.Controls.Add(this.teamNameLbl);
            this.serviceInfoGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceInfoGB.Location = new System.Drawing.Point(12, 12);
            this.serviceInfoGB.Name = "serviceInfoGB";
            this.serviceInfoGB.Size = new System.Drawing.Size(418, 237);
            this.serviceInfoGB.TabIndex = 0;
            this.serviceInfoGB.TabStop = false;
            this.serviceInfoGB.Text = "Service Information";
            // 
            // serviceNameTB
            // 
            this.serviceNameTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceNameTB.Location = new System.Drawing.Point(165, 74);
            this.serviceNameTB.Name = "serviceNameTB";
            this.serviceNameTB.ReadOnly = true;
            this.serviceNameTB.Size = new System.Drawing.Size(176, 26);
            this.serviceNameTB.TabIndex = 5;
            // 
            // teamNametb
            // 
            this.teamNametb.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamNametb.Location = new System.Drawing.Point(165, 40);
            this.teamNametb.Name = "teamNametb";
            this.teamNametb.ReadOnly = true;
            this.teamNametb.Size = new System.Drawing.Size(176, 26);
            this.teamNametb.TabIndex = 4;
            // 
            // serviceDescTB
            // 
            this.serviceDescTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceDescTB.Location = new System.Drawing.Point(6, 128);
            this.serviceDescTB.Multiline = true;
            this.serviceDescTB.Name = "serviceDescTB";
            this.serviceDescTB.ReadOnly = true;
            this.serviceDescTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.serviceDescTB.Size = new System.Drawing.Size(406, 103);
            this.serviceDescTB.TabIndex = 3;
            // 
            // serviceDescriptionLbl
            // 
            this.serviceDescriptionLbl.AutoSize = true;
            this.serviceDescriptionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceDescriptionLbl.Location = new System.Drawing.Point(6, 104);
            this.serviceDescriptionLbl.Name = "serviceDescriptionLbl";
            this.serviceDescriptionLbl.Size = new System.Drawing.Size(153, 20);
            this.serviceDescriptionLbl.TabIndex = 2;
            this.serviceDescriptionLbl.Text = "Service Description :";
            // 
            // serviceNameLbl
            // 
            this.serviceNameLbl.AutoSize = true;
            this.serviceNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceNameLbl.Location = new System.Drawing.Point(44, 74);
            this.serviceNameLbl.Name = "serviceNameLbl";
            this.serviceNameLbl.Size = new System.Drawing.Size(115, 20);
            this.serviceNameLbl.TabIndex = 1;
            this.serviceNameLbl.Text = "Service Name :";
            // 
            // teamNameLbl
            // 
            this.teamNameLbl.AutoSize = true;
            this.teamNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamNameLbl.Location = new System.Drawing.Point(56, 40);
            this.teamNameLbl.Name = "teamNameLbl";
            this.teamNameLbl.Size = new System.Drawing.Size(103, 20);
            this.teamNameLbl.TabIndex = 0;
            this.teamNameLbl.Text = "Team Name :";
            // 
            // serviceCallerGB
            // 
            this.serviceCallerGB.Controls.Add(this.argPanel);
            this.serviceCallerGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceCallerGB.Location = new System.Drawing.Point(12, 255);
            this.serviceCallerGB.Name = "serviceCallerGB";
            this.serviceCallerGB.Size = new System.Drawing.Size(417, 274);
            this.serviceCallerGB.TabIndex = 3;
            this.serviceCallerGB.TabStop = false;
            this.serviceCallerGB.Text = "Service Calling";
            // 
            // argPanel
            // 
            this.argPanel.AutoScroll = true;
            this.argPanel.Location = new System.Drawing.Point(6, 30);
            this.argPanel.Name = "argPanel";
            this.argPanel.Size = new System.Drawing.Size(405, 238);
            this.argPanel.TabIndex = 0;
            // 
            // executeBtn
            // 
            this.executeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeBtn.Location = new System.Drawing.Point(155, 535);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(131, 46);
            this.executeBtn.TabIndex = 5;
            this.executeBtn.Text = "Execute";
            this.executeBtn.UseVisualStyleBackColor = true;
            // 
            // arg1Err
            // 
            this.arg1Err.ContainerControl = this;
            // 
            // arg2Err
            // 
            this.arg2Err.ContainerControl = this;
            // 
            // responseGB
            // 
            this.responseGB.Controls.Add(this.responseTB);
            this.responseGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseGB.Location = new System.Drawing.Point(436, 12);
            this.responseGB.Name = "responseGB";
            this.responseGB.Size = new System.Drawing.Size(397, 569);
            this.responseGB.TabIndex = 4;
            this.responseGB.TabStop = false;
            this.responseGB.Text = "Response Output";
            // 
            // responseTB
            // 
            this.responseTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.responseTB.Location = new System.Drawing.Point(6, 29);
            this.responseTB.Multiline = true;
            this.responseTB.Name = "responseTB";
            this.responseTB.ReadOnly = true;
            this.responseTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responseTB.Size = new System.Drawing.Size(385, 534);
            this.responseTB.TabIndex = 0;
            // 
            // serviceCallerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 593);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.responseGB);
            this.Controls.Add(this.serviceCallerGB);
            this.Controls.Add(this.serviceInfoGB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "serviceCallerForm";
            this.ShowIcon = false;
            this.Text = "Service Caller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.serviceCaller_FormClosing);
            this.Load += new System.EventHandler(this.serviceCallerForm_Load);
            this.serviceInfoGB.ResumeLayout(false);
            this.serviceInfoGB.PerformLayout();
            this.serviceCallerGB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.arg1Err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arg2Err)).EndInit();
            this.responseGB.ResumeLayout(false);
            this.responseGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox serviceInfoGB;
        private System.Windows.Forms.TextBox serviceNameTB;
        private System.Windows.Forms.TextBox teamNametb;
        private System.Windows.Forms.TextBox serviceDescTB;
        private System.Windows.Forms.Label serviceDescriptionLbl;
        private System.Windows.Forms.Label serviceNameLbl;
        private System.Windows.Forms.Label teamNameLbl;
        private System.Windows.Forms.GroupBox serviceCallerGB;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.ErrorProvider arg1Err;
        private System.Windows.Forms.GroupBox responseGB;
        private System.Windows.Forms.TextBox responseTB;
        private System.Windows.Forms.ErrorProvider arg2Err;
        private System.Windows.Forms.Panel argPanel;
    }
}