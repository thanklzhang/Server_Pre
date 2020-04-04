using DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class ManagementTests
    {
        [TestInitialize()]
        public void Init()
        {
            Config.ConfigManager.Instance.LoadConfig();
        }

        [TestMethod()]
        public void StartNormalTest()
        {
            var user = User.Create(1, "");
            var player = Player.Create(user);

            List<SelectWorkBench> selectBenches = new List<SelectWorkBench>();
            selectBenches.Add(SelectWorkBench.Create(0, 10000, new List<int>() { 400002, 400003 }));
            selectBenches.Add(SelectWorkBench.Create(1, 10001, new List<int>() { 400001, 400004, 400005 }));
            selectBenches.Add(SelectWorkBench.Create(2, 10002, new List<int>() { 400007 }));

            Management management = Management.Create(player);
            management.StartNormal(selectBenches, player);

            Assert.IsTrue(management.GetAllBenches().Exists(p => p.index == 0));
            Assert.IsTrue(management.GetAllBenches().Exists(p => p.index == 1));
            Assert.IsTrue(management.GetAllBenches().Exists(p => p.index == 2));

            Assert.IsTrue(management.GetAllBenches().Exists(p => p.workerGuid == 10000));
            Assert.IsTrue(management.GetAllBenches().Exists(p => p.workerGuid == 10001));
            Assert.IsTrue(management.GetAllBenches().Exists(p => p.workerGuid == 10002));

            Assert.IsTrue(management.GetAllBenches().Exists(p => p.GetAllProjects().Exists(project => project.currDrawing?.SN == 400002)));
            Assert.IsTrue(management.GetAllBenches().Exists(p => p.GetAllProjects().Exists(project => project.currDrawing?.SN == 400007)));
        }

        [TestMethod()]
        public void FinishTest()
        {
            var user = User.Create(1,"");
            var player = Player.Create(user);

            List<SelectWorkBench> selectBenches = new List<SelectWorkBench>();
            selectBenches.Add(SelectWorkBench.Create(0, 10000, new List<int>() { 400002, 400003 }));
            selectBenches.Add(SelectWorkBench.Create(1, 10001, new List<int>() { 400001, 400004, 400005 }));
            selectBenches.Add(SelectWorkBench.Create(2, 10002, new List<int>() { 400007 }));

            Management management = Management.Create(player);
            management.StartNormal(selectBenches, player);

            management.Finish();

            var items = player.GetOwnItems();
            Assert.IsTrue(items.Count > 0);
            Assert.IsTrue(items.TrueForAll(item => item != null));
            Assert.IsTrue(player.GetOwnItems().Exists(item => item.config.SN == 200002));
            Assert.IsTrue(player.GetOwnItems().Exists(item => item.config.SN == 200003));
            Assert.IsTrue(player.GetOwnItems().Exists(item => item.config.SN == 200007));
        }
    }
}