using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Text;
using AcessoDados;
using WindowsETL.SrvAtendHBD;

namespace WindowsETL.Passos
{
    class HBD_Atendimentos
    {
        //to_char(hora_ent, 'HH24:MI')
        //select to_char(hora_ent, 'HH24:MI'),to_char(hora_alta, 'HH24:MI') from atendimento

        //Sigla UNIDADE
        const string mUnidade = "HBD";
       
        DataSet m_oDataSet;
        string m_sUnidade = mUnidade;

        rpLerDados m_oRP = new rpLerDados();

        ClDadosLOG m_oLog = new ClDadosLOG();

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
        public Boolean ProcessarAtendimentosHIS(Int32 m_iQtdeRegistros)
        {
            Boolean Retorno = false;

            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvAtendHBD.WSLerAtendimentosSoapClient DadosOrigens = new WSLerAtendimentosSoapClient();

            try
            {

                //Obtem o Ultimo Registro
                m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

                //Obtem as Informações 
                //DsDados = DadosOrigens.RetornarAtendimentos(m_sUltimoRegistro, m_iQtdeRegistros);    
                //m_oDataSet = DadosOrigens.RetornarAtendimentos(m_sUltimoRegistro, m_iQtdeRegistros);

                //Gravar no DWSATELITE
                //m_oDataSet = DsDados;

                //Retorno = Salvar();

                //Retorno = DadosOrigens.GravarAtendimento(mUnidade, m_oDataSet);

                for (int i = 0; i < DsDados.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = DsDados.Tables[0].Rows[i];

                    DataSet Ds = new DataSet();
                    DataTable Dt = new DataTable(DsDados.Tables[0].TableName);

                    Dt = DsDados.Tables[0].Clone();

                    Dt.ImportRow(Dr0);

                    Ds.Tables.Add(Dt);

                    Retorno = DadosOrigens.GravarAtendimento(mUnidade, Ds);
 
                    //m_oDataSet = Ds;

                    //Retorno = Salvar();
                }

            }
            catch (Exception ex)
            {
                m_oLog.CriarArquivoLog("ProcessarAtendimentosHIS " + ex.StackTrace , "HBD");
            }

            return Retorno;
        }

        protected Boolean Salvar()
        {
            Boolean Retorno = false;

            try
            {
                for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

                    StringBuilder sbSQL = new System.Text.StringBuilder();

                    sbSQL.Append(" INSERT INTO #0.ATENDIMENTO(cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp, dtcarg, cdund)");
                    sbSQL.Append(" VALUES(");
                    sbSQL.Append(" '");
                    sbSQL.Append(Dr0[0].ToString());
                    sbSQL.Append("','" + Dr0[1].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",TO_DATE('" + Dr0[4].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[5].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",TO_DATE('" + Dr0[6].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                    sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + m_sUnidade.Trim() + "'");
                    sbSQL.Append(")");

                    sbSQL.Replace("#0", StrSchema);  

                    Int32 LinhasRetorno =Convert.ToInt32( m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ATENDIMENTO", StrConexao));

                    if (LinhasRetorno > 0)
                    {
                        Retorno = true;
                    }
                }

                return Retorno;
            }
            catch (Exception en)
            {
                return Retorno;
            }

        }

    }
}
