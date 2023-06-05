using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

using (SqlConnection connection = new SqlConnection("Server=tcp:ternium-db.database.windows.net,1433;Initial Catalog=terniumDB;Persist Security Info=False;User ID=terniumAdmin;Password=Ternium44672?4_2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
{
    SQLReader sQLReader = new SQLReader();
    connection.Open();
    var builder = WebApplication.CreateBuilder(args);
    var app = builder.Build();

    try
    {
        app.MapGet("/", () => sQLReader.readQueryResult(new SqlCommand("SELECT * FROM users;", connection)));
    }
    catch (Exception ex) { app.MapGet("/", () => ex.Message); }

    app.Run();
}

class SQLReader
{
    public string readQueryResult(SqlCommand query)
    {
        try
        {
            using (SqlDataReader reader = query.ExecuteReader())
            {
                string s = "";
                while (reader.Read()) { s += reader.GetString(reader.GetOrdinal("nombres")); }

                return s;
            }
        }
        catch (Exception ex) { return ex.Message + "\n" + ex.StackTrace; }
    }
}
