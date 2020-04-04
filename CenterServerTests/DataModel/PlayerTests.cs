using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestInitialize()]
        public void Init()
        {
            Config.ConfigManager.Instance.LoadConfig();
        }

        [TestMethod()]
        public void StartManagementTest()
        {
            var user = User.Create(1, "");
            var player = Player.Create(user);

            List<SelectWorkBench> selectBenches = new List<SelectWorkBench>();
            selectBenches.Add(SelectWorkBench.Create(0, 10000, new List<int>() { 400002, 400003 }));
            selectBenches.Add(SelectWorkBench.Create(1, 10001, new List<int>() { 400001, 400004, 400005 }));
            selectBenches.Add(SelectWorkBench.Create(2, 10002, new List<int>() { 400007 }));

            player.StartManagement(0, selectBenches);

            Assert.IsTrue(player.IsStartManagement() == ManagementState.Doing);

        }

        [TestMethod()]
        public void CreateTest()
        {
            //DBMgr.Init();
           
            //var user = User.Create(1, "");
            //var player = Player.Create(user);
            //Assert.IsTrue(player.coin == 0);
        }
    }
}