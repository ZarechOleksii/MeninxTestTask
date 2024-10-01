using DAL.Implementations;
using DAL.Interfaces;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using WebForms.App_Start;
using Microsoft.EntityFrameworkCore;

namespace WebForms
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            ServiceCollection container = new ServiceCollection();

            container.AddDbContext<LibraryDBContext>(options => options.UseSqlServer("Server=localhost;Database=Meninx;TrustServerCertificate=True;Trusted_Connection=True;"));
            container.AddScoped<IBookRepository, BookRepository>();
            container.AddScoped<ICategoryRepository, CategoryRepository>();
            container.AddLogging();

            HttpRuntime.WebObjectActivator = new ServiceProviderWrapper(container.BuildServiceProvider());

            using (var serviceScope = HttpRuntime.WebObjectActivator.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<LibraryDBContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}