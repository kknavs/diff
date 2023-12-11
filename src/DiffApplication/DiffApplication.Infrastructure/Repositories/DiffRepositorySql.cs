using Dapper;
using DiffApplication.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DiffApplication.Infrastructure.Repositories
{
    public class DiffRepositorySql(IConfiguration configuration) : IDiffRepository
    {
        readonly string connectionString = configuration["DBConnectionString"] ?? throw new NullReferenceException();

        private static string GetTableName(Const.DiffType type)
        {
            return Enum.GetName(type)! + "Diff";
        }

        public async Task<Diff?> GetDiffAsync(int id, Const.DiffType type)
        {
            using IDbConnection connection = new SqlConnection(connectionString);

            var diff = await connection.QueryFirstOrDefaultAsync<Diff>(
                    $"SELECT * from {GetTableName(type)} WHERE id = @id", new { id });
            return diff;
        }

        public async Task PutDiffAsync(int id, Diff diff, Const.DiffType type)
        {
            var table = GetTableName(type);
            using IDbConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            string upsert =
                @$"UPDATE {table} SET data=@data WHERE id=@id
                IF @@ROWCOUNT = 0 INSERT INTO {table}(id, data) VALUES (@id, @data)";

            await connection.ExecuteAsync(upsert,
                new
                {
                    id,
                    data = diff.Data 
                },
                transaction
            );
            transaction.Commit();
            connection.Close();
        }
    }
}
