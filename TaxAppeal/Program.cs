using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Web;
using TaxAppeal.Data;
using TaxAppeal.Models;

var builder = WebApplication.CreateBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Host.UseSystemd();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

using (TextReader sr = new StringReader(@$"
		<rewrite>
			<rules>
				<clear />
				<rule enabled=""true"" stopProcessing=""true"">
					<match url=""(.*)"" />
					<conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
						<add input=""{{HTTPS}}"" pattern=""^OFF$"" />
					</conditions>
					<action type=""Redirect"" url=""https://{{HTTP_HOST}}{{REQUEST_URI}}"" appendQueryString=""false"" redirectType=""308"" />
				</rule>
				<rule enabled=""true"">
					<match url=""(.*)"" />
					<conditions logicalGrouping=""MatchAll"" trackAllCaptures=""false"">
						<add input=""{{HTTP_HOST}}"" pattern=""^cookcountypropertytaxappeal\.com$|^localhost"" negate=""true"" />
					</conditions>
					<action type=""Redirect"" url=""https://cookcountypropertytaxappeal.com/{{R:1}}"" redirectType=""308"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^step-four"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/StepFour"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^step-three"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/StepThree"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^step-two"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/StepTwo"" />
				</rule>
				<rule enabled=""true"">
					<match url=""^about-us"" />
					<action type=""Rewrite"" url=""https://{{HTTP_HOST}}/AboutUs"" />
				</rule>
			</rules>
		</rewrite>
	"))
{
	var options = new RewriteOptions()
			.AddIISUrlRewrite(sr);

	app.UseRewriter(options);
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/api:find-address", async (string query) =>
{
	// https://gis.cookcountyil.gov/traditional/rest/services/AddressLocator/addressPtMuniZip/GeocodeServer/findAddressCandidates?Street={HttpUtility.UrlEncode(query)}&f=json

	using HttpClient client = new()
	{
		BaseAddress = new Uri("https://gis.cookcountyil.gov")
	};

	try
	{
		GisAddressPin? JsonAddress = await client.GetFromJsonAsync<GisAddressPin>($"/traditional/rest/services/AddressLocator/addressPtMuniZip/GeocodeServer/findAddressCandidates?Street={HttpUtility.UrlEncode(query)}&f=json");
		string ffff = "";
		List<string> gggg = new();
		if (JsonAddress != null && JsonAddress.candidates != null && JsonAddress.candidates.Count > 0 && !String.IsNullOrEmpty(JsonAddress.candidates[0].address))
		{
			foreach (Candidate dddd in JsonAddress.candidates)
			{
				ffff = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dddd.address!.ToLower());
				for (int i = 0; i <= 9; i++)
				{
					string ordinal = i + (i == 0 ? "Th" : new[] { "St", "Nd", "Rd", "Th" }[Math.Min(3, (i - 1) % 10)]);
					ffff = ffff.Replace(ordinal, ordinal.ToLower());
				}
				ffff = ffff.Substring(0, ffff.LastIndexOf(',')) + ", IL," + ffff.Substring(ffff.LastIndexOf(',') + 1);
				gggg.Add(ffff);
			}
		}
		return gggg.ToArray();
	}
	catch (Exception e)
	{
		return new string[] { e.ToString() };
	}


});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}