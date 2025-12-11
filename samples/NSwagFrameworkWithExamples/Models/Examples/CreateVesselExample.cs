using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NSwag.Examples.Core;

namespace NSwagFrameworkWithExamples.Models.Examples
{
    public class CreateVesselExample : IExampleProvider<VesselCreate>
    {
        public VesselCreate GetExample()
        {
            return new VesselCreate()
            {
                Name = "Ever Given",
                Imo = "9811000",
                YearBuilt = 2018,
                Depth = 24,
                Length = 400
            };
        }
    }
}