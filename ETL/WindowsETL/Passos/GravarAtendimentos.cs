using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AcessoDados;
using WindowsETL.SrvGravarAtendHBD;

namespace WindowsETL.Passos
{
    class GravarAtendimentos
    {
        public string mUnidade { get; set; }

        rpLerDados m_oRP = new rpLerDados();

        public void ProcessarAtendimentosINT(Int32 m_iQtdeRegistros)
        {
            string m_sUltimoRegistro;
            DataSet DsDados = new DataSet();

            SrvGravarAtendHBD.WSGravarAtendimentosSoapClient DadosOrigens = new WSGravarAtendimentosSoapClient();

            try
            {
                //Obtem o Ultimo Registro
                m_sUltimoRegistro = DadosOrigens.RetornarUltimoRegistro(mUnidade);

                //Obtem as Informações 
                DsDados = DadosOrigens.RetornarAtendimentos(mUnidade, m_sUltimoRegistro, m_iQtdeRegistros);

                //Gravar no INTEGRADOR
                DadosOrigens.GravarAtendimento(mUnidade, DsDados);
            }
            catch (Exception en)
            {
                throw new Exception(en.Message);
            }
        }
    }
}
