using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class SiteTests
    {
        Site site;

        [TestInitialize]
        public void Setup()
        {
            site = new Site
            {
                Site_Id = 1,
                Campground_Id = 2,
                Campground_Name = "Campground Name",
                Site_Number = 3,
                Max_Occupancy = 4,
                Accessible = true,
                Max_RV_Length = 20,
                Utilities = true,
                DailyFee = 10.00M
            };
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("3              4              Yes                 20                  Yes            ", site.ToString());
        }

        [TestMethod]
        public void ToStringWithNameTest()
        {
            Assert.AreEqual("Campground Name                    3              4              Yes                 20                  Yes            ", site.ToStringWithName());
        }

        [TestMethod]
        public void PrintSiteHeadersTest()
        {
            Assert.AreEqual("Site No.       Max Occup.     Accessible?         Max RV Length       Utility        Cost", Site.PrintSiteHeaders());
        }

        [TestMethod]
        public void PrintSiteHeadersWithNameTest()
        {
            Assert.AreEqual("Campground                         Site No.       Max Occup.     Accessible?         Max RV Length       Utility        Cost", Site.PrintSiteHeadersWithName());
        }
    }
}
