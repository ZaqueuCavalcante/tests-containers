using API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TESTS;

public class ApiTestBase
{
    protected HttpClient _client = null!;
    protected ApiDbContext _context = null!;
    protected ApiWebAppFactory _factory = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new ApiWebAppFactory();
    }

    [SetUp]
    public void SetupBeforeEachTest()
    {
        using var scope = _factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        _client = _factory.CreateClient();

        var cnn = _context.Database.GetConnectionString()!;

        if (cnn.Contains("Database=api-tests-db"))
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
    }
}
