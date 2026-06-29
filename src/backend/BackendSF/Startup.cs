using System.Text;
using BackendSF.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using TripPlanner.Contracts.Services;

namespace BackendSF;

public sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins(
                        Configuration["Cors:ReactOrigin"] ?? "http://localhost:5173",
                        "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(_ => ServiceProxy.Create<IValidatorService>(
            new Uri(Configuration["ServiceFabric:ValidatorServiceUri"] ?? "fabric:/TripPlannerApplication/ValidatorService")));

        services.AddScoped(_ => ServiceProxy.Create<IShareService>(
            new Uri(Configuration["ServiceFabric:ShareServiceUri"] ?? "fabric:/TripPlannerApplication/ShareService")));

        services.AddScoped(_ => ServiceProxy.Create<IEventDispatcherService>(
            new Uri(Configuration["ServiceFabric:EventDispatcherServiceUri"] ?? "fabric:/TripPlannerApplication/EventDispatcherService")));

        var jwtKey = Configuration["Jwt:Key"] ?? "development-key-change-before-running-the-project";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors("AllowReactApp");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
