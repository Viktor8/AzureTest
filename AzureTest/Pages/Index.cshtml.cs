﻿using Microsoft.AspNetCore.Mvc;
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
			string connString = this._config.GetConnectionString("DefaultConnection");

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
	}
}
