using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using AcessoDados;

namespace WSLerLeitos
{
    /// <summary>
    /// Ler todos Leitos Cadastradas no Sistema WPD
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WSLerLeitos : System.Web.Services.WebService
    {
        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConn = ConfigurationManager.AppSettings["ConectaHIS"].ToString();
        string strSche = ConfigurationManager.AppSettings["strSchema"].ToString();

        [WebMethod]
        public DataSet RetornarLeitos()
        {
            DataSet Ds;

            sbSQL.Length = 0;

            sbSQL.Append(" SELECT leito, posto_enf, descricao, cod_pac, tipo,");
            sbSQL.Append(" 	andar, cod_apt, extra, cod_set, aih_leito, ");
            sbSQL.Append(" 	aih_cod_hospital, enfermaria_sus, desativado, em_limpeza, ramal, ");
            sbSQL.Append(" 	qtd_lim_visitantes ");
            sbSQL.Append(" FROM #0.faleicad");

            sbSQL.Replace("#0", strSche);

            Engine engine = new Engine(strConn);
            var ds = engine.RetornarDataSet(sbSQL.ToString(), "FALEICAD");
            //Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FALEICAD", strConn);

            return ds;
        }

    }
}
