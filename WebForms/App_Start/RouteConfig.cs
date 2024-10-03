using Microsoft.AspNet.FriendlyUrls;
using System.Web.Http;
using System.Web.Routing;

namespace WebForms
{
    public static class RouteConfig
    {
        public const string RouteName = "DefaultApi";

        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
