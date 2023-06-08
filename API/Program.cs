using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;


using (SqlConnection connection = new SqlConnection("Server=tcp:ternium-db.database.windows.net,1433;Initial Catalog=terniumDB;Persist Security Info=False;User ID=terniumAdmin;Password=Ternium44672?4_2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
{
	connection.Open();
	HttpController.setConnection(connection);

	var builder = WebApplication.CreateBuilder(args);
	var app = builder.Build();

	app.MapGet("/", () => "Hello");
	app.MapPost("/api/verify", HttpController.VerifyUser);
	app.MapPost("/api/login", HttpController.LogInUser);

	app.Run();
}

public static class HttpController
{
	static byte[] seed = Encoding.ASCII.GetBytes("3?Y↨?K?q↑J???d2?z??????????6B?9G?E?r]????P?^A??l<?↕??NK☻RQQ↓B?a&A@@q?♦C?(◄\r\n???y=▼0♠T?M?!?Q??L▼6??E???z?H◄?");

	static SqlConnection connection = null;
	static Dictionary<string, int> tokens = new Dictionary<string, int>();

	private static bool ValidateToken(string authToken, int logID) => 
		tokens.TryGetValue(authToken, out int id) && id == logID;

	private static string GenerateToken ()
	{
		Random res = new Random();

		string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int size = 15;

		string ran = "";

		for (int i = 0; i < size; i++) 
			ran = ran + str[res.Next(52)];

		return ran;
	}

	public static void setConnection(SqlConnection connection) => 
		HttpController.connection = connection;
	
	async public static Task VerifyUser (HttpContext context)
	{
		IFormCollection body = context.Request.Form;
		string token = body["token"];
		int logID = int.Parse(body["logID"]);

		await context.Response.WriteAsJsonAsync(new
		{
			validToken = ValidateToken(token, logID)
		});
	}

	async public static Task LogInUser (HttpContext context)
	{
		IFormCollection body = context.Request.Form;
		string user = body["user"];
		string pwd = body["password"];
		string hashedPwd = HashPassword(pwd);

		Console.WriteLine(string.Format("{0}", hashedPwd));
		SqlCommand sqlCommand = new SqlCommand($"*verificar user y password {user} {hashedPwd}", connection);
		bool successLogin = true;
		int logID = 3;

		//using (SqlDataReader reader = sqlCommand.ExecuteReader())
		//{
		//    while (reader.Read())
		//    {
		//        successLogin = (bool) reader["success"];
		//    }
		//}

		if (successLogin)
		{
			string token = GenerateToken();
			tokens[token] = logID;
 
			await context.Response.WriteAsJsonAsync(new 
			{ 
				successLogin,
				token,
				logID
			});
		} 

		else
		{
			await context.Response.WriteAsJsonAsync(new
			{
				successLogin,
				token = "",
				logID = 0
			});
		}

	}

	async public static Task FetchUserInfo (HttpContext context)
	{
		IFormCollection body = context.Request.Form;
		string loginID = (string)body["id"];
		string token = body["token"];
	}

	public static string HashPassword (string password)
	{
		using (var hmac = new HMACSHA512(seed))
		{
			return Encoding.ASCII.GetString(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
		}
	}
}