using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exercises.Test
{
    [TestClass]
    public class StringCalculatorTest
    {
        StringCalculator newObject;

        [TestInitialize]
        public void Initialize()
        {
            newObject = new StringCalculator();
        }

        [TestMethod]
        public void StringCalculatorTestNull()
        {
            Assert.AreEqual(0, newObject.Add(""));
        }

        [TestMethod]
        public void StringCalculatorTestOneNumber()
        {
            Assert.AreEqual(1, newObject.Add("1"));
            Assert.AreEqual(3, newObject.Add("3"));
        }

        [TestMethod]
        public void StringCalculatorTestCommaTwoNumbers()
        {
            Assert.AreEqual(3, newObject.Add("1,2"));
            Assert.AreEqual(6, newObject.Add("3,3"));
        }

        [TestMethod]
        public void StringCalculatorTestCommaMultipleNumbers()
        {
            Assert.AreEqual(24, newObject.Add("5,7,12"));
            Assert.AreEqual(17, newObject.Add("6,2,1,8"));
        }

        [TestMethod]
        public void StringCalculatorTestWeirdNumbers()
        {
            Assert.AreEqual(10, newObject.Add("5\n3,2"));
            Assert.AreEqual(14, newObject.Add("3\n5\n2,4"));
        }
    }

    [TestClass]
    public class NumbersToWordsTest
    {
        NumbersToWords newObject;

        [TestInitialize]
        public void Initialize()
        {
            newObject = new NumbersToWords();
        }

        [TestMethod]
        public void OneDigitNumbersTest()
        {

            Assert.AreEqual("Zero", newObject.ConvertNumberToWords(0));
            Assert.AreEqual("One", newObject.ConvertNumberToWords(1));
            Assert.AreEqual("Two", newObject.ConvertNumberToWords(2));
            Assert.AreEqual("Three", newObject.ConvertNumberToWords(3));
            Assert.AreEqual("Four", newObject.ConvertNumberToWords(4));
            Assert.AreEqual("Five", newObject.ConvertNumberToWords(5));
            Assert.AreEqual("Six", newObject.ConvertNumberToWords(6));
            Assert.AreEqual("Seven", newObject.ConvertNumberToWords(7));
            Assert.AreEqual("Eight", newObject.ConvertNumberToWords(8));
            Assert.AreEqual("Nine", newObject.ConvertNumberToWords(9));

        }
    }
}