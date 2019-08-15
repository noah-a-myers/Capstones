using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private const string ParkSelectQuery = @"SELECT park_id, name, location, establish_date, area, visitors, description 
                                                 FROM park
                                                 WHERE park_id = @park_id";

        private string connectionString;

        // Single Parameter Constructor
        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Adds Park to a list for display
        /// </summary>
        /// <returns></returns>
        public Park GetPark(int parkId)
        {
            Park park = new Park();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = ParkSelectQuery;

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("@park_id", parkId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    park.Park_Id = Convert.ToInt32(reader["park_id"]);
                    park.Name = reader["name"] as string + " National Park";
                    park.Location = reader["location"] as string;
                    park.Establish_date = Convert.ToDateTime(reader["establish_date"]);
                    park.Area = Convert.ToInt32(reader["area"]);
                    park.Visitors = Convert.ToInt32(reader["visitors"]);
                    park.Description = reader["description"] as string;
                }
            }

            return park;
        }

        /// <summary>
        /// Return list of parks sorted alphabetically by name
        /// </summary>
        /// <returns></returns>
        public List<Park> GetParkList()
        {
            List<Park> parks = new List<Park>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT park_id, name FROM park ORDER BY name",connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    Park park = new Park();
                    park.Park_Id = Convert.ToInt32(reader["park_id"]);
                    park.Name = reader["name"] as string;

                    parks.Add(park);
                }
            }
                return parks;
        }

        /// <summary>
        /// Returns name of park
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        public string GetParkName(int parkId)
        {
            string parkName = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = ParkSelectQuery;

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("@park_id", parkId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    parkName = reader["name"] as string + " National Park";
                }
            }

            return parkName;
        }
    }
}

