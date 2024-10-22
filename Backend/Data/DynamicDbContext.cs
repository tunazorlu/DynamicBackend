using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class DynamicDbContext : DbContext
    {
        public DynamicDbContext(DbContextOptions<DynamicDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Örnek dinamik tablo oluşturma
            Dictionary<string, Type> dynamicProperties = new Dictionary<string, Type>
            {
                { "DynamicProperty1", typeof(string) },
                { "DynamicProperty2", typeof(int) }
            };

            // Yeni bir dinamik entity tipi oluşturun
            CreateDynamicEntity("MyDynamicTable", dynamicProperties, modelBuilder);
        }

        private void CreateDynamicEntity(string entityName, Dictionary<string, Type> properties, ModelBuilder modelBuilder)
        {
            // Dinamik entity oluşturmak için
            var entityBuilder = modelBuilder.Entity(entityName);
            entityBuilder.HasKey("Id");

            foreach (var property in properties)
            {
                // Property'leri ekleyin
                entityBuilder.Property(property.Value, property.Key);
            }
        }
    }
}
