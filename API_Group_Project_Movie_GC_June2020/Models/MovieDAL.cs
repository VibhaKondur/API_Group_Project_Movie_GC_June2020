using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        /*
         * Gets the client
         */
        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://api.themoviedb.org/");
            return client;
        }

        /*
         * This method gets a list of movies by a search term
         */
        public async Task<List<MovieDetail>> GetMovieListByTitleSearch(string searchTitle)
        {
            var client = GetClient();
            // Get the list of simplified movies
            var response = await client.GetAsync($"/3/search/movie?api_key={_APIKey}&query={searchTitle}");
            var movieJson = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(movieJson);
            JToken modelData = json["results"];
            List<MovieDetail> movieObject = JsonConvert.DeserializeObject<List<MovieDetail>>(modelData.ToString());
            // Make additional API calls to get detailed information about movies
            foreach (MovieDetail m in movieObject)
            {
                var response2 = await client.GetAsync($"/3/movie/{m.id}?api_key={_APIKey}");
                var movieJson2 = await response2.Content.ReadAsStringAsync();
                JObject json2 = JObject.Parse(movieJson2);
                JToken runtimeFromJson = json2["runtime"];
                if (runtimeFromJson != null)
                {
                    if (!int.TryParse(runtimeFromJson.ToString(), out int i))
                    {
                        m.runtime = 0;
                    }
                    else
                    {
                        m.runtime = JsonConvert.DeserializeObject<int>(runtimeFromJson.ToString());
                    }
                }
                else
                {
                    m.runtime = 0;
                }
            }
            foreach (MovieDetail m in movieObject)
            {
                if (m.release_date == null || m.release_date == "")
                {
                    m.release_date = "n/a";
                }
            }
            return movieObject;
        }

        /*
         * Gets list of favorites a user added
         */
        public async Task<List<MovieDetail>> GetFavoriteMoviesList(List<Favorites> fl)
        {
            List<MovieDetail> ml = new List<MovieDetail>();
            var client = GetClient();
            foreach (Favorites f in fl)
            {
                var response = await client.GetAsync($"/3/movie/{f.ApiId}?api_key={_APIKey}");
                var movieJson = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(movieJson);
                MovieDetail movie = JsonConvert.DeserializeObject<MovieDetail>(json.ToString());
                ml.Add(movie);
            }
            return ml;
        }
    }
}
