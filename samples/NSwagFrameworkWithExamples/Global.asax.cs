using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace NSwagFrameworkWithExamples
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var jsonSerializerSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;

            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}