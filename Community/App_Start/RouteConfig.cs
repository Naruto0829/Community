using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Community
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GetStates",
                url: "Post/GetStates/{countryId}",
                defaults: new { controller = "Post", action = "GetStates", countryId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GetCities",
                url: "Post/GetCities/{regionId}",
                defaults: new { controller = "Post", action = "GetCities", regionId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
