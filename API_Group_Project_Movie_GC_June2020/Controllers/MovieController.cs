using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API_Group_Project_Movie_GC_June2020.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_Group_Project_Movie_GC_June2020.Controllers
{

    public class MovieController : Controller
    {
        private readonly FilmsDbContext _context;

        /*
         * Hid API key correctly with .gitIgnore
         */
        private readonly string _APIKey;

        private readonly MovieDAL _movieDAL;

        public MovieController(IConfiguration configuration, FilmsDbContext context)
        {
            _APIKey = configuration.GetSection("APIKey")["MovieAPI"];
            _movieDAL = new MovieDAL(_APIKey);
            _context = context;
        }

        /*
         * Displays SearchIndex view to search with
         */
        public IActionResult SearchIndex()
        {
            return View();
        }

        /*
         * Action to add a movie to your favorites
         * Redirects to favorites view
         */
        [Authorize]
        public IActionResult AddToFavorite(int id)
        {
            Favorites favorite = new Favorites
            {
                ApiId = id,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            };
            if (_context.Favorites.Where(x => (x.ApiId == id) && (x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)).ToList().Count > 0)
            {
                return RedirectToAction("Favorites");
            }
            if (ModelState.IsValid)
            {
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }
            return RedirectToAction("Favorites");
        }

        /*
         * Displays favorites view for user logged in
         */
        [Authorize]
        public async Task<IActionResult> Favorites()
        {
            List<MovieDetail> ml = new List<MovieDetail>();
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoritesOfUser = _context.Favorites.Where(x => x.UserId == id).ToList();
            ml = await _movieDAL.GetFavoriteMoviesList(favoritesOfUser);
            return View(ml);
        }

        /*
         * Remove film from your favorites
         * Redirects to favorites view
         */
        [Authorize]
        public IActionResult RemoveFilm(int id)
        {
            string uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Favorites found = _context.Favorites.FirstOrDefault(x => (x.ApiId == id) && (x.UserId == uid));
            if (found != null)
            {
                _context.Favorites.Remove(found);
                _context.SaveChanges();
            }
            return RedirectToAction("Favorites");
        }

        /*
         * Displays search results view after you've searched in the SearchIndex view
         */
        [HttpPost]
        public async Task<IActionResult> Results(string searchTitle)
        {
            List<MovieDetail> ml = await _movieDAL.GetMovieListByTitleSearch(searchTitle);
            return View(ml);
        }
    }
}
