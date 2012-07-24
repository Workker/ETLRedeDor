using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text;
using AcessoDados; 

namespace WSLerMedicos
{
    /// <summary>
    /// Ler Dados dos Médicos Cadastrados no Sistema WPD
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
   
    public class WSLerMedicos : System.Web.Services.WebService
    {

        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConn = ConfigurationManager.AppSettings["ConectaHIS"].ToString();
        string strSche = ConfigurationManager.AppSettings["strSchema"].ToString();

        [WebMethod]
        public DataSet RetornarMedicos(string UltimoRegistro, Int32 NumeroLinhas)
        {
            DataSet Ds;

            sbSQL.Length = 0;

            if (NumeroLinhas == 0)
            {
                NumeroLinhas = 1000;
            }

            sbSQL.Append(" SELECT  ");
            sbSQL.Append("    cod_pro, nome_pro, cpf, data_nascimento, numero_res, complemento_res,  ");
            sbSQL.Append("    bairro_res, cidade_res, estado_res, cep_res, fone_res, fone_celular, email, crm,  ");
            sbSQL.Append("    cod_tp_conselho, end, uf_crm ");
            sbSQL.Append(" FROM #0.faprocad  ");
            sbSQL.Append(" WHERE cod_pro>'#1' ");
            sbSQL.Append(" AND ");
            sbSQL.Append(" ROWNUM <= #2 ");
            sbSQL.Append(" ORDER BY cod_pro ");

            sbSQL.Replace("#0", strSche);
            sbSQL.Replace("#1", UltimoRegistro);
            sbSQL.Replace("#2", NumeroLinhas.ToString().Trim());

            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAPROCAD", strConn);

            return Ds;
        }

    }
}
