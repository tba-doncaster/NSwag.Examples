using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using NSwagFrameworkWithExamples.Models;

namespace NSwagFrameworkWithExamples.Controllers
{
    [RoutePrefix("api/vessels")]
    public class VesselsController : ApiController
    {
        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> GetVessels()
        {
            return Ok(new List<Vessel>
            {
                new Vessel(){ Name = "Vessel 1", Imo = "IMO000001", Depth = 50, Length = 1000, YearBuilt = 2023},
                new Vessel(){ Name = "Vessel 2", Imo = "IMO000002", Depth = 60, Length = 1500, YearBuilt = 2019}
            });
        }
    }
}