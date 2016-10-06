using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMTickets.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Configuration;

namespace PMTickets.DAL
{
    
    public class CRUD
    {
       string dataFileName = WebConfigurationManager.AppSettings["FileLocation"].ToString();       

        public IList<MainModel> getMovies()
        {
            IList<MainModel> movies = new List<MainModel>();
            try {
                if (File.Exists(dataFileName))
                {
                    JArray o1 = JArray.Parse(File.ReadAllText(dataFileName));

                    var jsonSchemaGenerator = new JsonSchemaGenerator();

                    var myType = typeof(List<MainModel>);

                    var schema = jsonSchemaGenerator.Generate(myType);

                    JArray o2 = new JArray();

                    using (StreamReader file = File.OpenText(dataFileName))

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        o2 = (JArray)JToken.ReadFrom(reader);
                        bool valid = o2.IsValid(schema);
                    }

                    movies = o2.ToObject<IList<MainModel>>();
                }
                return movies;
            }
            catch (Exception ex)
            {
                return movies;
            }
        }

        public bool updateMovieSeats(string movieID, string movieDate, string movieTime, string bookedSeats)
        {
            try
            {
                List<string> BookedSeats = bookedSeats.Split('$').ToList<string>();
                BookedSeats.Remove("");
                if (File.Exists(dataFileName))
                {
                    JArray o1 = JArray.Parse(File.ReadAllText(dataFileName));

                    var jsonSchemaGenerator = new JsonSchemaGenerator();

                    var myType = typeof(List<MainModel>);

                    var schema = jsonSchemaGenerator.Generate(myType);

                    JArray o2 = new JArray();

                    using (StreamReader file = File.OpenText(dataFileName))

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        o2 = (JArray)JToken.ReadFrom(reader);
                        bool valid = o2.IsValid(schema);
                    }

                    List<MainModel> ALLCategories = o2.ToObject<List<MainModel>>();
                    bool added = false;
                    for (int i = 0; i < ALLCategories.Count; i++)
                    {
                        if (ALLCategories[i].Movie == movieID && ALLCategories[i].ShowDate == movieDate && ALLCategories[i].ShowTime==movieTime)
                        {
                            ALLCategories[i].bookedseats.AddRange(BookedSeats);
                            added = true;
                        }
                    }
                    if (!added)
                    {
                        //ALLCategories.Add(cate); //NEW MOVIE
                    }

                    File.WriteAllText(string.Concat(dataFileName, "_New"), JsonConvert.SerializeObject(ALLCategories));
                    using (StreamWriter file = File.CreateText(string.Concat(dataFileName, "_New")))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, ALLCategories);
                    }

