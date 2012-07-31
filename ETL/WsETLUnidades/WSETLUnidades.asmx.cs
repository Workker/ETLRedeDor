using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Configuration;
using System.Data;
using AcessoDados;

namespace WsETLUnidades
{
    /// <summary>
    /// ETL - Ler Informações de Unidades
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WSETLUnidades : System.Web.Services.WebService
    {
        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConnINT = ConfigurationManager.AppSettings["ConectaINT"].ToString();
        string strScheINT = ConfigurationManager.AppSettings["strSchemaINT"].ToString();

        string strConnSAT = ConfigurationManager.AppSettings["ConectaSAT"].ToString();
        string strScheSAT = ConfigurationManager.AppSettings["strSchemaSAT"].ToString();

        string m_sUnidade = string.Empty;


        [WebMethod]
        [CatchException]
        public DataSet RetornarUnidades()
        {
            DataSet Ds;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;
            sbSQL.Append("SELECT ");
            sbSQL.Append("  IDETLUNID,");
            sbSQL.Append("  DESC_UNIDADE,");
            sbSQL.Append("  DESC_CONEXAO_BANCO,");
            sbSQL.Append("  QTDE_REGISTROS,");
            sbSQL.Append("  DESC_PREFIXO,");
            sbSQL.Append("  DESC_SCHEMA ");
            sbSQL.Append("FROM");
            sbSQL.Append("  #0.TBETLUNIDADES ");
            sbSQL.Append("WHERE");
            sbSQL.Append("  ATIVO = 'A'");

            sbSQL.Replace("#0", strScheINT);
            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "ETL", strConnINT);

            return Ds;

        }

        [WebMethod]
        [CatchException]
        public void ApagarPacientesSatelite(string m_sUnidade)
        {
            int Retorno;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            sbSQL.Append(" Delete ");
            sbSQL.Append(" From ");
            sbSQL.Append(" #0.PACIENTE ");
            sbSQL.Append(" Where  ");
            sbSQL.Append(" cod_prt in (SELECT SUBSTR(CLPESS,10,10) FROM #1.TBDWD004 WHERE SUBSTR(CLPESS,1,3) = '#2')");
            sbSQL.Append(" AND");
            sbSQL.Append(" CDUND = '#2'");
            sbSQL.Append(" AND ");
            sbSQL.Append(" DTCARG < '" + DateTime.Now.ToString("dd/MM/yyyy") + "'");

            sbSQL.Replace("#0", strScheSAT);
            sbSQL.Replace("#1", strScheINT);
            sbSQL.Replace("#2", m_sUnidade);
            Retorno = Convert.ToInt32(m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ETL", strConnINT));
        }

        [WebMethod]
        [CatchException]
        public void ApagarAtendimentosSatelite(string m_sUnidade)
        {
            int Retorno;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            sbSQL.Append(" Delete ");
            sbSQL.Append(" From ");
            sbSQL.Append(" #0.ATENDIMENTO ");
            sbSQL.Append(" Where  ");
            sbSQL.Append(" cod_pac in (SELECT SUBSTR(CLATND,10,10) FROM #1.TBDWD018 WHERE SUBSTR(CLATND,1,3) = '#2')");
            sbSQL.Append(" AND");
            sbSQL.Append(" CDUND = '#2'");
            sbSQL.Append(" AND ");
            sbSQL.Append(" DTCARG < '" + DateTime.Now.ToString("dd/MM/yyyy") + "'");

            sbSQL.Replace("#0", strScheSAT);
            sbSQL.Replace("#1", strScheINT);
            sbSQL.Replace("#2", m_sUnidade);
            Retorno = Convert.ToInt32(m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ETL", strConnINT));

        }
    }
}
