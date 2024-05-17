using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NLP_WebApp.Models.Entity;

namespace NLP_WebApp.Models.Data;

public class Context : IdentityDbContext<AppUser,AppRole,int>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=NLP;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=True;Encrypt=false;");
    }
    
    public DbSet<Interest> Interests { get; set; }
}