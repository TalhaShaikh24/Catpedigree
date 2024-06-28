
using ClassLibrary;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }


        public Register Authenticate(Register obj)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UsernameEmail", obj.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", obj.Password, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Register>(@"[sp_Login]", parameters);

            return data;
        }

        public int checkPackagesValidations(int userrid, int? packageId, string PackageType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", userrid, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PackageID", packageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PackageType", PackageType, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<int>(@"[sp_checkPackagesValidations]", parameters);

            return data;
        }

        public async Task<Register> RegisterUser(Register formData)
        {
            try
            {
                string folder = "Profile"; // Relative path

                string profileFilePath = null;
                string licenseFilePath = null;

                // For Profile Picture
                if (formData.ProfilePic != null)
                {
                    string profileFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(formData.ProfilePic.FileName);
                    profileFilePath = Path.Combine(folder, profileFileName);
                    string absoluteProfileFilePath = Path.Combine(_hostingEnvironment.WebRootPath, profileFilePath);

                    using (var profileStream = new FileStream(absoluteProfileFilePath, FileMode.Create))
                    {
                        await formData.ProfilePic.CopyToAsync(profileStream);
                    }
                }

                // For Breeder License
                if (formData.BreederLicense != null)
                {
                    string licenseFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(formData.BreederLicense.FileName);
                    licenseFilePath = Path.Combine(folder, licenseFileName);
                    string absoluteLicenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, licenseFilePath);

                    using (var licenseStream = new FileStream(absoluteLicenseFilePath, FileMode.Create))
                    {
                        await formData.BreederLicense.CopyToAsync(licenseStream);
                    }
                }

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Firstname", formData.UserId, DbType.String, ParameterDirection.Input);
                parameters.Add("@Firstname", formData.Firstname, DbType.String, ParameterDirection.Input);
                parameters.Add("@Lastname", formData.Lastname, DbType.String, ParameterDirection.Input);
                parameters.Add("@Username", formData.Username, DbType.String, ParameterDirection.Input);
                parameters.Add("@Email", formData.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("@Password", formData.Password, DbType.String, ParameterDirection.Input);
                parameters.Add("@ContactNo", formData.ContactNo, DbType.String, ParameterDirection.Input);
                parameters.Add("@Address", formData.Address, DbType.String, ParameterDirection.Input);
                parameters.Add("@ProfileInfo", formData.ProfileInfo, DbType.String, ParameterDirection.Input);
                parameters.Add("@ProfilePicPath", profileFilePath != null ? "~/" + profileFilePath : null, DbType.String, ParameterDirection.Input);
                parameters.Add("@BreederLicensePath", licenseFilePath != null ? "~/" + licenseFilePath : null, DbType.String, ParameterDirection.Input);
                parameters.Add("@ZoologicalNumber", formData.ZoologicalNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("@RoleId", formData.RoleId, DbType.Int32, ParameterDirection.Input);

                var data = _dapper.Insert<Register>(@"[sp_userRegister]", parameters);
                return data;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine("Error in RegisterUser: " + ex.Message);
                throw; // Rethrow the exception to notify the caller about the error
            }
        }



    }


}

