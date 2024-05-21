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
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public bool? IsBreerderLicenseUpload { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? BreerderName { get; set; }
        public int TypeOfCat { get; set; }
        public bool? ZoologicalNumber { get; set; }
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
        public int? Age { get; set; }
        public int CategoryId { get; set; }
        public int PackageId { get; set; }
        public bool? IsActive { get; set; }
        public new int CreatedBy { get; set; }
        public new DateTime? CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsExpired { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int TotalCount { get; set; }
    }

	public class ListingFilters
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}

	

	public class CatTypes
    {
        public int Id { get; set; }
        public string? CatType { get; set; }
    }


    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }


}
