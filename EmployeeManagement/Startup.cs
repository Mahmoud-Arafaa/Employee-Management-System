using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authentication;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("EmployeeDBConnection"))
            );
            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequiredLength = 10;
                option.Password.RequiredUniqueChars = 3;
                
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(

                options =>
                {
                    options.Password.RequiredLength = 10;
                    options.Password.RequiredUniqueChars = 3;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                }

                )
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc(options =>
            {
                {       /// No Controller Can Be Accessed Without Logoing !!!!!!!!
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }
            }).AddXmlSerializerFormatters();
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddScoped<IEmployeeReopsitory,SQLEmployeeRepository>();


            /// only this user can do this action 
            services.AddAuthorization(option =>
                option.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"))
            );
            services.AddAuthorization(option =>
               option.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement())));

            services.AddSingleton<IAuthorizationHandler, CanEditeOnlyOtherAdminInRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();



            /// Google 
            services.AddAuthentication().AddGoogle(
                options =>
                    {
                        options.ClientId = "353319846278-35rnihve5vupmt25c0n7vp2n24ij8khc.apps.googleusercontent.com";
                        options.ClientSecret = "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";
                    }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");

            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc(route => 
            {

                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
               
            });
        }
    }
}
