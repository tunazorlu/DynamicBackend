using Backend.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DynamicTableController : ControllerBase
{
    private readonly IDynamicTableService _tableService;

    public DynamicTableController(IDynamicTableService tableService)
    {
        _tableService = tableService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] CreateTableDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var tableId = await _tableService.CreateTableAsync(dto);
            return Ok(new { TableId = tableId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error creating table", Error = ex.Message });
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
