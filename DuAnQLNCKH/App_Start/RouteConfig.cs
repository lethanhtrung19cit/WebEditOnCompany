using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DuAnQLNCKH
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Detail",
            url: "thong-bao/{MetaTitle}-{id}",
            defaults: new { controller = "Notification", action = "DetailNo", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Notification", action = "NotifiStu", id = UrlParameter.Optional }
            );
        }
    }
}
