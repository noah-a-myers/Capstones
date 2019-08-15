using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface ICampgroundDAO
    {
        /// <summary>
        /// Adds viable campgrounds to list for display
        /// </summary>
        List<Campground> GetCampgrounds(int parkId);

        /// <summary>
        /// Return campground id given campground name
        /// </summary>
        /// <param name="campgroundName"></param>
        /// <returns></returns>
        int FindCampgroundId(string campgroundName);
    }
}
