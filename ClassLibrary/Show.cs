using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Show
    {
        public int ShowId { get; set; }

        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? FeatureImagePath { get; set; }

        public IFormFile? FeatureImage { get; set; }
        public string? GallaryImagePath { get; set; }
        public List<IFormFile>? GallaryImage { get; set; }



        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }


        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public string? Username { get; set; }


        public int TotalCount { get; set; }
        public int FetchedCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }


    public class ShowResult
    {
        public List<Show> Shows { get; set; }
        public int TotalCount { get; set; }
        public int FetchedCount { get; set; }
    }

}
