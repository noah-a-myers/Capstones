using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class CampgroundTests
    {
        Campground campground;

        [TestInitialize]
        public void Setup()
        {
            campground = new Campground
            {
                Campground_Id = 1,
                Park_Id = 1,
                Name = "Campground",
                Open_From_MM = "01",
                Open_To_MM = "12",
                Daily_Fee = 10.00M
            };
        }
        
        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("#1    Campground                         01             12             $10.00", campground.ToString());
        }

        [TestMethod]
        public void PrintCampgroundHeaderTest()
        {
            Assert.AreEqual("      Name                               Open           Close          Daily Fee", Campground.PrintCampgroundHeader());
        }
    }
}
