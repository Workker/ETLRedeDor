using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using AcessoDados; 

namespace WSLerMedicoEspecialidade
{
    /// <summary>
    ///  Ler todos Médicos X Especialidades Cadastradas no Sistema WPD
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WSLerMedicoEspecialidade : System.Web.Services.WebService
    {
        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConn = ConfigurationManager.AppSettings["ConectaHIS"].ToString();
        string strSche = ConfigurationManager.AppSettings["strSchema"].ToString();

        [WebMethod]
        public DataSet RetornarMedicosEspecialidades()
        {
            DataSet Ds;

            sbSQL.Length = 0;

            sbSQL.Append("SELECT ");
            sbSQL.Append(" cod_pro, cod_esp ");
            sbSQL.Append("FROM #0.faesppro ");

            sbSQL.Replace("#0", strSche);

            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAESPPRO", strConn);

            return Ds;
        }
    }
}
