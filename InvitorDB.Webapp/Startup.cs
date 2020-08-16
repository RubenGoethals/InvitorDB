using InvitorDB.Data;
using InvitorDB.Models;
using InvitorDB.Models.Data;
using InvitorDB.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InvitorDB.Webapp
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
            //1. Setup
            services.AddControllers();

            services.AddControllersWithViews();
            services.AddRazorPages();

            //2. Registraties (van context, Identity) 
            //2.1. Context
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<InvitorDBContext>(options => options.UseSqlServer(connectionString));

            //2.2 Identity ( NIET de AddDefaultIdentity())
            //services.AddIdentity<Person, Role>().AddEntityFrameworkStores<InvitorDBContext>();

            //3 Registraties van Repos
            services.AddScoped<IEventRepo, EventRepo>();
            services.AddScoped<IPersonRepo, PersonRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RoleManager<Role> roleMgr, UserManager<Person> userMgr, InvitorDBContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            //Seeder voor Identity & Data
            InvitorDBContextExtensions.SeedRoles(roleMgr).Wait();
            //InvitorDBContextExtensions.SeedUsers(userMgr).Wait();
            //context.SeedData().Wait(); //oproepen als extensiemetehode.

        }
    }
}
