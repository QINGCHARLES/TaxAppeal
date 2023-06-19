using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using TaxAppeal.Data;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
