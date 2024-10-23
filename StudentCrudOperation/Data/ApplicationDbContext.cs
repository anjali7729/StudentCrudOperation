using Microsoft.EntityFrameworkCore;
using StudentCrudOperation.Models;
using System.Data;

namespace StudentCrudOperation.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions):base(dbContextOptions) { }
        public DbSet<Student> students { get; set; }
        public DbSet<User> users { get; set; }
    }
}
