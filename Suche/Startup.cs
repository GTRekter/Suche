using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Suche.Models.Authentication;
using Suche.Models.Context;

namespace Suche
{
    public class Startup
    {
        private readonly string domain;
        private readonly string appIdentifier;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            domain = Configuration["Auth0:Domain"];
            appIdentifier = Configuration["Auth0:ApiIdentifier"];
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // add jwt bearer authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{domain}/";
                options.Audience = appIdentifier;
            });

            // add authorization policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("create:categories", policy => policy.Requirements.Add(new HasScopeRequirement("create:categories", domain)));
                options.AddPolicy("update:categories", policy => policy.Requirements.Add(new HasScopeRequirement("update:categories", domain)));
                options.AddPolicy("delete:categories", policy => policy.Requirements.Add(new HasScopeRequirement("delete:categories", domain)));
            });

            // add in-memory database
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("SucheDatabase"));

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}