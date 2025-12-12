using NSwag.Examples.Core;
using NSwag.Examples.Core.Annotations;

namespace NSwagAspNetCoreWithExamples.Models.Examples.Persons.Requests;

[ExampleAnnotation(Name = "Search text 'inspektor'", ExampleType = ExampleType.Request)]
public class PersonTextExample1 : IExampleProvider<string>
{
    public string GetExample() => "inspektor";
}