using bugdgetwarsapi.Models;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public  DbSet<Expense> Expenses { get; set; }
}