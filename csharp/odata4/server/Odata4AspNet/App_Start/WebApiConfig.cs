using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Odata4AspNet.Models;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;


namespace Odata4AspNet
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API の設定およびサービス
/*
            // Web API ルート
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
*/
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Movie>("Movies");

            builder.Namespace = "Action";

            //builder.EntityType<Movie>()
            builder.EntityType<Movie>().Collection
                    .Action("UpdateAll")
                .CollectionParameter<Movie>("value");


            config.Filter().Expand().Select().OrderBy().MaxTop(null).Count();
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel());

            // Warmup database
            MoviesContext context = new MoviesContext();
            context.Database.Initialize(false);
        }
    }
}
