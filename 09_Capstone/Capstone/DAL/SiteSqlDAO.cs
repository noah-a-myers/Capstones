using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {
        private const string AvailableSitesQuery = @"SELECT c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee 
                                                     FROM site s
                                                     JOIN campground c
                                                     ON s.campground_id = c.campground_id
                                                     JOIN park p
                                                     ON c.park_id = p.park_id
                                                     WHERE p.park_id = @park_id
                                                     AND (site_id not in (SELECT site_id 
                                                                          FROM reservation 
                                                                          WHERE @from_date BETWEEN from_date AND DATEADD(day, -1, to_date)
								                                          OR @to_date BETWEEN DATEADD(day, 1, from_date) AND to_date))";

        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns list of available sites in a specified campground
        /// </summary>
        /// <param name="parkId"></param>
        /// <param name="campgroundId"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public List<Site> GetAvailableSitesInCampground(int parkId, int campgroundId, Reservation reservation)
        {
            List<Site> sites = new List<Site>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = AvailableSitesQuery + @"AND s.campground_id = @campground_id";

                command.Parameters.AddWithValue("@park_id", parkId);
                command.Parameters.AddWithValue("@campground_id", campgroundId);
                command.Parameters.AddWithValue("@from_date", reservation.From_Date);
                command.Parameters.AddWithValue("@to_date", reservation.To_Date);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                sites = MapSites(reader);
            }

            return sites;
        }

        /// <summary>
        /// Returns list of available sites across all campgrounds in a specified park
        /// </summary>
        /// <param name="parkId"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public List<Site> GetAvailableSitesParkwide(int parkId, Reservation reservation)
        {
            List<Site> sites = new List<Site>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = AvailableSitesQuery;

                command.Parameters.AddWithValue("@park_id", parkId);
                command.Parameters.AddWithValue("@from_date", reservation.From_Date);
                command.Parameters.AddWithValue("@to_date", reservation.To_Date);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                sites = MapSites(reader);
            }

            return sites;
        }

        public int FindSiteIdFromSiteNumber(int siteNumber, int campgroundId)
        {
            int siteId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT site_id
                      FROM site
                      WHERE site_number = @site_number
                      AND campground_id = @campground_id",
                    connection);

                command.Parameters.AddWithValue("@site_number", siteNumber);
                command.Parameters.AddWithValue("@campground_id", campgroundId);

                connection.Open();

                siteId = Convert.ToInt32(command.ExecuteScalar());
            }

            return siteId;
        }

        /// <summary>
        /// Maps site info to list entries
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<Site> MapSites(SqlDataReader reader)
        {
            List<Site> sites = new List<Site>();

            while (reader.Read())
            {
                Site site = new Site
                {
                    Campground_Name = reader["name"] as string,
                    Site_Id = Convert.ToInt32(reader["site_id"]),
                    Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]),
                    Site_Number = Convert.ToInt32(reader["site_number"]),
                    Accessible = Convert.ToBoolean(reader["accessible"]),
                    Max_RV_Length = Convert.ToInt32(reader["max_rv_length"]),
                    Utilities = Convert.ToBoolean(reader["utilities"]),
                    DailyFee = Convert.ToDecimal(reader["daily_fee"])
                };

                sites.Add(site);
            }
            return sites;
        }
    }
}
