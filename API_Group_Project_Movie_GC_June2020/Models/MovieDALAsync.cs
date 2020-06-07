﻿using Newtonsoft.Json;
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
    public class MovieDALAsync
    {
        private readonly string _APIKey;

        public MovieDALAsync(string APIKey)
        {
            _APIKey = APIKey;
        }

        public HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"https://api.themoviedb.org/");
            return client;
        }

        public async Task<MovieDetail> GetMovieByTitle(string searchTitle, int i)
        {
            var client = GetClient();
            var response = await client.GetAsync($"/3/search/movie?api_key={_APIKey}&query={searchTitle}");
            var movieJson = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(movieJson);
            List<JToken> modelData = json["results"].ToList();
            MovieDetail movieObject = JsonConvert.DeserializeObject<MovieDetail>(modelData[i].ToString());
            response = await client.GetAsync($"/3/movie/{movieObject.id}?api_key={_APIKey}");
            movieJson = await response.Content.ReadAsStringAsync();
            json = JObject.Parse(movieJson);
            JToken runtimeFromJson = json["runtime"];
            movieObject.runtime = JsonConvert.DeserializeObject<int>(runtimeFromJson.ToString());
            return movieObject;
        }

        public async Task<List<MovieDetail>> GetMovieListByTitleSearch(string searchTitle)
        {
            List<MovieDetail> ml = new List<MovieDetail>();
            MovieDetail m;
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    m = await GetMovieByTitle(searchTitle, i);
                    ml.Add(m);
                }
                catch (ArgumentOutOfRangeException)
                {
                    i = 20;
                }
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