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
        public Task<UsefulLinks> AddUsefulLink(UsefulLinks obj);

    }
}
