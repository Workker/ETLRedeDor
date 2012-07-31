using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSLerMedicosTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_medico_deve_retornar_execao()
        {
            WSLerMedicos.WSLerMedicos medico = new WSLerMedicos.WSLerMedicos();
            medico.RetornarMedicos("medicos", 12);
        }
    }
}
