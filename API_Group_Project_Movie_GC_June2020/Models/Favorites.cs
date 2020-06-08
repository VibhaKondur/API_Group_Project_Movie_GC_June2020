using System;
using System.Collections.Generic;

namespace API_Group_Project_Movie_GC_June2020.Models
{

    /*
     * ApiId is a film's ID in the TheMovieDb.org api
     * UserId is the user's primary key of user currently logged in
     */
    public partial class Favorites
    {
        public int Id { get; set; }
        public int? ApiId { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
