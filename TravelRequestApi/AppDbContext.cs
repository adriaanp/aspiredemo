using Microsoft.EntityFrameworkCore;

namespace TravelRequestApi;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
        
    }
    public DbSet<TravelRequest> TravelRequests { get; set; }
}