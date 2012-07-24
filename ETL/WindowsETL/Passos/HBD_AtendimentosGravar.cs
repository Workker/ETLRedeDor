using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AcessoDados;
using WindowsETL.SrvGravarAtendHBD;

namespace WindowsETL.Passos
{
    class HBD_AtendimentosGravar
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
        public void ProcessarAtendimentosINT(Int32 m_iQtdeRegistros)
        {
            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvGravarAtendHBD.WSGravarAtendimentosSoapClient DadosOrigens = new WSGravarAtendimentosSoapClient();

            //Obtem o Ultimo Registro
            m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

            //Obtem as Informações 
            DsDados = DadosOrigens.RetornarAtendimentos(mUnidade, m_sUltimoRegistro, m_iQtdeRegistros);

            //Gravar no INTEGRADOR
            DadosOrigens.GravarAtendimento(mUnidade, DsDados);

            //for (int i = 0; i < DsDados.Tables[0].Rows.Count; i++)
            //{
            //    DataRow Dr0 = DsDados.Tables[0].Rows[i];

            //    DataSet Ds = new DataSet();
            //    DataTable Dt = new DataTable(DsDados.Tables[0].TableName);

            //    Dt = DsDados.Tables[0].Clone();

            //    Dt.ImportRow(Dr0);

            //    Ds.Tables.Add(Dt);

            //    //DadosOrigens.GravarAtendimento(mUnidade, Ds);

            //    m_oDataSet = Ds;

            //    Salvar();
            //}

        }

       

    }
}
