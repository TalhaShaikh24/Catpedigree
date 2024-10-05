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
                //if (!String.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                //    request.Headers.Add("authorization", httpContext.Session.GetString("authorization"));

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    request.Headers.Add("authorization", authorizationToken);
                }

                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.SendAsync(request);
                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<Response>(response);
                    //httpContext.Session.SetString("authorization", obj.Token == null ? "" : obj.Token);

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", obj.Token == null ? "" : obj.Token, cookieOptions);
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
                
                    client.BaseAddress = new Uri(baseUrl);

                    // Set the authorization header if it exists in the cookies

                    if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                    {
                        client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                    }

                var multiContent = new MultipartFormDataContent();


                string dateOfBirthString = obj.DateofBirth?.ToString("yyyy-MM-dd") ?? "";

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

                multiContent.Add(new StringContent(obj.Country ?? ""), "country");
                multiContent.Add(new StringContent(obj.City ?? ""), "city");
                multiContent.Add(new StringContent(obj.Province ?? ""), "province");
                multiContent.Add(new StringContent(dateOfBirthString ?? null), "DateofBirth");

                multiContent.Add(new StringContent(obj.FaceBook ?? ""), "FaceBook");
                multiContent.Add(new StringContent(obj.Insta ?? ""), "Insta");
                multiContent.Add(new StringContent(obj.Twitter ?? ""), "Twitter");


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
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(5)
                        };

                        httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);

                        var objUser = new
                        {
                            dataObj = deserializedResponse.Data,
                        };

                        httpContext.Response.Cookies.Append("user", JsonConvert.SerializeObject(objUser), cookieOptions);
                        return responseBody;

                        }
                    else
                        return null;
                }
        }
        public static async Task<object> CustomHttpIfileDashboardUserUpdate(string baseUrl, string url, Register obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
                
                    client.BaseAddress = new Uri(baseUrl);

                    // Set the authorization header if it exists in the cookies

                    if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                    {
                        client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                    }

                var multiContent = new MultipartFormDataContent();


                string dateOfBirthString = obj.DateofBirth?.ToString("yyyy-MM-dd") ?? "";

                // Add JSON content
                multiContent.Add(new StringContent(obj.UserId.ToString()), "UserId");
                multiContent.Add(new StringContent(obj.Firstname ?? ""), "firstname");
                    multiContent.Add(new StringContent(obj.Lastname ?? ""), "lastname");
                    multiContent.Add(new StringContent(obj.Username ?? ""), "username");
                    multiContent.Add(new StringContent(obj.Email ?? ""), "email");
                    multiContent.Add(new StringContent(obj.ContactNo ?? ""), "contactNo");
                    multiContent.Add(new StringContent(obj.Address ?? ""), "address");
                    multiContent.Add(new StringContent(obj.ProfileInfo ?? ""), "profileInfo");
                    multiContent.Add(new StringContent(obj.ZoologicalNumber ?? ""), "zoologicalNumber");

                multiContent.Add(new StringContent(obj.Country ?? ""), "country");
                multiContent.Add(new StringContent(obj.City ?? ""), "city");
                multiContent.Add(new StringContent(obj.Province ?? ""), "province");
                multiContent.Add(new StringContent(dateOfBirthString ?? null), "DateofBirth");

                multiContent.Add(new StringContent(obj.FaceBook ?? ""), "FaceBook");
                multiContent.Add(new StringContent(obj.Insta ?? ""), "Insta");
                multiContent.Add(new StringContent(obj.Twitter ?? ""), "Twitter");


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
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(5)
                        };

                        httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);

                        return responseBody;

                    }
                    else
                        return null;
                }
        }


        public static async Task<object> CustomHttpreplaceFileDashboard(string baseUrl, string url, IFormFile file, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);

                // Set the authorization header if it exists in the cookies

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                }

                var multiContent = new MultipartFormDataContent();

                if (file != null)
                {

                    multiContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                }

                // Send the HTTP request
                HttpResponseMessage response = await client.PostAsync(url, multiContent);



                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);

                    return responseBody;

                }
                else
                    return null;
            }
        }


        public static async Task<object> CustomHttSingleFileDashboard(string baseUrl, string url, IFormFile file, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);

                // Set the authorization header if it exists in the cookies

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                }

                var multiContent = new MultipartFormDataContent();

                if (file != null)
                {

                    multiContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                }

                // Send the HTTP request
                HttpResponseMessage response = await client.PostAsync(url, multiContent);



                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);

                    return responseBody;

                }
                else
                    return null;
            }
        }
        public static async Task<object> CustomHttSingleFileDashboard(string baseUrl, string url, List<IFormFile> files, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);

                // Set the authorization header if it exists in the cookies

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                }

                var multiContent = new MultipartFormDataContent();
                foreach (var item in files)
                {

                    if (item != null)
                    {

                        multiContent.Add(new StreamContent(item.OpenReadStream()), "files", item.FileName);
                    }
                }

            

                // Send the HTTP request
                HttpResponseMessage response = await client.PostAsync(url, multiContent);



                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);

                    return responseBody;

                }
                else
                    return null;
            }
        }
        public static async Task<object> CustomHttpBlogFileDashboard(string baseUrl, string url, IFormFile file, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);

                // Set the authorization header if it exists in the cookies

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                }


                var multiContent = new MultipartFormDataContent();

                if (file != null)
                {

                    multiContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                }

               

                // Send the HTTP request
                HttpResponseMessage response = await client.PostAsync(url, multiContent);



                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var deserializedResponse = JsonConvert.DeserializeObject<Response>(responseBody);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);

                    return responseBody;

                }
                else
                    return null;
            }
        }



        public static async Task<object> CustomHttp(string baseUrl, string url, string content, HttpContext httpContext)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUrl) })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                };

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    request.Headers.Add("authorization", authorizationToken);
                }

                    var Res = await client.SendAsync(request);


                   var response = Res.Content.ReadAsStringAsync().Result;


                if (Res.IsSuccessStatusCode)
                    {
            
                        
                               var obj = JsonConvert.DeserializeObject<Response>(response);

                   
                                var cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                                    SameSite = SameSiteMode.Strict,
                                    Expires = DateTimeOffset.UtcNow.AddDays(5)
                                };

                                httpContext.Response.Cookies.Append("authorization", obj.Token == null ? "" : obj.Token, cookieOptions);

                                return response;
                            }
                            else
                                return null;

                   }

                  
                
               
            
        }
        public static async Task<object> LogInCustomHttp(string baseUrl, string url, string content, HttpContext httpContext)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUrl) })
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                };

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    request.Headers.Add("authorization", authorizationToken);
                }

                    var Res = await client.SendAsync(request);


                   var response = Res.Content.ReadAsStringAsync().Result;


                if (Res.IsSuccessStatusCode)
                    {
            
                        
                               var obj = JsonConvert.DeserializeObject<Response>(response);

                   
                                var cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                                    SameSite = SameSiteMode.Strict,
                                    Expires = DateTimeOffset.UtcNow.AddDays(5)
                                };

                                httpContext.Response.Cookies.Append("authorization", obj.Token == null ? "" : obj.Token, cookieOptions);


                                httpContext.Response.Cookies.Append("user", JsonConvert.SerializeObject(obj.Data), cookieOptions);

                                return response;
                            }
                            else
                                return null;

                   }

                  
                
               
            
        }

        public static async Task<object> LogOutCustomHttp(string BaseUrl, string Url, string content, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(BaseUrl);

                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);


                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    request.Headers.Add("authorization", authorizationToken);
                }



                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.SendAsync(request);

                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;

                    CookieOptions options = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Ensure this is set to true in production to send the cookie only over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(-1) // Expire the cookie immediately
                    };

                    httpContext.Response.Cookies.Append("authorization", "", options);
                    httpContext.Response.Cookies.Append("user", "", options);

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


                    // Set the authorization header if it exists in the cookies

                    if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                    {
                        client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                    }

                    var multiContent = new MultipartFormDataContent();



                    // Convert DateTime to string
                    string dateOfBirthString = obj.DateofBirth?.ToString("yyyy-MM-dd") ?? "";

                    // Create StringContent with the formatted date string
             
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
                    multiContent.Add(new StringContent(obj.RoleId.ToString() ?? null), "roleid");

                    multiContent.Add(new StringContent(obj.Country ?? ""), "country");
                    multiContent.Add(new StringContent(obj.City ?? ""), "city");
                    multiContent.Add(new StringContent(obj.Province ?? ""), "province");
                    multiContent.Add(new StringContent(dateOfBirthString ?? null), "DateofBirth");

                    multiContent.Add(new StringContent(obj.FaceBook ?? ""), "FaceBook");
                    multiContent.Add(new StringContent(obj.Insta ?? ""), "Insta");
                    multiContent.Add(new StringContent(obj.Twitter ?? ""), "Twitter");



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
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(5)
                        };

                        httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);
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
                //if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                //    client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                }

                var multiContent = new MultipartFormDataContent();

                multiContent.Add(new StringContent(obj.CategoryId.ToString() ?? ""), "CategoryId");
                multiContent.Add(new StringContent(obj.Id.ToString() ?? "0"), "Id");
                multiContent.Add(new StringContent(obj.Title ?? ""), "Title");
                multiContent.Add(new StringContent(obj.Country ?? ""), "Country");
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
                multiContent.Add(new StringContent(obj.Weigth.ToString() ?? ""), "Weigth");
                multiContent.Add(new StringContent(obj.Price.ToString() ?? ""), "Price");
                multiContent.Add(new StringContent(obj.IsPriceRequest.ToString() ?? ""), "IsPriceRequest");
                multiContent.Add(new StringContent(obj.Color ?? ""), "Color");
                multiContent.Add(new StringContent(obj.IsVaccinated.ToString()), "IsVaccinated");
                multiContent.Add(new StringContent(obj.IsCastration.ToString()), "IsCastration");
                multiContent.Add(new StringContent(obj.IsSterilization.ToString()), "IsSterilization");
                multiContent.Add(new StringContent(obj.PromotionPackageId.ToString() ?? ""), "PromotionPackageId");
                multiContent.Add(new StringContent(obj.CatteryName ?? ""), "CatteryName");
                multiContent.Add(new StringContent(obj.PhoneCode ?? ""), "PhoneCode");
                multiContent.Add(new StringContent(obj.CountryDialCode ?? ""), "CountryDialCode");
                multiContent.Add(new StringContent(obj.latitude ?? ""), "latitude");
                multiContent.Add(new StringContent(obj.longitude ?? ""), "longitude");
                
                multiContent.Add(new StringContent(obj.FamilyTreeMother ?? ""), "FamilyTreeMother");
                multiContent.Add(new StringContent(obj.FamilyTreeFather ?? ""), "FamilyTreeFather");
                multiContent.Add(new StringContent(obj.MotherTested ?? ""), "MotherTested");
                multiContent.Add(new StringContent(obj.FatherTested ?? ""), "FatherTested");
                multiContent.Add(new StringContent(obj.DateofBirth.ToString() ?? ""), "DateofBirth");

                multiContent.Add(new StringContent(obj.PartOfAssociation ?? ""), "PartOfAssociation");
                multiContent.Add(new StringContent(obj.Website ?? ""), "Website");
                if (obj.GalleryImageFiles != null)
                {
                    foreach (var item in obj.GalleryImageFiles)
                    {

                     multiContent.Add(new StreamContent(item.OpenReadStream()), "GalleryImageFiles", item.FileName);

                    }

                }

                if (obj.PedigreeFile != null)
                {

                   
                  multiContent.Add(new StreamContent(obj.PedigreeFile.OpenReadStream()), "PedigreeFile", obj.PedigreeFile.FileName);



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
                    //httpContext.Session.SetString("authorization", result?.Token == null ? "" : result.Token);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", result?.Token == null ? "" : result?.Token, cookieOptions);
                    return response;
                }
                else
                    return null;
            }

        }
        public static async Task<object> CustomHttpUtilizeAdvertisementPackage(string baseUrl, string url, UtilizePurchasedAdvertisementPackage obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                // Set the authorization header if it exists in the session
                //if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                //    client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                {
                    client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                }

                var multiContent = new MultipartFormDataContent();
                    multiContent.Add(new StringContent(obj.UserAdvertisementPackageID.ToString()??"0"), "UserAdvertisementPackageID");



                if (obj.AddFile != null)
                {

                   
                  multiContent.Add(new StreamContent(obj.AddFile.OpenReadStream()), "AddFile", obj.AddFile.FileName);



                }

       
                HttpResponseMessage Res = await client.PostAsync(url, multiContent);

                if (Res.IsSuccessStatusCode)
                {
                    var response = Res.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Response>(response);
                    //httpContext.Session.SetString("authorization", result?.Token == null ? "" : result.Token);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", result?.Token == null ? "" : result?.Token, cookieOptions);
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
        public static async Task<object> CustomHttpWithoutTokenBool(string BaseUrl, string Url, bool content, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Convert bool content to JSON string
                string jsonContent = JsonConvert.SerializeObject(content);

                // Create request with JSON content
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Url);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send request and get response
                HttpResponseMessage Res = await client.SendAsync(request);

                if (Res.IsSuccessStatusCode)
                {
                    var response = await Res.Content.ReadAsStringAsync();
                    return response;
                }
                else
                {
                    return null;
                }
            }
        }


        public static async Task<object> CustomHttpBlog(string baseUrl, string url, Blog obj, HttpContext httpContext)
        {
            using (var client = new HttpClient())
            {
              
                    client.BaseAddress = new Uri(baseUrl);

                    // Set the authorization header if it exists in the Cookies
                    //if (!string.IsNullOrEmpty(httpContext.Session.GetString("authorization")))
                    //    client.DefaultRequestHeaders.Add("authorization", httpContext.Session.GetString("authorization"));

                    if (httpContext.Request.Cookies.TryGetValue("authorization", out var authorizationToken))
                    {
                        client.DefaultRequestHeaders.Add("authorization", authorizationToken);
                    }

                var multiContent = new MultipartFormDataContent();
                    multiContent.Add(new StringContent(obj.BlogID.ToString()), "BlogID");
                    multiContent.Add(new StringContent(obj.Title ?? ""), "Title");
                    multiContent.Add(new StringContent(obj.ShortDescription ?? ""), "ShortDescription");
                    multiContent.Add(new StringContent(obj.Content ?? ""), "Content");
                    multiContent.Add(new StringContent(obj.BlogCategoryId.ToString()), "BlogCategoryId");
                    multiContent.Add(new StringContent(obj.Tags ?? ""), "Tags");


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

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(5)
                        };

                        httpContext.Response.Cookies.Append("authorization", deserializedResponse.Token == null ? "" : deserializedResponse.Token, cookieOptions);
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
                    //httpContext.Session.SetString("authorization", obj.Token == null ? "" : obj.Token);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Should be true in production to ensure cookies are sent over HTTPS
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(5)
                    };

                    httpContext.Response.Cookies.Append("authorization", obj.Token == null ? "" : obj.Token, cookieOptions);
                    return response;

                }
                else
                    return null;
            }
        }

    }
}
