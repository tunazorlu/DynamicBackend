// Services/DynamicEntityService.cs

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Backend.Services
{
    public class DynamicEntityService
    {
        public Type CreateNewDynamicType(string entityName, Dictionary<string, Type> properties)
        {
            // Assembly ve module tanımları
            var assemblyName = new AssemblyName("DynamicEntitiesAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            // Yeni entity tipi oluşturma
            var typeBuilder = moduleBuilder.DefineType(entityName, TypeAttributes.Public | TypeAttributes.Class);

            // "Id" property ekleme
            typeBuilder.DefineField("Id", typeof(int), FieldAttributes.Public);

            // Dinamik olarak property'ler ekleme
            foreach (var property in properties)
            {
                typeBuilder.DefineField(property.Key, property.Value, FieldAttributes.Public);
            }

            // Yeni oluşturulan tipi geri döndür
            return typeBuilder.CreateType();
        }
    }
}
