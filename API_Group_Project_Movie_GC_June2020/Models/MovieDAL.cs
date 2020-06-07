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
    public class MovieDAL
    {
        private readonly string _APIKey;

        public MovieDAL(string APIKey)
        {
            _APIKey = APIKey;
        }

        public string GetBaseUrl()
        {
            return "https://api.themoviedb.org/";
        }

        public MovieDetail GetMovieByTitleSearchJSON(string searchTitle, int i)
        {
            string url = GetBaseUrl() + $"3/search/movie?api_key={_APIKey}&query={searchTitle}";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string output = rd.ReadToEnd();
            JObject json = JObject.Parse(output);
            List<JToken> modelData = json["results"].ToList();
            MovieDetail movieObject = JsonConvert.DeserializeObject<MovieDetail>(modelData[i].ToString());
            url = GetBaseUrl() + $"3/movie/{movieObject.id}?api_key={_APIKey}";
            request = WebRequest.CreateHttp(url);
            response = (HttpWebResponse)request.GetResponse();
            rd = new StreamReader(response.GetResponseStream());
            output = rd.ReadToEnd();
            json = JObject.Parse(output);
            JToken runtimeFromJson = json["runtime"];
            movieObject.runtime = JsonConvert.DeserializeObject<int>(runtimeFromJson.ToString());
            return movieObject;
        }

        public List<MovieDetail> GetMovieListByTitleSearch(string searchTitle)
        {
            List<MovieDetail> ml = new List<MovieDetail>();
            MovieDetail m;
            for (int i = 0; i < 2000; i++)
            {
                try
                {
                    m = GetMovieByTitleSearchJSON(searchTitle, i);
                    ml.Add(m);
                }
                catch(ArgumentOutOfRangeException e)
                {
                    i = 2000;
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
