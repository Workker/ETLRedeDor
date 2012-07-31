using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSLerAtendimentosTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_ultimo_registro_deve_retornar_execao()
        {
            WSLerAtendimentos.WSLerAtendimentos paciente = new WSLerAtendimentos.WSLerAtendimentos();
            paciente.RetornarUltimoRegistro("unidade");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_pacientes_deve_retornar_execao()
        {
            WSLerAtendimentos.WSLerAtendimentos paciente = new WSLerAtendimentos.WSLerAtendimentos();
            paciente.GravarAtendimento("unidade", new System.Data.DataSet());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_conciliacao_pacientes_deve_retornar_execao()
        {
            WSLerAtendimentos.WSLerAtendimentos paciente = new WSLerAtendimentos.WSLerAtendimentos();
            paciente.RetornarAtendimentoConciliacao("date", "conexao", "schema");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void gravar_pacientes_deve_retornar_execao()
        {
            WSLerAtendimentos.WSLerAtendimentos paciente = new WSLerAtendimentos.WSLerAtendimentos();
            paciente.RetornarAtendimentos("registero", 12, "conexao", "schema");
        }
    }
}
