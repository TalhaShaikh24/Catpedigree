using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    
    public class Package : Common
    {
        public int PackageID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Duration { get; set; }
        public string? CategoriesIds { get; set; }
        public string? CategoryNames { get; set; }
        public int? AllowedListings { get; set; }
        public int? BaseListingCount { get; set; }
        public int? ExtraListingCount { get; set; }
        public bool? IsUnlimited { get; set; }
        public int? RemainingListings { get; set; }
    }

    public class UserPackages : Common
    {
        public int UserPackageID { get; set; }
        public int? UserID { get; set; }
        public int? PackageID { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? RemainingListings { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsExpired { get; set; }

    }


    public class VideoPackages
    {

        public int Id { get; set; }
        public string? PackageName { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public bool IsActive { get; set; }
        public int? VideoCount { get; set; }

    }
}
