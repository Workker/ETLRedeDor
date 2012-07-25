namespace WindowsETL
{
    partial class ETLMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ETLMonitor));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "",
            "TESTE1",
            "TESTE2",
            "TESTE3",
            "TESTE4"}, -1);
            this.btnProcessar = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.iniciarETLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pararETLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.sairToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUltimaExecucao = new System.Windows.Forms.Label();
            this.ListaProcesso = new System.Windows.Forms.ListView();
            this.col01 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col05 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col02 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col03 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListaControle = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProcessar
            // 
            this.btnProcessar.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessar.Location = new System.Drawing.Point(686, 27);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(145, 48);
            this.btnProcessar.TabIndex = 0;
            this.btnProcessar.Text = "Iniciar ETL";
            this.btnProcessar.UseVisualStyleBackColor = true;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "ETL - Monitor";
            this.notifyIcon1.BalloonTipTitle = "ETL - Monitor";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "ETL - Monitor ";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iniciarETLToolStripMenuItem,
            this.pararETLToolStripMenuItem,
            this.toolStripMenuItem1,
            this.sairToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 76);
            // 
            // iniciarETLToolStripMenuItem
            // 
            this.iniciarETLToolStripMenuItem.Name = "iniciarETLToolStripMenuItem";
            this.iniciarETLToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.iniciarETLToolStripMenuItem.Text = "Iniciar ETL";
            this.iniciarETLToolStripMenuItem.Click += new System.EventHandler(this.iniciarETLToolStripMenuItem_Click);
            // 
            // pararETLToolStripMenuItem
            // 
            this.pararETLToolStripMenuItem.Name = "pararETLToolStripMenuItem";
            this.pararETLToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.pararETLToolStripMenuItem.Text = "Parar ETL";
            this.pararETLToolStripMenuItem.Click += new System.EventHandler(this.pararETLToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 6);
            // 
            // sairToolStripMenuItem1
            // 
            this.sairToolStripMenuItem1.Name = "sairToolStripMenuItem1";
            this.sairToolStripMenuItem1.Size = new System.Drawing.Size(128, 22);
            this.sairToolStripMenuItem1.Text = "Sair";
            this.sairToolStripMenuItem1.Click += new System.EventHandler(this.sairToolStripMenuItem1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sairToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(838, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "Última Execução:";
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Última Execução:";
            // 
            // lblUltimaExecucao
            // 
            this.lblUltimaExecucao.AutoSize = true;
            this.lblUltimaExecucao.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUltimaExecucao.Location = new System.Drawing.Point(116, 41);
            this.lblUltimaExecucao.Name = "lblUltimaExecucao";
            this.lblUltimaExecucao.Size = new System.Drawing.Size(76, 16);
            this.lblUltimaExecucao.TabIndex = 5;
            this.lblUltimaExecucao.Text = "00/00/0000 ";
            // 
            // ListaProcesso
            // 
            this.ListaProcesso.BackColor = System.Drawing.Color.White;
            this.ListaProcesso.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col01,
            this.col05,
            this.col02,
            this.col03});
            this.ListaProcesso.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListaProcesso.ForeColor = System.Drawing.Color.LightYellow;
            this.ListaProcesso.FullRowSelect = true;
            this.ListaProcesso.GridLines = true;
            this.ListaProcesso.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.ListaProcesso.Location = new System.Drawing.Point(8, 208);
            this.ListaProcesso.Name = "ListaProcesso";
            this.ListaProcesso.ShowItemToolTips = true;
            this.ListaProcesso.Size = new System.Drawing.Size(821, 220);
            this.ListaProcesso.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.ListaProcesso.TabIndex = 7;
            this.ListaProcesso.UseCompatibleStateImageBehavior = false;
            this.ListaProcesso.View = System.Windows.Forms.View.Details;
            this.ListaProcesso.SelectedIndexChanged += new System.EventHandler(this.ListaProcesso_SelectedIndexChanged);
            // 
            // col01
            // 
            this.col01.Text = "Data ";
            this.col01.Width = 130;
            // 
            // col05
            // 
            this.col05.Text = "Unidade";
            // 
            // col02
            // 
            this.col02.Text = "Processo";
            this.col02.Width = 300;
            // 
            // col03
            // 
            this.col03.Text = "Status";
            this.col03.Width = 300;
            // 
            // ListaControle
            // 
            this.ListaControle.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListaControle.FormattingEnabled = true;
            this.ListaControle.ItemHeight = 14;
            this.ListaControle.Location = new System.Drawing.Point(8, 86);
            this.ListaControle.Name = "ListaControle";
            this.ListaControle.Size = new System.Drawing.Size(821, 116);
            this.ListaControle.Sorted = true;
            this.ListaControle.TabIndex = 8;
            // 
            // ETLMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 445);
            this.Controls.Add(this.ListaControle);
            this.Controls.Add(this.ListaProcesso);
            this.Controls.Add(this.lblUltimaExecucao);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnProcessar);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ETLMonitor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETL - Monitor";
            this.Load += new System.EventHandler(this.ETLMonitor_Load);
            this.Resize += new System.EventHandler(this.ETLMonitor_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem iniciarETLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pararETLToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUltimaExecucao;
        private System.Windows.Forms.ListView ListaProcesso;
        private System.Windows.Forms.ColumnHeader col01;
        private System.Windows.Forms.ColumnHeader col02;
        private System.Windows.Forms.ColumnHeader col03;
        private System.Windows.Forms.ColumnHeader col05;
        private System.Windows.Forms.ListBox ListaControle;
    }
}

