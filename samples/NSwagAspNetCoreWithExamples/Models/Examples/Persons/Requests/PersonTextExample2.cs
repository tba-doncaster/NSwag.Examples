using NSwag.Examples.Core;
using NSwag.Examples.Core.Annotations;

namespace NSwagAspNetCoreWithExamples.Models.Examples.Persons.Requests;

[ExampleAnnotation(Name = "Search text 'podez'", ExampleType = ExampleType.Request)]
public class PersonTextExample2 : IExampleProvider<string>
{
    public string GetExample() => "podez";
}