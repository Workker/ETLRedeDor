using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using WSLerAtendimentos;
using AcessoDados;  


namespace WSLerAtendimentos
{
    /// <summary>
    /// Ler todos Atendimentos Cadastrados no Sistema WPD
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WSLerAtendimentos : System.Web.Services.WebService
    {
        rpLerDados m_oRP = new rpLerDados();

        //Versão Antiga
        //string strConnHIS = ConfigurationManager.AppSettings["ConectaHIS"].ToString();
        //string strScheHIS = ConfigurationManager.AppSettings["strSchemaHIS"].ToString();
        //string m_sUnidade = string.Empty;
      
        string strConnSAT = ConfigurationManager.AppSettings["ConectaSAT"].ToString();
        string strScheSAT = ConfigurationManager.AppSettings["strSchemaSAT"].ToString();

        DataSet m_oDataSet;
   
        public WSLerAtendimentos()
        { 
        
        }

        [WebMethod]
        [CatchException]
        public string RetornarUltimoRegistro(string str_Unidade)
        {
            string Resposta = string.Empty;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;
            // ORIGINAL // sbSQL.Append("SELECT COD_PAC AS ULTIMOVALOR FROM (SELECT COD_PAC FROM #0.ATENDIMENTO WHERE CDUND = '#1' ORDER BY DATA_ENT DESC, COD_PAC DESC) WHERE ROWNUM = 1");
          
            sbSQL.Append("SELECT COD_PAC || ';' || DATA_ENT AS ULTIMOVALOR FROM (SELECT COD_PAC,DATA_ENT FROM #0.ATENDIMENTO WHERE CDUND = '#1' ORDER BY DATA_ENT DESC, COD_PAC DESC) WHERE ROWNUM = 1");

            sbSQL.Replace("#0", strScheSAT);
            sbSQL.Replace("#1", str_Unidade.ToUpper());

            try
            {
                Resposta = m_oRP.RetornarConsulta(sbSQL.ToString(), "ATENDIMENTO", strConnSAT);

                return Resposta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [WebMethod]
        [CatchException]
        public DataSet RetornarAtendimentos(string UltimoRegistro, Int32 NumeroLinhas, string m_sConexao, string m_sSchema)
        {
            DataSet Ds;

            string[] xUltmoRegistro;

            xUltmoRegistro = UltimoRegistro.Split(';');  

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            if (NumeroLinhas == 0)
            {
                NumeroLinhas = 1000;
            }

            //ANTIGO = 30/05/2012
            //sbSQL.Append(" SELECT ");
            //sbSQL.Append("    cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp,leito ");
            //sbSQL.Append(" FROM #0.fapaccad  ");
            //sbSQL.Append(" WHERE cod_pac>'#1' AND data_ent >= '#3'");
            //sbSQL.Append(" AND ");
            //sbSQL.Append(" ROWNUM <= #2 ");
            //sbSQL.Append(" ORDER BY cod_pac ");

            sbSQL.Append(" SELECT ");
            sbSQL.Append("    cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp,leito ");
            sbSQL.Append(" FROM #0.fapaccad  ");
            sbSQL.Append(" WHERE ");
            sbSQL.Append(" TIPO_PAC = 'I' AND  DATA_ALTA IS NULL ");
            sbSQL.Append(" ORDER BY cod_pac ");

            //ORIGINAL // sbSQL.Replace("#1", UltimoRegistro);
            //ANTIGO = 30/05/2012 // sbSQL.Replace("#1", xUltmoRegistro[0].ToString().Trim());
            //ANTIGO = 30/05/2012 // sbSQL.Replace("#2", NumeroLinhas.ToString().Trim());
            //ANTIGO = 30/05/2012 // sbSQL.Replace("#3", xUltmoRegistro[1].ToString().Trim());

            sbSQL.Replace("#0", m_sSchema);

            try
            {
                Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAPACCAD", m_sConexao);

                return Ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [WebMethod]
        [CatchException]
        public DataSet RetornarAtendimentoConciliacao(string m_DataDe, string m_sConexao, string m_sSchema)
        {
            DataSet Ds;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            sbSQL.Append(" SELECT ");
            sbSQL.Append("    cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp,leito ");
            sbSQL.Append(" FROM #0.fapaccad  ");
            sbSQL.Append(" WHERE data_ent>='#1' ");
            sbSQL.Append(" ORDER BY cod_pac ");

            sbSQL.Replace("#0", m_sSchema);
            sbSQL.Replace("#1", m_DataDe);

            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAPACCAD", m_sConexao);

            return Ds;
        }

        [WebMethod]
        [CatchException]
        public Boolean GravarAtendimento(string m_strUnidade, DataSet m_oDsDados)
        {
            try
            {
                m_oDataSet = m_oDsDados;

                Boolean Resposta = Salvar(m_strUnidade);

                return Resposta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected Boolean Salvar(string m_strUnidade)
        {
            Boolean Retorno = false;

            //try
            //{
                for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

                    StringBuilder sbSQL = new System.Text.StringBuilder();

                    sbSQL.Length = 0;

                    try
                    {
                        if (!Dr0.IsNull(5))
                        {
                            sbSQL.Append(" INSERT INTO #0.ATENDIMENTO(cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp, dtcarg, cdund, leito)");
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
                            sbSQL.Append(",'" + m_strUnidade.Trim() + "'");
                            sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                            sbSQL.Append(")");
                        }
                        else
                        {
                            sbSQL.Append(" INSERT INTO #0.ATENDIMENTO(cod_pac, cod_prt, tip_atend, data_ent, hora_ent, cod_pro, cod_esp, dtcarg, cdund,leito)");
                            sbSQL.Append(" VALUES(");
                            sbSQL.Append(" '");
                            sbSQL.Append(Dr0[0].ToString());
                            sbSQL.Append("','" + Dr0[1].ToString() + "'");
                            sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                            sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                            sbSQL.Append(",TO_DATE('" + Dr0[4].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                            sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                            sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                            sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            sbSQL.Append(",'" + m_strUnidade.Trim() + "'");
                            sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                            sbSQL.Append(")");
                        }

                        //sbSQL.Append(" INSERT INTO #0.ATENDIMENTO(cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp, dtcarg, cdund)");
                        //sbSQL.Append(" VALUES(");
                        //sbSQL.Append(" '");
                        //sbSQL.Append(Dr0[0].ToString());
                        //sbSQL.Append("','" + Dr0[1].ToString() + "'");
                        //sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                        //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                        //sbSQL.Append(",TO_DATE('" + Dr0[4].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                        //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[5].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                        //sbSQL.Append(",TO_DATE('" + Dr0[6].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                        //sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                        //sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                        //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                        //sbSQL.Append(",'" + m_strUnidade.Trim() + "'");
                        //sbSQL.Append(")");

                        sbSQL.Replace("#0", strScheSAT);

                        m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ATENDIMENTO", strConnSAT);

                    }
                    catch (Exception en)
                    {
                        //throw;
                    }
                    finally { }
                }

                return Retorno;
            //}
            //catch (Exception en)
            //{
            //    throw new Exception(en.Message);
            //}
        }

    }
}
