using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSGravarPacientesTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_ultimo_registro_deve_retornar_execao() 
        {
            WSGravarPacientes.WSGravarPacientes paciente = new WSGravarPacientes.WSGravarPacientes();
            paciente.RetornarUltimoRegistro("unidade");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_pacientes_deve_retornar_execao()
        {
            WSGravarPacientes.WSGravarPacientes paciente = new WSGravarPacientes.WSGravarPacientes();
            paciente.RetornarPacientes("unidade", "UtimoRegistro", 12);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_conciliacao_pacientes_deve_retornar_execao()
        {
            WSGravarPacientes.WSGravarPacientes paciente = new WSGravarPacientes.WSGravarPacientes();
            paciente.RetornarConciliacaoPacientes("unidade", 12);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void gravar_pacientes_deve_retornar_execao()
        {
            WSGravarPacientes.WSGravarPacientes paciente = new WSGravarPacientes.WSGravarPacientes();
            paciente.GravarPacientes("unidade", new System.Data.DataSet());
        }
    }
}
