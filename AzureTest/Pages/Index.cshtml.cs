using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace AzureTest.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;

		public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
		{
			_logger = logger;
			_config = configuration;
		}

		public void OnGet()
		{
			string connString = "null";
			try
			{
				bool isOnAzure = !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));

				connString =  this._config.GetConnectionString(isOnAzure ? "ConnectionStringFromAzure" : "LocalTestDB");

				using (var conn = new SqlConnection(connString))
				{
					conn.Open();
					var cmd = new SqlCommand("SELECT * FROM AzureHelloTable", conn);
					var dataReader = cmd.ExecuteReader();
					while (dataReader.Read())
					{
						ViewData["messageFromDB"] += "<br/>" + (string)dataReader.GetValue(1);
					}
				}
			}
			catch(Exception e)
			{
				ViewData["messageFromDB"] = $"<span style=\"color: red;\"> Failed to get data from DB <br/>{e.Message}<br/>Connection String: {connString}</span>";
			}

		}
	}
}
