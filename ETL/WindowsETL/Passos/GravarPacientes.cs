using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AcessoDados;
using WindowsETL.SrvGravarPacHBD;

namespace WindowsETL.Passos
{
    class GravarPacientes
    {
        #region Variáveis

        public string strConnHIS { get; set; }
        public string strScheHIS { get; set; }
        public string m_sUnidade { get; set; }

        public int m_iQtdeRegistros { get; set; }

        DataSet m_oDataSet;

        rpLerDados m_oRP = new rpLerDados();

        ClDadosLOG m_oLog = new ClDadosLOG();

        #endregion

        #region Métodos
        public void ProcessarPacientesINT(Int32 m_iQtdeRegistros)
        {
            try
            {
                string m_sUltimoRegistro;
                DataSet DsDados = new DataSet();

                SrvGravarPacHBD.WSGravarPacientesSoapClient DadosOrigens = new WSGravarPacientesSoapClient();

                //Obtem o Ultimo Registro
                m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(m_sUnidade);

                //Obtem as Informações 
                m_oDataSet = DadosOrigens.RetornarPacientes(m_sUnidade, m_sUltimoRegistro, m_iQtdeRegistros);

                //Gravar no DWSATELITE
                DadosOrigens.GravarPacientes(m_sUnidade, m_oDataSet);
                // Salvar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public DataSet ConciliarAtendimentoPacientesINT(Int32 m_iQtdeRegistros)
        {
            try
            {
                DataSet DsDados = new DataSet();

                SrvGravarPacHBD.WSGravarPacientesSoapClient DadosOrigens = new WSGravarPacientesSoapClient();

                //Obtem as Informações 
                DsDados = DadosOrigens.RetornarConciliacaoPacientes(m_sUnidade, m_iQtdeRegistros);

                return DsDados;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void ProcessarPacientesConciliacao(DataSet m_oDs, Int32 m_iQtdeRegistros)
        {
            try
            {
                string m_sUltimoRegistro;
                DataSet DsDados = new DataSet();

                SrvGravarPacHBD.WSGravarPacientesSoapClient DadosOrigens = new WSGravarPacientesSoapClient();

                //Obtem o Ultimo Registro
                m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(m_sUnidade);

                //Obtem as Informações 
                // m_oDataSet = DadosOrigens.RetornarConciliacaoPacientes(mUnidade, m_sUltimoRegistro, m_iQtdeRegistros);

                //Gravar no DWSATELITE
                DadosOrigens.GravarPacientes(m_sUnidade, m_oDataSet);
                // Salvar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}
