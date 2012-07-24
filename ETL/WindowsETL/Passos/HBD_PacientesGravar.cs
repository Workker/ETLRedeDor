using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AcessoDados;
using WindowsETL.SrvGravarPacHBD;

namespace WindowsETL.Passos
{
    class HBD_PacientesGravar
    {

        //Sigla UNIDADE
        const string mUnidade = "HBD";

        DataSet m_oDataSet;
        string m_sUnidade = mUnidade;

        rpLerDados m_oRP = new rpLerDados();

        public string StrConexao
        {
            get;
            set;
        }

        public string StrSchema
        {
            get;
            set;
        }

        /// <summary>
        /// Obtem todos atendimentos a partir do Ultimo Registro.
        /// </summary>
        /// <param name="m_iQtdeRegistros">Quantidade de Registro</param>
        /// <returns></returns>
        public void ProcessarPacientesINT(Int32 m_iQtdeRegistros)
        {
            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvGravarPacHBD.WSGravarPacientesSoapClient DadosOrigens = new WSGravarPacientesSoapClient(); 

            //Obtem o Ultimo Registro
            m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

            //Obtem as Informações 
            m_oDataSet = DadosOrigens.RetornarPacientes(mUnidade, m_sUltimoRegistro, m_iQtdeRegistros);

            //Gravar no DWSATELITE
            DadosOrigens.GravarPacientes(mUnidade, m_oDataSet);  
           // Salvar();
        }

        public DataSet ConciliarAtendimentoPacientesINT(Int32 m_iQtdeRegistros)
        {
            DataSet DsDados = new DataSet();

            SrvGravarPacHBD.WSGravarPacientesSoapClient DadosOrigens = new WSGravarPacientesSoapClient();

            //Obtem as Informações 
            DsDados = DadosOrigens.RetornarConciliacaoPacientes(mUnidade, m_iQtdeRegistros);

            return DsDados;
        }

        public void ProcessarPacientesConciliacao(DataSet m_oDs,Int32 m_iQtdeRegistros)
        {
            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvGravarPacHBD.WSGravarPacientesSoapClient DadosOrigens = new WSGravarPacientesSoapClient();

            //Obtem o Ultimo Registro
            m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

            //Obtem as Informações 
           // m_oDataSet = DadosOrigens.RetornarConciliacaoPacientes(mUnidade, m_sUltimoRegistro, m_iQtdeRegistros);

            //Gravar no DWSATELITE
            DadosOrigens.GravarPacientes(mUnidade, m_oDataSet);
            // Salvar();
        }


    }
}
