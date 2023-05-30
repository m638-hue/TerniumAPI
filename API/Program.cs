using Microsoft.Data.SqlClient;

//using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLConnectionString")))
//{
    //connection.Open();
    // Do work here.  
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.MapGet("/hello", () => Environment.GetEnvironmentVariable("SQLConnectionString"));


    app.Run();
//}
