using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSLerMedicoEspecialidadeTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_medicos_especialidades() 
        {
            WSLerMedicoEspecialidade.WSLerMedicoEspecialidade medico = new WSLerMedicoEspecialidade.WSLerMedicoEspecialidade();
            medico.RetornarMedicosEspecialidades();
        }
    }
}
