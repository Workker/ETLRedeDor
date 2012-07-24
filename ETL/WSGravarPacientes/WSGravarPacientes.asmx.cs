using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Configuration;
using System.Data;
using AcessoDados;


namespace WSGravarPacientes
{
    /// <summary>
    /// Gravar Paciente da Unidade no DW
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class WSGravarPacientes : System.Web.Services.WebService
    {
        rpLerDados m_oRP = new rpLerDados();

        StringBuilder sbSQL = new System.Text.StringBuilder();

        string strConnINT = ConfigurationManager.AppSettings["ConectaINT"].ToString();
        string strScheINT = ConfigurationManager.AppSettings["strSchemaINT"].ToString();

        string strConnSAT = ConfigurationManager.AppSettings["ConectaSAT"].ToString();
        string strScheSAT = ConfigurationManager.AppSettings["strSchemaSAT"].ToString();

        DataSet m_oDataSet;
        string m_sUnidade = string.Empty;

        [WebMethod]
        public string RetornarUltimoRegistro(string str_Unidade)
        {
            string Resposta = string.Empty;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;
            sbSQL.Append("SELECT COD_PRT AS ULTIMOVALOR FROM (SELECT COD_PRT FROM #0.PACIENTE WHERE CDUND = '#1' AND NOT DATA_ABERTU IS NULL ORDER BY DATA_ABERTU DESC, COD_PRT DESC) WHERE ROWNUM = 1");

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
        public DataSet RetornarPacientes(string str_Unidade, string UltimoRegistro, Int32 NumeroLinhas)
        {
            DataSet Ds;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            if (NumeroLinhas == 0)
            {
                NumeroLinhas = 1000;
            }

            sbSQL.Append(" SELECT cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai,est_civil,");
            sbSQL.Append("        nome_conjuge, cor, profissao, identidade, natural_de, uf_naturalidade,");
            sbSQL.Append("        nacionalidade, pais_res, end_res, end_numero, end_complemento, bai_res, cid_res,");
            sbSQL.Append("        est_res, cep_res, ponto_ref, fone_res, dsc_email, data_abertu");
            sbSQL.Append(" FROM ");
            sbSQL.Append(" #0.Paciente ");
            //sbSQL.Append(" WHERE cdund = '#1' AND cod_prt > '#2'");
            sbSQL.Append(" WHERE cdund = '#5' AND cod_prt Not in (Select substr(clpess,10,9) from #4.tbdwd004 where substr(CLPESS,1,3) = '#5')");
            sbSQL.Append(" AND ");
            sbSQL.Append(" ROWNUM <= #3 ");
            sbSQL.Append(" ORDER BY cod_prt ");

            sbSQL.Replace("#0", strScheSAT);
            //sbSQL.Replace("#1", str_Unidade.Trim() + "WPDPAC");
            //sbSQL.Replace("#2", UltimoRegistro);
            sbSQL.Replace("#3", NumeroLinhas.ToString().Trim());
            sbSQL.Replace("#4", strScheINT);
            sbSQL.Replace("#5", str_Unidade.Trim());

            try
            {
                Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "PACIENTE", strConnSAT);

                return Ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [WebMethod]
        public DataSet RetornarConciliacaoPacientes(string str_Unidade, Int32 NumeroLinhas)
        {
            DataSet Ds;

            StringBuilder sbSQL = new System.Text.StringBuilder();

            sbSQL.Length = 0;

            if (NumeroLinhas == 0)
            {
                NumeroLinhas = 1000;
            }

            sbSQL.Append("SELECT");
            sbSQL.Append(" CLATND,");
            sbSQL.Append(" CLPACN,");
            sbSQL.Append(" SUBSTR(CLPACN,10,10) AS COD_PRT,");
            sbSQL.Append(" DTCARG");
            sbSQL.Append(" FROM #0.TBDWD018 ");
            sbSQL.Append(" WHERE");
            sbSQL.Append(" SUBSTR(CLATND,1,3) = '#1'");
            sbSQL.Append(" AND ");
            sbSQL.Append(" CLPACN NOT IN ( SELECT CLPESS AS CLPACN FROM #0.TBDWD004 )");
            sbSQL.Append(" AND ");
            sbSQL.Append(" ROWNUM <= #2;");

            sbSQL.Replace("#0", strScheINT);
            sbSQL.Replace("#1", str_Unidade);
            sbSQL.Replace("#2", NumeroLinhas.ToString().Trim());

            try
            {
                Ds = m_oRP.RetornarDataSet(sbSQL.ToString(), "PACIENTE", strConnSAT);

                return Ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [WebMethod]
        public Boolean GravarPacientes(string m_strUnidade, DataSet m_oDsDados)
        {

            Boolean Resposta = false;

            try
            {
                m_oDataSet = m_oDsDados;
                m_sUnidade = m_strUnidade;

                Resposta = Salvar();

                return Resposta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected Boolean Salvar()
        {
            Boolean Retorno = false;

            //try
            //{
                string V_PREFIXO = m_sUnidade.Trim().ToUpper() + "WPDPAC";

                int m_iSexo = 0;
                int m_iEstadoCivil = 0;
                int m_iNaturalidade = 0;
                int m_iEstadoResidencia = 0;

                Int32 RetornoLinhas = 0;

                string m_sEnderecoRES;

                for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

                    //SEXO  
                    switch (Dr0[3].ToString().Trim())
                    {
                        case "M":
                            m_iSexo = 1;
                            break;
                        case "F":
                            m_iSexo = 2;
                            break;
                        default:
                            m_iSexo = 3;
                            break;
                    }

                    //ESTADO CIVIL
                    switch (Dr0[7].ToString().Trim())
                    {
                        case "S":
                            m_iEstadoCivil = 1;
                            break;
                        case "C":
                            m_iEstadoCivil = 2;
                            break;
                        case "D":
                            m_iEstadoCivil = 3;
                            break;
                        case "V":
                            m_iEstadoCivil = 4;
                            break;
                        case "O":
                            m_iEstadoCivil = 6;
                            break;
                        default:
                            m_iEstadoCivil = 5;
                            break;
                    }

                    //NATURALIDADE
                    switch (Dr0[13].ToString().Trim())
                    {
                        case "AC":
                            m_iNaturalidade = 1;
                            break;
                        case "RJ":
                            m_iNaturalidade = 2;
                            break;
                        case "AL":
                            m_iNaturalidade = 3;
                            break;
                        case "SP":
                            m_iNaturalidade = 4;
                            break;
                        case "AP":
                            m_iNaturalidade = 5;
                            break;
                        case "MG":
                            m_iNaturalidade = 6;
                            break;
                        case "BA":
                            m_iNaturalidade = 7;
                            break;
                        case "CE":
                            m_iNaturalidade = 8;
                            break;
                        case "DF":
                            m_iNaturalidade = 9;
                            break;
                        case "ES":
                            m_iNaturalidade = 10;
                            break;
                        case "GO":
                            m_iNaturalidade = 11;
                            break;
                        case "MA":
                            m_iNaturalidade = 12;
                            break;
                        case "MT":
                            m_iNaturalidade = 13;
                            break;
                        case "MS":
                            m_iNaturalidade = 14;
                            break;
                        case "AM":
                            m_iNaturalidade = 15;
                            break;
                        case "PA":
                            m_iNaturalidade = 16;
                            break;
                        case "PB":
                            m_iNaturalidade = 17;
                            break;
                        case "PR":
                            m_iNaturalidade = 18;
                            break;
                        case "PE":
                            m_iNaturalidade = 19;
                            break;
                        case "PI":
                            m_iNaturalidade = 20;
                            break;
                        case "RN":
                            m_iNaturalidade = 21;
                            break;
                        case "RS":
                            m_iNaturalidade = 22;
                            break;
                        case "RO":
                            m_iNaturalidade = 23;
                            break;
                        case "RR":
                            m_iNaturalidade = 24;
                            break;
                        case "SC":
                            m_iNaturalidade = 25;
                            break;
                        case "SE":
                            m_iNaturalidade = 26;
                            break;
                        case "TO":
                            m_iNaturalidade = 27;
                            break;
                        default:
                            m_iNaturalidade = 0;
                            break;
                    }

                    //ESTADO
                    switch (Dr0[21].ToString().Trim())
                    {
                        case "AC":
                            m_iEstadoResidencia = 1;
                            break;
                        case "RJ":
                            m_iEstadoResidencia = 2;
                            break;
                        case "AL":
                            m_iEstadoResidencia = 3;
                            break;
                        case "SP":
                            m_iEstadoResidencia = 4;
                            break;
                        case "AP":
                            m_iEstadoResidencia = 5;
                            break;
                        case "MG":
                            m_iEstadoResidencia = 6;
                            break;
                        case "BA":
                            m_iEstadoResidencia = 7;
                            break;
                        case "CE":
                            m_iEstadoResidencia = 8;
                            break;
                        case "DF":
                            m_iEstadoResidencia = 9;
                            break;
                        case "ES":
                            m_iEstadoResidencia = 10;
                            break;
                        case "GO":
                            m_iEstadoResidencia = 11;
                            break;
                        case "MA":
                            m_iEstadoResidencia = 12;
                            break;
                        case "MT":
                            m_iEstadoResidencia = 13;
                            break;
                        case "MS":
                            m_iEstadoResidencia = 14;
                            break;
                        case "AM":
                            m_iEstadoResidencia = 15;
                            break;
                        case "PA":
                            m_iEstadoResidencia = 16;
                            break;
                        case "PB":
                            m_iEstadoResidencia = 17;
                            break;
                        case "PR":
                            m_iEstadoResidencia = 18;
                            break;
                        case "PE":
                            m_iEstadoResidencia = 19;
                            break;
                        case "PI":
                            m_iEstadoResidencia = 20;
                            break;
                        case "RN":
                            m_iEstadoResidencia = 21;
                            break;
                        case "RS":
                            m_iEstadoResidencia = 22;
                            break;
                        case "RO":
                            m_iEstadoResidencia = 23;
                            break;
                        case "RR":
                            m_iEstadoResidencia = 24;
                            break;
                        case "SC":
                            m_iEstadoResidencia = 25;
                            break;
                        case "SE":
                            m_iEstadoResidencia = 26;
                            break;
                        case "TO":
                            m_iEstadoResidencia = 27;
                            break;
                        default:
                            m_iEstadoResidencia = 0;
                            break;
                    }

                    //ENDEREÇO RESIDENCIAL
                    m_sEnderecoRES = string.Empty;

                    //if (Dr0[16].ToString().Trim().Length > 0)
                    //{
                    //    m_sEnderecoRES = Dr0[16].ToString().Trim() + ", " + Dr0[17].ToString().Trim();
                    //}

                    try 
                    {   
                  
                        //Pessoa 
                        sbSQL.Length = 0;
                        sbSQL.Append(" INSERT INTO #0.tbdwd004(clpess, nmpess, nrcpfcnpj, iddwd006, flclnt, flforn, dtcarg,dssexo,dtnasc,dtabrtprnt)");
                        sbSQL.Append(" VALUES (");
                        sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                        sbSQL.Append(",'" + VerificarPalavra(Dr0[1].ToString()) + "'");
                        sbSQL.Append(",'" + SomenteNumeros(Dr0[2].ToString()) + "'");
                        sbSQL.Append(",1");
                        sbSQL.Append(",1");
                        sbSQL.Append(",0");
                        sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                        sbSQL.Append(",'" + Dr0[3].ToString().Trim() + "'");
                        sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                       
                        if (!Dr0.IsNull(26))
                        {
                            sbSQL.Append(",'" + Convert.ToDateTime(Dr0[26].ToString()).Date.ToString("dd/MM/yyyy") + "'"); 
                        }
                        else 
                        {
                            sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                        }
                        sbSQL.Append(")");

                        sbSQL.Replace("#0", strScheINT);

                        RetornoLinhas = m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "TBDWD004", strConnINT);

                        if (RetornoLinhas > 0)
                        {
                            Retorno = true;
                        }

                        if (Retorno)
                        {
                            //Pessoa Física
                            //sbSQL.Length = 0;
                            //sbSQL.Append(" INSERT INTO #0.tbdwd005(clpessfisc, iddwd025, dtnasc, nmmae, nmpai, iddwd020, nmconj, nmcor,");
                            //sbSQL.Append(" nmprfs, nridnt, nmnatr, idpro021, nmnacn, nmpaisresd, dtcarg) ");
                            //sbSQL.Append(" VALUES (");
                            //sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                            //sbSQL.Append(",");
                            //sbSQL.Append(m_iSexo);
                            //sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[5].ToString()) + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[6].ToString()) + "'");
                            //sbSQL.Append(",");
                            //sbSQL.Append(m_iEstadoCivil);
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[8].ToString()) + "'");
                            //sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                            //sbSQL.Append(",'" + Dr0[10].ToString() + "'");
                            //sbSQL.Append(",'" + SomenteNumeros(Dr0[11].ToString()) + "'");
                            //sbSQL.Append(",'" + Dr0[12].ToString() + "'");
                            //sbSQL.Append(",");
                            //sbSQL.Append(m_iNaturalidade);
                            //sbSQL.Append(",'" + Dr0[14].ToString() + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[15].ToString()) + "'");
                            //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            //sbSQL.Append(")");

                            //sbSQL.Replace("#0", strScheINT);

                            //m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "TBDWD005", strConnINT);

                            //ENDEREÇO
                            //sbSQL.Length = 0;
                            //sbSQL.Append(" INSERT INTO #0.tbdwd021(clpess, iddwd022, dsendr, dscompendr, nmbarr, nmcidd, idpro021,");
                            //sbSQL.Append("nrcep, dspontrefr, dtcarg) ");
                            //sbSQL.Append(" VALUES (");
                            //sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                            //sbSQL.Append(",1");
                            //sbSQL.Append(",'" + m_sEnderecoRES.Trim() + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[18].ToString()) + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[19].ToString()) + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[20].ToString()) + "'");
                            //sbSQL.Append(",");
                            //sbSQL.Append(m_iEstadoResidencia);
                            //sbSQL.Append(",'" + Dr0[22].ToString() + "'");
                            //sbSQL.Append(",'" + VerificarPalavra(Dr0[23].ToString()) + "'");
                            //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            //sbSQL.Append(")");

                            //sbSQL.Replace("#0", strScheINT);

                            //m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "TBDWD021", strConnINT);

                            //TELEFONE
                            //sbSQL.Length = 0;
                            //sbSQL.Append("INSERT INTO #0.tbdwd023(clpess, iddwd024, nrtelf, dtcarg) ");
                            //sbSQL.Append(" VALUES (");
                            //sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                            //sbSQL.Append(",1");
                            //sbSQL.Append(",'" + SomenteNumeros(Dr0[24].ToString()) + "'");
                            //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            //sbSQL.Append(")");

                            //sbSQL.Replace("#0", strScheINT);

                            //m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "TBDWD023", strConnINT);

                            //EMAIL
                            //sbSQL.Length = 0;
                            //sbSQL.Append("INSERT INTO #0.tbdwd026(clpess, iddwd027, dsemail, dtcarg) ");
                            //sbSQL.Append(" VALUES (");
                            //sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                            //sbSQL.Append(",1");
                            //sbSQL.Append(",'" + Dr0[25].ToString().Trim() + "'");
                            //sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            //sbSQL.Append(")");

                            //sbSQL.Replace("#0", strScheINT);

                            //m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "TBDWD026", strConnINT);

                            //DATA ABERTURA PRONTUARIO
                            //sbSQL.Length = 0;

                            //if (!Dr0.IsNull(26))
                            //{
                            //    sbSQL.Append("INSERT INTO tbdwd007(clpacn, dtcarg, dtabrtprnt) ");
                            //    sbSQL.Append(" VALUES (");
                            //    sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                            //    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            //    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[26].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                            //    sbSQL.Append(")");
                            //}
                            //else
                            //{
                            //    sbSQL.Append("INSERT INTO tbdwd007(clpacn, dtcarg) ");
                            //    sbSQL.Append(" VALUES (");
                            //    sbSQL.Append("'" + V_PREFIXO.Trim() + Dr0[0].ToString().Trim() + "'");
                            //    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                            //    sbSQL.Append(")");
                            //}

                            //sbSQL.Replace("#0", strScheINT);

                            //m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "TBDWD007", strConnINT);
                         }
                    }
                    catch (Exception en)
                    {
                        //throw new Exception(en.Message);
                    }
                    finally
                    {
                
                    }

                }

                return Retorno; 
            //}
            //catch (Exception en)
            //{
            //    throw new Exception(en.Message);
            //}
            //finally
            //{
                
            //}
        }

        protected string SomenteNumeros(string m_sValor)
        {
           string Retorno = string.Empty;
           string Dado;

           for (int i = 0; i < m_sValor.Trim().Length; i++)
           {
               Dado = m_sValor.Substring(i,1);

               switch (Dado)
               {
                   case ".":
                      Dado = "";
                       break;
                   case "/":
                       Dado = "";
                       break;
                   case "-":
                       Dado = "";
                       break;
                   case "|":
                       Dado = "";
                       break;
                   case ":":
                       Dado = "";
                       break;
                   case ";":
                       Dado = "";
                       break;
                   case "=":
                       Dado = "";
                       break;
                   case ",":
                       Dado = "";
                       break;
                   case "+":
                       Dado = "";
                       break;
               }

               Retorno += Dado;
           }

           return Retorno;
          
        }

        // VerificarPalavra(
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
        


  }

}
