using System.Text;
using GameStore.Api.Configurations;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value!);
    // This line instructs the authentication middleware to store the JWT token in the authentication context after validating and decoding it. Useful if you needed in the future
    options.SaveToken = true;
    options.IncludeErrorDetails = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = false, //sometime in local machine can  generate an error cause we're ussing http rather than https, so it could generate an error; so we could put it in false to solve it
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JwtConfig:ValidIssuer"],
        ValidAudience = builder.Configuration["JwtConfig:ValidAudiences"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<GameStoreContext>();


builder.Services.AddAuthorization();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

//this allow us to use the routes specified on our controllers
app.MapControllers();

app.MapGamesEndpoints();
app.MapGenresEndpoints();

await app.MigrateDbAsync();

app.Run();
