using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{

    public class ForgotPassword
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? VerificationCode { get; set; }
        public string? Password { get; set; }
    }
    public class Register: Common
    {
        public int UserId { get; set; }
        public string? RoleIds { get; set; }
        public string? Roles { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ContactNo { get; set; }
        public string? Address { get; set; }
        public string? ProfileInfo { get; set; }

        public DateTime? DateofBirth { get; set; }

        public IFormFile? ProfilePic { get; set; }
        public IFormFile? BreederLicense { get; set; }
        public string? ProfilePicPath { get; set; }
        public string? BreederLicensePath { get; set; }
        public string? ZoologicalNumber { get; set; }
        public int? RoleId { get; set; }


        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }


        public int UserSocialLinskID { get; set; }


        public string? FaceBook { get; set; }
        public string? Insta { get; set; }
        public string? Twitter { get; set; }

        public string? IsActive { get; set; }


    }


    public class Roles
    {

        public int Id { get; set; }

        public string Role { get; set; }
    }

    public  class UsersDTO
    {


        public Register Register { get; set; }

        public List<Roles> Roles { get; set; }


    }




    public class ListUsersDTO
    {

        public  List<Register> Register { get; set; }

        public List<Roles> Roles { get; set; }

    }

    public class userRolesUpdate
    {

        public int UserId { get; set; }


        public string RolesIds { get; set; }

    }



}
