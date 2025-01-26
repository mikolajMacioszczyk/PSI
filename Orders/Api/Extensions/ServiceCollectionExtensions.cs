using Application.Interfaces;
using FluentValidation;
using IdentityModel.Client;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;

namespace Api.Extensions;

// TODO: Common Adapter
public static class ServiceCollectionExtensions
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddFluentValidators(this IServiceCollection services)
    {
        //services.AddScoped<IValidator<>, >();
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            AddBearerHeader(c);
            c.SupportNonNullableReferenceTypes();
        });
    }

    private static void AddBearerHeader(SwaggerGenOptions options)
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        options.OperationFilter<AddAuthorizationHeaderOperationHeader>();

        options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    }

    public const string IntrospectionSchemeName = "Introspection";

    public static IServiceCollection AddKeycloakJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder, IHostEnvironment env)
    {
        var keycloakServiceConfig = builder.Configuration.GetSection(nameof(KeycloakServiceConfig)).Get<KeycloakServiceConfig>()!;

        var authorizationBuilder = services.AddKeycloakWebApiAuthentication(authenticationOptions =>
        {
            authenticationOptions.AuthServerUrl = keycloakServiceConfig.AuthServerUrl;
            authenticationOptions.Realm = keycloakServiceConfig.Realm;
            authenticationOptions.Resource = keycloakServiceConfig.ClientId;
            authenticationOptions.VerifyTokenAudience = true;
            authenticationOptions.TokenClockSkew = TimeSpan.FromSeconds(5);
            authenticationOptions.SslRequired = "none";
            authenticationOptions.Credentials = new KeycloakClientInstallationCredentials
            {
                Secret = keycloakServiceConfig.Secret,
            };
        },
        jwtBearerOptions =>
        {
            jwtBearerOptions.RequireHttpsMetadata = false;

            // Prevent middleware to modify claims after successful authentication,
            // enables using "roles" and "name" claims in authorization attributes
            jwtBearerOptions.MapInboundClaims = false;
            jwtBearerOptions.SaveToken = env.IsDevelopment();
            jwtBearerOptions.ForwardAuthenticate = IntrospectionSchemeName;

            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                LogValidationExceptions = true,

                // Ensures that "roles" claim type received in Access Token from Azure AD
                // will be used in Authorize attribute eg. [Authorize(Roles = "Admin")]
                // or ClaimsPrincipal user.IsInRole("Admin")
                RoleClaimType = "appRole",
                NameClaimType = "name",
            };

            jwtBearerOptions.Events = new JwtBearerEvents()
            {
                OnTokenValidated = (ctx) =>
                {
                    ArgumentNullException.ThrowIfNull(ctx.Principal);
                    var identity = ctx.Principal?.Identities.Single();

                    // This mapping is required by Ocelot as it checks for "scope" claim instead of "scp"    
                    if (identity != null)
                    {
                        var scopeClaim = identity.FindFirst("scope");

                        if (scopeClaim != null && scopeClaim.Value.Contains("api"))
                        {
                            var newScopeClaim = new Claim("scope", "api");
                            identity.TryRemoveClaim(scopeClaim);
                            identity.AddClaim(newScopeClaim);
                        }
                    }

                    return Task.CompletedTask;
                }
            };

            if (env.IsDevelopment())
            {
                var httpHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = delegate { return true; },
                    AllowAutoRedirect = false,
                    CheckCertificateRevocationList = false,
                    ClientCertificateOptions = ClientCertificateOption.Manual
                };

                jwtBearerOptions.Backchannel = new HttpClient(httpHandler);
                jwtBearerOptions.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core JwtBearer handler");
                jwtBearerOptions.Backchannel.Timeout = jwtBearerOptions.BackchannelTimeout;
                jwtBearerOptions.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }
        });

        services.AddAuthentication(authorizationBuilder.JwtBearerAuthenticationScheme)
            .AddOAuth2Introspection(IntrospectionSchemeName, options =>
            {
                options.Authority = $"{keycloakServiceConfig.AuthServerUrl}realms/{keycloakServiceConfig.Realm}";
                options.ClientId = keycloakServiceConfig.ClientId;
                options.ClientSecret = keycloakServiceConfig.Secret;
                options.DiscoveryPolicy = new DiscoveryPolicy
                {
                    RequireHttps = false,
                    ValidateIssuerName = false,
                    ValidateEndpoints = false,
                };
            });

        return services;
    }
}
