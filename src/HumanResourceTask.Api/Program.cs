using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FastEndpoints;
using HumanResourceTask.Api.Authentication;
using HumanResourceTask.Api.Dto;
using HumanResourceTask.Api.Initialisation;
using HumanResourceTask.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace HumanResourceTask.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            builder.Services.AddCors(
                options => options.AddDefaultPolicy(
                    policy => policy
                        //.WithOrigins([builder.Configuration["ApiUrl"] ?? "https://localhost:7204", builder.Configuration["ClientUrl"] ?? "https://localhost:7020"])
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
                {
                    var configurationSection = builder.Configuration.GetSection("AzureAd");
                    configurationSection.Bind(jwtBearerOptions);
                    jwtBearerOptions.TokenValidationParameters.NameClaimType = "sub"; 
                    jwtBearerOptions.TokenValidationParameters.RoleClaimType = "role";
                    jwtBearerOptions.Audience = "bb441160-e951-4cd5-a1ac-9123e93167fc";
                    //jwtBearerOptions.Events = new JwtBearerEvents();
                    //jwtBearerOptions.Events.OnMessageReceived = OnMessageReceived;
                    //jwtBearerOptions.Events.OnAuthenticationFailed = OnAuthenticationFailed;
                    //jwtBearerOptions.Events.OnTokenValidated = OnTokenValidated;
                }, identityOptions =>
                {
                    var configurationSection = builder.Configuration.GetSection("AzureAd");
                    configurationSection.Bind(identityOptions);
                });

            builder.Services
                .AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.GetEmployee, b => b.RequireClaim("api"))
                .AddPolicy(PolicyNames.CreateEmployee, b => b.RequireClaim("api"))
                .AddPolicy(PolicyNames.UpdateEmployee, b => b.RequireClaim("api"))
                .AddPolicy(PolicyNames.DeleteEmployee, b => b.RequireClaim("api"));

            builder.Services.AddFastEndpoints();

            builder.Services.AddOptions();
            builder.Services.Configure<DbConnectionOptions>(builder.Configuration.GetSection("Database"));

            builder.Services.AddApplication();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            //app.Use(async (context, next) =>
            //{
            //    if (context.User.Identity?.IsAuthenticated ?? false)
            //    {
            //        var claims = context.User.Claims.Select(c => $"{c.Type}: {c.Value}");
            //        Console.WriteLine("Authenticated User Claims:");
            //        Console.WriteLine(string.Join(Environment.NewLine, claims));
            //    }
            //    await next();
            //});

            app.UseAuthorization();

            app.UseFastEndpoints(c =>
            {
                c.Binding.ValueParserFor(typeof(Sort), CustomParsers.SortParser);

                c.Endpoints.Configurator = ep =>
                {
                    ep.Options(b =>
                    {
                        b.AllowAnonymous();
                        b.RequireCors(policy => policy
                            //.WithOrigins([builder.Configuration["ApiUrl"] ?? "https://localhost:7204", builder.Configuration["ClientUrl"] ?? "https://localhost:7099"])
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                    });
                };
            });

            app.Run();
        }

        //private static Task OnTokenValidated(TokenValidatedContext context)
        //{
        //    var identity = context.Principal?.Identity as ClaimsIdentity;
        //    identity?.AddClaim(new Claim("role", "admin"));
        //    return Task.CompletedTask;
        //}

        //private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        //{
        //    return Task.CompletedTask;
        //}

        //private static Task OnMessageReceived(MessageReceivedContext context)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
