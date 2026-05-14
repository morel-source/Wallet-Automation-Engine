using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace Wallet.Tests.Infrastructure;

public static class TestDbInitializer
{
    public static async Task Initialize(string connectionString)
    {
        var basePaths = new[] { "Tables", "Procedures", "Triggers", "Indexes" };
        var rootPath = Path.Combine(AppContext.BaseDirectory, "DbMigrations", "Scripts");

        await using var conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

        foreach (var subDir in basePaths)
        {
            var fullPath = Path.Combine(rootPath, subDir);
            if (!Directory.Exists(fullPath)) continue;

            var files = Directory.GetFiles(fullPath, searchPattern: "*.sql")
                .OrderBy(Path.GetFileName);

            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                await ExecuteScriptBatches(conn, content);
                Console.WriteLine($@"Applied: {subDir}/{Path.GetFileName(file)}");
            }
        }
    }

    private static async Task ExecuteScriptBatches(SqlConnection conn, string script)
    {
        var batchRegex = new Regex(@"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        var batches = batchRegex.Split(script);

        foreach (var batch in batches.Where(b => !string.IsNullOrWhiteSpace(b)))
        {
            await using var cmd = new SqlCommand(batch, conn);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}