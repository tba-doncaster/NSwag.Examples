using System.Collections.Generic;
using NSwag.Examples.Core;

namespace NSwagAspNetCoreWithExamples.Models.Examples;

[ExampleAnnotation(Name = "Brno")]
public class BrnoExample : IExampleProvider<City>
{
    public City GetExample()
    {
        return new City {
            Id = 5,
            Name = "Brno",
            People = new List<Person> {
                new Person(1, "Henry", "Cavill"),
                new Person(2, "John", "Doe")
            }
        };
    }
}