                    if (File.Exists(dataFileName))
                    {
                        File.Delete(dataFileName);
                        System.IO.File.Move(string.Concat(dataFileName, "_New"), dataFileName);
                    }
                }
                else
                {
                    //List<MainModel> C = new List<MainModel>();
                    //C.Add(cate);
                    //File.WriteAllText(dataFileName, JsonConvert.SerializeObject(C));
                    //using (StreamWriter file = File.CreateText(dataFileName))
                    //{
                    //    JsonSerializer serializer = new JsonSerializer();
                    //    serializer.Serialize(file, C);
                    //}
                }
            }
            catch (Exception ex)
            { }
            return false;
        }

        public List<SelectListItem> getMoviesDropDownList()
        {
            List<SelectListItem> objSelectListMain = new List<SelectListItem>();
            
            try
            {
                if (File.Exists(dataFileName))
                {
                    JArray o1 = JArray.Parse(File.ReadAllText(dataFileName));

                    var jsonSchemaGenerator = new JsonSchemaGenerator();

                    var myType = typeof(List<MainModel>);

                    var schema = jsonSchemaGenerator.Generate(myType);

                    JArray o2 = new JArray();

                    using (StreamReader file = File.OpenText(dataFileName))

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        o2 = (JArray)JToken.ReadFrom(reader);
                        bool valid = o2.IsValid(schema);
                    }
                    
                    List<MainModel> ALLMovies = o2.ToObject<List<MainModel>>();
                    
                    foreach (MainModel m in ALLMovies)
                    {
                        if (!objSelectListMain.Exists(x => x.Text == m.Movie))
                        {
                            SelectListItem objSelectList = new SelectListItem();
                            objSelectList.Text = m.Movie;
                            objSelectList.Value = m.MovieID;

                            objSelectListMain.Add(objSelectList);
                        }
                    }
                }
                return objSelectListMain;
            }
            catch (Exception ex)
            {
                return objSelectListMain;
            }
        }

        public List<SelectListItem> getMovieTimings()
        {
            List<SelectListItem> objSelectList = new List<SelectListItem>();

            string movieTimes = WebConfigurationManager.AppSettings["MoveTimes"].ToString();
            
            List<string> objList = movieTimes.Split('$').ToList<string>();
            foreach (string str in objList)
            { 
                SelectListItem newItem = new SelectListItem();
                newItem.Text=str;
                newItem.Value=str;
                objSelectList.Add(newItem);
            }

            return objSelectList;            
        }

        public List<string> getBookedSeats(string movieID, string movieDate, string movieTime, out bool returnVal)
        {
            returnVal = false;

            List<string> returnBookedSeats = new List<string>();
            try
            {
                if (File.Exists(dataFileName))
                {
                    JArray o1 = JArray.Parse(File.ReadAllText(dataFileName));

                    var jsonSchemaGenerator = new JsonSchemaGenerator();

                    var myType = typeof(List<MainModel>);

                    var schema = jsonSchemaGenerator.Generate(myType);

                    JArray o2 = new JArray();

                    using (StreamReader file = File.OpenText(dataFileName))

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        o2 = (JArray)JToken.ReadFrom(reader);
                        bool valid = o2.IsValid(schema);
                    }

                    List<MainModel> ALLMovies = o2.ToObject<List<MainModel>>();

                    MainModel m = ALLMovies.Where(x => x.Movie == movieID && x.ShowDate==movieDate && x.ShowTime==movieTime).FirstOrDefault();

                    returnBookedSeats = m.bookedseats;

                    returnVal = true;
                }
                
            }
            catch (Exception ex)
            {
                return returnBookedSeats;
            }

            return returnBookedSeats;
        }
        public void createMovie(string movieName, string movieDate, string movieTime, out bool retValue)
        {
            retValue = false;
            try
            {                
                if (File.Exists(dataFileName))
                {
                    JArray o1 = JArray.Parse(File.ReadAllText(dataFileName));

                    var jsonSchemaGenerator = new JsonSchemaGenerator();

                    var myType = typeof(List<MainModel>);

                    var schema = jsonSchemaGenerator.Generate(myType);

                    JArray o2 = new JArray();

                    using (StreamReader file = File.OpenText(dataFileName))

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        o2 = (JArray)JToken.ReadFrom(reader);
                        bool valid = o2.IsValid(schema);
                    }

                    List<MainModel> ALLCategories = o2.ToObject<List<MainModel>>();
                    bool added = false;
                    ///////////////////////////////////////////////////
                    if (!ALLCategories.Exists(x => x.Movie == movieName && x.ShowDate == movieDate && x.ShowTime == movieTime))
                    {
                        retValue = true;
                        MainModel newObj = new MainModel();
                        newObj.Movie = movieName;
                        newObj.ShowDate = movieDate;
                        newObj.ShowTime = movieTime;
                        newObj.bookedseats = new List<string>();
                        newObj.MovieID = "movie" + (ALLCategories.Count + 1);
                        ALLCategories.Add(newObj);
                    }
                    else
                    {
                        retValue = false;
                        return;
                    }


                    ///////////////////////////////////////////////////
                    File.WriteAllText(string.Concat(dataFileName, "_New"), JsonConvert.SerializeObject(ALLCategories));
                    using (StreamWriter file = File.CreateText(string.Concat(dataFileName, "_New")))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, ALLCategories);
                    }

                    if (File.Exists(dataFileName))
                    {
                        File.Delete(dataFileName);
                        System.IO.File.Move(string.Concat(dataFileName, "_New"), dataFileName);
                    }
                }
                else
                {
                    List<MainModel> C = new List<MainModel>();
                    MainModel newObj = new MainModel();
                    newObj.Movie = movieName;
                    newObj.ShowDate = movieDate;
                    newObj.ShowTime = movieTime;
                    newObj.bookedseats = new List<string>();
                    newObj.MovieID = "movie01";
                    C.Add(newObj);

                    File.WriteAllText(dataFileName, JsonConvert.SerializeObject(C));
                    using (StreamWriter file = File.CreateText(dataFileName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, C);
                    }
                    retValue = true;
                }
            }
            catch (Exception ex)
            { }            
        }
    }
}