using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Group_Project_Movie_GC_June2020.Models
{
    /*
     * Currently uses:
     * title
     * id
     * release_date
     * runtime
     * tagline
     * poster_path
     * overview
     * ---------
     * Unused properties (To be implemented post-bootcamp)
     * budget
     * revenue
     * video
     * popularity
     */
    public class MovieDetail
    {
        public string title { get; set; }
        public int budget { get; set; }
        public int id { get; set; }
        public string original_title { get; set; }
        public float popularity { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public int revenue { get; set; }
        public int? runtime { get; set; }
        public string overview { get; set; }
        public string tagline { get; set; }
        public bool video { get; set; }
    }
}
