using Backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DynamicEntityController(DynamicDbContext context) : ControllerBase
{
    // Post: /api/DynamicEntity
    [HttpPost]
    public IActionResult AddProperty([FromBody] DynamicPropertyInputModel input)
    {
        if (input == null || string.IsNullOrEmpty(input.Name))
        {
            return BadRequest("Invalid input");
        }

        var dynamicProperty = new DynamicProperty
        {
            Name = input.Name,
            Value = input.Value // Frontend'den gelen değeri kullan
        };

        // Kullanım sırasında dönüşüm yapabilirsiniz
        string stringValue = dynamicProperty.Value as string; // 'as' ile dönüşüm yap
        if (stringValue != null)
        {
            Console.WriteLine($"String Value: {stringValue}");
        }

        // Başka bir türde değer atama
        if (dynamicProperty.Value is int intValue) // 'is' ile kontrol yap
        {
            Console.WriteLine($"Int Value: {intValue}");
        }

        return Ok("Dynamic property added");
    }


    // Get: /api/DynamicEntity
    [HttpGet]
    public async Task<IActionResult> GetAllEntities()
    {
        var entities = await context.Entities.ToListAsync();
        return Ok(entities);
    }
}
