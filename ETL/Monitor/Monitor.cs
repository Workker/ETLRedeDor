using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.Passos;
using Monitor.ServiceHBD;

namespace Monitor 
{
    public partial class Monitor : Form
    {
        public Monitor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HBD_Atendimentos PassoAtendHBD = new HBD_Atendimentos();


            PassoAtendHBD.ProcessarAtendimentosHIS(100);  
        }
    }
}
