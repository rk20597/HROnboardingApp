using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OfficeOpenXml;

ExcelPackage.License.SetNonCommercialPersonal(
    "TeamTracker");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Register ExcelRepository for HRData
var hrDataPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "..", "..", "Data", "HRData.xlsx");
Console.WriteLine($"HRData path: {hrDataPath}");
Console.WriteLine($"HRData exists: {File.Exists(hrDataPath)}");

builder.Services.AddSingleton<ExcelRepository>(
    new ExcelRepository(hrDataPath));

// Register TeamTrackerRepository
var teamTrackerPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "..", "..", "Data", "TeamTracker.xlsx");
Console.WriteLine($"TeamTracker path: {teamTrackerPath}");
Console.WriteLine($"TeamTracker exists: {File.Exists(teamTrackerPath)}");

builder.Services.AddSingleton<TeamTrackerRepository>(
    new TeamTrackerRepository(teamTrackerPath));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add JWT Authentication
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder
                    .Configuration["Jwt:Issuer"],
                ValidAudience = builder
                    .Configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration[
                                "Jwt:Key"] ?? ""))
            };
    });

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
