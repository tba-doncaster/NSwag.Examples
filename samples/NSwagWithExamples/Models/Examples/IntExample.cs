using NSwag.Examples;

namespace NSwagAspNetCoreWithExamples.Models.Examples;

public class IntExample : IExampleProvider<int>
{
    public int GetExample() => 40;
}