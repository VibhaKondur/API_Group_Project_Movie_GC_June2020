using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Group_Project_Movie_GC_June2020.Models
{
    public class MovieDetail
    {
        public string title { get; set; }
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public object belongs_to_collection { get; set; }
        public int budget { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public DateTime release_date { get; set; }
        public int revenue { get; set; }
        public int runtime { get; set; }
        public string overview { get; set; }
        public string tagline { get; set; }
        public string status { get; set; }
        public bool video { get; set; }
        public float vote_average { get; set; }
        public int vote_count { get; set; }
    }
}
