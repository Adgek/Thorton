namespace ThortonSOAClient
{
    partial class pageOne
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
            this.serviceSelectGB = new System.Windows.Forms.GroupBox();
            this.executeBtn = new System.Windows.Forms.Button();
            this.serviceSelectCB = new System.Windows.Forms.ComboBox();
            this.error1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.serviceSelectGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.error1)).BeginInit();
            this.SuspendLayout();
            // 
            // serviceSelectGB
            // 
            this.serviceSelectGB.Controls.Add(this.executeBtn);
            this.serviceSelectGB.Controls.Add(this.serviceSelectCB);
            this.serviceSelectGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceSelectGB.Location = new System.Drawing.Point(12, 12);
            this.serviceSelectGB.Name = "serviceSelectGB";
            this.serviceSelectGB.Size = new System.Drawing.Size(280, 129);
            this.serviceSelectGB.TabIndex = 0;
            this.serviceSelectGB.TabStop = false;
            this.serviceSelectGB.Text = "ServiceSelection";
            // 
            // executeBtn
            // 
            this.executeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executeBtn.Location = new System.Drawing.Point(96, 86);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(86, 37);
            this.executeBtn.TabIndex = 1;
            this.executeBtn.Text = "Start";
            this.executeBtn.UseVisualStyleBackColor = true;
            this.executeBtn.Click += new System.EventHandler(this.executeBtn_Click);
            // 
            // serviceSelectCB
            // 
            this.serviceSelectCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serviceSelectCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceSelectCB.FormattingEnabled = true;
            this.serviceSelectCB.Location = new System.Drawing.Point(6, 28);
            this.serviceSelectCB.Name = "serviceSelectCB";
            this.serviceSelectCB.Size = new System.Drawing.Size(236, 28);
            this.serviceSelectCB.TabIndex = 0;
            // 
            // error1
            // 
            this.error1.ContainerControl = this;
            // 
            // pageOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 151);
            this.Controls.Add(this.serviceSelectGB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "pageOne";
            this.ShowIcon = false;
            this.Text = "Soa1";
            this.Load += new System.EventHandler(this.pageOne_Load);
            this.serviceSelectGB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.error1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox serviceSelectGB;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.ComboBox serviceSelectCB;
        private System.Windows.Forms.ErrorProvider error1;
    }
}

