using Microsoft.Data.SqlClient;

using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_TERNIUMDB")))
{
    connection.Open();
    // Do work here.  
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.MapGet("/", () => Environment.GetEnvironmentVariable("SQLAZURECONNSTR_TERNIUMDB"));


    app.Run();
}
