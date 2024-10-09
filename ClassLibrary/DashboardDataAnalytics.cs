using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{

    public class UserPromotionPackagesAnalytics
    {

        public int UserPromotionPackageID { get; set; }

        public int PromotionPackagesID { get; set; }

        public string? Username { get; set; }

        public string? PromotionPackageName { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime ExpiryDate { get; set; }

    }

        public class DashboardDataAnalytics
    {
        public string? jsonObject { get; set; }

        public List<UserAdvertisementPackage>? advertisementPackage { get; set; }
        public List<UserPromotionPackagesAnalytics>? assignPromotionPackage { get; set; }


    }
}
