using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    
    [TestClass()]
    public class ItemTests
    {
        [TestInitialize()]
        public void Init()
        {
            Config.ConfigManager.Instance.LoadConfig();
        }
        
        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }
    }
}