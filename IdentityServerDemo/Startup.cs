using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddIdentityServer(options =>
            {
                options.PublicOrigin = "https://localhost:44366/";
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryPersistedGrants()
            .AddInMemoryClients(GetClients().ToList())
            .AddTestUsers(GetUsers().ToList())
            .AddInMemoryIdentityResources(GetResources().ToList());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }

        private static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "fefd4c8c-6558-45f9-9553-81003102a524",
                    ClientName = "Демонстрационно приложение",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44303/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44303/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("cb0c0c5c-4d94-4bf5-8a9e-ab405bef06aa".Sha256())
                    }
                },
                new Client()
                {
                    ClientId = "eb9d7a89-42f6-4205-a490-5f342287eb87",
                    ClientName = "Демонстрационно приложение 2",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44335/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44335/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("362cffb3-9ffe-4f04-889f-07d9b19f0d40".Sha256())
                    }
                }
            };
        }

        private static IEnumerable<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    Username = "stamo",
                    Password = "passw0rd",
                    IsActive = true,
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Claims = new List<Claim>()
                    {
                             new Claim("given_name", "Stamo"),
                             new Claim("family_name", "Petkov"),
                             new Claim("email", "stamo.petkov@gmail.com"),
                             new Claim("name", "Stamo Petkov")
                    }
                }
            };
        }

        private static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
