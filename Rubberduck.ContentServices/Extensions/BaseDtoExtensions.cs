using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Rubberduck.Model.DTO;

namespace Rubberduck.ContentServices.Extensions
{
    internal static class BaseDtoExtensions
    {
        public static string ToSqlSelect(this Type type, string fk = null)
        {
            var columns = string.Join(", ", type.GetSqlColumnNames().Quote());
            var table = type.GetSqlQuotedTableName();

            var where = fk is null ? string.Empty : $" WHERE {fk.Quote()} = @id";
            return $"SELECT {columns} FROM {table}" + where;
        }

        public static string SqlSelect(this BaseDto dto, string fk = null)
        {
            var type = dto.GetType();
            return type.ToSqlSelect(fk);
        }

        public static string ToSqlInsert(this Type type, IEnumerable<string> except = null)
        {
            var table = type.GetSqlQuotedTableName();
            var columnNames = type.GetSqlColumnNames()
                .Except((except ?? Enumerable.Empty<string>()).Concat(new[]
                {
                    nameof(BaseDto.Id), // PK has identity specification
                    nameof(BaseDto.DateUpdated), // skipped on insert
                })).ToArray();

            var columns = string.Join(", ", columnNames.Quote());
            var values = string.Join(", ", columnNames.Select(name => $"@{name}"));

            return $"INSERT INTO {table} ({columns}) VALUES ({values})";
        }

        public static string ToSqlInsert(this BaseDto dto, IEnumerable<string> except = null)
        {
            var type = dto.GetType();
            return type.ToSqlInsert(except);
        }

        public static string ToSqlUpdate(this Type type, IEnumerable<string> except = null)
        {
            var table = type.GetSqlQuotedTableName();
            var columnNames = type.GetSqlColumnNames()
                .Except((except ?? Enumerable.Empty<string>()).Concat(new[]
                {
                    nameof(BaseDto.Id), // PK
                    nameof(BaseDto.DateInserted), // skipped on update
                })).ToArray();

            static string asUpdate(string name) => $"t.{name.Quote()} = @{name}";

            var columns = string.Join(", ", columnNames.Select(asUpdate));
            return $"UPDATE t SET {columns} FROM {table} AS t WHERE [Id] = @id";
        }

        public static string ToSqlUpdate(this BaseDto dto)
        {
            var type = dto.GetType();
            return type.ToSqlUpdate();
        }

        public static string ToSqlDelete(this Type type)
        {
            var table = type.GetSqlQuotedTableName();
            return $"DELETE FROM {table} WHERE [Id] = @id";
        }

        public static string ToSqlDelete(this BaseDto dto)
        {
            var type = dto.GetType();
            return type.ToSqlDelete();
        }

        private static string GetSqlQuotedTableName(this Type type)
        {
            var tableAttribute = type.GetCustomAttributes<TableAttribute>().SingleOrDefault();

            var schema = tableAttribute?.Schema ?? SqlSchema.DefaultSchema;
            var table = tableAttribute?.Name ?? type.Name;
            return $"{schema.Quote()}.{table.Quote()}";
        }

        private static IEnumerable<string> GetSqlColumnNames(this Type type)
        {
            var properties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.CanRead);

            return properties.Select(p => $"{p.GetCustomAttributes<ColumnAttribute>().SingleOrDefault()?.Name ?? p.Name}");
        }

        public static IEnumerable<string> Quote(this IEnumerable<string> values) => values.Select(Quote);

        public static string Quote(this string value) => $"[{value}]";
    }
}
