using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AssignAdvertisementPackages
    {
        public int AdvertisementPackageID { get; set; }
        public string AdvertisementPackageType { get; set; }
        public string AdvertisementPackageName { get; set; }

        public decimal AdvertisementPackageCost { get; set; }
        public int NumberOfAdvertisement { get; set; }
    }

    public  class AssignAdvertisementPackagesDTO
    {
        public List<AssignAdvertisementPackages> assignAdvertisementPackages { get; set; }


        public List<Register> Users { get; set; }


    }
    public class AssignedUserAdvertisementsList
    {
        public string Username { get; set; }
        public string AdvertisementPackageName { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public DateTime CreatedOn { get; set; }

    }


}
