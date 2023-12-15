using HospitalManagement.Models.DoctorModels;

namespace HospitalManagement.Services.DoctorService
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctor();

        Task<Doctor?> GetSingleDoctor(int id);

        Task<List<Doctor>> AddDoctor(DoctorModel doctorId, string? adminId);

      Task<List<DoctorModel>?> UpdateDoctor(int id, DoctorModel request);

    }
}
