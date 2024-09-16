using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {

        return Ok();
    }

    [HttpPost("{value}")]
    public async Task PostAsync([FromBody] string value)
    {
        
        
    }

    [HttpDelete("{value}")]
    public async Task DeleteAsync(string value)
    {
        
    }

    [HttpPut("{value}")]
    public async Task PutAsync(string value)
    {
        
    }
    
}