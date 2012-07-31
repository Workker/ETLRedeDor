using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSLerPacientesTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void gravar_paciente_deve_retornar_execao()
        {
            LerPacientesWPD paciente = new LerPacientesWPD();
            paciente.GravarPaciente("unidade", new System.Data.DataSet());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_pacientes_deve_retornar_execao()
        {
            LerPacientesWPD paciente = new LerPacientesWPD();
            paciente.RetornarPacientes("unidade", 12, "", "");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_paciente_conciliacao_deve_retornar_execao()
        {
            LerPacientesWPD paciente = new LerPacientesWPD();
            paciente.RetornarPacientesConciliacao("unidade");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_Ultimo_registro_deve_retornar_execao()
        {
            LerPacientesWPD paciente = new LerPacientesWPD();
            paciente.RetornarUltimoRegistro("uniudade");
        }
    }
}
