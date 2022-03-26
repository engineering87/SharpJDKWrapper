// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using Owin;
using Swashbuckle.Application;
using System.Linq;
using System.Web.Http;

namespace SharpJDKWrapper
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Setup Swagger on http://localhost:12345/swagger
            config.EnableSwagger(c =>
                {                  
                    c.SingleApiVersion("v1", "JavaServiceWrapper");
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                })
                .EnableSwaggerUi(c => c.DisableValidator());

            appBuilder.UseWebApi(config);
        }
    }
}
