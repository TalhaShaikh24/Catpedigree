
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
    public class UsefulLinksRepository : IUsefulLinksRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsefulLinksRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }
        public List<UsefulLinks> GetAllUsefulLinks()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<UsefulLinks>(@"[dbo].[sp_GetAllUsefulLinks]", parameters);
            return data;
        }
        public UsefulLinks GetUsefulLinkById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Get<UsefulLinks>(@"[dbo].[sp_GetUsefulLinkById]", parameters);
            return data;
        }
        public async Task<UsefulLinks> AddUsefulLink(UsefulLinks obj)
        {
            // Function to replace spaces with underscores in file names
            string ReplaceSpaces(string input) => input.Replace(' ', '_').Replace('(', '_').Replace(')', '_');



            if (obj.UsefulLinkFile != null)
            {
                string FileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(obj.UsefulLinkFile.FileName));
                string FilePath = Path.Combine("UsefulLinks", FileName);
                string FilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FilePath);
                try
                {
                    using (var stream = new FileStream(FilePathDirectory, FileMode.Create))
                    {
                        await obj.UsefulLinkFile.CopyToAsync(stream);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
               
                obj.UsefulLinkFilePath = FilePath;
            }

           

            DynamicParameters parameters = new DynamicParameters();
           
            parameters.Add("UsefulLinkFilePath", obj.UsefulLinkFilePath, DbType.String, ParameterDirection.Input);
           
            parameters.Add("CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
           
            parameters.Add("Url", obj.Url, DbType.String, ParameterDirection.Input);


            var data = _dapper.Insert<UsefulLinks>(@"[dbo].[sp_AddUsefulLinks]", parameters);
            return data;
        }
        public async Task<UsefulLinks> UpdateUsefulLinkById(UsefulLinks obj)
        {
            // Function to replace spaces with underscores in file names
            string ReplaceSpaces(string input) => input.Replace(' ', '_').Replace('(','_').Replace(')','_');

           

            if (obj.UsefulLinkFile != null)
            {
                string FileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(obj.UsefulLinkFile.FileName));
                string FilePath = Path.Combine("UsefulLinks", FileName);
                string FilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FilePath);
                try
                {
                    using (var stream = new FileStream(FilePathDirectory, FileMode.Create))
                    {
                        await obj.UsefulLinkFile.CopyToAsync(stream);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
               
                obj.UsefulLinkFilePath = FilePath;
            }

           

            DynamicParameters parameters = new DynamicParameters();
           
            parameters.Add("Id", obj.Id, DbType.Int64, ParameterDirection.Input);
            parameters.Add("UsefulLinkFilePath", obj.UsefulLinkFilePath, DbType.String, ParameterDirection.Input);
            parameters.Add("ModifiedBy", obj.ModifiedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("Url", obj.Url, DbType.String, ParameterDirection.Input);


            var data = _dapper.Insert<UsefulLinks>(@"[dbo].[sp_UpdateUsefulLinkById]", parameters);
            return data;
        }

        public int DeleteUsefulLinkById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);


            var data = _dapper.Insert<int>(@"dbo.[sp_DeleteUsefulLinkById]", parameters);

            return data;
        }


    }

}

