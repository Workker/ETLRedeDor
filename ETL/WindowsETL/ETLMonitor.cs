using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsETL.SrvETLUnidades;
using WindowsETL.Passos;

namespace WindowsETL
{
    public partial class ETLMonitor : Form
    {
        #region Variáveis

        string strConnINT = System.Configuration.ConfigurationManager.AppSettings["ConectaINT"].ToString();
        string strScheINT = System.Configuration.ConfigurationManager.AppSettings["strSchemaINT"].ToString();

        string strConnSAT = System.Configuration.ConfigurationManager.AppSettings["ConectaSAT"].ToString();
        string strScheSAT = System.Configuration.ConfigurationManager.AppSettings["strSchemaSAT"].ToString();

        string strConciDias = System.Configuration.ConfigurationManager.AppSettings["Conciliacao_Dias"].ToString();

        string strVersao = System.Configuration.ConfigurationManager.AppSettings["Versao"].ToString();

        Int32 QtdeRegistros = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QtdeRegistros"].ToString());
        Int32 IntervaloMiliSeg = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IntervaloMinutos"].ToString());

        DataSet m_oDsConciliacao = new DataSet();

        ClDadosLOG m_oLog = new ClDadosLOG();

        DateTime HoraLimpeza;

        Double Contador = 0;

        Boolean Conciliador = false;
        DateTime DataConciliador;

        //HBD_Atendimentos oAtendimentosLer = new HBD_Atendimentos();
        //HBD_AtendimentosGravar oAtendGravar = new HBD_AtendimentosGravar();
        //HBD_Pacientes oPacientes = new HBD_Pacientes();
        //HBD_PacientesGravar oPacGravar = new HBD_PacientesGravar();

        #region Unidades

        SrvETLUnidades.WSETLUnidadesSoapClient m_oUnid = new WSETLUnidadesSoapClient();  

        #endregion

        #region Atendimento
        //ATENDIMENTO LER - His > Satelite
        LerAtendimentos m_oLerAtendimentos = new LerAtendimentos();
        
        //ATENDIMENTO GRAVAR - Sateliete > Integrador
        GravarAtendimentos m_oGravarAtendimentos = new GravarAtendimentos();
        #endregion

        #region Pacientes
        //PACIENTE LER - His > Satelite
        LerPacientes m_oLerPacientes = new LerPacientes();
 
        //PACIENTE LER - GRAVAR - Sateliete > Integrador
        GravarPacientes m_oGravarPacientes = new GravarPacientes();
        #endregion

        #endregion

        public ETLMonitor()
        {
            InitializeComponent();
        }

        private void ETLMonitor_Load(object sender, EventArgs e)
        {
            timer1.Interval = (IntervaloMiliSeg * 60000); // 1 Seg = 1000 mils - 1 Min = 60 Seg então 60 * 1000 = 60000
            timer1.Enabled = false;

            btnProcessar.Text = "Iniciar ETL";

            this.WindowState = FormWindowState.Minimized;

            this.ShowInTaskbar = false;

            this.Text += " - Versão " + strVersao; 

            notifyIcon1.BalloonTipTitle = "ETL - Monitor";
            notifyIcon1.BalloonTipText = "ETL está Iniciado (Parado)...";

            notifyIcon1.ShowBalloonTip(0);

            ListaProcesso.Items.Clear();

            ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - Iniciado... ", false);

            Contador = 0;
            Conciliador = false;
            DataConciliador = DateTime.Now;

            //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Iniciado... \r\n";
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            if (btnProcessar.Text.Trim() == "Iniciar ETL")
            {
                //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - em Execução... \r\n";

                ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - em Execução... ",false);

                iniciarETLToolStripMenuItem.Enabled = false;
                pararETLToolStripMenuItem.Enabled = true; 
                btnProcessar.Text = "Parar ETL";
                notifyIcon1.BalloonTipTitle = "ETL - Monitor";
                notifyIcon1.BalloonTipText = "ETL em Execução...";
                notifyIcon1.ShowBalloonTip(0);  

                Inicio();

                timer1.Enabled = true; 
            }
            else 
            {
                //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Parado... \r\n";
                ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - Parado... ", true);

                btnProcessar.Text = "Iniciar ETL";
                iniciarETLToolStripMenuItem.Enabled = true;
                pararETLToolStripMenuItem.Enabled = false;
                timer1.Enabled = false;
             
                notifyIcon1.BalloonTipTitle = "ETL - Monitor";
                notifyIcon1.BalloonTipText = "ETL Parado!!!";

                notifyIcon1.ShowBalloonTip(0);  
            }
        }

