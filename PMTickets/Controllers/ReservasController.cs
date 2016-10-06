using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMTickets.DAL;
using PMTickets.Models;
using System.Web.Configuration;

namespace PMTickets.Controllers
{
    public class ReservasController : Controller
    {
        public ActionResult Index()
        {
            CRUD objDAL = new CRUD();
            searchModel sm = new searchModel();

            sm.movieNamesList = objDAL.getMoviesDropDownList();
            sm.movieTimingsList = objDAL.getMovieTimings();
            sm.movieDate = DateTime.Now.ToString(("MM'/'dd'/'yyyy"));
            return View(sm);
        }

        [HttpPost]
        public ActionResult Index(searchModel SearchModel)
        {
            CRUD objDAL = new CRUD();
            List<SelectListItem> movies = objDAL.getMoviesDropDownList();
            string ddlSelectedMoviesNameValue = SearchModel.moveNamesId;
            SearchModel.movieNamesList = new SelectList(movies, "Value", "Text", ddlSelectedMoviesNameValue).ToList();

            List<SelectListItem> moviesTimings = objDAL.getMovieTimings();
            string ddlMovieTimingsValue = SearchModel.movieTimeID;
            SearchModel.movieTimingsList = new SelectList(moviesTimings, "Value", "Text", ddlMovieTimingsValue).ToList();
            SearchModel.movieDate = SearchModel.movieDate;

            return View(SearchModel);
        }

        public ActionResult getBookedSeats(string movieID, string movieDate, string movieTime)
        {
            CRUD objCRUD = new CRUD();
            bool retData;
            List<string> retStr = objCRUD.getBookedSeats(movieID, movieDate, movieTime, out retData);
            if (retData)
            {
                return Json(retStr, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return ExecutionError("Error Message");
            }
        }

        public ActionResult createMovie()
        {
            CRUD objDAL = new CRUD();
            searchModel sm = new searchModel();
            //sm.movieNamesList = objDAL.getMoviesDropDownList();
            sm.movieTimingsList = objDAL.getMovieTimings();
            sm.movieDate = DateTime.Now.ToString(("MM'/'dd'/'yyyy"));
            return View(sm);

        }

        public EmptyResult ExecutionError(string message)
        {
            Response.StatusCode = 550;
            Response.Write(message);
            return new EmptyResult();
        }

        public void updateMovieSeats(string movieID, string movieDate, string movieTime, string bookedSeats)
        {
            CRUD objCRUD = new CRUD();
            objCRUD.updateMovieSeats(movieID, movieDate, movieTime, bookedSeats);
        }

        public ActionResult createMovieINFile(string movieID, string movieDate, string movieTime)
        {
            CRUD objCRUD = new CRUD();
            bool retData;
            objCRUD.createMovie(movieID, movieDate, movieTime, out retData);
            if (retData)
            {
                return new EmptyResult();
            }
            else
            {
                return ExecutionError("Error Message");
            }
        }

        public ActionResult getMovieData()
        {
            CRUD objCRUD = new CRUD();
            IList<MainModel> listMovies = objCRUD.getMovies();
            return Json(listMovies, JsonRequestBehavior.AllowGet);
        }

    }
}
