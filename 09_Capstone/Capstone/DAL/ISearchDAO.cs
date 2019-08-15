using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ISearchDAO
    {
        /// <summary>
        /// Search query builder
        /// </summary>
        /// <param name="campground_id">ID of desired campgrounds</param>
        /// <returns>Compiled string search query</returns>
        string CampgroundLevelSearch(int campground_id);

        /// <summary>
        /// Search query builder
        /// </summary>
        /// <param name="park_id">ID of desired park</param>
        /// <returns>Compiled string search query</returns>
        string ParkLevelSearch(int park_id);

        /// <summary>
        /// Date window query builder
        /// </summary>
        /// <param name="from_date">Desired date of arrival to site</param>
        /// <param name="to_date">Desired date of departure from site</param>
        /// <returns>String date window query</returns>
        string ArrivalAndDeparture(DateTime from_date, DateTime to_date);
        
        /// <summary>
        /// Accessibility query builder
        /// </summary>
        /// <param name="isAccessible">Does the site need to be handicap accessible</param>
        /// <returns>String accessibility query</returns>
        string Accessible(bool isAccessible);

        /// <summary>
        /// Max RV length query builder
        /// </summary>
        /// <param name="max_rv_length">Length of their RV</param>
        /// <returns>String rv length query</returns>
        string MaxRVLength(int max_rv_length);

        /// <summary>
        /// Utility query builder
        /// </summary>
        /// <param name="hasUtilities">If utilities are desired</param>
        /// <returns>String utility query</returns>
        string Utility(bool hasUtilities);

        /// <summary>
        /// Max occupancy query builder
        /// </summary>
        /// <param name="max_occupancy">Number of people you wish to have at site</param>
        /// <returns>String occupancy query</returns>
        string MaxOccupancy(int max_occupancy);

        /// <summary>
        /// Creates list of sites that meet prior built criteria
        /// </summary>
        /// <returns>List of sites matching your search</returns>
        List<Site> RunCampgroundLevelSearch();

        /// <summary>
        /// Creates list of sites that meet prior built criteria
        /// </summary>
        /// <returns>List of sites matching your search</returns>
        List<Site> RunParkLevelSearch();
    }
}
