using AutoMapper;
using HospitalManagement.Models;
using HospitalManagement.Models.AdminModels;
using HospitalManagement.Models.Entity;
using HospitalManagement.Services.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private static Dictionary<string, string> storedOtp = new Dictionary<string, string>();

        private readonly IAdminService _adminService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AdminController(IAdminService adminService,DataContext context, IMapper mapper)
        {
            _adminService = adminService;
            _context=context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdminDto>>> GetAdmin()
        {
            var adminEntities = await _adminService.GetAdmin();
            var adminsDto = _mapper.Map<List<AdminDto>>(adminEntities);

            return adminsDto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetSingleAdmin(int id)
        {
            var result = await _adminService.GetSingleAdmin(id);
            if (result is null)
            {
                return NotFound("No Admin Registered");
            }
            return Ok(result);


        }

        [HttpPost]
        public async Task<ActionResult<List<Admin>>> AddAdmin(AdminDto admin)
        {
            try
            {
                string password = admin.Password;

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                admin.Password = passwordHash;

                var result = await _adminService.AddAdmin(admin);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAdmin(LoginRequest loginRequest)
        {
            try
            {

                var isMobileRegistered = await _adminService.IsMobileRegistered(loginRequest.Mobile);

                if (!isMobileRegistered)
                {
                    return NotFound("Mobile number is not registered");
                }


                var otp = GenerateRandomOtp();

                OTPCache[loginRequest.Mobile] = otp;

                SendOtpToMobile(loginRequest.Mobile, otp);

                Console.WriteLine(otp);

                return Ok($"OTP sent successfully. Your OTP is: {otp}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        private void SendOtpToMobile(string mobileNumber, string otp)
        {

            Console.WriteLine($"OTP sent to {mobileNumber}: {otp}");
        }



        private static Dictionary<string, string> OTPCache = new Dictionary<string, string>();

        private string GenerateRandomOtp()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString("D4");
        }


        [HttpPost("verifyotp")]
        public async Task<ActionResult<string>> VerifyOtp(VerifyOtpRequest verifyOtpRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(verifyOtpRequest.Mobile) || string.IsNullOrEmpty(verifyOtpRequest.Otp))
                {
                    return BadRequest("Mobile number and OTP are required");
                }
                

                if (OTPCache.TryGetValue(verifyOtpRequest.Mobile, out var storedOtp) && verifyOtpRequest.Otp == storedOtp)
                {
                    var AdminId=await _context.AdminDetails.Where(s=>s.Mobile==verifyOtpRequest.Mobile).Select(s=>s.Id).FirstOrDefaultAsync();
                    var token = GenerateJwtToken(AdminId);

                    return Ok(new { Token = token, Message = "OTP verification successful. User is authenticated." });
                }
                else
                {
                    return BadRequest("Invalid OTP. Please try again.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(int AdminId)
        {
             var claims = new List<Claim>
            {
                new Claim("Admin", AdminId.ToString()),
               
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AppSettings:Token"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_context.WebRootPath ?? "wwwroot", "uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Ok($"File uploaded successfully! File Name: {uniqueFileName}");
                }
                else
                {
                    return BadRequest("Invalid file");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


    }
}
//[HttpPost("login")]
//public async Task<ActionResult<string>> LoginAdmin(LoginRequest loginRequest)
//{
//    try
//    {
//        var admin = await _adminService.GetAdminByFirstName(loginRequest.FirstName);

//        if (admin == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, admin.Password))
//        {
//            return NotFound("Invalid login credentials");
//        }

//        var claims = new List<Claim>
//  {
//      new Claim("AdminId", admin.Id.ToString()),
//      new Claim(ClaimTypes.Role, "Admin")
//  };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AppSettings:Token"));

//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(

//            claims: claims,
//            expires: DateTime.Now.AddDays(1),
//            signingCredentials: creds
//        );

//        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
//    }
//    catch (Exception ex)
//    {
//        return BadRequest($"Error: {ex.Message}");
//    }
//}