        private void Inicio()
        {
            bool OcorreuError = false;

            string m_sUnidade = string.Empty;
            string m_sConexao = string.Empty;
            string m_sSchema = string.Empty;

            string Mensagem = string.Empty; 

            int m_iQtdeRegistros = 0;

            try 
            {
                DataSet Ds = m_oUnid.RetornarUnidades();

                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr = Ds.Tables[0].Rows[i];

                    m_sUnidade = Dr["DESC_PREFIXO"].ToString().Trim();
                    m_sConexao = Dr["DESC_CONEXAO_BANCO"].ToString().Trim();
                    m_sSchema = Dr["DESC_SCHEMA"].ToString().Trim();
                    m_iQtdeRegistros = Convert.ToInt16(Dr["QTDE_REGISTROS"].ToString().Trim());

                    //Ler Atendimentos
                    Mensagem = "Processando Atendimentos HIS para Satélite";
                    ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                    m_oLerAtendimentos.m_sUnidade = m_sUnidade;
                    m_oLerAtendimentos.strConnHIS = m_sConexao;
                    m_oLerAtendimentos.strScheHIS = m_sSchema;
                    m_oLerAtendimentos.ProcessarAtendimentosHIS(m_iQtdeRegistros);
                    Thread.Sleep(20000);
                    ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                    //Ler Pacientes
                    Mensagem = "Processando Pacientes HIS para Satélite";
                    ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                    m_oLerPacientes.m_sUnidade = m_sUnidade;
                    m_oLerPacientes.strConnHIS = m_sConexao;
                    m_oLerPacientes.strScheHIS = m_sSchema;
                    m_oLerPacientes.strConnSAT = strConnSAT;
                    m_oLerPacientes.strScheSAT = strScheSAT;
                    m_oLerPacientes.ProcessarPacientesHIS(m_iQtdeRegistros);
                    Thread.Sleep(20000);
                    ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                    //Gravar Atendimentos
                    Mensagem = "Processando Atendimentos Satélite para Integrador";
                    ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                    m_oGravarAtendimentos.mUnidade = m_sUnidade;
                    m_oGravarAtendimentos.ProcessarAtendimentosINT(m_iQtdeRegistros);
                    Thread.Sleep(20000);
                    ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                    //Gravar Pacientes
                    Mensagem = "Processando Pacientes Satélite para Integrador";
                    ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                    m_oGravarPacientes.m_sUnidade = m_sUnidade;
                    m_oGravarPacientes.strConnHIS = m_sConexao;
                    m_oGravarPacientes.strScheHIS = m_sSchema;
                    m_oGravarPacientes.ProcessarPacientesINT(m_iQtdeRegistros);
                    Thread.Sleep(20000);
                    ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                    //Apagar Tabelas Satélite
                    //Mensagem = "Limpando dados do Satélite.";
                    //HoraLimpeza = DateTime.Now;

                    //m_oUnid.ApagarAtendimentosSatelite(m_sUnidade);
                    //m_oUnid.ApagarPacientesSatelite(m_sUnidade);


                    Mensagem = string.Empty; 
                }

                //Conciliação

                if (DataConciliador.ToString("yyyyMMdd") != DateTime.Now.ToString("yyyyMMdd"))
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        DataRow Dr = Ds.Tables[0].Rows[i];

                        m_sUnidade = Dr["DESC_PREFIXO"].ToString().Trim();
                        m_sConexao = Dr["DESC_CONEXAO_BANCO"].ToString().Trim();
                        m_sSchema = Dr["DESC_SCHEMA"].ToString().Trim();
                        //m_iQtdeRegistros = Convert.ToInt16(Dr["QTDE_REGISTROS"].ToString().Trim());
                        m_iQtdeRegistros = 0;

                        //CONCILIANDO ATENDIMENTOS
                        Mensagem = "Conciliando Atendimentos HIS para Satélite";
                        ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);

