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
    public class SearchDAOTests : DatabaseTest
    {
        private SearchSqlDAO dao;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new SearchSqlDAO(ConnectionString);
        }

        [TestMethod]
        public void ArrivalAndDepartureTest()
        {
            string result = dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));

            Assert.AreEqual(@"AND s.site_id not in (SELECT site_id FROM reservation WHERE
								         (@from_date BETWEEN r.from_date AND DATEADD(day, -1, r.to_date)
								          OR @to_date BETWEEN DATEADD(day, 1, r.from_date) AND r.to_date)
								          AND MONTH(@from_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm
								          AND MONTH(@to_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm)", result);
        }

        [TestMethod]
        public void AccessibleTest()
        {
            string result = dao.Accessible(true);

            Assert.AreEqual($"\nAND s.accessible = @isAccessible", result);
        }

        [TestMethod]
        public void MaxRVLengthTest()
        {
            string result = dao.MaxRVLength(35);

            Assert.AreEqual($"\nAND s.max_rv_length >= @max_rv_length", result);
        }

        [TestMethod]
        public void UtilityTest()
        {
            string result = dao.Utility(true);

            Assert.AreEqual($"\nAND s.utilities = @hasUtilities", result);
        }

        [TestMethod]
        public void MaxOccupancyTest()
        {
            string result = dao.MaxOccupancy(6);

            Assert.AreEqual($"\nAND s.max_occupancy >= @max_occupancy", result);
        }

        [TestMethod]
        public void CampgroundLevelSearchTest()
        {
            string result = dao.CampgroundLevelSearch(1);

            Assert.AreEqual(@"SELECT TOP 5 c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee
                                   FROM site s
                                   LEFT JOIN reservation r
                                   ON s.site_id = r.site_id
                                   JOIN campground c
								   ON s.campground_id = c.campground_id
                                   WHERE s.campground_id = @campground_id
                                   AND r.site_id is null
                                   
GROUP BY s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.name, c.daily_fee", result);
        }

        [TestMethod]
        public void CampgroundLevelSearchWithFiltersTest()
        {
            dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));
            dao.Accessible(true);
            dao.MaxRVLength(20);
            dao.Utility(true);
            dao.MaxOccupancy(6);

            string result = dao.CampgroundLevelSearch(1);
            string expected = @"SELECT TOP 5 c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee
                                   FROM site s
                                   LEFT JOIN reservation r
                                   ON s.site_id = r.site_id
                                   JOIN campground c
								   ON s.campground_id = c.campground_id
                                   WHERE s.campground_id = @campground_id
                                   AND r.site_id is null
                                   AND s.site_id not in (SELECT site_id FROM reservation WHERE
								         (@from_date BETWEEN r.from_date AND DATEADD(day, -1, r.to_date)
								          OR @to_date BETWEEN DATEADD(day, 1, r.from_date) AND r.to_date)
								          AND MONTH(@from_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm
								          AND MONTH(@to_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm)
AND s.accessible = @isAccessible
AND s.max_rv_length >= @max_rv_length
AND s.utilities = @hasUtilities
AND s.max_occupancy >= @max_occupancy
GROUP BY s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.name, c.daily_fee";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParkLevelSearchTest()
        {
            string result = dao.ParkLevelSearch(1);

            Assert.AreEqual(@"SELECT c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee
                                   FROM site s
                                   FULL JOIN reservation r
                                   ON s.site_id = r.site_id
                                   JOIN campground c
								   ON s.campground_id = c.campground_id
                                   WHERE c.park_id = @park_id
                                   
GROUP BY s.campground_id, s.site_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.name, c.daily_fee", result);
        }

        [TestMethod]
        public void ParkLevelSearchWithFiltersTest()
        {
            dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));
            dao.Accessible(true);
            dao.MaxRVLength(20);
            dao.Utility(true);
            dao.MaxOccupancy(6);

            string result = dao.ParkLevelSearch(1);
            string expected = @"SELECT c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee
                                   FROM site s
                                   FULL JOIN reservation r
                                   ON s.site_id = r.site_id
                                   JOIN campground c
								   ON s.campground_id = c.campground_id
                                   WHERE c.park_id = @park_id
                                   AND s.site_id not in (SELECT site_id FROM reservation WHERE
								         (@from_date BETWEEN r.from_date AND DATEADD(day, -1, r.to_date)
								          OR @to_date BETWEEN DATEADD(day, 1, r.from_date) AND r.to_date)
								          AND MONTH(@from_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm
								          AND MONTH(@to_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm)
AND s.accessible = @isAccessible
AND s.max_rv_length >= @max_rv_length
AND s.utilities = @hasUtilities
AND s.max_occupancy >= @max_occupancy
GROUP BY s.campground_id, s.site_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.name, c.daily_fee";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RunCampgroundLevelSearch()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);

            Assert.AreEqual(5, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunCampgroundLevelSearchPlusAccessibe()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);
            dao.Accessible(true);

            Assert.AreEqual(3, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunCampgroundLevelSearchPlusMaxRV()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);
            dao.MaxRVLength(20);

            Assert.AreEqual(3, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunCampgroundLevelSearchPlusUtility()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);
            dao.Utility(true);

            Assert.AreEqual(4, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunCampgroundLevelSearchPlusMaxOcc()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);
            dao.MaxOccupancy(6);

            Assert.AreEqual(3, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunCampgroundLevelSearchPlusArrDep()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);
            dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));

            Assert.AreEqual(5, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunCampgroundLevelSearchPlusAllFilters()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT campground_id FROM campground WHERE name = 'The Back Yard'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.CampgroundLevelSearch(ID);
            dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));
            dao.MaxOccupancy(6);
            dao.Utility(true);
            dao.MaxRVLength(20);
            dao.Accessible(true);

            Assert.AreEqual(1, dao.RunCampgroundLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearch()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);

            Assert.AreEqual(7, dao.RunParkLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearchPlusAccessibe()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);
            dao.Accessible(true);

            Assert.AreEqual(6, dao.RunParkLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearchPlusMaxRV()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);
            dao.MaxRVLength(20);

            Assert.AreEqual(5, dao.RunParkLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearchPlusUtility()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);
            dao.Utility(true);

            Assert.AreEqual(6, dao.RunParkLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearchPlusMaxOcc()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);
            dao.MaxOccupancy(6);

            Assert.AreEqual(4, dao.RunParkLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearchPlusArrDep()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);
            dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));

            Assert.AreEqual(7, dao.RunParkLevelSearch().Count);
        }

        [TestMethod]
        public void RunParkLevelSearchPlusAllFilters()
        {
            //List<Site> sites = new List<Site>();
            int ID;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(
                    "SELECT park_id FROM park WHERE name = 'Casa de Jesse'", connection);

                connection.Open();

                ID = Convert.ToInt32(command.ExecuteScalar());
            }

            dao.ParkLevelSearch(ID);
            dao.ArrivalAndDeparture(DateTime.Parse("2019-07-01"), DateTime.Parse("2019-07-05"));
            dao.MaxOccupancy(6);
            dao.Utility(true);
            dao.MaxRVLength(20);
            dao.Accessible(true);

            Assert.AreEqual(2, dao.RunParkLevelSearch().Count);
        }
    }
}

