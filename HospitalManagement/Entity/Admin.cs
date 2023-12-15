
namespace HospitalManagement.Models
{
    public class Admin
    {

        public int Id { get; set; }
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string Place { get; set; }=string.Empty;
        public string Password { get;  set; }= string.Empty;
        public string Mobile { get; set; } = string.Empty;

    }
}
