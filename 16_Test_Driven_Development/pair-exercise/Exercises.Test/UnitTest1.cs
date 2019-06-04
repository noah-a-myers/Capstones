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
            Assert.AreEqual("zero", newObject.ConvertNumberToWords(0));
            Assert.AreEqual("one", newObject.ConvertNumberToWords(1));
            Assert.AreEqual("two", newObject.ConvertNumberToWords(2));
            Assert.AreEqual("three", newObject.ConvertNumberToWords(3));
            Assert.AreEqual("four", newObject.ConvertNumberToWords(4));
            Assert.AreEqual("five", newObject.ConvertNumberToWords(5));
            Assert.AreEqual("six", newObject.ConvertNumberToWords(6));
            Assert.AreEqual("seven", newObject.ConvertNumberToWords(7));
            Assert.AreEqual("eight", newObject.ConvertNumberToWords(8));
            Assert.AreEqual("nine", newObject.ConvertNumberToWords(9));
        }

        [TestMethod]
        public void TwoDigitNumbersTest()
        {
            Assert.AreEqual("eleven", newObject.ConvertNumberToWords(11));
            Assert.AreEqual("twelve", newObject.ConvertNumberToWords(12));
            Assert.AreEqual("thirteen", newObject.ConvertNumberToWords(13));
            Assert.AreEqual("fourteen", newObject.ConvertNumberToWords(14));
            Assert.AreEqual("fifteen", newObject.ConvertNumberToWords(15));
            Assert.AreEqual("sixteen", newObject.ConvertNumberToWords(16));
            Assert.AreEqual("seventeen", newObject.ConvertNumberToWords(17));
            Assert.AreEqual("eighteen", newObject.ConvertNumberToWords(18));
            Assert.AreEqual("nineteen", newObject.ConvertNumberToWords(19));
            Assert.AreEqual("twenty", newObject.ConvertNumberToWords(20));
            Assert.AreEqual("twenty one", newObject.ConvertNumberToWords(21));
        }

        [TestMethod]
        public void ThreeDigitNumbersTest()
        {
            Assert.AreEqual("one hundred fifty two", newObject.ConvertNumberToWords(152));
            Assert.AreEqual("one hundred", newObject.ConvertNumberToWords(100));
            Assert.AreEqual("one hundred twenty", newObject.ConvertNumberToWords(120));
            Assert.AreEqual("one hundred fifteen", newObject.ConvertNumberToWords(115));
        }

        [TestMethod]
        public void FourDigitNumbersTest()
        {
            Assert.AreEqual("one thousand", newObject.ConvertNumberToWords(1000));
            Assert.AreEqual("one thousand two hundred", newObject.ConvertNumberToWords(1200));
            Assert.AreEqual("one thousand two hundred twenty", newObject.ConvertNumberToWords(1220));
            Assert.AreEqual("one thousand two hundred twenty two", newObject.ConvertNumberToWords(1222));
            Assert.AreEqual("one thousand two hundred fifteen", newObject.ConvertNumberToWords(1215));
        }

        [TestMethod]
        public void FiveDigitNumbersTest()
        {
            Assert.AreEqual("ten thousand", newObject.ConvertNumberToWords(10000));
            Assert.AreEqual("eleven thousand", newObject.ConvertNumberToWords(11000));
            Assert.AreEqual("twenty thousand", newObject.ConvertNumberToWords(20000));
            Assert.AreEqual("twenty one thousand", newObject.ConvertNumberToWords(21000));
            Assert.AreEqual("ten thousand five hundred", newObject.ConvertNumberToWords(10500));
            Assert.AreEqual("eleven thousand five hundred", newObject.ConvertNumberToWords(11500));
            Assert.AreEqual("twenty thousand five hundred", newObject.ConvertNumberToWords(20500));
            Assert.AreEqual("twenty one thousand five hundred", newObject.ConvertNumberToWords(21500));
            Assert.AreEqual("ten thousand five hundred fifty", newObject.ConvertNumberToWords(10550));
            Assert.AreEqual("eleven thousand five hundred seventeen", newObject.ConvertNumberToWords(11517));
            Assert.AreEqual("twenty thousand five hundred five", newObject.ConvertNumberToWords(20505));
            Assert.AreEqual("twenty one thousand five hundred ten", newObject.ConvertNumberToWords(21510));
        }

        [TestMethod]
        public void SixDigitNumbersTest()
        {
            Assert.AreEqual("one hundred ten thousand", newObject.ConvertNumberToWords(110000));
            Assert.AreEqual("one hundred eleven thousand", newObject.ConvertNumberToWords(111000));
            Assert.AreEqual("one hundred twenty thousand", newObject.ConvertNumberToWords(120000));
            Assert.AreEqual("one hundred twenty one thousand", newObject.ConvertNumberToWords(121000));
            Assert.AreEqual("one hundred ten thousand five hundred", newObject.ConvertNumberToWords(110500));
            Assert.AreEqual("one hundred eleven thousand five hundred", newObject.ConvertNumberToWords(111500));
            Assert.AreEqual("one hundred twenty thousand five hundred", newObject.ConvertNumberToWords(120500));
            Assert.AreEqual("one hundred twenty one thousand five hundred", newObject.ConvertNumberToWords(121500));
            Assert.AreEqual("one hundred ten thousand five hundred fifty", newObject.ConvertNumberToWords(110550));
            Assert.AreEqual("one hundred eleven thousand five hundred seventeen", newObject.ConvertNumberToWords(111517));
            Assert.AreEqual("one hundred twenty thousand five hundred five", newObject.ConvertNumberToWords(120505));
            Assert.AreEqual("one hundred twenty one thousand five hundred ten", newObject.ConvertNumberToWords(121510));
        }
    }
}