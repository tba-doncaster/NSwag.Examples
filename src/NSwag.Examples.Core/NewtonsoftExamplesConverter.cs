using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSwag.Examples.Core;

/// <summary>
/// Converts examples to OpenAPI format using Newtonsoft.Json
/// </summary>
public class NewtonsoftExamplesConverter : IExamplesConverter
{
    private readonly JsonSerializerSettings? _jsonSerializerSettings;

    public NewtonsoftExamplesConverter(JsonSerializerSettings? jsonSerializerSettings = null) {
        _jsonSerializerSettings = jsonSerializerSettings;
    }

    private object SerializeExampleJson(object value) {
        var serializeObject = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
        return JToken.Parse(serializeObject);
    }

    public IDictionary<string, OpenApiExample> ToOpenApiExamplesDictionary(IEnumerable<KeyValuePair<string, Tuple<object, string?>>> examples) {
        return examples.ToDictionary(
            example => example.Key,
            example => new OpenApiExample { Value = SerializeExampleJson(example.Value.Item1), Description = example.Value.Item2 });
    }
}