using Coffee.Journal.Application.Contracts;
using Coffee.Journal.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Coffee.Journal.API.Authentication;

internal static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        AuthenticationSettings settings = configuration
            .GetSection(AuthenticationSettings.SectionName)
            .Get<AuthenticationSettings>()
            ?? new AuthenticationSettings();

        if (string.IsNullOrWhiteSpace(settings.Authority)
            || string.IsNullOrWhiteSpace(settings.Issuer)
            || string.IsNullOrWhiteSpace(settings.Audience)
            || string.IsNullOrWhiteSpace(settings.RequiredScope))
        {
            throw new InvalidOperationException(
                $"Configuration '{AuthenticationSettings.SectionName}' must contain 'Authority', 'Issuer', 'Audience' and 'RequiredScope'.");
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = settings.Authority;

                // Keep the short claim names (oid, sub, scp) instead of the long Microsoft ones.
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = settings.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(AuthenticationSettings.ApiScopePolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context => CurrentUserClaims.HasScope(context.User, settings.RequiredScope)))
            .AddPolicy(AuthenticationSettings.ContributorPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context => CurrentUserClaims.HasScope(context.User, settings.RequiredScope))
                .AddRequirements(new RoleRequirement(RoleNames.Contributor, RoleNames.Admin)))
            .AddPolicy(AuthenticationSettings.AdminPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context => CurrentUserClaims.HasScope(context.User, settings.RequiredScope))
                .AddRequirements(new RoleRequirement(RoleNames.Admin)));

        services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, HttpContextCurrentUser>();

        return services;
    }
}
