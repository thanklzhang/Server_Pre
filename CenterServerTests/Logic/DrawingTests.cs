using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class DrawingTests
    {
        [TestInitialize()]
        public void Init()
        {
            Config.ConfigManager.Instance.LoadConfig();
        }

        [TestMethod()]
        public void GetOutItemTest()
        {
            var drawing = Drawing.Create(400005);
            var item = drawing.GetOutItem();
            Assert.IsTrue(item != null && item.config.SN == 200005);
        }
    }
}