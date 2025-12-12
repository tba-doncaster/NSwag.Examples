using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSwag.Examples.Core;

/// <summary>
/// Example registry that uses reflection to discover and instantiate example providers from assemblies.
/// Does not require dependency injection - creates instances directly.
/// </summary>
public class ReflectionExampleRegistry : IExampleRegistry
{
    private readonly SwaggerExampleTypeMapper _typeMapper;
    private readonly Dictionary<Type, List<object>> _providerCache;

    public ReflectionExampleRegistry(SwaggerExampleTypeMapper typeMapper)
    {
        _typeMapper = typeMapper;
        _providerCache = new Dictionary<Type, List<object>>();
    }

    /// <summary>
    /// Scan assemblies for IExampleProvider implementations and register them
    /// </summary>
    public void AddExampleProviders(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var providerType in assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IExampleProvider<>))))
            {
                // Get the value type from IExampleProvider<TValue>
                var valueType = providerType.GetTypeInfo()
                    .ImplementedInterfaces
                    .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IExampleProvider<>))
                    .GetTypeInfo()
                    .GenericTypeArguments
                    .Single();

                // Register the mapping
                _typeMapper.Add(valueType, providerType);

                // Create an instance and cache it
                var instance = Activator.CreateInstance(providerType);
                if (instance != null)
                {
                    if (!_providerCache.ContainsKey(providerType))
                    {
                        _providerCache[providerType] = new List<object>();
                    }
                    _providerCache[providerType].Add(instance);
                }
            }
        }
    }

    public IEnumerable<object> GetProviders(Type providerType)
    {
        return _providerCache.TryGetValue(providerType, out var providers)
            ? providers
            : Enumerable.Empty<object>();
    }

    public IEnumerable<Type> GetProviderTypes(Type? valueType)
    {
        return _typeMapper.GetProviderTypes(valueType);
    }
}
