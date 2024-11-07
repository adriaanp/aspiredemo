using Npgsql;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddContainer("ollama", "ollama/ollama")
    .WithHttpEndpoint(port: 11434, targetPort: 11434);

var ollamaendpoint = ollama.GetEndpoint("http");

var dbserver = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent);

var db = dbserver.AddDatabase("travelrequests")
    .WithClearTableCommand("TravelRequests");

var api = builder.AddProject<TravelRequestApi>("apiserver")
    .WaitFor(db)
    .WithReference(db)
    .WithReference(ollamaendpoint)
    .WithHttpHealthCheck("/health");


builder.AddProject<TravelRequestUI>("ui")
    .WithReference(api)
    .WaitFor(api);


builder.Build().Run();


public static class DbCommandExtensions
{
    public static IResourceBuilder<PostgresDatabaseResource> WithClearTableCommand(
        this IResourceBuilder<PostgresDatabaseResource> builder, string tableName)
    {
        builder.WithCommand($"clear-{tableName}", $"Clear {tableName} Table",
            async context =>
            {
                var connectionString =
                    await builder.Resource.ConnectionStringExpression.GetValueAsync(CancellationToken.None);
                var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                var command = new NpgsqlCommand($"TRUNCATE TABLE \"{tableName}\"", connection);
                await command.ExecuteNonQueryAsync();
                return CommandResults.Success();
            });
        return builder;
    }
}