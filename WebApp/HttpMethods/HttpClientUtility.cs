using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.HttpMethods
{
    public class HttpClientUtility
    {




        public static async Task<object> CustomHttpDashboard(string BaseUrl, string Url, string content, HttpContext httpContext)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));




                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
                if (!String.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                    request.Headers.Add("authorization", httpContext.Session.GetString("authorization"));
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.SendAsync(request);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<Response>(response);
                    httpContext.Session.SetString("authorization", obj.Token == null ? "" : obj.Token);
                    return response;

                }
                else
                    return null;
            }
        }

        public static async Task<object> CustomHttpIfileDashboard(string baseUrl, string url, Register obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseUrl);

                    // Set the authorization header if it exists in the session
                    if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                        client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                    var multiContent = new MultipartFormDataContent();

                    // Add JSON content
                    multiContent.Add(new StringContent(obj.Firstname ?? ""), "firstname");
                    multiContent.Add(new StringContent(obj.Lastname ?? ""), "lastname");
                    multiContent.Add(new StringContent(obj.Username ?? ""), "username");
                    multiContent.Add(new StringContent(obj.Email ?? ""), "email");
                    multiContent.Add(new StringContent(obj.Password ?? ""), "password");
                    multiContent.Add(new StringContent(obj.ContactNo ?? ""), "contactNo");
                    multiContent.Add(new StringContent(obj.Address ?? ""), "address");
                    multiContent.Add(new StringContent(obj.ProfileInfo ?? ""), "profileInfo");
                    multiContent.Add(new StringContent(obj.ZoologicalNumber ?? ""), "zoologicalNumber");

                    if (obj.ProfilePic != null)
                    {

                        multiContent.Add(new StreamContent(obj.ProfilePic.OpenReadStream()), "profilePic", obj.ProfilePic.FileName);
                    }
                    else
                    {

                        multiContent.Add(new StringContent(obj.ProfilePicPath ?? ""), "ProfilePicPath");
                    }

                    if (obj.BreederLicense != null)
                    {

                        multiContent.Add(new StreamContent(obj.BreederLicense.OpenReadStream()), "breederLicense", obj.BreederLicense.FileName);
                    }
                    else
                    {

                        multiContent.Add(new StringContent(obj.BreederLicensePath ?? ""), "BreederLicensePath");
                    }

                    // Send the HTTP request
                    HttpResponseMessage response = await client.PostAsync(url, multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                        httpContext.Session.SetString("authorization", deserializedResponse.Token ?? ""); // Ensure token is set
                        return responseBody;
                    }
                    else
                    {
                        // Handle unsuccessful response
                        // Log error message
                        Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                        // Return a meaningful response indicating failure
                        return new { Success = false, ErrorMessage = "Failed to send request" };
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine("Error in CustomHttp: " + ex.Message);
                    return new { Success = false, ErrorMessage = ex.Message };
                }
            }
        }

        public static async Task<object> CustomHttp(string BaseUrl, string Url, string content, HttpContext httpContext)
        {
           
             using (var client = new HttpClient())
             {
                    client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

               


                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
                if (!String.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                    request.Headers.Add("authorization", httpContext.Session.GetString("authorization"));
                    request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.SendAsync(request);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<Response>(response);
                    httpContext.Session.SetString("authorization", obj.Token == null ? "" : obj.Token);
                    return response;

                }
                else
                    return null;
            }
        }

        public static async Task<object> CustomHttpIfile(string baseUrl, string url, Register obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(baseUrl);

                    // Set the authorization header if it exists in the session
                    if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                        client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                    var multiContent = new MultipartFormDataContent();

                    // Add JSON content
                    multiContent.Add(new StringContent(obj.Firstname ?? ""), "firstname");
                    multiContent.Add(new StringContent(obj.Lastname ?? ""), "lastname");
                    multiContent.Add(new StringContent(obj.Username ?? ""), "username");
                    multiContent.Add(new StringContent(obj.Email ?? ""), "email");
                    multiContent.Add(new StringContent(obj.Password ?? ""), "password");
                    multiContent.Add(new StringContent(obj.ContactNo ?? ""), "contactNo");
                    multiContent.Add(new StringContent(obj.Address ?? ""), "address");
                    multiContent.Add(new StringContent(obj.ProfileInfo ?? ""), "profileInfo");
                    multiContent.Add(new StringContent(obj.ZoologicalNumber ?? ""), "zoologicalNumber");

                    if (obj.ProfilePic != null)
                    {

                        multiContent.Add(new StreamContent(obj.ProfilePic.OpenReadStream()), "profilePic", obj.ProfilePic.FileName);
                    }
                    else
                    {

                        multiContent.Add(new StringContent(obj.ProfilePicPath ?? ""), "ProfilePicPath");
                    }

                    if (obj.BreederLicense != null)
                    {

                        multiContent.Add(new StreamContent(obj.BreederLicense.OpenReadStream()), "breederLicense", obj.BreederLicense.FileName);
                    }
                    else
                    {

                        multiContent.Add(new StringContent(obj.BreederLicensePath ?? ""), "BreederLicensePath");
                    }

                    // Send the HTTP request
                    HttpResponseMessage response = await client.PostAsync(url, multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                        httpContext.Session.SetString("authorization", deserializedResponse.Token ?? ""); // Ensure token is set
                        return responseBody;
                    }
                    else
                    {
                        // Handle unsuccessful response
                        // Log error message
                        Console.WriteLine($"HTTP request failed with status code: {response.StatusCode}");
                        // Return a meaningful response indicating failure
                        return new { Success = false, ErrorMessage = "Failed to send request" };
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    Console.WriteLine("Error in CustomHttp: " + ex.Message);
                    return new { Success = false, ErrorMessage = ex.Message };
                }
            }
        }

        public static async Task<object> CustomHttpListing(string baseUrl, string url, Listing obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                // Set the authorization header if it exists in the session
                if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                    client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                    var multiContent = new MultipartFormDataContent();

                    multiContent.Add(new StringContent(obj.CategoryId.ToString() ?? ""), "CategoryId");
                    multiContent.Add(new StringContent(obj.Id.ToString()??"0"), "Id");
                    multiContent.Add(new StringContent(obj.Title ?? ""), "Title");
                    multiContent.Add(new StringContent(obj.Location ?? ""), "Location");
                    multiContent.Add(new StringContent(obj.State ?? ""), "State");
                    multiContent.Add(new StringContent(obj.City ?? ""), "City");
                    multiContent.Add(new StringContent(obj.Gender ?? ""), "Gender");
                    multiContent.Add(new StringContent(obj.Phone ?? ""), "Phone");
                    multiContent.Add(new StringContent(obj.Email ?? ""), "Email");
                    multiContent.Add(new StringContent(obj.BreerderName ?? ""), "BreerderName");
                    multiContent.Add(new StringContent(obj.TypeOfCat.ToString() ?? ""), "TypeOfCat");
                    multiContent.Add(new StringContent(obj.Age.ToString() ?? ""), "Age");
                    multiContent.Add(new StringContent(obj.PackageId.ToString() ?? ""), "PackageId");
                    multiContent.Add(new StringContent(obj.CategoryId.ToString() ?? ""), "CategoryId");
                    multiContent.Add(new StringContent(obj.IsBreerderLicenseUpload.ToString() ?? ""), "IsBreerderLicenseUpload");
                    multiContent.Add(new StringContent(obj.ZoologicalNumber.ToString() ?? ""), "ZoologicalNumber");
                    multiContent.Add(new StringContent(obj.Description ?? ""), "Description");


                if (obj.GalleryImageFiles != null)
                {
                    foreach (var item in obj.GalleryImageFiles)
                    {

                     multiContent.Add(new StreamContent(item.OpenReadStream()), "GalleryImageFiles", item.FileName);

                    }

                }

                if (obj.PedigreeFile != null)
                {

                   
                  multiContent.Add(new StreamContent(obj.PedigreeFile.OpenReadStream()), "GalleryImageFiles", obj.PedigreeFile.FileName);



                }

                if (obj.FeatureImageFile != null)
                {

                    multiContent.Add(new StreamContent(obj.FeatureImageFile.OpenReadStream()), "FeatureImageFile", obj.FeatureImageFile.FileName);
                }

                if (obj.VideoFile != null)
                {

                    multiContent.Add(new StreamContent(obj.VideoFile.OpenReadStream()), "VideoFile", obj.VideoFile.FileName);
                }

                HttpResponseMessage Res = await client.PostAsync(url, multiContent);

                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Response>(response);
                    httpContext.Session.SetString("authorization", result?.Token == null ? "" : result.Token);
                    return response;
                }
                else
                    return null;
            }

        }

        public static async Task<object> CustomHttpWithoutToken(string BaseUrl, string Url, string content, HttpContext httpContext)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.SendAsync(request);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    return response;

                }
                else
                    return null;
            }
        }

        public static async Task<object> CustomHttpBlog(string baseUrl, string url, Blog obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
              
                    client.BaseAddress = new Uri(baseUrl);

                    // Set the authorization header if it exists in the session
                    if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                        client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                    var multiContent = new MultipartFormDataContent();
                    multiContent.Add(new StringContent(obj.BlogID.ToString()), "BlogID");
                    multiContent.Add(new StringContent(obj.Title ?? ""), "Title");
                    multiContent.Add(new StringContent(obj.ShortDescription ?? ""), "ShortDescription");
                    multiContent.Add(new StringContent(obj.Content ?? ""), "Content");


                    if (obj.FeatureImage != null)
                    {

                        multiContent.Add(new StreamContent(obj.FeatureImage.OpenReadStream()), "FeatureImage", obj.FeatureImage.FileName);
                    }


                    // Send the HTTP request
                    HttpResponseMessage response = await client.PostAsync(url, multiContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                        httpContext.Session.SetString("authorization", deserializedResponse.Token ?? ""); // Ensure token is set
                        return responseBody;
                    }

                    return null;

                }
                
        }
   

        public static async Task<object> CustomHttpForGetAll(string BaseUrl, string Url, string content, HttpContext httpContext)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler)) 
            { 

                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
                request.Headers.Add("draw", httpContext.Request.Form["draw"].FirstOrDefault());
                request.Headers.Add("start", httpContext.Request.Form["start"].FirstOrDefault());
                request.Headers.Add("length", httpContext.Request.Form["length"].FirstOrDefault());
                request.Headers.Add("sortColumn", httpContext.Request.Form["columns[" + httpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault());
                request.Headers.Add("sortColumnDir", httpContext.Request.Form["order[0][dir]"].FirstOrDefault());
                request.Headers.Add("searchValue", httpContext.Request.Form["search[value]"].FirstOrDefault());



                if (!String.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                    request.Headers.Add("authorization", httpContext.Session.GetString("authorization"));
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.SendAsync(request);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<Response>(response);
                    httpContext.Session.SetString("authorization", obj.Token == null ? "" : obj.Token);
                    return response;

                }
                else
                    return null;
            }
        }

    }
}
