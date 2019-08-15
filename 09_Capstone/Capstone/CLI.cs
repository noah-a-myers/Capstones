using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class CLI
    {
        private int menuParkId;
        private Dictionary<int, string> parkPairs = new Dictionary<int, string>();
        private string mainMenuInput = null;

        // cases for all switchcase-driven menus below
        const string Command_Quit = "q";

        const string Command_ViewCampgrounds = "1";
        const string Command_SearchForReservation = "2";
        const string Command_DisplayAMonthOfReservations = "3";
        const string Command_ReturnToMainMenuScreen = "4";

        const string Command_SearchForAvailableReservation = "1";
        const string Command_ReturnToParkInformationScreen = "2";

        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        private ISearchDAO searchDAO;

        private List<int> validCampgrounds = new List<int>();

        public CLI(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO, ISearchDAO searchDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
            this.searchDAO = searchDAO;
        }

        /// <summary>
        /// Display list of parks and main menu options
        /// </summary>
        public void MainMenu()
        {
            bool isDone = false;
            bool isNumberOrQ;

            while (!isDone)
            {
                PrintHeader();
                parkPairs = PrintMainMenu();
                isNumberOrQ = false;

                while (!isNumberOrQ)
                {
                    // make selection from menu
                    mainMenuInput = Console.ReadLine().ToLower();

                    try
                    {
                        foreach (KeyValuePair<int, string> kvp in parkPairs)
                        {
                            if (kvp.Key == Convert.ToInt32(mainMenuInput))
                            {
                                menuParkId = Convert.ToInt32(mainMenuInput);
                                isNumberOrQ = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        if (mainMenuInput == "q")
                        {
                            isNumberOrQ = true;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid selection."); // if invalid selection is made, display error message
                        }
                    }
                }

                Console.Clear();

                // act on selection from menu by choosing park with corresponding park_id or quitting the app
                switch (mainMenuInput)
                {
                    case Command_Quit:          // quit app
                        isDone = true;
                        break;
                    default:
                        foreach (KeyValuePair<int, string> kvp in parkPairs) // if valid park number is entered, go to park info screen, 
                        {
                            if (kvp.Key == Convert.ToInt32(mainMenuInput))
                            {
                                menuParkId = Convert.ToInt32(mainMenuInput);
                                DisplayParkInfo(menuParkId);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid selection.");
                                Console.WriteLine();
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Display info for selected park and display menu options
        /// </summary>
        /// <param name="parkId"></param>
        private void DisplayParkInfo(int parkId)
        {
            string parkInfoScreenInput = null;
            bool doneWithParkInfo = false;

            while (!doneWithParkInfo)
            {
                PrintHeader();
                PrintParkMenu(parkId);

                parkInfoScreenInput = Console.ReadLine();
                Console.Clear();

                switch (parkInfoScreenInput)
                {
                    case Command_ViewCampgrounds:               // view campgrounds at park
                        DisplayCampgroundInfo(parkId);
                        break;
                    case Command_SearchForReservation:          // search for available reservations across park
                        ParkwideSiteSearch(parkId);
                        break;
                    case Command_DisplayAMonthOfReservations:   // display all reservations in next 30 days
                        DisplayAMonthOfReservations(parkId);
                        break;
                    case Command_ReturnToMainMenuScreen:        // return to main menu
                        doneWithParkInfo = true;
                        break;
                    default:                                    // error message if invalid selection is made
                        Console.WriteLine("Please select a valid option");
                        Console.WriteLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Display all reservations whose date ranges fall within the next 30 days
        /// </summary>
        /// <param name="parkId"></param>
        private void DisplayAMonthOfReservations(int parkId)
        {
            List<Reservation> reservations;
            bool IsDone = false;

            while (!IsDone)
            {
                PrintHeader();
                reservations = reservationDAO.DisplayAMonthOfReservations(parkId);
                Console.WriteLine(Reservation.ReservationListHeader());
                foreach (Reservation reservation in reservations)
                {
                    Console.WriteLine(reservation.ToString());
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to return to Park Info Menu.");
                Console.ReadKey();
                Console.Clear();
                IsDone = true;
            }
        }

        /// <summary>
        /// Search for available reservations at campsites across an entire park, rather than a single campground
        /// </summary>
        /// <param name="parkId"></param>
        private void ParkwideSiteSearch(int parkId)
        {
            int siteId = 0;
            int confirmationId = 0;
            int campgroundId = 0;
            int siteToBeReserved = 0;
            int lengthOfStay = 0;
            decimal totalCost = 0M;
            string campgroundName = null;
            string reservationName = null;
            Reservation reservation;
            string[] enteredDates = EnterReservationDates();
            List<Site> sites = new List<Site>();
            bool doneSearching = false;
            bool nameIsValid = false;

            // build parkwide search query
            searchDAO.ParkLevelSearch(parkId);
            while (!doneSearching)
            {
                PrintHeader();

                // create reservation from previously entered data to filter out campsites being used in the requested date range
                reservation = new Reservation
                {
                    From_Date = DateTime.Parse(enteredDates[0]),
                    To_Date = DateTime.Parse(enteredDates[1])
                };

                // create list of sites where reservations are available given criteria above
                sites = searchDAO.RunParkLevelSearch();

                // if no sites meet the search criteria
                if (sites.Count == 0)
                {
                    PrintIfNoCampsitesFound();
                    break;
                }

                // print all matching campsites in park with campground names
                Console.WriteLine(Site.PrintSiteHeadersWithName()); // print column headers
                foreach (Site site in sites)
                {
                    lengthOfStay = ((TimeSpan)(reservation.To_Date - reservation.From_Date)).Days;          // generate num. of days in reservation request
                    totalCost = (decimal)lengthOfStay * site.DailyFee;                                      // calculate cost of reservation request
                    Console.Write(site.ToStringWithName());                                 // display site info
                    Console.WriteLine($"${string.Format("{0:0.00}", totalCost)}");          // display cost for reservation
                }
                Console.WriteLine();

                // enter desired campground name
                Console.Write("Which campground is the site located in? (enter 0 to cancel):    ");
                campgroundName = Console.ReadLine();

                while (!nameIsValid)
                {
                    campgroundId = campgroundDAO.FindCampgroundId(campgroundName);
                    if (campgroundId != 0)
                    {
                        nameIsValid = true;
                    }
                    else
                    {
                        Console.Write("Please enter the name exactly as displayed in the list above (enter 0 to cancel):    ");
                        campgroundName = Console.ReadLine();
                    }
                }

                if (campgroundName == "0")
                {
                    Console.Clear();
                    doneSearching = true;
                    break;
                }

                // request desired campsite to reserve
                Console.Write("Which site should be reserved? (enter 0 to cancel):    ");
                siteToBeReserved = Convert.ToInt32(Console.ReadLine());

                // exit to previous screen if 0 is entered
                if (siteToBeReserved == 0)
                {
                    Console.Clear();
                    doneSearching = true;
                    break;
                }

                siteToBeReserved = siteDAO.FindSiteIdFromSiteNumber(siteToBeReserved, campgroundId);

                foreach (Site site in sites)
                {
                    if (site.Site_Number == siteToBeReserved)
                    {
                        siteId = site.Site_Id;
                        break;
                    }
                }

                // enter reservation name
                Console.Write("What name should the reservation be made under?:    ");
                reservationName = Console.ReadLine();
                Console.WriteLine();

                // map site id and reservation name to temp reservation used to add reservation to database
                reservation.Site_Id = siteId;
                reservation.Name = reservationName;

                // check to see if requested reservation is valid
                if (sites.Count != 0)
                {
                    // finalize reservation and receive confirmation number(res id)
                    confirmationId = reservationDAO.MakeReservation(parkId, reservation);
                }

                Console.Clear();
                PrintHeader();
                Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}. Please press any key to continue.");
                Console.ReadKey();
                Console.Clear();
                doneSearching = true;
            }
        }

        /// <summary>
        /// Display all campgrounds in a park and options menu
        /// </summary>
        /// <param name="parkId"></param>
        private void DisplayCampgroundInfo(int parkId)
        {
            string campgroundInfoScreenInput = null;
            bool doneDisplayingCampgrounds = false;

            while (!doneDisplayingCampgrounds)
            {
                PrintHeader();
                PrintCampgrounds(parkId);
                PrintCampgroundMenu();

                campgroundInfoScreenInput = Console.ReadLine();
                Console.Clear();

                switch (campgroundInfoScreenInput)
                {
                    case Command_SearchForAvailableReservation:         // search for reservations in a campground
                        SearchCampsites(parkId);
                        break;
                    case Command_ReturnToParkInformationScreen:         // return to park info screen
                        doneDisplayingCampgrounds = true;
                        break;
                    default:                                            // error message if invalid option is chosen
                        Console.WriteLine("Please enter a valid option");
                        Console.WriteLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Display all campgrounds within the currently selected park
        /// </summary>
        /// <param name="parkId"></param>
        private void SearchCampsites(int parkId)
        {
            int siteId = 0;
            int confirmationId = 0;
            int desiredCampground = 0;
            int siteToBeReserved = 0;
            string advanced = null;
            Reservation reservation;
            string[] enteredDates = new string[2];
            List<Site> sites = new List<Site>();
            bool doneSearching = false;

            while (!doneSearching)
            {

                validCampgrounds = new List<int>();

                PrintHeader();
                PrintCampgrounds(parkId); // adds all campgrounds in park to validCampgrounds list

                // request desired campground 
                Console.Write("Which campground? (enter 0 to cancel):    ");
                desiredCampground = 0;
                desiredCampground = ReturnNumberEntry();

                // exit to previous menu if 0 is entered
                if (desiredCampground == 0)
                {
                    Console.Clear();
                    doneSearching = true;
                    break;
                }
                // return to top of screen if invalid campground number is entered
                else if (!validCampgrounds.Contains(desiredCampground))
                {
                    Console.Clear();
                    PrintHeader();
                    Console.WriteLine("The campground you selected is not in this park. Please press any key to return to the list of campgrounds.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                }
                // if entry is valid, begins building search query with entered campground id
                else
                {
                    searchDAO.CampgroundLevelSearch(desiredCampground);
                }

                enteredDates = EnterReservationDates();

                // create reservation from previously entered data to filter out campsites being used in the requested date range
                reservation = new Reservation
                {
                    From_Date = DateTime.Parse(enteredDates[0]),
                    To_Date = DateTime.Parse(enteredDates[1])
                };

                // create list of sites where reservations are available given criteria above
                sites = searchDAO.RunCampgroundLevelSearch();

                // if no sites meet the search criteria
                if (sites.Count == 0)
                {
                    PrintIfNoCampsitesFound();
                    break;
                }

                Console.WriteLine(Site.PrintSiteHeaders()); // print site header
                PrintFoundCampsites(sites, reservation); // print list of sites

                // activate advanced search function, if desired
                Console.WriteLine("Would you like to refine your search? (Y/N):    ");
                advanced = Console.ReadKey().KeyChar.ToString();
                if (advanced.ToUpper() == "Y")
                {
                    Console.Clear();
                    PrintHeader();
                    sites = AdvancedSearch();

                    // if no sites meet the search criteria
                    if (sites.Count == 0)
                    {
                        PrintIfNoCampsitesFound();
                        break;
                    }

                    Console.WriteLine(Site.PrintSiteHeaders()); // print site header
                    PrintFoundCampsites(sites, reservation);  // print list of sites

                    advanced = null; // resets advanced search function
                }

                // request desired campsite to reserve
                Console.Write("Which site should be reserved? (enter 0 to cancel):    ");
                siteToBeReserved = ReturnNumberEntry(); // create site for tracking reservation site

                // return to previous screen if 0 is entered
                if (siteToBeReserved == 0)
                {
                    Console.Clear();
                    doneSearching = true;
                    break;
                }

                // find site id from campground id and site number
                siteToBeReserved = siteDAO.FindSiteIdFromSiteNumber(siteToBeReserved, desiredCampground);
                foreach (Site site in sites)
                {
                    if (site.Site_Id == siteToBeReserved)
                    {
                        siteId = site.Site_Id;
                        break;
                    }
                }

                // enter name for reservation
                Console.Write("What name should the reservation be made under?:    ");
                string reservationName = Console.ReadLine();
                Console.WriteLine();

                reservation.Site_Id = siteId;           // add site id to reservation object
                reservation.Name = reservationName;     // add name to reservation object

                // check to see if requested reservation is valid
                if (sites.Count != 0)
                {
                    // finalize reservation and receive confirmation number(res id)
                    confirmationId = reservationDAO.MakeReservation(parkId, reservation);
                }
                else
                {
                    PrintIfInvalidSiteNumber();
                    break;
                }

                Console.Clear();
                PrintHeader();
                Console.WriteLine($"The reservation has been made and the confirmation id is {confirmationId}. Please press any key to continue.");
                Console.ReadKey();
                Console.Clear();

                doneSearching = true;
            }
        }

        /// <summary>
        /// Prints list of campgrounds with headers
        /// </summary>
        /// <param name="parkId"></param>
        private void PrintCampgrounds(int parkId)
        {
            List<Campground> campgrounds = new List<Campground>();

            Console.WriteLine("Park Campgrounds");
            Console.WriteLine(parkDAO.GetParkName(parkId));
            Console.WriteLine();
            Console.WriteLine(Campground.PrintCampgroundHeader());

            // generate list of campgrounds to display for campground selection screen
            campgrounds = campgroundDAO.GetCampgrounds(parkId);
            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine(campground.ToString());
                validCampgrounds.Add(campground.Campground_Id);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Print message if no campsites match search criteria
        /// </summary>
        private void PrintIfNoCampsitesFound()
        {
            Console.Clear();
            PrintHeader();
            Console.WriteLine("There are no campsites available during the requested time range. Please press any key to try a different search.");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Print message if site number entered is not available in the given campground
        /// </summary>
        private void PrintIfInvalidSiteNumber()
        {
            Console.Clear();
            PrintHeader();
            Console.WriteLine("The site number you entered was not valid. Please press any key to try again.");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Print and return a list of campsites that matches the input criteria
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        private void PrintFoundCampsites(List<Site> sites, Reservation reservation)
        {
            int lengthOfStay = 0;
            decimal totalCost = 0;

            foreach (Site site in sites)
            {
                lengthOfStay = ((TimeSpan)(reservation.To_Date - reservation.From_Date)).Days;  // generate num. of days in reservation request
                totalCost = (decimal)lengthOfStay * site.DailyFee;                              // calculate cost of reservation request
                Console.Write(site.ToString());                                     // display site info
                Console.WriteLine($"${string.Format("{0:0.00}", totalCost)}");      // display cost for reservation
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Accept desired reservation dates and add to search query
        /// </summary>
        /// <returns></returns>
        private string[] EnterReservationDates()
        {
            string fromDate = null;
            string toDate = null;
            string[] enteredDates = new string[2];

            // enter desired arrival(from) and departure(to) dates to check for available sites in selected campground
            Console.Write("What is the arrival date? (YYYY-MM-DD):    ");
            fromDate = ReceiveDate();

            Console.Write("What is the departure date? (YYYY-MM-DD):    ");
            toDate = ReceiveDate();

            if (DateTime.Parse(toDate) <= DateTime.Parse(fromDate))
            {
                Console.WriteLine("The departure date must be at least one day after the arrival date.");
                Console.Write("What is the departure date? (YYYY-MM-DD):    ");
                toDate = ReceiveDate();
            }

            enteredDates[0] = fromDate;
            enteredDates[1] = toDate;

            // add dates to search query and return sites that match search criteria
            searchDAO.ArrivalAndDeparture(DateTime.Parse(fromDate), DateTime.Parse(toDate));

            Console.WriteLine();
            Console.WriteLine("Results matching your search criteria:");
            Console.WriteLine();

            return enteredDates;
        }

        /// <summary>
        /// Accepts dates for reservations and returns them as a string with appropriate formatting
        /// </summary>
        /// <returns></returns>
        private string ReceiveDate()
        {
            string Date = "";
            bool validDate = false;

            while (!validDate)
            {
                try
                {
                    Date = Console.ReadKey().KeyChar.ToString();
                    Date += Console.ReadKey().KeyChar.ToString();
                    Date += Console.ReadKey().KeyChar.ToString();
                    Date += Console.ReadKey().KeyChar.ToString();
                    Console.Write("-");
                    Date += "-";
                    Date += Console.ReadKey().KeyChar.ToString();
                    Date += Console.ReadKey().KeyChar.ToString();
                    Console.Write("-");
                    Date += "-";
                    Date += Console.ReadKey().KeyChar.ToString();
                    Date += Console.ReadKey().KeyChar.ToString();
                    Console.WriteLine();

                    DateTime dateTime = DateTime.Parse(Date);
                    validDate = true;
                }
                catch
                {
                    Console.Write("Please enter a valid date:    ");
                }
            }

            return Date;
        }

        /// <summary>
        /// Runs additional search filters
        /// </summary>
        /// <returns></returns>
        private List<Site> AdvancedSearch()
        {
            int rvLength = 0;
            int campers = 0;
            string access = null;
            string utility = null;
            bool validLength = false;
            bool validOccupancy = false;

            Console.Write("Does the site need to be handicap accessible? (Y/N):    ");
            access = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine();
            if (access.ToUpper() == "Y")
            {
                searchDAO.Accessible(true);
            }

            Console.Write("How many feet long is their RV? (enter 0 for no RV):    ");
            while (!validLength)
            {
                try
                {
                    rvLength = Convert.ToInt32(Console.ReadLine());
                    searchDAO.MaxRVLength(rvLength);
                    validLength = true;
                }
                catch (Exception)
                {
                    Console.Write("Please enter the RV length in feet as a numeral:    ");
                }
            }

            Console.Write("Does the site need to have utility access? (Y/N):    ");
            utility = Console.ReadKey().KeyChar.ToString();
            Console.WriteLine();
            if (utility.ToUpper() == "Y")
            {
                searchDAO.Utility(true);
            }

            Console.Write("How many people will be camping? (enter 0 if unsure):    ");
            while (!validOccupancy)
            {
                try
                {
                    campers = Convert.ToInt32(Console.ReadLine());
                    searchDAO.MaxOccupancy(campers);
                    validOccupancy = true;
                }
                catch (Exception)
                {
                    Console.Write("Please enter the number of people camping as a numeral:    ");
                }
            }

            return searchDAO.RunCampgroundLevelSearch();
        }

        /// <summary>
        /// Ensure that user input is an integer
        /// </summary>
        /// <returns></returns>
        private int ReturnNumberEntry()
        {
            int desiredNumber = 0;
            bool enteringANumber = false;

            while (!enteringANumber)
            {
                try
                {
                    desiredNumber = Convert.ToInt32(Console.ReadLine());
                    enteringANumber = true;
                }
                catch
                {
                    Console.Write("Please enter a numeric value:    ");
                }
            }

            return desiredNumber;
        }

        /// <summary>
        /// Prints logo when main menu is first called
        /// </summary>
        private void PrintHeader()
        {
            Console.WriteLine(@"                                                                                                                 ");
            Console.WriteLine(@"                               _                  _          _                      _         _                  ");
            Console.WriteLine(@"                              (_)                | |        (_)                    | |       (_)                 ");
            Console.WriteLine(@"  ___  __ _  _ __ ___   _ __   _  _ __    __ _   | |_  _ __  _  _ __      __ _   __| |__   __ _  ___   ___  _ __ ");
            Console.WriteLine(@" / __|/ _` || '_ ` _ \ | '_ \ | || '_ \  / _` |  | __|| '__|| || '_ \    / _` | / _` |\ \ / /| |/ __| / _ \| '__|");
            Console.WriteLine(@"| (__| (_| || | | | | || |_) || || | | || (_| |  | |_ | |   | || |_) |  | (_| || (_| | \ V / | |\__ \|  __/| |   ");
            Console.WriteLine(@" \___|\__,_||_| |_| |_|| .__/ |_||_| |_| \__, |   \__||_|   |_|| .__/    \__,_| \__,_|  \_/  |_||___/ \___||_|   ");
            Console.WriteLine(@"                       | |                __/ |                | |                                               ");
            Console.WriteLine(@"                       |_|               |___/                 |_|                                               ");
            Console.WriteLine(@"                                                                                                                 ");
        }

        /// <summary>
        /// Prints top level menu
        /// </summary>
        private Dictionary<int, string> PrintMainMenu()
        {
            List<Park> parks = parkDAO.GetParkList();
            Dictionary<int, string> parkPairs = new Dictionary<int, string>();

            Console.WriteLine("Welcome to the National Park Information and Reservation Service");
            Console.WriteLine();
            Console.WriteLine("Park Selection Interface");

            foreach (Park park in parks)
            {
                Console.WriteLine($"  {park.Park_Id}) {park.Name} National Park");
                parkPairs[park.Park_Id] = park.Name;
            }
            Console.WriteLine();
            Console.WriteLine("  Q) Quit");
            Console.WriteLine();
            Console.Write("Select a park for more details:    ");

            return parkPairs;
        }

        /// <summary>
        /// Prints park info and options menu
        /// </summary>
        /// <param name="parkId"></param>
        private void PrintParkMenu(int parkId)
        {
            Console.WriteLine("Park Information Screen");
            Console.WriteLine(parkDAO.GetPark(parkId).ToString());
            Console.WriteLine("Select an option");
            Console.WriteLine("  1) View Campgrounds");
            Console.WriteLine("  2) Search for Reservations in All Campgrounds");
            Console.WriteLine("  3) Display All Reservations Whose Date Range Includes the Next 30 Days");
            Console.WriteLine("  4) Return to Previous Screen");
        }

        /// <summary>
        /// Prints options menu for campground screens
        /// </summary>
        private void PrintCampgroundMenu()
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("  1) Search for Available Reservation");
            Console.WriteLine("  2) Return to Previous Screen");
        }
    }
}
