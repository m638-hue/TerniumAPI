using Microsoft.Data.SqlClient;

using (SqlConnection connection = new SqlConnection("Server=tcp:ternium-db.database.windows.net,1433;Initial Catalog=terniumDB;Persist Security Info=False;User ID=terniumAdmin;Password=Ternium44672?4_2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
{
    connection.Open();
    // Do work here.  
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.MapGet("/hello", () => "Hello World!");


    app.Run();
}
