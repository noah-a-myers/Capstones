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
    public class SiteDAOTests : DatabaseTest
    {
        private SiteSqlDAO dao;
        private int parkId;
        private int campgroundId;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new SiteSqlDAO(ConnectionString);
        }

        [TestMethod]
        public void GetAvailableSitesInCampgroundTest()
        {
            // Arrange
            Reservation reservation = new Reservation
            {
                Name = "Und R. Doge",
                From_Date = DateTime.Now,
                To_Date = DateTime.Now.AddDays(7)
            };

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT p.park_id 
                      FROM park p
                      JOIN campground c
                      ON p.park_id = c.park_id
                      JOIN site s 
                      ON c.campground_id = s.campground_id 
                      WHERE s.site_number = 1 
                      AND c.campground_id = (SELECT c.campground_id 
                                             FROM campground c 
                                             WHERE c.name = 'The Back Yard')",
                    connection);
                SqlCommand command2 = new SqlCommand(
                    @"SELECT s.campground_id 
                      FROM site s 
                      JOIN campground c 
                      ON c.campground_id = s.campground_id 
                      WHERE s.site_number = 1 
                      AND c.name = 'The Back Yard'",
                    connection);

                connection.Open();

                parkId = Convert.ToInt32(command.ExecuteScalar());
                campgroundId = Convert.ToInt32(command2.ExecuteScalar());
            }

            // Act and Assert
            Assert.AreEqual(6, (dao.GetAvailableSitesInCampground(parkId, campgroundId, reservation).Count));
            Assert.AreEqual(2, (dao.GetAvailableSitesInCampground(parkId, campgroundId, reservation)[0].Max_Occupancy));
        }

        [TestMethod]
        public void GetAvailableSitesParkwideTest()
        {
            // Arrange
            Reservation reservation = new Reservation
            {
                Name = "Jeeyai Jhoe",
                From_Date = DateTime.Now.AddDays(7),
                To_Date = DateTime.Now.AddDays(12)
            };

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT park_id
                      FROM park
                      WHERE name = 'Casa de Jesse'",
                    connection);

                connection.Open();

                parkId = Convert.ToInt32(command.ExecuteScalar());
            }

            // Act and Assert
            Assert.AreEqual(8, (dao.GetAvailableSitesParkwide(parkId, reservation).Count));
            Assert.AreEqual(6, (dao.GetAvailableSitesParkwide(parkId, reservation)[1].Max_Occupancy));
        }

        [TestMethod]
        public void FindSiteIdFromSiteNumberTest()
        {
            // Arrange
            int campgroundId1;
            int campgroundId2;
            int siteId1;
            int siteId2;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT campground_id
                      FROM campground
                      WHERE name = 'The Back Yard'",
                    connection);

                SqlCommand command2 = new SqlCommand(
                    @"SELECT campground_id
                      FROM campground
                      WHERE name = 'The Front Yard'",
                    connection);

                SqlCommand command3 = new SqlCommand(
                    @"SELECT site_id
                      FROM site
                      WHERE site_number = 2
                      AND campground_id = @campground_id",
                    connection);

                SqlCommand command4 = new SqlCommand(
                    @"SELECT site_id
                      FROM site
                      WHERE site_number = 1
                      AND campground_id = @campground_id",
                    connection);

                connection.Open();

                campgroundId1 = Convert.ToInt32(command.ExecuteScalar());
                campgroundId2 = Convert.ToInt32(command2.ExecuteScalar());

                command3.Parameters.AddWithValue("@campground_id", campgroundId1);
                command4.Parameters.AddWithValue("@campground_id", campgroundId2);

                siteId1 = Convert.ToInt32(command3.ExecuteScalar());
                siteId2 = Convert.ToInt32(command4.ExecuteScalar());
            }

            Assert.AreEqual(siteId1, dao.FindSiteIdFromSiteNumber(2, campgroundId1));
            Assert.AreEqual(siteId2, dao.FindSiteIdFromSiteNumber(1, campgroundId2));
        }
    }
}
