using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSLerLeitosTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_leitos() 
        {
            WSLerLeitos.WSLerLeitos leitos = new WSLerLeitos.WSLerLeitos();
            leitos.RetornarLeitos();
        }
    }
}
