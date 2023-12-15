global using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base (options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=HospitalManagementdb;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        public DbSet <Doctor> DoctorsDetails { get; set; }
        public DbSet<Admin> AdminDetails { get; set; }
        public string WebRootPath { get; internal set; }
    }
}
