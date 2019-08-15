using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class SearchSqlDAO : ISearchDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public SearchSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        private int campground_Id { get; set; } = 0;
        private int park_Id { get; set; } = 0;

        private string arrivalAndDepartureQuery;
        private DateTime arrival { get; set; }
        private DateTime departure { get; set; }

        private string accessibleQuery;
        private bool accessible { get; set; }

        private string maxRVQuery;
        private int max_RV_Length { get; set; }

        private string utilitiesQuery;
        private bool utilities { get; set; }

        private string maxOccQuery;
        private int max_Occupancy { get; set; }

        public string CampgroundLevelSearch(int campground_id)
        {
            string searchQuery = @"SELECT TOP 5 c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee
                                   FROM site s
                                   LEFT JOIN reservation r
                                   ON s.site_id = r.site_id
                                   JOIN campground c
								   ON s.campground_id = c.campground_id
                                   WHERE s.campground_id = @campground_id
                                   AND r.site_id is null
                                   "
                                   + arrivalAndDepartureQuery
                                   + accessibleQuery
                                   + maxRVQuery
                                   + utilitiesQuery
                                   + maxOccQuery
                                   + "\nGROUP BY s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.name, c.daily_fee";
            campground_Id = campground_id;

            return searchQuery;
        }

        public string ParkLevelSearch(int park_id)
        {
            string searchQuery = @"SELECT c.name, s.site_id, s.campground_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.daily_fee
                                   FROM site s
                                   FULL JOIN reservation r
                                   ON s.site_id = r.site_id
                                   JOIN campground c
								   ON s.campground_id = c.campground_id
                                   WHERE c.park_id = @park_id
                                   "
                       + arrivalAndDepartureQuery
                       + accessibleQuery
                       + maxRVQuery
                       + utilitiesQuery
                       + maxOccQuery
                       + "\nGROUP BY s.campground_id, s.site_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities, c.name, c.daily_fee";
            park_Id = park_id;

            return searchQuery;
        }

        public string ArrivalAndDeparture(DateTime from_date, DateTime to_date)
        {
            arrivalAndDepartureQuery = @"AND s.site_id not in (SELECT site_id FROM reservation WHERE
								         (@from_date BETWEEN r.from_date AND DATEADD(day, -1, r.to_date)
								          OR @to_date BETWEEN DATEADD(day, 1, r.from_date) AND r.to_date)
								          AND MONTH(@from_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm
								          AND MONTH(@to_date) NOT BETWEEN c.open_from_mm AND c.open_to_mm)";
            arrival = from_date;
            departure = to_date;

            return arrivalAndDepartureQuery;
        }

        public string Accessible(bool isAccessible)
        {
            accessibleQuery = $"\nAND s.accessible = @isAccessible";
            accessible = isAccessible;

            return accessibleQuery;
        }

        public string MaxRVLength(int max_rv_length)
        {
            maxRVQuery = $"\nAND s.max_rv_length >= @max_rv_length";
            max_RV_Length = max_rv_length;

            return maxRVQuery;
        }

        public string Utility(bool hasUtilities)
        {
            utilitiesQuery = $"\nAND s.utilities = @hasUtilities";
            utilities = hasUtilities;

            return utilitiesQuery;
        }

        public string MaxOccupancy(int max_occupancy)
        {
            maxOccQuery = $"\nAND s.max_occupancy >= @max_occupancy";
            max_Occupancy = max_occupancy;

            return maxOccQuery;
        }

        public List<Site> RunCampgroundLevelSearch()
        {
            List<Site> sites = new List<Site>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = CampgroundLevelSearch(campground_Id);

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("@campground_id", campground_Id);
                if (arrival > DateTime.Parse("1753-01-01") && departure > DateTime.Parse("1753-01-01"))
                {
                    command.Parameters.AddWithValue("@from_date", arrival.Date);
                    command.Parameters.AddWithValue("@to_date", departure.Date);
                }
                if (accessible)
                {
                    command.Parameters.AddWithValue("@isAccessible", accessible);
                }
                if (max_RV_Length >= 0)
                {
                    command.Parameters.AddWithValue("@max_rv_length", max_RV_Length);
                }
                if (utilities)
                {
                    command.Parameters.AddWithValue("@hasUtilities", utilities);
                }
                if (max_Occupancy >= 0)
                {
                    command.Parameters.AddWithValue("@max_occupancy", max_Occupancy);
                }

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Site site = new Site();
                    site.Campground_Name = reader["name"] as string;
                    site.Site_Id = Convert.ToInt32(reader["site_id"]);
                    site.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                    site.Site_Number = Convert.ToInt32(reader["site_number"]);
                    site.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                    site.Accessible = Convert.ToBoolean(reader["accessible"]);
                    site.Max_RV_Length = Convert.ToInt32(reader["max_rv_length"]);
                    site.Utilities = Convert.ToBoolean(reader["utilities"]);
                    site.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                    sites.Add(site);
                }
            }

            return sites;
        }

        public List<Site> RunParkLevelSearch()
        {
            List<Site> sites = new List<Site>();
            List<int> campIds = new List<int>();
            List<int> campIdCounts = new List<int>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = ParkLevelSearch(park_Id);

                SqlCommand command = connection.CreateCommand();
                command.CommandText = query;

                command.Parameters.AddWithValue("@park_id", park_Id);                
                if (arrival > DateTime.Parse("1753-01-01") && departure > DateTime.Parse("1753-01-01"))
                {
                    command.Parameters.AddWithValue("@from_date", arrival.Date);
                    command.Parameters.AddWithValue("@to_date", departure.Date);
                }
                if (accessible)
                {
                    command.Parameters.AddWithValue("@isAccessible", accessible);
                }
                if (max_RV_Length > 0)
                {
                    command.Parameters.AddWithValue("@max_rv_length", max_RV_Length);
                }
                if (utilities)
                {
                    command.Parameters.AddWithValue("@hasUtilities", utilities);
                }
                if (max_Occupancy > 0)
                {
                    command.Parameters.AddWithValue("@max_occupancy", max_Occupancy);
                }

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Site site = new Site();
                    site.Campground_Name = reader["name"] as string;
                    site.Site_Id = Convert.ToInt32(reader["site_id"]);
                    site.Campground_Id = Convert.ToInt32(reader["campground_id"]);
                    site.Site_Number = Convert.ToInt32(reader["site_number"]);
                    site.Max_Occupancy = Convert.ToInt32(reader["max_occupancy"]);
                    site.Accessible = Convert.ToBoolean(reader["accessible"]);
                    site.Max_RV_Length = Convert.ToInt32(reader["max_rv_length"]);
                    site.Utilities = Convert.ToBoolean(reader["utilities"]);
                    site.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                    if(!campIds.Contains(site.Campground_Id))
                    {
                        campIds.Add(site.Campground_Id);
                        campIdCounts.Add(1);
                        sites.Add(site);
                    }
                    else if (campIds.Contains(site.Campground_Id) && campIdCounts[campIds.IndexOf(site.Campground_Id)] < 5)
                    {
                        campIdCounts[campIds.IndexOf(site.Campground_Id)]++;
                        sites.Add(site);
                    }
                }
            }

            return sites;
        }
    }
}
