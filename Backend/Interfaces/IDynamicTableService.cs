using Shared.DTOs;
using Shared.Entities;

namespace Backend.Interfaces;

public interface IDynamicTableService
{
    Task<int> CreateTableAsync(CreateTableDto dto);
    Task<bool> DeleteTableAsync(string tableName);
    Task<List<Dictionary<string, object>>> QueryTableAsync(string tableName, string whereClause = null);
    Task<int> InsertDataAsync(string tableName, Dictionary<string, object> data);
    Task<int> UpdateDataAsync(string tableName, int id, Dictionary<string, object> updatedData);
    Task<int> DeleteDataAsync(string tableName, int id);
    Task<bool> RenameTableAsync(string oldTableName, string newTableName);
    Task<List<MetaTable>> GetAllTablesAsync();
}