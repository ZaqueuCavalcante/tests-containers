namespace API;

using Microsoft.EntityFrameworkCore;

public class ApiDbContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public ApiDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("Database"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApiKeyValue>().HasKey(a => a.Key);
        modelBuilder.Entity<ApiKeyValue>().Property(a => a.Key).ValueGeneratedNever();
    }

    public DbSet<ApiKeyValue> KeyValues { get; set; }
}
