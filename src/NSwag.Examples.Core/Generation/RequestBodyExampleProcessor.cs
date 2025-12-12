using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSwag;
using NSwag.Examples.Core.Annotations;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace NSwag.Examples.Core.Generation;

public class RequestBodyExampleProcessor : IOperationProcessor
{
    private const string MediaTypeName = "application/json";
    private readonly IExampleLogger? _logger;
    private readonly ExampleProvider _exampleProvider;
    private readonly IExamplesConverter _examplesConverter;

    public RequestBodyExampleProcessor(ExampleProvider exampleProvider, IExamplesConverter examplesConverter, IExampleLogger? logger = null) {
        _exampleProvider = exampleProvider;
        _examplesConverter = examplesConverter;
        _logger = logger ?? NullExampleLogger.Instance;
    }

    public bool Process(OperationProcessorContext context) {
        SetRequestExamples(context, _exampleProvider);
        SetResponseExamples(context, _exampleProvider);

        return true;
    }

    private void SetRequestExamples(OperationProcessorContext context, ExampleProvider exampleProvider) {
        // Filter body parameters, but safely handle IsBinaryBodyParameter which may throw in Web API
        var bodyParameters = new List<OpenApiParameter>();
        foreach (var param in context.OperationDescription.Operation.Parameters.Where(x => x.Kind == OpenApiParameterKind.Body))
        {
            try {
                if (!param.IsBinaryBodyParameter)
                {
                    bodyParameters.Add(param);
                }
            } catch {
                // IsBinaryBodyParameter may fail in Web API if ActualConsumesCollection is null
                // Assume it's not a binary parameter and include it
                bodyParameters.Add(param);
            }
        }

        foreach (var apiParameter in bodyParameters) {
            var parameter = context.Parameters.SingleOrDefault(x => x.Value.Name == apiParameter.Name);

            // Check if RequestBody exists (may be null in Web API)
            if (context.OperationDescription.Operation.RequestBody == null)
                continue;

            if (!context.OperationDescription.Operation.RequestBody.Content.TryGetValue(MediaTypeName, out var mediaType))
                continue;

            var endpointSpecificExampleAttributes = context.MethodInfo.GetCustomAttributes<EndpointSpecificExampleAttribute>();
            SetExamples(
                GetExamples(
                    exampleProvider, 
                    parameter.Key.ParameterType, 
                    endpointSpecificExampleAttributes
                        .Where(x => x.ExampleType == ExampleType.Request || x.ExampleType == ExampleType.Both)
                        .SelectMany(x => x.ExampleTypes), ExampleType.Request
                    ), 
                mediaType
                );
        }
    }

    private void SetResponseExamples(OperationProcessorContext context, ExampleProvider exampleProvider) {
        foreach (var response in context.OperationDescription.Operation.Responses) {
            if (!int.TryParse(response.Key, out var responseStatusCode))
                continue;

            if (!response.Value.Content.TryGetValue(MediaTypeName, out var mediaType))
                continue;

            var attributesWithSameKey = ResponseTypeAttributeHelper.GetAttributesWithSameStatusCode(context.MethodInfo, responseStatusCode);

            //get attributes from controller, in case when no attribute on action was found
            if (!attributesWithSameKey.Any())
                attributesWithSameKey = ResponseTypeAttributeHelper.GetAttributesWithSameStatusCode(context.MethodInfo.DeclaringType, responseStatusCode);

            if (attributesWithSameKey.Count > 1)
                _logger.LogWarning($"Multiple response type attributes defined for method {context.MethodInfo.Name}, selecting first.");
            else if (attributesWithSameKey.Count == 0)
                continue;

            var endpointSpecificExampleAttributes = context.MethodInfo.GetCustomAttributes<EndpointSpecificExampleAttribute>();
            var valueType = attributesWithSameKey.FirstOrDefault()?.Type;
            SetExamples(GetExamples(exampleProvider, valueType, endpointSpecificExampleAttributes
                .Where(x => x.ExampleType == ExampleType.Response || x.ExampleType == ExampleType.Both)
                .Where(x => x.ResponseStatusCode != 0 && x.ResponseStatusCode == responseStatusCode)
                .SelectMany(x => x.ExampleTypes), ExampleType.Response), mediaType);
        }
    }

    private static void SetExamples(IDictionary<string, OpenApiExample> openApiExamples, OpenApiMediaType mediaType) {
        switch (openApiExamples) {
            case { Count: > 1 }:
            {
                foreach (var openApiExample in openApiExamples)
                    mediaType.Examples.Add(openApiExample.Key, openApiExample.Value);

                break;
            }
            case { Count: 1 }:
                mediaType.Example = openApiExamples.Single().Value.Value;
                break;
        }
    }

    private IDictionary<string, OpenApiExample> GetExamples(ExampleProvider exampleProvider, Type? valueType, IEnumerable<Type> exampleTypes, ExampleType exampleType) {
        var providerValues = exampleProvider.GetProviderValues(valueType, exampleTypes, exampleType);
        var openApiExamples = _examplesConverter.ToOpenApiExamplesDictionary(providerValues.Select((x, i) => new KeyValuePair<string, Tuple<object, string?>>(x.Key ?? $"Example {i + 1}", x.Value)));
        return openApiExamples;
    }

}