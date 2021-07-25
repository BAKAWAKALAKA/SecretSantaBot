using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SecretSantaBot.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void RandomListTest()
        {
            var res = new List<List<int>>();

                    var list = Extension.Rand(10);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == list[i])
                            Assert.IsTrue(false);
                    }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ROPTest()
        {
            var t = Extension.ROP(0, "test");
            Assert.IsTrue(true);
        }
    }
}
