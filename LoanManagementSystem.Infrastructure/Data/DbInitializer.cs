using Dapper;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace LoanManagementSystem.Infrastructure.Data;

public static class DbInitializer
{
    private const string DatabaseName = "lmsdb";

    public static async Task Initialize(string connectionString)
    {
        var masterConnectionString = connectionString.Replace($"Database={DatabaseName}", "Database=master");

        using (var masterConnection = new SqlConnection(masterConnectionString))
        {
            await masterConnection.OpenAsync();

            await masterConnection.ExecuteAsync($@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{DatabaseName}')
                BEGIN
                    CREATE DATABASE [{DatabaseName}];
                END
            ");
        }

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var assembly = Assembly.GetExecutingAssembly();

        var scripts = assembly.GetManifestResourceNames()
            .Where(r => r.EndsWith(".sql"))
            .OrderBy(r => r)
            .ToList();

        foreach (var resourceName in scripts)
        {
            var script = GetEmbeddedResource(assembly, resourceName);

            script = script.Replace("\r\nGO\r\n", "\n");

            await connection.ExecuteAsync(script);
        }
    }

    private static string GetEmbeddedResource(Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new Exception($"Resource {resourceName} not found.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
