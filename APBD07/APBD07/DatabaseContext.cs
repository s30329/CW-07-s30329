using Microsoft.EntityFrameworkCore;
using APBD07.Models;

namespace APBD07;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Country> Countries { get; set; }
}