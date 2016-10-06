using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PMTickets.Models
{
    public class MainModel
    {
        public string MovieID { get; set; }
        public string Movie { get; set; }
        //public DateTime ShowDateTime { get; set; }
        public string ShowDate { get; set; }
        public string ShowTime { get; set; }
        public List<string> bookedseats { get; set; }
    }


    public class searchModel
    {

        public string movieDate { get; set; }
        public string movieTimeID { get; set; }

        public string moveNamesId { get; set; }
        public List<SelectListItem> movieNamesList { get; set; }
        public List<SelectListItem> movieTimingsList { get; set; }
    }
}