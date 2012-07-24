using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AcessoDados;
u

namespace Monitor.Passos
{
    class HBD_Pacientes
    {
        //Sigla UNIDADE
        const string mUnidade = "HBD";

        DataSet m_oDataSet;
        string m_sUnidade = mUnidade;

        rpLerDados m_oRP = new rpLerDados();

        /// <summary>
        /// Obtem todos atendimentos a partir do Ultimo Registro.
        /// </summary>
        /// <param name="m_iQtdeRegistros">Quantidade de Registro</param>
        /// <returns></returns>
        public void ProcessarAtendimentosHIS(Int32 m_iQtdeRegistros)
        {
            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            
            ServiceHBD.WSLerAtendimentosSoapClient DadosOrigens = new WSLerAtendimentosSoapClient();

            //Obtem o Ultimo Registro
            m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

            //Obtem as Informações 
            DsDados = DadosOrigens.RetornarAtendimentos(m_sUltimoRegistro, m_iQtdeRegistros);

            //Gravar no DWSATELITE
            //DadosOrigens.GravarAtendimento(mUnidade, DsDados);

            for (int i = 0; i < DsDados.Tables[0].Rows.Count; i++)
            {
                DataRow Dr0 = DsDados.Tables[0].Rows[i];

                DataSet Ds = new DataSet();
                DataTable Dt = new DataTable(DsDados.Tables[0].TableName);

                Dt = DsDados.Tables[0].Clone();

                Dt.ImportRow(Dr0);

                Ds.Tables.Add(Dt);

                //DadosOrigens.GravarAtendimento(mUnidade, Ds);

                m_oDataSet = Ds;

                Salvar();
            }

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

                    sbSQL.Append(" INSERT INTO ATENDIMENTO(cod_pac, cod_prt, tip_atend, data_ent, hora_ent, data_alta, hora_alta, cod_pro, cod_esp, dtcarg, cdund)");
                    sbSQL.Append(" VALUES(");
                    sbSQL.Append(" '");
                    sbSQL.Append(Dr0[0].ToString());
                    sbSQL.Append("','" + Dr0[1].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[2].ToString() + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[3].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[4].ToString()).TimeOfDay + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[5].ToString()).Date.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + Convert.ToDateTime(Dr0[6].ToString()).TimeOfDay + "'");
                    sbSQL.Append(",'" + Dr0[7].ToString() + "'");
                    sbSQL.Append(",'" + Dr0[8].ToString() + "'");
                    sbSQL.Append(",'" + DateTime.Now.ToString("dd/MM/yyyy") + "'");
                    sbSQL.Append(",'" + m_sUnidade.Trim() + "'");
                    sbSQL.Append(")");

                    m_oRP.ExecutarComandoSQL(sbSQL.ToString(), "ATENDIMENTO", "RABhAHQAYQAgAFMAbwB1AHIAYwBlAD0AKABEAEUAUwBDAFIASQBQAFQASQBPAE4APQAoAEEARABEAFIARQBTAFMAXwBMAEkAUwBUAD0AKABBAEQARABSAEUAUwBTAD0AKABQAFIATwBUAE8AQwBPAEwAPQBUAEMAUAApACgASABPAFMAVAA9ADEANwAyAC4AMgAwAC4AMAAuADEANwA5ACkAKABQAE8AUgBUAD0AMQA1ADIAMQApACkAKQAoAEMATwBOAE4ARQBDAFQAXwBEAEEAVABBAD0AKABTAEUAUgBWAEUAUgA9AEQARQBEAEkAQwBBAFQARQBEACkAKABTAEUAUgBWAEkAQwBFAF8ATgBBAE0ARQA9AEQARQBTAEUATgBWADAAMgApACkAKQA7AFUAcwBlAHIAIABJAGQAPQBEAFcAUwBBAFQARQBMAEkAVABFADsAUABhAHMAcwB3AG8AcgBkAD0AZAB3AHMAYQB0AGUAbABpAHQAZQA7AA==");

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
