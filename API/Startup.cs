using Microsoft.EntityFrameworkCore;

namespace API;

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddRouting(options => options.LowercaseUrls = true);

        services.AddControllers();

        services.AddDbContext<ApiDbContext>(options =>
            options.UseSnakeCaseNamingConvention()
        );
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(builder => builder.MapControllers());
    }
}
