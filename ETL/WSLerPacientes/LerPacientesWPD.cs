using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Text; 
using AcessoDados; 

//[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
/// <summary>
/// Ler Dados dos Pacientes do Sistema WPD
/// </summary>

[WebService(Namespace = "http://microsoft.com/webservices/")]
public class LerPacientesWPD : System.Web.Services.WebService
{
    
    rpLerDados m_oRP = new rpLerDados();

    StringBuilder sbSQL = new System.Text.StringBuilder();

    string strConn = ConfigurationManager.AppSettings["ConectaHIS"].ToString();
    string strSche = ConfigurationManager.AppSettings["strSchemaHIS"].ToString();

    string strConnSAT = ConfigurationManager.AppSettings["ConectaSAT"].ToString();
    string strScheSAT = ConfigurationManager.AppSettings["strSchemaSAT"].ToString();

    DataSet m_oDataSet;
    string m_sUnidade = string.Empty; 

    public LerPacientesWPD()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string RetornarUltimoRegistro(string str_Unidade)
    {
        string Resposta = string.Empty;

        StringBuilder sbSQL = new System.Text.StringBuilder();

        sbSQL.Length = 0;
        // ORIGINAL // sbSQL.Append("SELECT COD_PRT AS ULTIMOVALOR FROM (SELECT COD_PRT FROM #0.PACIENTE WHERE CDUND = '#1' ORDER BY COD_PRT DESC) WHERE ROWNUM = 1");
        sbSQL.Append("SELECT COD_PRT || ';' || DATA_ABERTU AS ULTIMOVALOR FROM (SELECT COD_PRT,DATA_ABERTU FROM #0.PACIENTE WHERE CDUND = '#1' ORDER BY COD_PRT DESC) WHERE ROWNUM = 1");
        
        sbSQL.Replace("#0", strScheSAT);
        
        sbSQL.Replace("#0", strSche);
        sbSQL.Replace("#1", str_Unidade.ToUpper());

        try
        {
            Resposta = m_oRP.RetornarConsulta(sbSQL.ToString(), "PACIENTE", strConnSAT);

            return Resposta;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    
    [WebMethod]
    public DataSet RetornarPacientes(string UltimoRegistro, Int32 NumeroLinhas, string strConnHIS, string strScheHIS)
    {
        DataSet Ds;

        sbSQL.Length = 0;

        string[] xUltimoRegistro = UltimoRegistro.Split(';');

        if (NumeroLinhas == 0)
        {
            NumeroLinhas = 1000;
        }

        // ANTIGO - 30/05/2012
        //sbSQL.Append(" SELECT ");
        //sbSQL.Append("  cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
        //sbSQL.Append("	est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
        //sbSQL.Append("	uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
        //sbSQL.Append("	end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
        //sbSQL.Append("  fone_res, dsc_email, data_abertu ");
        //sbSQL.Append(" FROM #0.FAPRTCAD ");
        ////sbSQL.Append(" WHERE cod_prt>'#1' AND data_abertu >= '#3' ");
        //sbSQL.Append(" WHERE data_abertu >= '#3' "); 
        ////sbSQL.Append(" AND ");
        ////sbSQL.Append(" ROWNUM <= #2 ");
        //sbSQL.Append(" ORDER BY cod_prt ");

        sbSQL.Append(" SELECT ");
        sbSQL.Append("         B1.cod_prt, B1.nome_pac, B1.cpf, B1.sexo, B1.nascimento, B1.nome_mae, B1.nome_pai, ");
        sbSQL.Append("         B1.est_civil, B1.nome_conjuge, B1.cor, B1.profissao, B1.identidade, B1.natural_de, ");
        sbSQL.Append("         B1.uf_naturalidade, B1.nacionalidade, B1.pais_res, B1.end_res, B1.end_numero, ");
        sbSQL.Append("         B1.end_complemento, B1.bai_res, B1.cid_res, B1.est_res, B1.cep_res, B1.ponto_ref,");
        sbSQL.Append("         B1.fone_res, B1.dsc_email, B1.data_abertu ");
        sbSQL.Append(" FROM ");
        sbSQL.Append("  #0.FAPACCAD A1 INNER JOIN  #0.FAPRTCAD B1 ON A1.COD_PRT = B1.COD_PRT ");
        sbSQL.Append(" WHERE ");
        sbSQL.Append(" A1.TIPO_PAC = 'I' ");
        sbSQL.Append(" AND ");
        sbSQL.Append(" A1.DATA_ALTA IS NULL "); 


        sbSQL.Replace("#0", strScheHIS);   
        // ORIGINAL // sbSQL.Replace("#1", UltimoRegistro);
        // ANTIGO - 30/05/2012 // sbSQL.Replace("#2", NumeroLinhas.ToString().Trim());

        // ANTIGO - 30/05/2012 // sbSQL.Replace("#1", xUltimoRegistro[0].ToString().Trim());
        // ANTIGO - 30/05/2012 // sbSQL.Replace("#3", xUltimoRegistro[1].ToString().Trim());

        try
        {
            Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAPRTCAD", strConnHIS);

            return Ds;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [WebMethod]
    public DataSet RetornarPacientesConciliacao(String m_DataDe)
    {
        DataSet Ds;
 
        sbSQL.Length = 0;

        sbSQL.Append("SELECT  ");
        sbSQL.Append("cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
        sbSQL.Append("est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
        sbSQL.Append("uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
        sbSQL.Append("end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
        sbSQL.Append("fone_res, dsc_email, data_abertu ");
        sbSQL.Append("FROM #0.FAPRTCAD  ");
        sbSQL.Append("WHERE data_abertu >= '#1' ");
        sbSQL.Append("ORDER BY cod_prt,data_abertu ");

        sbSQL.Replace("#0", strSche);
        sbSQL.Replace("#1", m_DataDe);

        Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAPRTCAD", strConn);

        return Ds;
    }

    [WebMethod]
    public Boolean GravarPaciente(string m_strUnidade, DataSet m_oDsDados)
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

    protected Boolean Salvar(String m_sUnidade)
    {
        Boolean Retorno = true;

        for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
        {
            DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

            StringBuilder sbSQL = new System.Text.StringBuilder();
            try
            {

                //if ("005379678".Contains(Dr0[0].ToString().Trim()))
                //{

                //}


                if (!Dr0.IsNull(26))
                {
                    sbSQL.Append(" INSERT INTO #0.PACIENTE(cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
                    sbSQL.Append("                      est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
                    sbSQL.Append("                      uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
                    sbSQL.Append("                      end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
                    sbSQL.Append("                      fone_res, dsc_email, data_abertu, dtcarg, cdund) ");
                    sbSQL.Append(" VALUES(");
                    sbSQL.Append("'" + Dr0[0].ToString() + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[1].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[3].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[5].ToString()) + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[6].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[10].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[11].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[12].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[13].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[14].ToString() + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[15].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[16].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[17].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[18].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[19].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[20].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[21].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[22].ToString() + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[23].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[24].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[25].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[26].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + m_sUnidade.Trim() + "'");
                    sbSQL.Append(")");
                }
                else
                {
                    sbSQL.Append(" INSERT INTO #0.PACIENTE(cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
                    sbSQL.Append("                      est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
                    sbSQL.Append("                      uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
                    sbSQL.Append("                      end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
                    sbSQL.Append("                      fone_res, dsc_email, dtcarg, cdund) ");
                    sbSQL.Append(" VALUES(");
                    sbSQL.Append("'" + Dr0[0].ToString() + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[1].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[3].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[5].ToString()) + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[6].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[10].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[11].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[12].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[13].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[14].ToString() + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[15].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[16].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[17].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[18].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[19].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[20].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[21].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[22].ToString() + "'");
                    sbSQL.Append(",'" + VerificarPalavra(Dr0[23].ToString()) + "'");
                    sbSQL.Append(",'" + Dr0[24].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[25].ToString() + "'");
                    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + m_sUnidade.Trim() + "'");
                    sbSQL.Append(")");
                }

                //sbSQL.Append(" INSERT INTO #0.PACIENTE(cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
                //sbSQL.Append("                      est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
                //sbSQL.Append("                      uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
                //sbSQL.Append("                      end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
                //sbSQL.Append("                      fone_res, dsc_email, data_abertu, dtcarg, cdund) ");
                //sbSQL.Append(" VALUES(");
                //sbSQL.Append("'" + Dr0[0].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[1].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[3].ToString() + "'");
                //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                //sbSQL.Append(",'" + Dr0[5].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[6].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[10].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[11].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[12].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[13].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[14].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[15].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[16].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[17].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[18].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[19].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[20].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[21].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[22].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[23].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[24].ToString() + "'");
                //sbSQL.Append(",'" + Dr0[25].ToString() + "'");
                //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[26].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                //sbSQL.Append(",'" + m_sUnidade.Trim() + "'");
                //sbSQL.Append(")");

                sbSQL.Replace("#0", strScheSAT);

                m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "PACIENTES", strConnSAT);
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
            }
            finally
            {

            }
        }

        return Retorno;
    }

    private string VerificarPalavra(string m_sPalavra)
    {
        string PalavraRetorno = string.Empty;
        string Letra = string.Empty;

        for (int i = 0; i < m_sPalavra.Trim().Length; i++)
        {
            //Letra = m_sPalavra.Substring(i, 1).Trim();
            Letra = m_sPalavra.Substring(i, 1);

            switch (Letra)
            {
                case "'":
                    Letra = " ";

                    break;

                case "-":
                    Letra = " ";

                    break;

                case ",":
                    Letra = " ";

                    break;

                case ";":
                    Letra = " ";

                    break;

                default:
                    break;
            }

            PalavraRetorno += Letra;
        }

        return PalavraRetorno;
    }
        


    //[WebMethod] // ANTIGO
    //public DataSet RetornarPacientesConciliacao(DataSet dsConciliacao, Int32 NumeroLinhas)
    //{
    //    DataSet Ds;

    //    string Condicao = string.Empty;

    //    for (int i = 0; i < dsConciliacao.Tables[0].Rows.Count; i++)
    //    {
    //        DataRow Dr0 = dsConciliacao.Tables[0].Rows[i];

    //        if (i == 0)
    //        {
    //            Condicao += "'" + Dr0["COD_PRT"].ToString().Trim() + "'";
    //        }
    //        else
    //        {
    //            Condicao += ",'" + Dr0["COD_PRT"].ToString().Trim() + "'";
    //        }
    //    }

    //    sbSQL.Length = 0;

    //    if (NumeroLinhas == 0)
    //    {
    //        NumeroLinhas = 1000;
    //    }

    //    sbSQL.Append(" SELECT ");
    //    sbSQL.Append("  cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
    //    sbSQL.Append("	est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
    //    sbSQL.Append("	uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
    //    sbSQL.Append("	end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
    //    sbSQL.Append("  fone_res, dsc_email, data_abertu ");
    //    sbSQL.Append(" FROM #0.FAPRTCAD ");
    //    sbSQL.Append(" WHERE cod_prt IN(#1) ");
    //    sbSQL.Append(" AND ");
    //    sbSQL.Append(" ROWNUM <= #2 ");
    //    sbSQL.Append(" ORDER BY cod_prt ");

    //    sbSQL.Replace("#0", strSche);
    //    sbSQL.Replace("#1", Condicao);
    //    sbSQL.Replace("#2", NumeroLinhas.ToString().Trim());

    //    Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "FAPRTCAD", strConn);

    //    return Ds;
    //}

}