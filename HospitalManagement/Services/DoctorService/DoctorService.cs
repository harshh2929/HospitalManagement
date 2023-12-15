using HospitalManagement.Models.DoctorModels;

namespace HospitalManagement.Services.DoctorService
{

    public class DoctorService : IDoctorService
    {
        
        private readonly DataContext _context;
        public DoctorService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Doctor>> AddDoctor(DoctorModel doctor, string adminId)
        {
            try
            {
                doctor.AdminId = adminId;
                var DoctorObject = new Doctor
                {
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Place = doctor.Place,
                    AdminId = doctor.AdminId,
                };
                _context.DoctorsDetails.Add(DoctorObject);
                await _context.SaveChangesAsync();
                return await _context.DoctorsDetails.ToListAsync();
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }



        public async Task<List<Doctor>> GetDoctor()
        {
            var doctors=await _context.DoctorsDetails.ToListAsync();
            return doctors;
        }

        public async Task<Doctor?> GetSingleDoctor(int id)
        {
            var doctorId = await _context.DoctorsDetails.FindAsync(id);
            if (doctorId is null)
            {
                return null;

            }
            return doctorId;
        }

        public async Task<List<DoctorModel>?> UpdateDoctor(int id, DoctorModel request)
        {
            var doctor = await _context.DoctorsDetails.FindAsync(id);
            if (doctor == null)
            {
                return null; 
            }

            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;
            doctor.Place = request.Place;

            await _context.SaveChangesAsync();

            return await _context.DoctorsDetails.Select(d => new DoctorModel
            {
                FirstName = d.FirstName,
                LastName = d.LastName,
                Place = d.Place
            }).ToListAsync();
        }

    }
}
