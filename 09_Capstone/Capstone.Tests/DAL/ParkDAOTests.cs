using Capstone.DAL;
using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectOrganizer.Tests.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.Tests.DAL
{
    [TestClass]
    public class ParkDAOTests : DatabaseTest
    {
        private ParkSqlDAO dao;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new ParkSqlDAO(ConnectionString);
        }

        [TestMethod]
        public void GetParkTest()
        {
            // Arrange
            int parkId;
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE location = 'Ohio'",
                    connection);
                          
                connection.Open();

                parkId = Convert.ToInt32(command.ExecuteScalar());
            }

            // Act and Assert
            Assert.AreEqual("Casa de Jesse National Park", (dao.GetPark(parkId).Name));
        }

        [TestMethod]
        public void GetParkListTest()
        {
            // Arrange
            List<Park> parks = new List<Park>();
            int parkId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE location = 'Ohio'",
                    connection);

                connection.Open();

                parkId = Convert.ToInt32(command.ExecuteScalar());
            }

            // Act
            parks = dao.GetParkList();

            // Assert
            Assert.AreEqual(parkId, parks[0].Park_Id);
        }
    }
}
