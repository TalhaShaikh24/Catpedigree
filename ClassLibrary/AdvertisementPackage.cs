using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class AdvertisementPackage
    {
        public int AdvertisementPackageID { get; set; }
        public string? AdvertisementPackageType { get; set; }
        public string? AdvertisementPackageName { get; set; }
        public decimal AdvertisementPackageCost { get; set; }
        public int NumberOfAdvertisement { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }

    public class UserAdvertisementPackage: AdvertisementPackage
    {
        public int UserAdvertisementPackageID { get; set; }

        public int UserId { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public string? Status { get; set; }
        public int ApprovedBy { get; set; }

        public string? Username { get; set; }

        public string? StatusApproved { get; set; }

        public string? FilePath { get; set; }


        public string? CardNumber { get; set; }
        public int? expireMonth { get; set; }
        public int? expireYear { get; set; }

        public string? cvc { get; set; }

        public string? stripeSubscriptionId { get; set; }
    }



    public class UtilizePurchasedAdvertisementPackage
    {
        public int UtilizePurchasedAdvertisementPackageID { get; set; }
        public int UserId { get; set; }
        public int UserAdvertisementPackageID { get; set; }
        public string? Status { get; set; }

        public IFormFile? AddFile { get; set; }
        public string? FilePath { get; set; }
        
    }




    public class AdvertisementPackageAndUserPackages
    {
        public List<UserAdvertisementPackage> dropdown { get; set; }
        public List<UserAdvertisementPackage> userAdvertisementPackages { get; set; }

    } 
    
    public class PaidAdvertisementsForView
    {
        public string? PaidAdvertisments { get; set; }

    }
}
