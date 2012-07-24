namespace WinETL
{
    partial class frmMonitoramento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMonitoramento));
            this.NotificarETL = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnProcessar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NotificarETL
            // 
            this.NotificarETL.Icon = ((System.Drawing.Icon)(resources.GetObject("NotificarETL.Icon")));
            this.NotificarETL.Text = "ETL - Monitoramento";
            this.NotificarETL.Visible = true;
            // 
            // btnProcessar
            // 
            this.btnProcessar.Location = new System.Drawing.Point(30, 62);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(75, 23);
            this.btnProcessar.TabIndex = 0;
            this.btnProcessar.Text = "button1";
            this.btnProcessar.UseVisualStyleBackColor = true;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
            // 
            // frmMonitoramento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 375);
            this.Controls.Add(this.btnProcessar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMonitoramento";
            this.Text = "ETL - Monitoramento ";
            this.Load += new System.EventHandler(this.frmMonitoramento_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotificarETL;
        private System.Windows.Forms.Button btnProcessar;
    }
}

