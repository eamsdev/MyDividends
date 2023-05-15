using System.Security.Authentication;
using System.Text.Json.Serialization;
using Application.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using WebApi.Services;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        return services
            .AddApi()
            .AddAuth()
            .AddProblemDetails()
            .AddCors(ConfigureCors)
            .AddControllerInternal()
            .AddProblemDetailsConventions()
            .AddHttpContextAccessor()
            .AddScoped<ICurrentUserService, CurrentUserService>();
    }    
    
    private static IServiceCollection AddControllerInternal(this IServiceCollection services)
    {

        services.AddControllers()
            .AddProblemDetailsConventions()
            .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        
        return services;
    }
    
    private static IServiceCollection AddProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(opt =>
        {
            opt.MapToStatusCode<AuthenticationException>(StatusCodes.Status401Unauthorized);
            opt.IncludeExceptionDetails = (_, _) => false;
        });
        
        return services;
    }
    
    private static void ConfigureCors(CorsOptions opt)
    {
        opt.AddPolicy("CorsPolicy", policy =>
        {
            policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders(
                    "WWW-Authenticate") // expose header so client can understand when to log user out
                .WithOrigins("http://localhost:8080"); // required when access resource from a different domain
        });
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddCookie(IdentityConstants.ApplicationScheme, ConfigureCookie)
            .AddCookie(IdentityConstants.ExternalScheme)
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme);
        
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

        return services;
    }
    
    private static void ConfigureCookie(CookieAuthenticationOptions options)
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Events.OnRedirectToLogin = c =>
        {
            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.FromResult<object>(null!);
        };
    }

    private static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
            });
        return services;
    }
}