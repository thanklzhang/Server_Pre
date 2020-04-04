using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class WorkProjectTests
    {
        [TestInitialize()]
        public void Init()
        {
            Config.ConfigManager.Instance.LoadConfig();
        }

        [TestMethod()]
        public void CreateTest()
        {
            var workProject = WorkProject.Create();
            Assert.IsTrue(workProject != null);
        }

        [TestMethod()]
        public void SetTest()
        {
            var workProject = WorkProject.Create();
            //workProject.SetPreStart(400003);
            //Assert.IsTrue(workProject.state == WorkProjectState.Pre);
            //Assert.IsTrue(workProject.currDrawing != null);
            //Assert.IsTrue(workProject.during > 0);
            //Assert.IsTrue(null == workProject.finishOutItem);
        }

        [TestMethod()]
        public void StartTest()
        {
            var workProject = WorkProject.Create();
            //workProject.SetPreStart(400003);
            //workProject.Start();
            //Assert.IsTrue(workProject.state == WorkProjectState.Doing);
            //Assert.IsTrue(workProject.lastStartTime > 0);
        }

        [TestMethod()]
        public void ForceFinishTest()
        {
            var workProject = WorkProject.Create();
            //workProject.SetPreStart(400003);
            //workProject.Start();
            //workProject.ForceFinish(null);
            //Assert.IsTrue(workProject.state == WorkProjectState.Finish);
            //Assert.IsTrue(workProject.finishOutItem != null &&
            //    workProject.finishOutItem.config.SN == 200003);
        }
    }
}