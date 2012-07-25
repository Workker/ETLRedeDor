using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AcessoDados;
using WindowsETL.SrvAtendHBD;

namespace WindowsETL.Passos
{
    class LerAtendimentos
    {

        #region Variáveis

        public string strConnHIS { get; set; }
        public string strScheHIS { get; set; }
        public string m_sUnidade { get; set; }
        public string m_sConciliacaoDias { get; set; }

        public int m_iQtdeRegistros { get; set; }

        DataSet m_oDataSet;

        rpLerDados m_oRP = new rpLerDados();

        ClDadosLOG m_oLog = new ClDadosLOG();
        
        #endregion

        #region Métodos

        public Boolean ProcessarAtendimentosHIS(Int32 m_iQtdeRegistros)
        {
            Boolean Retorno = false;

            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvAtendHBD.WSLerAtendimentosSoapClient DadosOrigens = new WSLerAtendimentosSoapClient();
            
            try
            {
                //Obtem o Ultimo Registro
                m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(m_sUnidade);

                //Obtem as Informações 
                DsDados = DadosOrigens.RetornarAtendimentos(m_sUltimoRegistro, m_iQtdeRegistros, strConnHIS, strScheHIS);

                //Gravar no DWSATELITE
                for (int i = 0; i < DsDados.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = DsDados.Tables[0].Rows[i];

                    DataSet Ds = new DataSet();
                    DataTable Dt = new DataTable(DsDados.Tables[0].TableName);

                    Dt = DsDados.Tables[0].Clone();

                    Dt.ImportRow(Dr0);

                    Ds.Tables.Add(Dt);

                    Retorno = DadosOrigens.GravarAtendimento(m_sUnidade, Ds);
                }

                return Retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Boolean  ProcessarConciliacaoAtendimentosHIS(Int32 m_iQtdeRegistros,string m_sData)
        {
            try
            {
                Boolean Retorno = false;

                DataSet DsDados = new DataSet();

                string m_sUltimoRegistro;

                SrvAtendHBD.WSLerAtendimentosSoapClient DadosOrigens = new WSLerAtendimentosSoapClient();

                if (m_sData.Trim().Length == 0)
                {
                    //Obtem o Ultimo Registro
                    m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(m_sUnidade);

                    string[] xUltmoRegistro;

                    xUltmoRegistro = m_sUltimoRegistro.Split(';');

                    m_sData = xUltmoRegistro[1].ToString();  
                }

                //m_sConciliacaoDias

                DateTime Dt01 = Convert.ToDateTime(m_sData);

                Dt01 = Dt01.AddDays(-Convert.ToInt16(m_sConciliacaoDias));

                //Obtem as Informações 
                DsDados = DadosOrigens.RetornarAtendimentoConciliacao(Dt01.ToString("dd/MM/yyyy"), strConnHIS, strScheHIS);

                //Gravar no DWSATELITE
                for (int i = 0; i < DsDados.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = DsDados.Tables[0].Rows[i];

                    DataSet Ds = new DataSet();
                    DataTable Dt = new DataTable(DsDados.Tables[0].TableName);

                    Dt = DsDados.Tables[0].Clone();

                    Dt.ImportRow(Dr0);

                    Ds.Tables.Add(Dt);

                    Retorno = DadosOrigens.GravarAtendimento(m_sUnidade, Ds);
                }

                return Retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
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

                    Int32 LinhasRetorno = 0;

                    try
                    {

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

                        sbSQL.Replace("#0", strScheHIS);

                        LinhasRetorno =Convert.ToInt32( m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ATENDIMENTO", strConnHIS));
                    }
                    catch
                    {}

                    if (LinhasRetorno > 0)
                    {
                        Retorno = true;
                    }
                }

                return Retorno;
            }
            catch (Exception en)
            {
                throw new Exception(en.Message);
            }

        }

        #endregion

    }
}
