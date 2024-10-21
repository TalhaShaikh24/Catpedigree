using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static WebApi.Repositories.BlogRepository;


namespace WebApi.IRepositories
{
    public interface IUsefulLinksRepository
    {
        List<UsefulLinks> GetAllUsefulLinks();
        UsefulLinks GetUsefulLinkById(int Id);
        public Task<UsefulLinks> AddUsefulLink(UsefulLinks obj);
        public Task<UsefulLinks> UpdateUsefulLinkById(UsefulLinks obj);
        int DeleteUsefulLinkById(int Id);

    }
}
