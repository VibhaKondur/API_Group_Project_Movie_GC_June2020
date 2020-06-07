using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Group_Project_Movie_GC_June2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_Group_Project_Movie_GC_June2020.Controllers
{
    public class MovieTestController : Controller
    {
        private readonly string _APIKey;

        private readonly MovieDALAsync _movieDAL;

        public MovieTestController(IConfiguration configuration)
        {
            _APIKey = configuration.GetSection("APIKey")["MovieAPI"];
            _movieDAL = new MovieDALAsync(_APIKey);
        }
        public IActionResult SearchIndex()
        {
            return View();
        }


        //public IActionResult SearchResult(string searchTitle)
        //{
        //    List<Movie> ml = new List<Movie>();
        //    ml = _movieDAL.GetMovieListByTitleSearch(searchTitle);
        //    return RedirectToAction("Results", ml);
        //}

        [HttpPost]
        public async Task<IActionResult> Results(string searchTitle)
        {
            List<MovieDetail> ml = new List<MovieDetail>();
            ml = await _movieDAL.GetMovieListByTitleSearch(searchTitle);
            return View(ml);
        }

    }
}
