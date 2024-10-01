using DAL;
using DAL.Implementations;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Web;

//[assembly: PreApplicationStartMethod(typeof(WebForms.App_Start.PreStart), methodName: nameof(WebForms.App_Start.PreStart.OnPreStart))]

namespace WebForms.App_Start
{
    public class PreStart
    {
        public static void OnPreStart()
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
                context.Database.EnsureCreated();
            }
        }
    }
}