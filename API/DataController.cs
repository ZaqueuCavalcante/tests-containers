using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace API;

[ApiController, Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly ApiDbContext _context;
    private readonly IDistributedCache _cache;

    public DataController(ApiDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] ApiKeyValue data)
    {
        // Postgres
        var kv = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == data.Key);

        if (kv is null)
            _context.KeyValues.Add(data);
        else
            kv.Value = data.Value;

        await _context.SaveChangesAsync();

        // Redis
        await _cache.SetStringAsync(data.Key, data.Value);

        // RabbitMQ
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare("events");

        var json = JsonConvert.SerializeObject($"KeyValuePair Create. Key={data.Key} | Value={data.Value}");
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: "events", body: body);

        return Ok(data);
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> Get([FromRoute] string key)
    {
        var data = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == key);

        if (data is null) return NotFound();

        return Ok(data.Value);
    }
}
