using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NSwagFrameworkWithExamples.Models
{
    public class Vessel
    {
        public string Name { get; set; }
        public string Imo { get; set; }
        public int YearBuilt { get; set; }
        public int Depth { get; set; }
        public int Length { get; set; }
    }
}