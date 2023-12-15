using Azure.Core;
using HospitalManagement.Models.DoctorModels;
using HospitalManagement.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
                _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetDoctor()
        {

            return await _doctorService.GetDoctor();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetSingleDoctor(int id)
        {
            var result = await _doctorService.GetSingleDoctor(id);
            if (result is null)
            {
                return NotFound("No Doctor Registered");
            }
            return Ok(result);


        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Doctor>>> AddDoctor(DoctorModel doctor)
        {
            var adminId = User.Claims.FirstOrDefault(c => c.Type == "Admin")?.Value;

                if (User.IsInRole("Admin"))
            {
                var result = await _doctorService.AddDoctor(doctor, adminId);
                return Ok(result);
            }
            else
            {
                return Forbid();
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<DoctorModel>> UpdateDoctor(int id, DoctorModel request)
        {
          var result = await _doctorService.UpdateDoctor(id, request);
            if(result is null)
            {
                return NotFound("No Doctor Registered");
            }
            return Ok(result);

        }

    }
}
