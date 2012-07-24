using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindowsETL.SrvPacHBD; 
using AcessoDados;

namespace WindowsETL.Passos
{
    class HBD_Pacientes
    {
        //Sigla UNIDADE
        const string mUnidade = "HBD";

        //public DataSet m_oDataSet  
     
        public  DataSet m_oDataSet { get; set; }
        
        string m_sUnidade = mUnidade;

        rpLerDados m_oRP = new rpLerDados();

        public string StrConexao
        {
            get;set;
        }

        public string StrSchema
        {
            get;
            set;
        }

        /// <summary>
        /// Obtem todos atendimentos a partir do Ultimo Registro.
        /// </summary>
        /// <param name="m_iQtdeRegistros">Quantidade de Registro</param>
        /// <returns></returns>
        public void ProcessarPacientesHIS(Int32 m_iQtdeRegistros)
        {
            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvPacHBD.LerPacientesWPDSoapClient DadosOrigens = new LerPacientesWPDSoapClient();

            //Obtem o Ultimo Registro
            m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

            //Obtem as Informações 
            //m_oDataSet = DadosOrigens.RetornarPacientes(m_sUltimoRegistro, m_iQtdeRegistros);

            //Gravar no DWSATELITE
            Salvar();
        }

        public void ProcessarConciliacaoPacientesHIS(Int32 m_iQtdeRegistros)
        {
            //Gravar no DWSATELITE
            Salvar();
        }

        protected Boolean Salvar()
        {
            Boolean Retorno = true;

            try
            {
                for (int i = 0; i < m_oDataSet.Tables[0].Rows.Count; i++)
                {
                    DataRow Dr0 = m_oDataSet.Tables[0].Rows[i];

                    StringBuilder sbSQL = new System.Text.StringBuilder();

                    sbSQL.Append(" INSERT INTO #0.PACIENTE(cod_prt, nome_pac, cpf, sexo, nascimento, nome_mae, nome_pai, ");
                    sbSQL.Append("                      est_civil, nome_conjuge, cor, profissao, identidade, natural_de, ");
                    sbSQL.Append("                      uf_naturalidade, nacionalidade, pais_res, end_res, end_numero, ");
                    sbSQL.Append("                      end_complemento, bai_res, cid_res, est_res, cep_res, ponto_ref, ");
                    sbSQL.Append("                      fone_res, dsc_email, data_abertu, dtcarg, cdund) ");
                    sbSQL.Append(" VALUES(");
                    sbSQL.Append("'" + Dr0[0].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[1].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[3].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + Dr0[5].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[6].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[9].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[10].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[11].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[12].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[13].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[14].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[15].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[16].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[17].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[18].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[19].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[20].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[21].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[22].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[23].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[24].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[25].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[26].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + m_sUnidade.Trim() + "'");
                    sbSQL.Append(")");

                    sbSQL.Replace("#0", StrSchema);   

                    m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "PACIENTES", StrConexao);

                }

                return Retorno;
            }
            catch (Exception en)
            {
                Retorno = false;

                return Retorno;
            }

        }

    }
}
