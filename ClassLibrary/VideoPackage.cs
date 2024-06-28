using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class VideoPackage
    {

        public int PackageID { get; set; }
        public string? CardNumber { get; set; }
        public int? expireMonth { get; set; }
        public int? expireYear { get; set; }

        public string? cvc { get; set; }

        public string? stripeSubscriptionId { get; set; }





    }
}
