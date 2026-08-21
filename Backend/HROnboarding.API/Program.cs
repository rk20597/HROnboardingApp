using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Text;
using OfficeOpenXml;

ExcelPackage.License.SetNonCommercialPersonal("Rohan");

var excelPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Data", "HRData.xlsx");
Console.WriteLine($"Looking for excel at :{excelPath}");
Console.WriteLine($"Fileexists:{File.Exists(excelPath)}");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<ExcelRepository>(new ExcelRepository(excelPath));

builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(

    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
         builder.Configuration["JWT:Key"] ?? ""))
    
    });

builder.Services.AddControllers();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();