                        m_oLerAtendimentos.m_sUnidade = m_sUnidade;
                        m_oLerAtendimentos.strConnHIS = m_sConexao;
                        m_oLerAtendimentos.strScheHIS = m_sSchema;
                        m_oLerAtendimentos.m_sConciliacaoDias = strConciDias;
                        m_oLerAtendimentos.ProcessarConciliacaoAtendimentosHIS(m_iQtdeRegistros, " ");
                        Thread.Sleep(10000);
                        ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                        //CONCILIANDO PACIENTES
                       Mensagem = "Conciliando Pacientes HIS para Satélite";
                        ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                        m_oLerPacientes.m_sUnidade = m_sUnidade;
                        m_oLerPacientes.strConnHIS = m_sConexao;
                        m_oLerPacientes.strScheHIS = m_sSchema;
                        m_oLerPacientes.strConnSAT = strConnSAT;
                        m_oLerPacientes.strScheSAT = strScheSAT;
                        m_oLerPacientes.m_sConciliacaoDias = strConciDias;
                        m_oLerPacientes.ProcessarConciliacaoPacientesHIS(m_iQtdeRegistros, " ");
                        Thread.Sleep(10000);
                        ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                        //Gravar Atendimentos
                        Mensagem = "Conciliando Atendimentos Satélite para Integrador";
                        ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                        m_oGravarAtendimentos.mUnidade = m_sUnidade;
                        m_oGravarAtendimentos.ProcessarAtendimentosINT(m_iQtdeRegistros);
                        Thread.Sleep(10000);
                        ListarItens(Mensagem, m_sUnidade, "Finalizado", false);

                        //Gravar Pacientes
                        Mensagem = "Conciliando Pacientes Satélite para Integrador";
                        ListarItens(Mensagem, m_sUnidade, "Em Andamento", false);
                        m_oGravarPacientes.m_sUnidade = m_sUnidade;
                        m_oGravarPacientes.strConnHIS = m_sConexao;
                        m_oGravarPacientes.strScheHIS = m_sSchema;
                        m_oGravarPacientes.ProcessarPacientesINT(m_iQtdeRegistros);
                        Thread.Sleep(10000);
                        ListarItens(Mensagem, m_sUnidade, "Finalizado", false);
                        

                        Mensagem = string.Empty;

