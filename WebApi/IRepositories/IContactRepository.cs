using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebApi.IRepositories
{
    public interface IContactRepository
    {
        Task<Contact> AddContact(Contact contactInfo);
        

    }
}
