using ClassLibrary;
using Dapper;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class ShowRepository: IShowRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ShowRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public ShowResult GetAllShowsPagination(Show show)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PageNumber", show.PageNumber);
            parameters.Add("@PageSize", show.PageSize);
         

            var data = _dapper.GetAll<Show>(@"[dbo].[sp_GetAllShowPagination]", parameters).ToList();
            int totalCount = data.Any() ? data.First().TotalCount : 0;
            int fetchedCount = data.Count;

            return new ShowResult
            {
                Shows = data,
                TotalCount = totalCount,
                FetchedCount = fetchedCount
            };
        }


        public Show GetAllShowDetails(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);


            var data = _dapper.Get<Show>(@"[dbo].[sp_GetAllShowDetails]", parameters);

            return data;
        }



    }
}