                        DataConciliador = DateTime.Now; 
                    }
                }
            }
            catch (Exception ex)
            {
                OcorreuError = true;

                lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());

                m_oLog.CriarArquivoLog(Mensagem + " - " + ex.StackTrace, m_sUnidade);

                ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - " + Mensagem + " : " + ex.Message + "... ", true);
                ListarItens(Mensagem, m_sUnidade, "Erro!", true); 
 
            }

            if (!OcorreuError)
            {
                lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());
            }
        }

        #region ANTIGO
        //private void InicioXXX()
        //{

        //    bool OcorreuError = false;

        //    try
        //    {
        //        ProcessarLerAtendimentos("HBD");
        //        Thread.Sleep(10000);
        //    }
        //    catch (Exception ex)
        //    {
        //        OcorreuError = true;

        //        lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - LER ATENDIMENTO - ERROR " + ex.Message + " ... \r\n\r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Aguardando... \r\n\r\n";

        //        m_oLog.CriarArquivoLog("LER ATENDIMENTOS HIS " + ex.StackTrace, "HBD");

        //        ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - LER ATENDIMENTO - ERROR " + ex.Message + "... ", true);
        //    }

        //    try
        //    {

        //        ProcessarGravarAtendimentos("HBD");
        //        Thread.Sleep(10000);

        //    }
        //    catch (Exception ex)
        //    {
        //        OcorreuError = true;

        //        lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());

        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - GRAVAR ATENDIMENTO - ERROR " + ex.Message + " ... \r\n\r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Aguardando... \r\n\r\n";

        //        m_oLog.CriarArquivoLog("GRAVAR ATENDIMENTOS" + ex.Message, "HBD");

        //        ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - GRAVAR ATENDIMENTO - ERROR " + ex.Message + "... ", true);
        //    }

        //    try
        //    {

        //        ProcessarLerPacientes("HBD");
        //        Thread.Sleep(10000);

        //    }
        //    catch (Exception ex)
        //    {
        //        lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - LER PACIENTES - ERROR " + ex.Message + " ... \r\n\r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Aguardando... \r\n\r\n";

        //        ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - LER PACIENTES - ERROR " + ex.Message + "... ", true);

        //        OcorreuError = true;

        //        m_oLog.CriarArquivoLog("LER PACIENTES HIS " + ex.Message, "HBD");
        //    }

        //    try
        //    {
        //        ProcessarGravarPacientes("HBD");
        //        Thread.Sleep(10000);
            
        //    }
        //    catch (Exception ex)
        //    {
        //        OcorreuError = true;

        //        lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - GRAVAR PACIENTES - ERROR " + ex.Message + " ... \r\n\r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Aguardando... \r\n\r\n";

        //        ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - GRAVAR PACIENTES - ERROR " + ex.Message + "... ", true);

        //        m_oLog.CriarArquivoLog("GRAVAR PACIENTES " + ex.Message, "HBD");
        //    }

        //    if (! OcorreuError)
        //    {
        //        lblUltimaExecucao.Text = string.Format("{0:G}", DateTime.Now.ToString());
        //        //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Fim... \r\n\r\n";
        //       // Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Aguardando... \r\n\r\n";

        //        ListarItensControle(string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - Aguardando... ", true);

        //    }

        //}

        //private void ProcessarLerAtendimentos(string m_sUnidade)
        //{

        //    ListarItens("Processando Atendimentos HIS para Satélite", m_sUnidade, "Em Andamento", false);  

        //    //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Processando Atendimentos HIS >> Satélite... \r\n";

        //    try
        //    {
        //        oAtendimentosLer.StrConexao = strConnSAT;
        //        oAtendimentosLer.StrSchema = strScheSAT;
        //        oAtendimentosLer.ProcessarAtendimentosHIS(QtdeRegistros);

        //        ListarItens("Processando Atendimentos HIS para Satélite", m_sUnidade, "Finalizado", false);  

        //    }
        //    catch (Exception ex)
        //    {
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - ERROR - Processando Atendimentos HIS >> Satélite... \r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - " + ex.Message + " \r\n";

        //        ListarItens("Processando Atendimentos HIS para Satélite", m_sUnidade, ex.Message, true);
        //    }

        //}

        //private void ProcessarGravarAtendimentos(string m_sUnidade)
        //{
        //    //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Processando Atendimentos Satélite >> Integrador... \r\n";

        //    ListarItens("Processando Atendimentos Satélite para Integrador", m_sUnidade, "Em Andamento", false);  
            
        //    try
        //    {
        //        oAtendGravar.StrConexao = strConnINT;
        //        oAtendGravar.StrSchema = strScheINT;
        //        oAtendGravar.ProcessarAtendimentosINT(QtdeRegistros);

        //        ListarItens("Processando Atendimentos Satélite para Integrador", m_sUnidade, "Finalizado", false);  
        //    }
        //    catch (Exception ex)
        //    {
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - ERROR - Processando Atendimentos HIS >> Satélite... \r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - " + ex.Message + " \r\n";

        //        ListarItens("Processando Atendimentos Satélite para Integrador", m_sUnidade, ex.Message, true);
        //    }

        //}

        //private void ProcessarLerPacientes(string m_sUnidade)
        //{
        //    //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Processando Pacientes HIS >> Satélite... \r\n";

        //    ListarItens("Processando Pacientes HIS para Satélite", m_sUnidade, "Em Andamento", false);  

        //    try
        //    {
        //        oPacientes.StrConexao = strConnSAT;
        //        oPacientes.StrSchema = strScheSAT;
        //        oPacientes.ProcessarPacientesHIS(QtdeRegistros);

        //        ListarItens("Processando Pacientes HIS para Satélite", m_sUnidade, "Finalizado", false);  

        //    }
        //    catch (Exception ex)
        //    {
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - ERROR - Processando Pacientes HIS >> Satélite... \r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - " + ex.Message + " \r\n";

        //        ListarItens("Processando Pacientes HIS para Satélite", m_sUnidade, ex.Message, true);
        //    }

        //}

        //private void ProcessarGravarPacientes(string m_sUnidade)
        //{
        //    //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Processando Pacientes Satélite >> Integrador... \r\n";

        //    ListarItens("Processando Pacientes Satélite para Integrador", m_sUnidade, "Em Andamento", false);  

        //    try
        //    {
        //        oPacGravar.StrConexao = strConnINT;
        //        oPacGravar.StrSchema = strScheINT;
        //        oPacGravar.ProcessarPacientesINT(QtdeRegistros);

        //        ListarItens("Processando Pacientes Satélite para Integrador", m_sUnidade, "Finalizado", false);  

        //    }
        //    catch (Exception ex)
        //    {
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - ERROR - Processando Pacientes Satélite >> Integrador... \r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - " + ex.Message + " \r\n";

        //        ListarItens("Processando Pacientes Satélite para Integrador", m_sUnidade, ex.Message, true);
        //    }
        //}

        //private DataSet ObterPacientesOrfaos_ETL(string m_sUnidade)
        //{
        //    //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Processando Pacientes Satélite >> Integrador... \r\n";

        //    ListarItens("Analisando Pacientes Orfãos no Integrador", m_sUnidade, "Em Andamento", false);

        //    DataSet Ds = new DataSet(); 

        //    try
        //    {
        //        oPacGravar.StrConexao = strConnINT;
        //        oPacGravar.StrSchema = strScheINT;
        //        Ds = oPacGravar.ConciliarAtendimentoPacientesINT(QtdeRegistros);

        //        ListarItens("Analisando Pacientes Orfãos no Integrador", m_sUnidade, "Finalizado", false);

        //        return Ds;

        //    }
        //    catch (Exception ex)
        //    {
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - ETL - ERROR - Processando Pacientes Satélite >> Integrador... \r\n";
        //        //Texto.Text = Texto.Text + string.Format("{0:G}", DateTime.Now.ToString()) + " - " + ex.Message + " \r\n";

        //        ListarItens("Analisando Pacientes Orfãos no Integrador", m_sUnidade, ex.Message, true);

        //        return Ds;
        //    }
        //}

        //private void ConciliarPacientesHIS_ETL(string m_sUnidade, DataSet m_oDs)
        //{
        //    //Texto.Text = Texto.Text + string.Format("{0:G}",DateTime.Now.ToString()) + " - ETL - Processando Pacientes Satélite >> Integrador... \r\n";

        //    ListarItens("Analisando Pacientes do HIS para ETL", m_sUnidade, "Em Andamento", false);

        //    try
        //    {
        //        oPacientes.StrConexao = strConnSAT;
        //        oPacientes.StrSchema = strScheSAT;
        //        oPacientes.m_oDataSet = m_oDs;  
        //        oPacientes.ProcessarConciliacaoPacientesHIS(QtdeRegistros);

        //        ListarItens("Analisando Pacientes do HIS para ETL", m_sUnidade, "Finalizado", false);

        //    }
        //    catch (Exception ex)
        //    {

        //        ListarItens("Analisando Pacientes do HIS para ETL", m_sUnidade, ex.Message, true);
                
        //    }
        //}
        #endregion 

        private void timer1_Tick(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "ETL - Monitor";
            notifyIcon1.BalloonTipText = "O ETL está em Execução...";

            notifyIcon1.ShowBalloonTip(0);

            timer1.Enabled = false;

            Inicio();

            timer1.Enabled = true;
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Resposta = MessageBox.Show("Deseja finalizar execução do ETL ?","Mensagem do Sistema",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if (Resposta == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ETLMonitor_Resize(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;

            if (timer1.Enabled)
            {
                notifyIcon1.BalloonTipTitle = "ETL - Monitor";
                notifyIcon1.BalloonTipText = "O ETL Está em execução...";

                notifyIcon1.ShowBalloonTip(0);  
            }
            else
            {
                notifyIcon1.BalloonTipTitle = "ETL - Monitor";
                notifyIcon1.BalloonTipText = "O ETL está Parado...";

                notifyIcon1.ShowBalloonTip(0);  
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Aqui escondemos o notifyIcon:
            notifyIcon1.Visible = false;

            if (timer1.Enabled)
            {
                notifyIcon1.BalloonTipTitle = "ETL - Monitor";
                notifyIcon1.BalloonTipText = "O ETL está Iniciado...";

                notifyIcon1.ShowBalloonTip(0);
            }
            else
            {
                notifyIcon1.BalloonTipTitle = "ETL - Monitor";
                notifyIcon1.BalloonTipText = "O ETL está Parado...";

                notifyIcon1.ShowBalloonTip(0);
            }

            this.WindowState = FormWindowState.Normal;

            this.ShowInTaskbar = true;

        }

        private void iniciarETLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnProcessar.Text = "Parar ETL";
            pararETLToolStripMenuItem.Enabled = true;
            iniciarETLToolStripMenuItem.Enabled = false;
            timer1.Enabled = true;

            notifyIcon1.BalloonTipTitle = "ETL - Monitor";
            notifyIcon1.BalloonTipText = "O ETL está Iniciado...";

            notifyIcon1.ShowBalloonTip(0);  
        }

        private void pararETLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnProcessar.Text = "Iniciar ETL";
            pararETLToolStripMenuItem.Enabled = false;
            iniciarETLToolStripMenuItem.Enabled = true;
            timer1.Enabled = false;
            
            notifyIcon1.BalloonTipTitle = "ETL - Monitor";
            notifyIcon1.BalloonTipText = "O ETL está Parado...";

            notifyIcon1.ShowBalloonTip(0);  
        }

        public void ListarItens(string Processo,string Unidade,string Observacao, Boolean Alerta)
        {

            if (ListaProcesso.Items.Count >= 1000)
            {
                Contador = 0;
                ListaProcesso.Items.Clear();   
            }
            
            ListViewItem Item = new ListViewItem();

            Contador += 1;

            // Item.Text = String.Format("{0:D10}","0" + Contador.ToString());
            // Item.SubItems.Add(string.Format("{0:G}", DateTime.Now.ToString()));
            Item.Text = string.Format("{0:G}", DateTime.Now.ToString());
            Item.SubItems.Add(Unidade);
            Item.SubItems.Add(Processo);
            Item.SubItems.Add(Observacao);
            Item.Focused = true; 

            if (Alerta)
            {
                Item.BackColor = Color.DarkRed;
                Item.ForeColor = Color.LightYellow;
            }
            else
            {
                Item.BackColor = Color.White;
                Item.ForeColor = Color.Black;
            }

            ListaProcesso.Items.Add(Item);
           
        }

        public void ListarItensControle(string Texto, Boolean Alerta)
        {
            if (ListaControle.Items.Count >= 1000)
            {
                ListaControle.Items.Clear();
            }
            
            ListaControle.Items.Add(Texto); 
        }

        //public DataSet RetornarUnidades()
        //{
        //    DataSet Ds;

        //    StringBuilder sbSQL = new System.Text.StringBuilder();

        //    sbSQL.Length = 0;
        //    sbSQL.Append("SELECT ");
        //    sbSQL.Append("  IDETLUNID,");
        //    sbSQL.Append("  DESC_UNIDADE,");
        //    sbSQL.Append("  DESC_CONEXAO_BANCO,");
        //    sbSQL.Append("  QTDE_REGISTROS,");
        //    sbSQL.Append("  DESC_PREFIXO,");
        //    sbSQL.Append("  DESC_SCHEMA ");
        //    sbSQL.Append("FROM");
        //    sbSQL.Append("  #0.TBETLUNIDADES ");
        //    sbSQL.Append("WHERE");
        //    sbSQL.Append("  ATIVO = 'A'");

        //    try
        //    {
        //        sbSQL.Replace("#0", strScheINT);
        //        Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "ETL", strConnINT);

        //        return Ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public void ApagarPacientesSatelite(string m_sUnidade)
        //{
        //    int Retorno;

        //    StringBuilder sbSQL = new System.Text.StringBuilder();

        //    sbSQL.Length = 0;

        //    sbSQL.Append(" Delete ");
        //    sbSQL.Append(" From ");
        //    sbSQL.Append(" #0.PACIENTE ");
        //    sbSQL.Append(" Where  ");
        //    sbSQL.Append(" cod_prt in (SELECT SUBSTR(CLPESS,10,10) FROM #1.TBDWD004 WHERE SUBSTR(CLPESS,1,3) = '#2')");
        //    sbSQL.Append(" AND");
        //    sbSQL.Append(" CDUND = '#2'");
        //    sbSQL.Append(" AND ");
        //    sbSQL.Append(" DTCARG < '" + DateTime.Now.ToString("dd/MM/yyyy") + "'");

        //    try
        //    {
        //        sbSQL.Replace("#0", strScheSAT);
        //        sbSQL.Replace("#1", strScheINT);
        //        sbSQL.Replace("#2", m_sUnidade);
        //        Retorno = m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ETL", strConnINT);
            
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public void ApagarAtendimentosSatelite(string m_sUnidade)
        //{
        //    int Retorno;

        //    StringBuilder sbSQL = new System.Text.StringBuilder();

        //    sbSQL.Length = 0;

        //    sbSQL.Append(" Delete ");
        //    sbSQL.Append(" From ");
        //    sbSQL.Append(" #0.ATENDIMENTO ");
        //    sbSQL.Append(" Where  ");
        //    sbSQL.Append(" cod_pac in (SELECT SUBSTR(CLATND,10,10) FROM #1.TBDWD018 WHERE SUBSTR(CLATND,1,3) = '#2')");
        //    sbSQL.Append(" AND");
        //    sbSQL.Append(" CDUND = '#2'");
        //    sbSQL.Append(" AND ");
        //    sbSQL.Append(" DTCARG < '" + DateTime.Now.ToString("dd/MM/yyyy") + "'");

        //    try
        //    {
        //        sbSQL.Replace("#0", strScheSAT);
        //        sbSQL.Replace("#1", strScheINT);
        //        sbSQL.Replace("#2", m_sUnidade);
        //        Retorno = m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ETL", strConnINT);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        private void sairToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
