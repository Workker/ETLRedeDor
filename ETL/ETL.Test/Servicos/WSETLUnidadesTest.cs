using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WsETLUnidades;
using AcessoDados;

namespace ETL.Test.Servicos
{
    [TestFixture]
    public class WSETLUnidadesTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void retornar_unidades__sem_a_tabela_retornar_execao()
        {
            WSETLUnidades unidades = new WSETLUnidades();
            unidades.RetornarUnidades();
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void apagar_paciente_satelite_sem_a_tabela_retornar_execao() 
        {
            WSETLUnidades unidades = new WSETLUnidades();
            unidades.ApagarPacientesSatelite("producao");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(BusinessException))]
        public void apagar_atendimento_satelite_sem_tabela_deve_retornar_execao() 
        {
            WSETLUnidades unidades = new WSETLUnidades();
            unidades.ApagarAtendimentosSatelite("producao");
        }

    }
}
