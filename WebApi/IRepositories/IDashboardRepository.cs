using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IDashboardRepository
    {
        dynamic GetJsonDataAsync(int userId);

    }
}
