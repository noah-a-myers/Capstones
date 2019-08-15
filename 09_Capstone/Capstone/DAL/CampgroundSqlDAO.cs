using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private const string CampgroundSelectQuery = @"SELECT campground_id, park_id, name, open_from_mm, open_to_mm, daily_fee 
                                                       FROM campground
                                                       WHERE park_id = @park_id";

        private string connectionString;

        // Single Parameter Constructor
        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Add campgrounds within specified park to a list for display
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        public List<Campground> GetCampgrounds(int parkId)
        {
            List<Campground> campgrounds = new List<Campground>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = CampgroundSelectQuery;

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("@park_id", parkId);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
                    Campground campground = new Campground();
                    campground.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                    campground.Park_Id = Convert.ToInt32(reader["park_id"]);
                    campground.Name = reader["name"] as string;
                    campground.Open_From_MM = dateTimeFormatInfo.GetMonthName(Convert.ToInt32(reader["open_from_mm"]));
                    campground.Open_To_MM = dateTimeFormatInfo.GetMonthName(Convert.ToInt32(reader["open_to_mm"]));
                    campground.Daily_Fee = Convert.ToDecimal(reader["daily_fee"]);

                    campgrounds.Add(campground);
                }
            }

            return campgrounds;
        }

        public int FindCampgroundId(string campgroundName)
        {
            int campgroundId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT campground_id
                      FROM campground
                      WHERE name = @name",
                    connection);

                command.Parameters.AddWithValue("@name", campgroundName);

                connection.Open();

                campgroundId = Convert.ToInt32(command.ExecuteScalar());
            }

            return campgroundId;
        }
    }
}
