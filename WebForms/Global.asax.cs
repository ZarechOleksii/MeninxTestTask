using DAL;
using DAL.Implementations;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using WebForms.App_Start;
using WebForms.Controllers;

namespace WebForms
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServiceCollection container = new ServiceCollection();

            container.AddDbContext<LibraryDBContext>(options => options.UseSqlServer("Server=localhost;Database=Meninx;TrustServerCertificate=True;Trusted_Connection=True;"));
            container.AddScoped<IBookRepository, BookRepository>();
            container.AddScoped<ICategoryRepository, CategoryRepository>();
            container.AddTransient<BooksController>();
            container.AddLogging();

            ServiceProvider serviceProvider = container.BuildServiceProvider();

            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(serviceProvider);

            HttpRuntime.WebObjectActivator = new ServiceProviderWrapper(serviceProvider);

            #if DEBUG
            using (var serviceScope = HttpRuntime.WebObjectActivator.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<LibraryDBContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            #endif
        }
    }
}