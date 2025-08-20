using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Data
{
    public class LoginDBContext : DbContext
    {
        public LoginDBContext(DbContextOptions<LoginDBContext> options)
            : base(options)
        {
        }
        public DbSet<LoginModel> Logins { get; set; }
     
    }
}
