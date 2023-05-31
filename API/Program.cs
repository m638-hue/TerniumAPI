using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

 

using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_TERNIUMDB")))
{
    connection.Open();
    SQLReader sQLReader = new SQLReader(connection);
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    app.MapGet("/", () => sQLReader.readQueryResult("select * from users");


    app.Run();
}

class SQLReader
{
    private SqlConnection connection;
    public SQLReader(SqlConnection connection) => this.connection = connection;

    public string readQueryResult(string query)
    {
        using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
        {
            return reader.Read().ToString();
        }
    }
}
