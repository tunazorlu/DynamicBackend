using Backend.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Text.Json;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DynamicTableController : ControllerBase
{
    private readonly IDynamicTableService _tableService;
    private readonly ILogger<DynamicTableController> _logger;

    public DynamicTableController(IDynamicTableService tableService, ILogger<DynamicTableController> logger)
    {
        _tableService = tableService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTableDto dto)
    {
        try
        {
            // Gelen veriyi logla
            _logger.LogInformation($"Received request: {JsonSerializer.Serialize(dto)}");

            // Basit validasyonlar
            if (string.IsNullOrEmpty(dto.TableName))
                return BadRequest("Table name is required");

            if (!dto.Columns.Any())
                return BadRequest("At least one column is required");

            // Primary key kontrolü
            if (!dto.Columns.Any(c => c.IsPrimaryKey))
                return BadRequest("At least one column must be marked as primary key");

            var result = await _tableService.CreateTableAsync(dto);

            return Ok(result);
        }
        catch (Exception ex)
        {
            // İç hatayı loglayalım
            _logger.LogError(ex, "Error creating table");

            // Detaylı hata mesajını döndürelim
            return StatusCode(500, new
            {
                message = "Error creating table",
                error = ex.ToString(),  // Tüm hata stacktrace'ini görelim
                innerException = ex.InnerException?.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTables()
    {
        var tables = await _tableService.GetAllTablesAsync();
        return Ok(tables);
    }

    [HttpGet("{tableName}")]
    public async Task<IActionResult> QueryTable(string tableName, [FromQuery] string where = null)
    {
        try
        {
            var data = await _tableService.QueryTableAsync(tableName, where);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error querying table", Error = ex.Message });
        }
    }

    [HttpPost("{tableName}/data")]
    public async Task<IActionResult> InsertData(string tableName, [FromBody] Dictionary<string, object> data)
    {
        try
        {
            var rowsAffected = await _tableService.InsertDataAsync(tableName, data);
            return Ok(new { RowsAffected = rowsAffected });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error inserting data", Error = ex.Message });
        }
    }
}
