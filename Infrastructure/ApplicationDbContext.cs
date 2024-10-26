using Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options) 
{
    public DbSet<UserSettings> UserSettings { get; init; }
}