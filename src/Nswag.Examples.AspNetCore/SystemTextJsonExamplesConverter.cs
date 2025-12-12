using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using NSwag;
using NSwag.Examples.Core;

namespace Nswag.Examples.AspNetCore;

/// <summary>
/// Converts examples to OpenAPI format using System.Text.Json
/// </summary>
public class SystemTextJsonExamplesConverter : IExamplesConverter
{
    private readonly JsonSerializerOptions? _jsonSerializerOptions;

    public SystemTextJsonExamplesConverter(JsonSerializerOptions? jsonSerializerOptions = null)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    private object SerializeExampleJson(object value)
    {
        var serializeObject = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        return JToken.Parse(serializeObject);
    }

    public IDictionary<string, OpenApiExample> ToOpenApiExamplesDictionary(IEnumerable<KeyValuePair<string, Tuple<object, string?>>> examples)
    {
        return examples.ToDictionary(
            example => example.Key,
            example => new OpenApiExample { Value = SerializeExampleJson(example.Value.Item1), Description = example.Value.Item2 });
    }
}
