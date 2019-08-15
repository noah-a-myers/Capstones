using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectOrganizer.Tests.DAL
{
    [TestClass]
    public abstract class DatabaseTest
    {
        private IConfigurationRoot config;

        /// <summary>
        /// The transaction for each test.
        /// </summary>
        private TransactionScope transaction;

        /// <summary>
        /// The Configuration options specified in appsettings.json
        /// </summary>
        protected IConfigurationRoot Config
        {
            get
            {
                if (config == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    config = builder.Build();
                }
                return config;
            }
        }

        /// <summary>
        /// The database connection string derived from the configuration settings
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return Config.GetConnectionString("Project");
            }
        }

        [TestInitialize]
        public virtual void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope();

            // Get the SQL Script to run
            string sql = @"DELETE FROM reservation;
                           DELETE FROM site;
                           DELETE FROM campground;
                           DELETE FROM park;
                           INSERT INTO park(name, location, establish_date, area, visitors, description)
                           VALUES ('Casa de Jesse', 'Ohio', '2016-05-24', 1, 5, 'A space predominately used by Jesse and his friends');
                           INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee)
                           VALUES ((SELECT park_id FROM park WHERE name = 'Casa de Jesse'), 'The Back Yard', 01, 12, 50.00);
                           INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee)
                           VALUES((SELECT park_id FROM park WHERE name = 'Casa de Jesse'), 'The Front Yard', 04, 09, 100.00);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Back Yard'), 1, 2, 1, 0, 0);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Back Yard'), 2, 6, 1, 20, 1);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Back Yard'), 3, 4, 0, 0, 0);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Back Yard'), 4, 12, 0, 35, 1);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Back Yard'), 5, 6, 1, 0, 1);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Back Yard'), 6, 4, 1, 20, 1);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES ((SELECT campground_id FROM campground WHERE name = 'The Front Yard'), 1, 4, 1, 20, 1);
                           INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES((SELECT campground_id FROM campground WHERE name = 'The Front Yard'), 2, 8, 1, 20, 1);
                           INSERT INTO reservation(site_id, name, from_date, to_date)
                           VALUES ((SELECT s.site_id FROM site s JOIN campground c ON s.campground_id = c.campground_id 
                                    WHERE s.site_number = 1 AND c.name = 'The Back Yard'), 'Jesse Reservation', GETDATE()+20, GETDATE()+27)
                           INSERT INTO reservation(site_id, name, from_date, to_date)
                           VALUES ((SELECT s.site_id FROM site s JOIN campground c ON s.campground_id = c.campground_id 
                                    WHERE s.site_number = 2 AND c.name = 'The Front Yard'), 'Noah Reservation', GETDATE()+27, GETDATE()+34)";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            transaction.Dispose();
        }

        /// <summary>
        /// Gets the row count for a table.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
