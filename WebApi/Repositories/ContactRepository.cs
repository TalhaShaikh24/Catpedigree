
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
    public class ContactRepository : IContactRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ContactRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

		public async Task<Contact> AddContact(Contact contactInfo)
        {

          

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Department", contactInfo.Department, DbType.String, ParameterDirection.Input);
            parameters.Add("@Firstname", contactInfo.Firstname, DbType.String, ParameterDirection.Input);
            parameters.Add("@Lastname", contactInfo.Lastname, DbType.String, ParameterDirection.Input);
            parameters.Add("@Phone", contactInfo.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", contactInfo.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@Subject", contactInfo.Subject, DbType.String, ParameterDirection.Input);
            parameters.Add("@Message", contactInfo.Message, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Contact>(@"[sp_AddContact]", parameters);

            return data;
        }  
       
    }

  }

