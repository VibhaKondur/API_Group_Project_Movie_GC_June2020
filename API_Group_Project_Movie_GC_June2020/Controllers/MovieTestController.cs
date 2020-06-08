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
    [Authorize]
    public class MovieTestController : Controller
    {
        private readonly FilmsDbContext _context;

        private readonly string _APIKey;

        private readonly MovieDALAsync _movieDAL;

        public MovieTestController(IConfiguration configuration, FilmsDbContext context)
        {
            _APIKey = configuration.GetSection("APIKey")["MovieAPI"];
            _movieDAL = new MovieDALAsync(_APIKey);
            _context = context;
        }

        public IActionResult SearchIndex()
        {
            return View();
        }

        public IActionResult AddToFavorite(int id)
        {
            Favorites favorite = new Favorites();
            favorite.ApiId = id;
            favorite.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if(_context.Favorites.Where(x => (x.ApiId == id) && (x.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)).ToList().Count > 0)
            {
                return RedirectToAction("Favorites");
            }
            if (ModelState.IsValid)
            {
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }
            return RedirectToAction("Favorites");/*, new { id = favorite.Id});*/
        }

        public async Task<IActionResult> Favorites()
        {
            List<MovieDetail> ml = new List<MovieDetail>();
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favoritesOfUser = _context.Favorites.Where(x => x.UserId == id).ToList();
            ml = await _movieDAL.GetFavoriteMoviesList(favoritesOfUser);
            return View(ml);
        }

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

        [HttpPost]
        public async Task<IActionResult> Results(string searchTitle)
        {
            List<MovieDetail> ml = await _movieDAL.GetMovieListByTitleSearch(searchTitle);
            return View(ml);
        }
    }
}
