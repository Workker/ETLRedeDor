using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindowsETL.SrvPacHBD;
using AcessoDados;

namespace WindowsETL.Passos
{
    class LerPacientes
    {
        #region Variáveis

        public string strConnHIS { get; set; }
        public string strScheHIS { get; set; }
        public string strConnSAT { get; set; }
        public string strScheSAT { get; set; }
        public string m_sUnidade { get; set; }
        public string m_sConciliacaoDias { get; set; }

        public int m_iQtdeRegistros { get; set; }

        DataSet m_oDataSet;

        rpLerDados m_oRP = new rpLerDados();

        ClDadosLOG m_oLog = new ClDadosLOG();

        #endregion

        #region Métodos

        public void ProcessarPacientesHIS(Int32 m_iQtdeRegistros)
        {
            try
            {
                string m_sUltimoRegistro;
                DataSet DsDados = new DataSet();

                SrvPacHBD.LerPacientesWPDSoapClient DadosOrigens = new LerPacientesWPDSoapClient();

                //Obtem o Ultimo Registro
                m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(m_sUnidade);

                //Obtem as Informações 
                m_oDataSet = DadosOrigens.RetornarPacientes(m_sUltimoRegistro, m_iQtdeRegistros, strConnHIS, strScheHIS);

                //Gravar no DWSATELITE
                //Salvar();
                DadosOrigens.GravarPaciente(m_sUnidade, m_oDataSet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ProcessarConciliacaoPacientesHIS(Int32 m_iQtdeRegistros, string m_sData)
        {
            try
            {
                string m_sUltimoRegistro;

                SrvPacHBD.LerPacientesWPDSoapClient DadosOrigens = new LerPacientesWPDSoapClient();

                if (m_sData.Trim().Length == 0)
                {
                    //Obtem o Ultimo Registro
                    m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(m_sUnidade);

                    string[] xUltmoRegistro;

                    xUltmoRegistro = m_sUltimoRegistro.Split(';');

                    m_sData = xUltmoRegistro[1].ToString();
                }

                DateTime Dt01 = Convert.ToDateTime(m_sData);

                Dt01 = Dt01.AddDays(-Convert.ToInt16(m_sConciliacaoDias));
                
                //Obtem as Informações 
                m_oDataSet = DadosOrigens.RetornarPacientesConciliacao(Dt01.ToString("dd/MM/yyyy"));

                DadosOrigens.GravarPaciente(m_sUnidade, m_oDataSet);

                //Gravar no DWSATELITE
                //Salvar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected Boolean Salvar()
        {
            Boolean Retorno = true;

                for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

                    StringBuilder sbSQL = new System.Text.StringBuilder();
                    try
                    {


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

            for (int i = 0; i < m_sPalavra.Trim().Length ; i++)
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
        
        #endregion

    }
}
