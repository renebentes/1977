using ExternalProviders.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace ExternalProviders
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAuthentication();
            //     .AddFacebook(facebookOptions =>
            //     {
            //         facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //         facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];

            //         // Obter dados por escopo - apenas após validação do app pelo Facebook
            //         facebookOptions.Scope.Add("user_birthday");

            //         // Obter locale usado pelo usuário
            //         facebookOptions.ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");

            //         facebookOptions.SaveTokens = true;
            //     })
            //     .AddGoogle(googleOptions =>
            //     {
            //         googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //         googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //         googleOptions.SaveTokens = true;
            //     });
            // .AddTwitter(twitterOptions =>
            //     {
            //         twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
            //         twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
            //         twitterOptions.SaveTokens = true;

            //         twitterOptions.RetrieveUserDetails = true;

            //         twitterOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
            //     })
            //     .AddMicrosoftAccount(maOptions =>
            //     {
            //         maOptions.ClientId = Configuration["Authentication:Microsoft:ClientId"];
            //         maOptions.ClientSecret = Configuration["Authentication:Microsoft:ClientSecret"];
            //         maOptions.SaveTokens = true;
            //     })
            //     .AddLinkedIn(linkedInOptions =>
            //     {
            //         linkedInOptions.ClientId = Configuration["Authentication:LinkedIn:ClientId"];
            //         linkedInOptions.ClientSecret = Configuration["Authentication:LinkedIn:ClientSecret"];
            //         linkedInOptions.SaveTokens = true;
            //     });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }
    }
}