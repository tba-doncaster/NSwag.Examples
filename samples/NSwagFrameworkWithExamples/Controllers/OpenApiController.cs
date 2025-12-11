using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NSwag.Generation.WebApi;
using NSwagFrameworkWithExamples.Models;

namespace NSwagFrameworkWithExamples.Controllers
{
    [RoutePrefix("openapi")]
    public class OpenApiController : ApiController
    {
        [HttpGet]
        [Route]
        [Route("spec")]
        public HttpResponseMessage GetSpec()
        {
            var settings = new WebApiOpenApiDocumentGeneratorSettings
            {
                Title = "CommTrac API",
                Version = "1.0",
                AddMissingPathParameters = true,
                UseControllerSummaryAsTagDescription = true,
            };
            var apiExplorer = GlobalConfiguration.Configuration.Services.GetApiExplorer();

            var controllerTypes = apiExplorer.ApiDescriptions.GroupBy(desc => desc.ActionDescriptor.ControllerDescriptor.ControllerType).Select(g => g.Key);

            var generator = new WebApiOpenApiDocumentGenerator(settings);
            var document = generator.GenerateForControllersAsync(controllerTypes).Result;

            var json = document.ToJson();

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };

            return response;
        }
    }
}