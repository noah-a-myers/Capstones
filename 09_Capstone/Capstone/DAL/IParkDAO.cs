using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IParkDAO
    {
        /// <summary>
        /// Generates Park object and populates properties for display
        /// </summary>
        /// <returns></returns>
        Park GetPark(int parkId);

        /// <summary>
        /// Return list of parks sorted alphabetically by name
        /// </summary>
        /// <returns></returns>
        List<Park> GetParkList();

        /// <summary>
        /// Returns name of park
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns></returns>
        string GetParkName(int parkId);
    }
}
