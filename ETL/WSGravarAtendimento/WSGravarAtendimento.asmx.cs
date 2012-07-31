using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using AcessoDados; 

namespace WSGravarAtendimento
{
    /// <summary>
    /// Gravar Atendimento da Unidade no DW
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
 
    public class WSGravarAtendimento : System.Web.Services.WebService
    {
        
        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConnINT = ConfigurationManager.AppSettings["ConectaINT"].ToString();
        string strScheINT = ConfigurationManager.AppSettings["strSchemaINT"].ToString();
        string m_sUnidade = string.Empty;

        string strConnSAT = ConfigurationManager.AppSettings["ConectaSAT"].ToString();
        string strScheSAT = ConfigurationManager.AppSettings["strSchemaSAT"].ToString();

        DataSet m_oDataSet;

        [WebMethod]
        [CatchException]
        public string RetornarUltimoRegistro(string str_Unidade)
        {
            string Resposta = string.Empty;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;
            sbSQL.Append("SELECT RTRIM(SUBSTR(CLATND,10,10)) AS ULTIMOVALOR FROM (SELECT CLATND FROM #0.TBDWD018 WHERE SUBSTR(CLATND,1,3) = '#1' ) ");

            sbSQL.Replace("#0", strScheINT);
            sbSQL.Replace("#1", str_Unidade.ToUpper());

            Resposta = m_oRP.RetornarConsulta(sbSQL.ToString(), "ATENDIMENTO", strConnINT);

            return Resposta;
        }

        [WebMethod]
        [CatchException]
        public DataSet RetornarAtendimentos(string str_Unidade, string UltimoRegistro, Int32 NumeroLinhas)
        {
            DataSet Ds;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            if (NumeroLinhas == 0)
            {
                NumeroLinhas = 1000;
            }

            sbSQL.Append(" SELECT ");
            sbSQL.Append("    cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp, leito ");
            sbSQL.Append(" FROM #0.atendimento  ");
            //sbSQL.Append(" WHERE cdund = '#1' AND cod_pac>'#2' ");
            sbSQL.Append(" WHERE cdund = '#1' AND cod_pac NOT IN (SELECT TRIM(SUBSTR(CLATND,10,10)) FROM #4.TBDWD018 WHERE SUBSTR(CLATND,1,3) = '#1') ");
            sbSQL.Append(" AND ");
            sbSQL.Append(" ROWNUM <= #3 ");
            sbSQL.Append(" ORDER BY cod_pac ");

            sbSQL.Replace("#0", strScheSAT);
            sbSQL.Replace("#1", str_Unidade);
            //sbSQL.Replace("#2", UltimoRegistro);
            sbSQL.Replace("#3", NumeroLinhas.ToString().Trim());
            sbSQL.Replace("#4", strScheINT);

            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "ATENDIMENTO", strConnSAT);

            return Ds;
        }

        [WebMethod]
        [CatchException]
        public Boolean GravarAtendimento(string m_strUnidade, DataSet m_oDsDados)
        {
            Boolean Resposta = false;
            
            m_oDataSet = m_oDsDados;
            m_sUnidade = m_strUnidade; 
            
            Resposta = Salvar();

            return Resposta;
        }

