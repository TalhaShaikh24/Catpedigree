using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IVendorRepository
    {
        Vendor GetVednorDataAndList(int Id);


        List<Register> GetAllVendors();

    }
}
