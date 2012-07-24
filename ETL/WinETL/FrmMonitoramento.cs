using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinETL.Passos;  

namespace WinETL
{
    public partial class frmMonitoramento : Form
    {
        public frmMonitoramento()
        {
            InitializeComponent();

        }

        private void frmMonitoramento_Load(object sender, EventArgs e)
        {

        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            HBD_Atendimentos PassoAtendHBD = new HBD_Atendimentos();

                 
            PassoAtendHBD.ProcessarAtendimentosHIS(100);  

        }

        
        
   

        
        



    }
}
