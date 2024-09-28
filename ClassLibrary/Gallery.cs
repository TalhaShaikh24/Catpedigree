using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Gallery
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        public string? GalleryImagesPath { get; set; }
        public int CreatedBy { get; set; }



    }

    public class GalleryRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }


}
