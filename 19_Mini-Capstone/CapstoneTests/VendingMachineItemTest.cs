using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineItemTest
    {
        [TestInitialize]
        public void Initialize()
        {
            VendingMachineItem newItem = new VendingMachineItem();
        }

        [TestMethod]
        public void SlotTest()
        {
            Assert.AreEqual("A1", newItem.Slot = "A1");
        }
    }
}
