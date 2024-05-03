using System.Text;
using GameStore.Api.Configurations;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(connString);

//dependency injection
// Configures the JwtConfig object using the JwtConfig section from the configuration file.
// This allows the application to read JWT-related configuration settings, such as the token secret,
// expiration time, issuer, and audience, from the appsettings.json file and bind them to the JwtConfig object.

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));


//it set the authentication mechanism; first the authentication, later authorization
//
builder.Services.AddAuthentication(configureOptions: options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //sometime in local machine can  generate an error cause we're ussing http rather than https, so it could generate an error; so we could put it in false to solve it
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret"]!))
    };
});


builder.Services.AddAuthorization();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();
