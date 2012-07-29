using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AcessoDados;

namespace ETL.Test
{
    [TestFixture]
    public class EngineTest : TesteBase
    {
        string str = System.Configuration.ConfigurationManager.AppSettings["ConectaINT"].ToString();

        [Test]
        public void retornar_data_set_com_sucesso()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);

            var resultado = engine.RetornarDataSet("select * from TBDWD018", "TBDWD018");

            Assert.NotNull(resultado);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void executar_comando_sem_informar_query_deve_retornar_execao()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);

            var resultado = engine.ExecutaComando("");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_data_set_com_query_errada_deve_retornar_execao()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);

            var resultado = engine.RetornarDataSet("select  fom TBDWD018", "TBDWD018");
        }

        [Test]
        public void retornar_consulta_com_sucesso()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);

            var resultado = engine.RetornarConsulta("select * from TBDWD018", "TBDWD018");

            Assert.NotNull(resultado);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void consulta_com_query_errada_deve_retornar_execao()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);
            var resultado = engine.RetornarConsulta("selec from TBDWD018", "TBDWD018");
        }

        [Test]
        public void executar_comando_com_sucesso()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);

            var resultado = engine.ExecutaComando("Update TBDWD018 set IDDWD017 = 4 where IDDWD017 = 2");

            Assert.GreaterOrEqual(resultado, 0);
        }


        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void executar_comando_errado_deve_retornar_execao()
        {
            Engine engine = new Engine(str, Banco.SQLSERVER);

            var resultado = engine.ExecutaComando("Update from TBDWD018 set IDDWD017 = 4 where IDDWD017 = 2");
        }
    }
}
