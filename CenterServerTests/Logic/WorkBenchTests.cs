using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class WorkBenchTests
    {
        [TestInitialize()]
        public void Init()
        {
            Config.ConfigManager.Instance.LoadConfig();
        }

        [TestMethod()]
        public void StartTest()
        {
            var bench = WorkBench.Create(0);
            var selectBench = SelectWorkBench.Create(0, 10000, new List<int>() { 400003, 400005 });
            //bench.Start(10000, selectBench.workProjects);

            //Assert.IsTrue(bench.workerGuid == 10000);
            //Assert.IsTrue(bench.index == 0);
            //var projects = bench.GetAllProjects();
            //Assert.IsTrue(projects.Count > 0);

        }

        [TestMethod()]
        public void ForceALlFinishTest()
        {
            var bench = WorkBench.Create(0);
            var selectBench = SelectWorkBench.Create(0, 10000, new List<int>() { 400003, 400005 });
            //bench.Start(10000, selectBench.workProjects);

            //bench.ForceAllFinish();

            //Assert.IsTrue(bench.GetAllProjects().Exists(p => p.state == WorkProjectState.Finish));
        }

        [TestMethod()]
        public void GetAllFinishOutItemsTest()
        {
            var bench = WorkBench.Create(0);
            var selectBench = SelectWorkBench.Create(0, 10000, new List<int>() { 400003, 400005 });
            //bench.Start(10000, selectBench.workProjects);

            //bench.ForceAllFinish();

            //var items = bench.GetAllFinishOutItems();
            //Assert.IsTrue(items.Exists(item => item.config.SN == 200003));
            //Assert.IsTrue(items.Exists(item => item.config.SN == 200005));
        }
    }
}