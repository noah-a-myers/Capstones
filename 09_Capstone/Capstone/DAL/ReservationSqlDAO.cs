using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private const string ValidReservationSelectQuery = @"SELECT reservation_id 
                                                             FROM reservation 
                                                             WHERE site_id = @site_id AND name = @name 
                                                             AND from_date = @from_date 
                                                             AND to_date = @to_date";
        private const string ValidReservationInsertQuery = @"INSERT INTO reservation (site_id, name, from_date, to_date)
                                                             VALUES (@site_id, @name, @from_date, @to_date);
                                                             SELECT SCOPE_IDENTITY()";

        private readonly string connectionString;


        // Single Parameter Constructor
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Inserts valid reservation into reservation table and returns confirmation number
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public int MakeReservation(int parkId, Reservation reservation)
        {
            int reservationId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = ValidReservationInsertQuery;

                command.Parameters.AddWithValue("@site_id", reservation.Site_Id);
                command.Parameters.AddWithValue("@name", reservation.Name);
                command.Parameters.AddWithValue("@from_date", reservation.From_Date);
                command.Parameters.AddWithValue("@to_date", reservation.To_Date);

                connection.Open();

                reservationId = Convert.ToInt32(command.ExecuteScalar());
            }

            return reservationId;
        }

        /// <summary>
        /// Display all reservations whose date ranges include the next 30 days
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        public List<Reservation> DisplayAMonthOfReservations(int parkId)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(
                    @"SELECT r.reservation_id, r.site_id, r.name, r.from_date, r.to_date 
                      FROM reservation r
                      JOIN site s
                      ON r.site_id = s.site_id
                      JOIN campground c
                      ON s.campground_id = c.campground_id
                      WHERE c.park_id = @park_id 
                      AND from_date BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE()) 
                      OR to_date BETWEEN GETDATE() AND DATEADD(day, 30, GETDATE())"
                    , connection);

                command.Parameters.AddWithValue("@park_id", parkId);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Reservation reservation = new Reservation
                    {
                        Reservation_Id = Convert.ToInt32(reader["reservation_id"]),
                        Site_Id = Convert.ToInt32(reader["site_id"]),
                        Name = reader["name"] as string,
                        From_Date = Convert.ToDateTime(reader["from_date"]),
                        To_Date = Convert.ToDateTime(reader["to_date"]),
                    };

                    reservations.Add(reservation);
                }
            }

                return reservations;
        }
    }
}
