using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Group_Project_Movie_GC_June2020.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_Group_Project_Movie_GC_June2020.Controllers
{
    public class MovieController : Controller
    {
        private readonly string _APIKey;

        private readonly MovieDAL _movieDAL;

        public MovieController(IConfiguration configuration)
        {
            _APIKey = configuration.GetSection("APIKey")["MovieAPI"];
            _movieDAL = new MovieDAL(_APIKey);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
