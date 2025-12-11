using NSwag.Examples.Core;
using NSwag.Examples.Core.Annotations;

namespace NSwagAspNetCoreWithExamples.Models.Examples.Persons.Requests;

[ExampleAnnotation(Name = "Age 69", ExampleType = ExampleType.Request)]
public class PersonAge69Example : IExampleProvider<int>
{
    public int GetExample() => 69;
}