using HospitalManagement.Models.Entity;

namespace HospitalManagement.Services.AdminService
{
    public interface IAdminService
    {
        Task<List<Admin>> GetAdmin();

        Task<Admin?> GetSingleAdmin(int id);

        Task<List<Admin>> AddAdmin(AdminDto adminId);

        Task<bool> IsMobileRegistered(string mobile);


    };
}
