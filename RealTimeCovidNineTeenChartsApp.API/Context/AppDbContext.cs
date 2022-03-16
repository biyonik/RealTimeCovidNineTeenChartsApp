using Microsoft.EntityFrameworkCore;
using RealTimeCovidNineTeenChartsApp.API.Models;

namespace RealTimeCovidNineTeenChartsApp.API.Context;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }

    public DbSet<Covid> Covids { get; set; }
}