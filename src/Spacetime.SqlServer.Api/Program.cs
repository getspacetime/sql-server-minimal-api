using System.Data;
using Dapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// run a sql server database with docker
// https://www.twilio.com/blog/using-sql-server-on-macos-with-docker
IDbConnection GetConnection(string server, string db) =>
    new System.Data.SqlClient.SqlConnection($"Data Source={server};Initial Catalog={db};User Id=sa; Password=someThingComplicated1234;");

app.MapGet("/servers/{server}/databases/{database}/tables", (string server, string database) =>
{
    var results = GetConnection(server, database).Query<dynamic>("select * from information_schema.tables");

    return results;
})
.WithName("GetTables");

app.Run();
