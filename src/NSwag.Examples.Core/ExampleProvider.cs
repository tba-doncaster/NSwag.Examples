using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSwag.Examples.Core;

/// <summary>
/// Provides examples using the IExampleRegistry abstraction (no DI dependency)
/// </summary>
public class ExampleProvider
{
    private readonly SwaggerExampleTypeMapper _mapper;
    private readonly IExampleRegistry _registry;

    public ExampleProvider(SwaggerExampleTypeMapper mapper, IExampleRegistry registry)
    {
        _mapper = mapper;
        _registry = registry;
    }

    public IEnumerable<KeyValuePair<string?, Tuple<object, string?>>> GetProviderValues(Type? valueType, IEnumerable<Type> exampleTypes, ExampleType exampleType)
    {
        if (valueType == null)
            yield break;

        var providerTypes = exampleTypes.Any() ? exampleTypes : _registry.GetProviderTypes(valueType);
        foreach (var providerType in providerTypes)
        {
            var providerServices = _registry.GetProviders(providerType);
            var exampleAnnotationAttribute = providerType.GetCustomAttribute<Annotations.ExampleAnnotationAttribute>();
            foreach (var example in providerServices.Select(x => ((dynamic)x).GetExample()))
            {
                if (exampleAnnotationAttribute == null || exampleAnnotationAttribute.ExampleType == exampleType || exampleAnnotationAttribute.ExampleType == ExampleType.Both)
                {
                    yield return new KeyValuePair<string?, Tuple<object, string?>>(exampleAnnotationAttribute?.Name, new(example, exampleAnnotationAttribute?.Description));
                }
            }
        }
    }
}
