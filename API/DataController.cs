using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[ApiController, Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly ApiDbContext _context;

    public DataController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] ApiKeyValue data)
    {
        var kv = await _context.KeyValues.FirstOrDefaultAsync(x => x.Key == data.Key);

        if (kv is null)
            _context.KeyValues.Add(data);
        else
            kv.Value = data.Value;

        await _context.SaveChangesAsync();

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
