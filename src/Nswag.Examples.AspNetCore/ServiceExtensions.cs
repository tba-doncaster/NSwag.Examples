using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.Examples.Core;
using NSwag.Examples.Core.Generation;
using NSwag.Generation.AspNetCore;

namespace Nswag.Examples.AspNetCore;

public static class ServiceExtensions
{
    public static void AddExamples(this AspNetCoreOpenApiDocumentGeneratorSettings settings, IServiceProvider serviceProvider) {
        // Get dependencies from DI
        var typeMapper = serviceProvider.GetRequiredService<SwaggerExampleTypeMapper>();
        var logger = serviceProvider.GetRequiredService<ILogger<RequestBodyExampleProcessor>>();

        // Create adapters for DI-based services
        var exampleLogger = new MicrosoftExampleLogger(logger);
        var registry = new DiExampleRegistry(typeMapper, serviceProvider);
        var exampleProvider = new ExampleProvider(typeMapper, registry);

        // Create ExamplesConverter with ASP.NET Core JSON settings
        var examplesConverter = new ExamplesConverter(
            AspNetCoreOpenApiDocumentGenerator.GetJsonSerializerSettings(serviceProvider),
            AspNetCoreOpenApiDocumentGenerator.GetSystemTextJsonSettings(serviceProvider));

        // Register the Core processors with the logger abstraction
        settings.OperationProcessors.Add(new RequestBodyExampleProcessor(exampleProvider, examplesConverter, exampleLogger));
        settings.OperationProcessors.Add(new RequestExampleProcessor(exampleProvider, examplesConverter));
    }

    public static void AddExampleProviders(this IServiceCollection collection, params Assembly[] assemblies) {
        var typeMapper = new SwaggerExampleTypeMapper();
        foreach (var assembly in assemblies) {
            foreach (var providerType in assembly.GetTypes().Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IExampleProvider<>)))) {
                var valueType = providerType.GetTypeInfo()
                    .ImplementedInterfaces
                    .Single(x => x.GetGenericTypeDefinition() == typeof(IExampleProvider<>))
                    .GetTypeInfo()
                    .GenericTypeArguments
                    .Single();

                typeMapper.Add(valueType, providerType);
                collection.AddSingleton(providerType);
                collection.AddSingleton(typeof(IExampleProvider<>).MakeGenericType(valueType), providerType);
            }
        }

        collection.AddSingleton(typeMapper);
    }
}