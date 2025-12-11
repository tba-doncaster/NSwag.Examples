using System.Collections.Generic;

namespace NSwagAspNetCoreWithExamples.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Person> People { get; set; }
}