        protected Boolean Salvar()
        {
            Boolean Retorno = false;
            
            //try
            //{
            string V_PREFIXO =  m_sUnidade.Trim().ToUpper() + "WPDATE";
            string V_PREFIXO_Paciente = m_sUnidade.Trim().ToUpper() + "WPDPAC";
            string V_PREFIXO_Medico = m_sUnidade.Trim().ToUpper() + "WPDMED";
            string V_PREFIXO_Especialidade = m_sUnidade.Trim().ToUpper() + "WPDESP"; 
                
            int str_TipoAtendimento = 0;

            for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
            {
                DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

                str_TipoAtendimento = 1;

                switch (Dr0[2].ToString().Trim())
                {
                    case "I":
                        str_TipoAtendimento = 1;
                        break;
                    case "L":
                        str_TipoAtendimento = 2;
                        break;
                    case "M":
                        str_TipoAtendimento = 3;
                        break;
                    case "P":
                        str_TipoAtendimento = 4;
                        break;
                }

                sbSQL.Length = 0;

                try
                {
                    if (!Dr0.IsNull(3) && !Dr0.IsNull(4))
                    {
                        if (!Dr0.IsNull(5))
                        {
                            // hralta,
                            sbSQL.Append(" INSERT INTO #0.tbdwd018(clatnd, clpacn, iddwd017, dtatnd, hratnd, dtalta, clmedc, clespc, dsleito, dtcarg)");
                            sbSQL.Append(" VALUES (");
                            sbSQL.Append("'" + V_PREFIXO + Dr0[0].ToString() + "'");
                            sbSQL.Append(",'" + V_PREFIXO_Paciente + Dr0[1].ToString() + "'");
                            sbSQL.Append(",");
                            sbSQL.Append(str_TipoAtendimento);
                            sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                            sbSQL.Append(",TO_DATE('" + Dr0[4].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                            sbSQL.Append(",'" + Convert.ToDateTime(Dr0[5].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                            //sbSQL.Append(",TO_DATE('" + Dr0[6].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                            //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[6].ToString()).ToString("HH:mm") + "'");
                            sbSQL.Append(",'" + V_PREFIXO_Medico + Dr0[7].ToString() + "'");
                            sbSQL.Append(",'" + V_PREFIXO_Especialidade + Dr0[8].ToString() + "'");
                            sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                            sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            sbSQL.Append(")");
                        }
                        else
                        {
                            sbSQL.Append(" INSERT INTO #0.tbdwd018(clatnd, clpacn, iddwd017, dtatnd, hratnd, clmedc, clespc, dsleito, dtcarg)");
                            sbSQL.Append(" VALUES (");
                            sbSQL.Append("'" + V_PREFIXO + Dr0[0].ToString() + "'");
                            sbSQL.Append(",'" + V_PREFIXO_Paciente + Dr0[1].ToString() + "'");
                            sbSQL.Append(",");
                            sbSQL.Append(str_TipoAtendimento);
                            sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                            sbSQL.Append(",TO_DATE('" + Dr0[4].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                            sbSQL.Append(",'" + V_PREFIXO_Medico + Dr0[7].ToString() + "'");
                            sbSQL.Append(",'" + V_PREFIXO_Especialidade + Dr0[8].ToString() + "'");
                            sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                            sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            sbSQL.Append(")");
                        }

                        //sbSQL.Append(" INSERT INTO #0.tbdwd018(clatnd, clpacn, iddwd017, dtatnd, hratnd, dtalta, hralta, clmedc, clespc, dtcarg)");
                        //sbSQL.Append(" VALUES (");
                        //sbSQL.Append("'" + V_PREFIXO + Dr0[0].ToString() + "'");
                        //sbSQL.Append(",'" + V_PREFIXO_Paciente + Dr0[1].ToString() + "'");
                        //sbSQL.Append(",");
                        //sbSQL.Append(str_TipoAtendimento);
                        //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                        //sbSQL.Append(",TO_DATE('" + Dr0[4].ToString() + "','DD/MM/YYYY HH24:MI:SS')");

                        //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[5].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                        ////sbSQL.Append(",TO_DATE('" + Dr0[6].ToString() + "','DD/MM/YYYY HH24:MI:SS')");
                        //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[6].ToString()).ToString("HH:mm") + "'");

                        //sbSQL.Append(",'" + V_PREFIXO_Medico + Dr0[7].ToString() + "'");
                        //sbSQL.Append(",'" + V_PREFIXO_Especialidade + Dr0[8].ToString() + "'");
                        //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                        //sbSQL.Append(")");

                        sbSQL.Replace("#0", strScheINT);

                        Int32 RetornoLinhas =Convert.ToInt32( m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ATENDIMENTO", strConnINT));

                        if (RetornoLinhas > 0)
                        {
                            Retorno = true;
                        }
                    }
                }
                catch (Exception en)
                {
                    //// throw new Exception(en.Message);
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
