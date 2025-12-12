using NSwag.Examples.Core;

namespace NSwagAspNetCoreWithExamples.Models.Examples;

public class CustomInternalErrorExample : IExampleProvider<CustomInternalError>
{
    public CustomInternalError GetExample()
    {
        return new CustomInternalError { Reason = "Very serious problem occurred", Severity = 100 };
    }
}