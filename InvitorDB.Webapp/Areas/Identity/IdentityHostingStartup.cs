using InvitorDB.Data;
using InvitorDB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(InvitorDB.Webapp.Areas.Identity.IdentityHostingStartup))]
namespace InvitorDB.Webapp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDefaultIdentity<Person>(options =>
                options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<Role>()
                .AddEntityFrameworkStores<InvitorDBContext>();
            });
        }
    }
}