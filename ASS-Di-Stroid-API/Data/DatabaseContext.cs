using Microsoft.EntityFrameworkCore;
using ASS_Di_Stroid_API.Models;

namespace ASS_Di_Stroid_API.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public DbSet<TeamScore> TeamScores { get; set; }
}