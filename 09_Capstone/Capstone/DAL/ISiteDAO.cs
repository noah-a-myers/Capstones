using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        /// <summary>
        /// Returns list of available sites in a specified campground
        /// </summary>
        /// <param name="parkId"></param>
        /// <param name="campgroundId"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        List<Site> GetAvailableSitesInCampground(int parkId, int campgroundId, Reservation reservation);

        /// <summary>
        /// Returns list of available sites across all campgrounds in a specified park
        /// </summary>
        /// <param name="parkId"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        List<Site> GetAvailableSitesParkwide(int parkId, Reservation reservation);

        /// <summary>
        /// Returns site id based on site number and campground id
        /// </summary>
        /// <param name="siteNumber"></param>
        /// <param name="campgroundId"></param>
        /// <returns></returns>
        int FindSiteIdFromSiteNumber(int siteNumber, int campgroundId);

        /// <summary>
        /// Maps site info to list entries
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        List<Site> MapSites(SqlDataReader reader);
     }
}
