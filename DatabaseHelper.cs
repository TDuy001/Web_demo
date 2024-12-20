using System.Linq;
using Microsoft.EntityFrameworkCore;
using Demo3.Data;

namespace Demo3.Helpers
{
    public static class DatabaseHelper
    {
        public static void ClearDatabase(CourseDbContext context)
        {
            var tables = context.Model.GetEntityTypes()
                .Select(e => new
                {
                    TableName = e.GetTableName(),
                    Schema = e.GetSchema()
                })
                .Where(t => t.TableName != "__EFMigrationsHistory") 
                .ToList();

            foreach (var table in tables)
            {
                var fullTableName = string.IsNullOrEmpty(table.Schema)
                    ? table.TableName
                    : $"{table.Schema}.{table.TableName}";

                if (!string.IsNullOrEmpty(fullTableName) && fullTableName.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '.'))
                {
                    context.Database.ExecuteSql($"TRUNCATE TABLE [{fullTableName}]");
                }
            }
            context.SaveChanges();
        }
    }
}
