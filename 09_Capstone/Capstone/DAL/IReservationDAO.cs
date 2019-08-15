using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        /// <summary>
        /// Inserts valid reservation into reservation table and returns confirmation number
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        int MakeReservation(int parkId, Reservation reservation);

        List<Reservation> DisplayAMonthOfReservations(int parkId);
    }
}
