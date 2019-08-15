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
    public class ReservationDAOTests : DatabaseTest
    {
        private ReservationSqlDAO dao;
        private int parkId;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new ReservationSqlDAO(ConnectionString);
        }

        [TestMethod]
        public void MakeReservationTest()
        {
            // Arrange
            int resId1;
            int resId2;
            int resIdReturn2;
            int resIdReturn1;

            Reservation reservation = new Reservation
            {
                Name = "Und R. Doge",
                From_Date = DateTime.Now.AddDays(40),
                To_Date = DateTime.Now.AddDays(45)
            };

            Reservation reservation2 = new Reservation
            {
                Name = "Jeeyai Jhoe",
                From_Date = DateTime.Now.AddDays(45),
                To_Date = DateTime.Now.AddDays(52)
            };

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT s.site_id 
                      FROM site s 
                      JOIN campground c 
                      ON s.campground_id = c.campground_id 
                      WHERE s.site_number = 4 
                      AND c.name = 'The Back Yard'", 
                    connection);

                SqlCommand command2 = new SqlCommand(
                    @"SELECT s.site_id 
                      FROM site s 
                      JOIN campground c 
                      ON s.campground_id = c.campground_id 
                      WHERE s.site_number = 2 
                      AND c.name = 'The Front Yard'",
                    connection);

                SqlCommand command3 = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                reservation.Site_Id = Convert.ToInt32(command.ExecuteScalar());
                reservation2.Site_Id = Convert.ToInt32(command2.ExecuteScalar());

                parkId = Convert.ToInt32(command3.ExecuteScalar());
            }

            // Act
            resId1 = dao.MakeReservation(parkId, reservation);
            resId2 = dao.MakeReservation(parkId, reservation2);

            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT reservation_id FROM reservation WHERE name = 'Und R. Doge'", connection);

                SqlCommand command2 = new SqlCommand(
                    "SELECT reservation_id FROM reservation WHERE name = 'Jeeyai Jhoe'", connection);

                connection.Open();

                resIdReturn1 = Convert.ToInt32(command.ExecuteScalar());
                resIdReturn2 = Convert.ToInt32(command2.ExecuteScalar());
            }

            // Assert
            Assert.AreEqual(4, GetRowCount("reservation"));
            Assert.AreEqual(resIdReturn1, resId1);
            Assert.AreEqual(resIdReturn2, resId2);
        }
    }
}
