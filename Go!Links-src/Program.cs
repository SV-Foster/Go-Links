/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using Go_Links.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using RazorLight;
using System;
using System.Net;


const string AppConfigFileName = "config.xml";
const string QueryLinkID = "linkid";
const string SQLSelectData = "SELECT DisplayName, URL FROM `links` WHERE ID = @ID LIMIT 1";
const string SQLInsertRedir = "INSERT INTO `requests` (`DateTimeUTC`, `LinkID`, `IPv4`, `IPv6`, `User-Agent`, `Referer`, `Accept-Language`, `Cookie`) VALUES (@DateTimeUTC, @LinkID, @IPv4, @IPv6, @UserAgent, @Referer, @AcceptLanguage, @Cookie)";


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Configuration.AddXmlFile(AppConfigFileName);

var RazorLightEngine = new RazorLightEngineBuilder()
	.UseFileSystemProject(Path.GetFullPath(app.Configuration["TemplatesPath"] ?? ".\\"))
	.UseMemoryCachingProvider()
	.Build();


app.Use(CheckMethod);
app.Use(Standard404);
app.Use(CheckPath);
app.Use(HandleRequst);

app.Run();


async Task CheckMethod(HttpContext context, RequestDelegate next)
{
	if (context.Request.Method != "GET")
	{
		context.Response.StatusCode = 400;
		await context.Response.WriteAsync("");
		return;
	}

    await next.Invoke(context);
}

async Task Standard404(HttpContext context, RequestDelegate next)
{
	var StdArray = OptionsArrayGet("StandardPathArray:Item");


	foreach (string n in StdArray)
		if (String.Equals(n, context.Request.Path.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			context.Response.StatusCode = 404;
			await context.Response.WriteAsync("");
			return;
		}
	
	await next.Invoke(context);
}

async Task CheckPath(HttpContext context, RequestDelegate next)
{
	var PathValidArray = OptionsArrayGet("PathValidArray:Item");


	foreach (string n in PathValidArray)
		if (String.Equals(n, context.Request.Path.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			await next.Invoke(context);
			return;
		}
	
	context.Response.StatusCode = 400;
	await context.Response.WriteAsync("");
}

async Task HandleRequst(HttpContext context, RequestDelegate next)
{
	var rqst = context.Request;
	var resp = context.Response;
	var IPAddr = context.Connection.RemoteIpAddress;
	int linkid;


	// check and convert the ID provided
	string? linkidStr = rqst.Query[QueryLinkID];
	if (String.IsNullOrEmpty(linkidStr))
	{
		resp.StatusCode = 400;
		await resp.WriteAsync("");
		await next.Invoke(context);
		return;
	}

	if (!int.TryParse(linkidStr, out linkid))
	{
		resp.StatusCode = 400;
		await resp.WriteAsync("");
		await next.Invoke(context);
		return;
	}

	// Create a connection
	using var SQLConnection = new MySqlConnection(app.Configuration["MySQLConnectionParameters"]);	
	using var SQLLinkGet = new MySqlCommand(SQLSelectData, SQLConnection);
	using var SQLRequestSave = new MySqlCommand(SQLInsertRedir, SQLConnection);
	SQLLinkGet.Parameters.AddWithValue("@ID", linkid);

	// get data out of MySQL
	SQLConnection.Open();
	using var reader = SQLLinkGet.ExecuteReader();

	// no data found for this ID
	if (!reader.HasRows)
	{
		resp.StatusCode = 400;
		await resp.WriteAsync("");
		await next.Invoke(context);
		return;
	}

	// get data to show
	reader.Read();
	var model = new RedirectSimple(reader.GetString("DisplayName"), reader.GetString("URL"));
	var readerClosed = reader.CloseAsync();

	// form the answer
	resp.Headers.ContentLanguage = "en-US";
	resp.Headers.ContentType = "text/html; charset=US-ASCII";
#if DEBUG
	resp.Headers.Append("engineering-release", "true");
	resp.Headers.Append(QueryLinkID, linkidStr);
#endif
	resp.StatusCode = 200;

	// Compile and render the template
	var html = await RazorLightEngine.CompileRenderAsync("RedirectSimple.cshtml", model);
	var respDone = resp.WriteAsync(html,System.Text.Encoding.ASCII);

	// insert the log record into the database
	// Add the parameters to the command
	SQLRequestSave.Parameters.AddWithValue("@DateTimeUTC", DateTime.UtcNow);
	SQLRequestSave.Parameters.AddWithValue("@LinkID", linkid);
	SQLRequestSave.Parameters.AddWithValue("@IPv4", null);
	SQLRequestSave.Parameters.AddWithValue("@IPv6", null);
	SQLRequestSave.Parameters.AddWithValue("@UserAgent",      rqst.Headers.UserAgent.Count == 0      ? null : rqst.Headers.UserAgent.ToString());
	SQLRequestSave.Parameters.AddWithValue("@Referer",        rqst.Headers.Referer.Count == 0        ? null : rqst.Headers.Referer.ToString());
	SQLRequestSave.Parameters.AddWithValue("@AcceptLanguage", rqst.Headers.AcceptLanguage.Count == 0 ? null : rqst.Headers.AcceptLanguage.ToString());
	SQLRequestSave.Parameters.AddWithValue("@Cookie",         rqst.Headers.Cookie.Count == 0         ? null : rqst.Headers.Cookie.ToString());

	if (IPAddr != null)
		switch (IPAddr.AddressFamily)
		{
			case System.Net.Sockets.AddressFamily.InterNetwork:
				SQLRequestSave.Parameters.RemoveAt("@IPv4");
				SQLRequestSave.Parameters.AddWithValue("@IPv4", IPAddr.GetAddressBytes());
				break;

			case System.Net.Sockets.AddressFamily.InterNetworkV6:
				SQLRequestSave.Parameters.RemoveAt("@IPv6");
				SQLRequestSave.Parameters.AddWithValue("@IPv6", IPAddr.GetAddressBytes());
				break;
		}

	// Execute the command
	readerClosed.Wait();
	SQLRequestSave.ExecuteNonQuery();
	
	SQLConnection.Close();
	respDone.Wait();
	await next.Invoke(context);
}

List<string> OptionsArrayGet(string OptionsPath)
{
	var pathValidArrayConfig = app.Configuration.GetSection(OptionsPath);


	if (pathValidArrayConfig == null)
		return [];

	if (pathValidArrayConfig.GetChildren().Any())
		return pathValidArrayConfig.Get<List<string>>() ?? [];
	else
		return [pathValidArrayConfig.Value ?? ""];
}
