using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels
{
   

    public class ListingForView
    {
        public int status { get; set; }
        public string responseMsg { get; set; }
        public Listing data { get; set; }
        public string token { get; set; }
    }
}
