using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSLerEspecialidadesTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_especialidade() 
        {
            WSLerEspecialidades.WSLerEspecialidades especialidade = new WSLerEspecialidades.WSLerEspecialidades();
            especialidade.RetornarEspecialidades();
        }
    }
}
