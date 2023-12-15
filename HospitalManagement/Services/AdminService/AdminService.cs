using HospitalManagement.Models;
using HospitalManagement.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HospitalManagement.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly DataContext _context;
        public AdminService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Admin>> AddAdmin(AdminDto adminId)
        {
            var Adminobject = new Admin
            {
                FirstName = adminId.FirstName,
                LastName = adminId.LastName,
                Place = adminId.Place,
                Mobile = adminId.Mobile,
            };
            _context.AdminDetails.Add(Adminobject);
            await _context.SaveChangesAsync();
            return await _context.AdminDetails.ToListAsync();
        }

        public async Task<List<Admin>> GetAdmin()
        {
            var admins = await _context.AdminDetails.ToListAsync();
            return admins;
        }

      

        public async Task<Admin?> GetSingleAdmin(int id)
        {
            var adminId = await _context.AdminDetails.FindAsync(id);
            if (adminId is null)
            {
                return null;

            }
            return adminId;
        }

        public async Task<bool> IsMobileRegistered(string mobile)
        {
            var admin = await _context.AdminDetails.FirstOrDefaultAsync(a => a.Mobile == mobile);
            return admin != null;
        }


       

    }
}
