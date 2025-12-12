using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
using NSwag;
using NSwag.Examples.Core;

namespace Nswag.Examples.AspNetCore;

/// <summary>
/// Backwards compatibility wrapper for ExamplesConverter.
/// Automatically chooses between Newtonsoft.Json and System.Text.Json based on provided settings.
/// </summary>
[Obsolete("Use NewtonsoftExamplesConverter or SystemTextJsonExamplesConverter directly. This class will be removed in a future version.")]
public class ExamplesConverter : IExamplesConverter
{
    private readonly IExamplesConverter _implementation;

    public ExamplesConverter(JsonSerializerSettings? jsonSerializerSettings, JsonSerializerOptions? systemTextJsonSettings)
    {
        // Choose implementation based on which settings were provided
        _implementation = jsonSerializerSettings != null
            ? new NewtonsoftExamplesConverter(jsonSerializerSettings)
            : new SystemTextJsonExamplesConverter(systemTextJsonSettings);
    }

    public IDictionary<string, OpenApiExample> ToOpenApiExamplesDictionary(IEnumerable<KeyValuePair<string, Tuple<object, string?>>> examples)
    {
        return _implementation.ToOpenApiExamplesDictionary(examples);
    }
}
