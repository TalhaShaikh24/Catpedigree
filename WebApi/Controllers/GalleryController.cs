using ClassLibrary;
using Google.Api.Gax.Grpc;
using Google.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Models;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Repositories;
using WebApi.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DateTime = System.DateTime;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private string BaseUrl = "";
        private readonly IGalleryRepository _gallery;
        
        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImages");

        private readonly string _galleryimagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadGallery");

        public GalleryController(IConfiguration configuration, IGalleryRepository galleryRepository)
        {

            _gallery = galleryRepository;

           BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";
           
        }


        [HttpPost("GetAllGallery")]
        public Response GetAllGallery()
        {
            Response response = new Response();
            try
            {
                // Ensure the image directory exists
                if (!Directory.Exists(_galleryimagesPath))
                {
                    response = CustomStatusResponse.GetResponse(600);
                    response.ResponseMsg = "Image directory not found.";
                    response.Data = null;
                    return response;
                }

                // Get all images from the directory
                var existingImages = Directory.GetFiles(_galleryimagesPath).Select(Path.GetFileName).ToHashSet();

                // Retrieve gallery data from the repository
                var galleryData = _gallery.GetAllGallery();

                // Filter the gallery data to only include images present in the directory
                var images = galleryData
                    .Where(galleryItem => existingImages.Contains(galleryItem.FileName))
                    .Select(fileInfo => new Gallery
                    {
                        Id = Path.GetFileNameWithoutExtension(fileInfo.FileName).GetHashCode(),
                        FileName = fileInfo.FileName,
                        FilePath = $"{BaseUrl}{fileInfo.FilePath}?v={System.DateTime.UtcNow.Ticks}"
                    })
                    .ToList();

                // Handle response based on the filtered images
                if (images == null || !images.Any())
                {
                    return CustomStatusResponse.GetResponse(320); // No images found
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = images;
                    return response;
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }


    }
}
