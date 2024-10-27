using Backend.Data;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared.DTOs;
using Shared.Entities;

namespace Backend.Services;

public class DynamicTableService : IDynamicTableService
{
    private readonly DynamicDbContext _context;
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<DynamicTableService> _logger;

    public DynamicTableService(
        DynamicDbContext context,
        IConfiguration configuration,
        ILogger<DynamicTableService> logger)
    {
        _context = context;
        _connection = new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection"));
        _logger = logger;
    }

    public async Task<int> CreateTableAsync(CreateTableDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Meta tabloyu oluştur
            var metaTable = new MetaTable
            {
                Name = dto.TableName.ToLower(),
                CreatedAt = DateTime.UtcNow,
                Columns = dto.Columns.Select((c, i) => new MetaColumn
                {
                    Name = c.Name.ToLower(),
                    DataType = c.DataType,
                    IsNullable = c.IsNullable,
                    IsPrimaryKey = c.IsPrimaryKey,
                    Order = i
                }).ToList()
            };

            _context.MetaTables.Add(metaTable);
            await _context.SaveChangesAsync();

            // Fiziksel tabloyu oluştur
            var createTableSql = BuildCreateTableQuery(dto);
            await using var cmd = new NpgsqlCommand(createTableSql, _connection);
            await _connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            return metaTable.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating table {TableName}", dto.TableName);
            throw;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    private string BuildCreateTableQuery(CreateTableDto dto)
    {
        var columnDefinitions = dto.Columns.Select(column =>
        {
            var columnDef = $"{column.Name} {MapToPostgresType(column.DataType)}";

            if (column.IsPrimaryKey)
                columnDef += " PRIMARY KEY";
            if (!column.IsNullable)
                columnDef += " NOT NULL";

            return columnDef;
        });

        return $@"
            CREATE TABLE {dto.TableName} (
                {string.Join(",\n", columnDefinitions)}
            );";
    }

    private string MapToPostgresType(string dataType)
    {
        return dataType.ToLower() switch
        {
            "string" => "text",
            "int" => "integer",
            "long" => "bigint",
            "decimal" => "numeric",
            "datetime" => "timestamp",
            "bool" => "boolean",
            "double" => "double precision",
            "guid" => "uuid",
            _ => throw new ArgumentException($"Unsupported data type: {dataType}")
        };
    }

    public async Task<List<Dictionary<string, object>>> QueryTableAsync(string tableName, string whereClause = null)
    {
        var result = new List<Dictionary<string, object>>();

        var query = $"SELECT * FROM {tableName}";
        if (!string.IsNullOrEmpty(whereClause))
        {
            query += $" WHERE {whereClause}";
        }

        await using var cmd = new NpgsqlCommand(query, _connection);
        await _connection.OpenAsync();

        try
        {
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                result.Add(row);
            }
        }
        finally
        {
            await _connection.CloseAsync();
        }

        return result;
    }

    public async Task<int> InsertDataAsync(string tableName, Dictionary<string, object> data)
    {
        var columns = string.Join(", ", data.Keys);
        var parameters = string.Join(", ", data.Keys.Select((k, i) => $"@p{i}"));

        var query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

        await using var cmd = new NpgsqlCommand(query, _connection);
        var paramIndex = 0;
        foreach (var value in data.Values)
        {
            cmd.Parameters.AddWithValue($"@p{paramIndex++}", value ?? DBNull.Value);
        }

        await _connection.OpenAsync();
        try
        {
            return await cmd.ExecuteNonQueryAsync();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<MetaTable>> GetAllTablesAsync()
    {
        return await _context.MetaTables
            .Include(t => t.Columns)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    Task<bool> IDynamicTableService.DeleteTableAsync(string tableName)
    {
        throw new NotImplementedException();
    }
}