using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notebook.Entities.DbSet;

namespace Notebook.DataService.Data;

public class ApplicationDbContext : IdentityDbContext
{
    private DbSet<User> Users;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}