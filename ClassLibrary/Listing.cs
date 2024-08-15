using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Listing:Category
    {
        public new int Id { get; set; }
        public string? Title { get; set; }
        public string? Keyword { get; set; }
        public string? Location { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public bool? IsBreerderLicenseUpload { get; set; }
        public string? Phone { get; set; }
        public new string? Email { get; set; }
        public string? BreerderName { get; set; }
        public int TypeOfCat { get; set; }
        public string? CatType { get; set; }
        public new bool? ZoologicalNumber { get; set; }
        public string? Gender { get; set; }
        public string? Description { get; set; }
        public string? PedigreeFilePath { get; set; }
        public string? VideoPath { get; set; }
        public string? Status { get; set; }
        public string? FeatureImagePath { get; set; }
        public string? GallaryImagesPath { get; set; }
        public IFormFile? PedigreeFile { get; set; }
        public IFormFile? FeatureImageFile { get; set; }
        public IFormFile? VideoFile { get; set; }
        public List<IFormFile>? GalleryImageFiles { get; set; }
        public string? Age { get; set; }
        public int CategoryId { get; set; }
        public int PackageId { get; set; }
        public bool? IsActive { get; set; }
        public new int CreatedBy { get; set; }
        public new DateTime? CreatedOn { get; set; }
        public new int? ModifiedBy { get; set; }
        public new DateTime? ModifiedOn { get; set; }
        public bool? IsExpired { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int TotalCount { get; set; }
        public decimal Price { get; set; }
        public decimal Weigth { get; set; } = 0;
        public bool IsVaccinated { get; set; }
        public bool IsCastration { get; set; }
        public bool IsSterilization { get; set; }
        public string? Color { get; set; }

        public int PromotionPackageId { get; set; }

        public string? PropertiestoShow { get; set; }

        public string? PromotionName { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }

        public string? CatteryName { get; set; }


        public string? Currency { get; set; }
        public string? Reason { get; set; }
        public DateTime? FirstApprovedOn { get; set; }
    }

	//public class ListingFilters
	//{
	//	public int PageNumber { get; set; }
	//	public int PageSize { get; set; }
	//}

	

	public class CatTypes
    {
        public int Id { get; set; }
        public string? CatType { get; set; }
    }

    public class Category:Register
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public new DateTime? CreatedOn { get; set; }
        public new int? CreatedBy { get; set; }
    }


}
