using System;
using System.Collections.Generic;

namespace NSwag.Examples.Core;

/// <summary>
/// Converts example objects to OpenAPI example format
/// </summary>
public interface IExamplesConverter
{
    /// <summary>
    /// Convert example objects to OpenAPI examples dictionary
    /// </summary>
    IDictionary<string, OpenApiExample> ToOpenApiExamplesDictionary(IEnumerable<KeyValuePair<string, Tuple<object, string?>>> examples);
}
