using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebApi.IRepositories
{
    public interface IAccountRepository
    {
       
        Register Authenticate(Register obj);
        Task<Register> RegisterUser(Register obj);
        ForgotPassword ForgotPassword(ForgotPassword obj);
        ForgotPassword ResetPassword(ForgotPassword obj);
        int checkPackagesValidations(int userrid , int? packageId, string PackageType);


    }
}
