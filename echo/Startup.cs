using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Owin;
using System.Net.Http.Formatting;

namespace echo
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.Formatters.Add(new TextMediaTypeFormatter());

            config.MapHttpAttributeRoutes();

            appBuilder.UseWebApi(config);
        }
    } 
}
