using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SSO.Client.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace SSO.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => { })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            //  JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "Cookies";
                options.DefaultAuthenticateScheme = "Identity.Application";
            })
                .AddCookie("Cookies", auth =>
                 {
                     auth.LoginPath = "/identity/account/login";
                 })
                .AddOpenIdConnect("oidc", "Log in with SSO Auth Server", options =>
                 {
                     options.Authority = "https://localhost:44372/";
                     //options.ForwardChallenge = "oidc";

                     options.ClientId = "iclient";
                     options.ClientSecret = "secret";
                     options.ResponseType = "code";
                     options.SaveTokens = true;
                     options.GetClaimsFromUserInfoEndpoint = true;
                     options.Scope.Add("email");
                     options.Scope.Add("profile");
                     options.Scope.Add("roles");
                     options.ClaimActions.MapJsonKey("role", "role", "role"); //And this
                 });
            //services.ConfigureApplicationCookie(options => {
            //    options.Cookie.Name = ".AspNet.SharedCookie";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
