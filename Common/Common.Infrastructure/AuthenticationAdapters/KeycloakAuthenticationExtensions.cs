﻿using Common.Infrastructure.AuthenticationAdapters;
using IdentityModel.Client;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Infrastructure.AuthenticationAdapters
{
    public static class KeycloakAuthenticationExtensions
    {
        public const string IntrospectionSchemeName = "Introspection";
        public static IServiceCollection AddKeycloakJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder, IHostEnvironment env, bool withIntrospection)
        {
            var keycloakServiceConfig = builder.Configuration.GetSection(nameof(KeycloakServiceConfig)).Get<KeycloakServiceConfig>()!;

            var authenticationBuilder = services.AddKeycloakWebApiAuthentication(authenticationOptions =>
            {
                authenticationOptions.AuthServerUrl = keycloakServiceConfig.AuthServerUrl;
                authenticationOptions.Realm = keycloakServiceConfig.Realm;
                authenticationOptions.Resource = keycloakServiceConfig.ClientId;
                authenticationOptions.VerifyTokenAudience = false;
                authenticationOptions.TokenClockSkew = TimeSpan.FromSeconds(5);
                authenticationOptions.SslRequired = "none";
                authenticationOptions.Credentials = new KeycloakClientInstallationCredentials
                {
                    Secret = keycloakServiceConfig.Secret,
                };
            },
            jwtBearerOptions => ConfigureJwtBearerOptions(jwtBearerOptions, withIntrospection, env.IsDevelopment()));

            services.ConfigureIntrospection(authenticationBuilder, keycloakServiceConfig);

            return services;
        }

        private static void ConfigureJwtBearerOptions(JwtBearerOptions jwtBearerOptions, bool withIntrospection, bool isDevelopment)
        {
            jwtBearerOptions.RequireHttpsMetadata = false;

            // Prevent middleware to modify claims after successful authentication,
            // enables using "roles" and "name" claims in authorization attributes
            jwtBearerOptions.MapInboundClaims = false;
            jwtBearerOptions.SaveToken = isDevelopment;
            if (withIntrospection)
            {
                jwtBearerOptions.ForwardAuthenticate = IntrospectionSchemeName;
            }

            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudiences = ["account"],

                // Ensures that "roles" claim type received in Access Token from Azure AD
                // will be used in Authorize attribute eg. [Authorize(Roles = "Admin")]
                // or ClaimsPrincipal user.IsInRole("Admin")
                RoleClaimType = KeycloakClaims.RoleClaimType,
                NameClaimType = KeycloakClaims.NameClaimType,
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

            if (isDevelopment)
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
        }

        private static void ConfigureIntrospection(this IServiceCollection services, KeycloakWebApiAuthenticationBuilder authenticationBuilder, KeycloakServiceConfig keycloakServiceConfig)
        {
            services.AddAuthentication(authenticationBuilder.JwtBearerAuthenticationScheme)
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

                    options.Events = new IdentityModel.AspNetCore.OAuth2Introspection.OAuth2IntrospectionEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var claimsIdentity = context.Principal!.Identity as ClaimsIdentity;
                            if (claimsIdentity != null)
                            {
                                var roleClaims = claimsIdentity.FindAll("appRole").Select(c => c.Value).ToList();

                                foreach (var role in roleClaims)
                                {
                                    claimsIdentity.AddClaim(new Claim(claimsIdentity.RoleClaimType, role));
                                }
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
