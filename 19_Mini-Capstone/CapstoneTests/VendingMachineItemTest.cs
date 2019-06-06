using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class VendingMachineItemTest
    {
        VendingMachineItem newItem;
        [TestInitialize]
        public void Initialize()
        {
            newItem = new VendingMachineItem();
        }

        [TestMethod]
        public void SlotTest()
        {
            Assert.AreEqual("A1", newItem.Slot = "A1");
            Assert.AreEqual("B2", newItem.Slot = "B2");
            Assert.AreEqual("C5", newItem.Slot = "C5");
            Assert.AreEqual("N0", newItem.Slot = "N0");
        }

        [TestMethod]
        public void ProductNameTest()
        {
            Assert.AreEqual("Cheesy Puffs", newItem.ProductName = "Cheesy Puffs");
            Assert.AreEqual("Ranchero Chipos", newItem.ProductName = "Ranchero Chipos");
            Assert.AreEqual("Cosmic Booze", newItem.ProductName = "Cosmic Booze");
            Assert.AreEqual("Everlasting Gobbstopers", newItem.ProductName = "Everlasting Gobbstopers");
        }

        [TestMethod]
        public void PriceTest()
        {
            Assert.AreEqual(18.50M, newItem.Price = 18.50M);
            Assert.AreEqual(2.25M, newItem.Price = 2.25M);
            Assert.AreEqual(.95M, newItem.Price = .95M);
            Assert.AreEqual(1.30M, newItem.Price = 1.30M);
        }

        [TestMethod]
        public void QuantityTest()
        {
            Assert.AreEqual(5, newItem.Quantity);
            Assert.AreEqual(1, newItem.Quantity = 1);
            Assert.AreEqual(3, newItem.Quantity = 3);
            Assert.AreEqual(0, newItem.Quantity = 0);
        }

        [TestMethod]
        public void ConsumeTest()
        {
            Assert.AreEqual("","");
        }
    }
}
