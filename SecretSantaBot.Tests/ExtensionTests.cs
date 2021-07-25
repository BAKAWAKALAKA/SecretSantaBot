using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSantaBot;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSantaBot.Tests
{
    [TestClass()]
    public class ExtensionTests
    {
        [TestMethod()]
        public void RandTest()
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
    }
}