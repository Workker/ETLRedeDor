using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using AcessoDados; 

namespace WSLerEspecialidades
{
    /// <summary>
    /// Ler todas Especialidades Cadastradas no Sistema WPD
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSLerEspecialidades : System.Web.Services.WebService
    {
        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConn = ConfigurationManager.AppSettings["ConectaHIS"].ToString();
        string strSche = ConfigurationManager.AppSettings["strSchema"].ToString();

        [WebMethod]
        public DataSet RetornarEspecialidades()
        {
            DataSet Ds;

            sbSQL.Length = 0;

            sbSQL.Append(" SELECT  ");
            sbSQL.Append("      cod_esp, descricao ");
            sbSQL.Append(" FROM #0.faespcad "); 

            sbSQL.Replace("#0", strSche);

            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAESPCAD", strConn);

            return Ds;
        }

    }
}
