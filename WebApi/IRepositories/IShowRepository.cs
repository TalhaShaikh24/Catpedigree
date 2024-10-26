using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IShowRepository
    {
        public ShowResult GetAllShowsPagination(Show show);
        public Show GetAllShowDetails(int id);
    }
}
