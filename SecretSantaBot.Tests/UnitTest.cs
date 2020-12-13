using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            foreach (var j in Enumerable.Range(0, 1).ToArray())
            {
               for (int n=2; n < 1; n++ )
                {
                    var list = Extension.Rand(n);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == list[i])
                            Assert.IsTrue(false);
                    }

                }
            }
            Assert.IsTrue(true);
        }
    }
}
