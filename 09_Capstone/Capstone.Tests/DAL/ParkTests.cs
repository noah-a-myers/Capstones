using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class ParkTests
    {
        Park park;

        [TestInitialize]
        public void Setup()
        {
            park = new Park
            {
                Park_Id = 1,
                Name = "Park",
                Location = "Location",
                Establish_date = DateTime.Parse("2000-01-01"),
                Area = 2,
                Visitors = 3,
                Description = "Description"
            };
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual(@"Park
Location:           Location
Established:        1/1/2000
Area:               2 sq mi
Annual Visitors:    3

Description
", park.ToString());
        }
    }
}
