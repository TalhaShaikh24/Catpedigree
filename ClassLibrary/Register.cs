using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   
    public class Register: Common
    {
        public int UserId { get; set; }
        public string? RoleIds { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ContactNo { get; set; }
        public string? Address { get; set; }
        public string? ProfileInfo { get; set; }
        public IFormFile? ProfilePic { get; set; }
        public IFormFile? BreederLicense { get; set; }
        public string? ProfilePicPath { get; set; }
        public string? BreederLicensePath { get; set; }
        public string? ZoologicalNumber { get; set; }
    }

}
