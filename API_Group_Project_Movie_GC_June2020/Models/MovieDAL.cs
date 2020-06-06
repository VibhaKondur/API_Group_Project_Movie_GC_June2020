using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_Group_Project_Movie_GC_June2020.Models
{
    public class MovieDAL
    {
        private readonly string _APIKey;

        public MovieDAL(string APIKey)
        {
            _APIKey = APIKey;
        }

        public HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.themoviedb.org/");
            return client;
        }

        public async Task<Movie> GetMovieByTitleSearchJSON(string searchTitle, int i)
        {
            var client = GetClient();
            var response = await client.GetAsync($"/3/search/movie?api_key={_APIKey}&query={searchTitle}");
            var searchOutput = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(searchOutput);
            List<JToken> modelData = json["results"].ToList();
            Movie movieObject = JsonConvert.DeserializeObject<Movie>(modelData[i].ToString());
            return movieObject;
        }

        public async Task<List<Movie>> GetMovieListByTitleSearch(string searchTitle)
        {
            List<Movie> ml = new List<Movie>();
            Movie m;
            for (int i = 0; i < 200; i++)
            {
                m = await GetMovieByTitleSearchJSON(searchTitle, i);
                ml.Add(m);
            }
            return ml;
        }

        //public async Task<List<MovieDetail>> GetFavoritesList()
        //{
        //}

        //public async Task<MovieDetail> GetMovieDetailForFavorites(int movie_id)
        //{
        //    var client = GetClient();
        //    var response = await client.GetAsync($"/movie/{id}?api_key={_APIKey}");

        //}


    }
}
