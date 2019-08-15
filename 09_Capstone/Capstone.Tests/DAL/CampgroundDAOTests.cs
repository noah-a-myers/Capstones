using Capstone.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.Tests.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class CampgroundDAOTests : DatabaseTest
    {
        private CampgroundSqlDAO dao;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new CampgroundSqlDAO(ConnectionString);
        }

        [TestMethod]
        public void GetCampgroundTest()
        {
            // Arrange
            int parkId;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM campground",
                    connection);

                connection.Open();

                parkId = Convert.ToInt32(command.ExecuteScalar());
            }

            // Act and Assert
            Assert.AreEqual(2, (dao.GetCampgrounds(parkId).Count));
            Assert.AreEqual("January", (dao.GetCampgrounds(parkId)[0].Open_From_MM));
            Assert.AreEqual("April", (dao.GetCampgrounds(parkId)[1].Open_From_MM));
        }

        [TestMethod]
        public void FindCampgroundIdTest()
        {
            // Arrange
            int campgroundId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT campground_id
                      FROM site
                      WHERE site_number = 5",
                    connection);

                connection.Open();

                campgroundId = Convert.ToInt32(command.ExecuteScalar());
            }

            //Act and Assert
            Assert.AreEqual(campgroundId, dao.FindCampgroundId("The Back Yard"));
        }
    }
}
