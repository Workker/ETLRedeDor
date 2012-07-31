using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WSGravarAtendimento;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSGravarAtendimentoTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void gravar_atendimento() 
        {
            WSGravarAtendimento.WSGravarAtendimento atendimento = new WSGravarAtendimento.WSGravarAtendimento();
            atendimento.GravarAtendimento("unidades", new System.Data.DataSet());
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_atendimentos()
        {
            WSGravarAtendimento.WSGravarAtendimento atendimento = new WSGravarAtendimento.WSGravarAtendimento();
            atendimento.RetornarAtendimentos("unidades", "ultimoRegistro", 12);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_ultimo_registro()
        {
            WSGravarAtendimento.WSGravarAtendimento atendimento = new WSGravarAtendimento.WSGravarAtendimento();
            atendimento.RetornarUltimoRegistro("unidades");
        }
    }
}
