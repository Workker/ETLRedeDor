using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ETL.Test
{
    public abstract class TesteBase
    {
        [SetUp]
        public void SetUp() 
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
