using System.IdentityModel.Tokens.Jwt;

namespace HospitalManagement.Controllers
{
    public class Program
    {
        static void Main()
        {
            string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJBZG1pbiI6IjIiLCJleHAiOjE3MTk2NjI2MTl9.oCkUat8DQR6HuqeGZEVnpeu1fRda_eTh0GcH07BVeCg.eyJBZG1pbiI6IjIiLCJleHAiOjE3MTk2NjI2MTl9.oCkUat8DQR6HuqeGZEVnpeu1fRda_eTh0GcH07BVeCg.eyJBZG1pbiI6IjIiLCJleHAiOjE3MDI1NTUwMDB9.MAalRpZ-rkD884w8WhOhYlq-Vg6q5-FRP6ue0PBCjjc";

            var jwtHandler = new JwtSecurityTokenHandler();

            // Check if the token is in the correct format
            if (jwtHandler.CanReadToken(token))
            {
                var jsonToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    // Access claims from the payload
                    string adminId = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "Admin").Value;

                    Console.WriteLine($"Admin ID: {adminId}");
                }
                else
                {
                    Console.WriteLine("Invalid JWT token format.");
                }
            }
            else
            {
                Console.WriteLine("Invalid JWT token format.");
            }
        }
    }
}
