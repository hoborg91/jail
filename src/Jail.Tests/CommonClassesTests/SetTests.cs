using Jail.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jail.Tests.CommonClassesTests {
    [TestFixture]
    public class SetTests {
        [Test]
        public void T() {
            // Arrange, Act, Assert
            var set = new Set<int>();
            set.Add(1);
            set.Add(2);
            Assert.AreEqual(2, set.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, set);

            set.Add(2);
            Assert.AreEqual(2, set.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, set);

            set.Remove(2);
            Assert.AreEqual(1, set.Count);
            CollectionAssert.AreEquivalent(new[] { 1 }, set);
        }
    }
}
