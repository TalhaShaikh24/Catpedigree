using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels
{
    public class ResponseData
    {
        public Listing Listing { get; set; }
        public List<Package> Package { get; set; }
    }

    public class ListingForView
    {
        public int status { get; set; }
        public string responseMsg { get; set; }
        public ResponseData data { get; set; }
        public string token { get; set; }

    }
}
