using System;
using System.Collections.Generic;

namespace NSwag.Examples.Core
{
    /// <summary>
    /// Registry for discovering and retrieving example providers
    /// </summary>
    public interface IExampleRegistry
    {
        /// <summary>
        /// Get all provider instances for the specified provider type
        /// </summary>
        IEnumerable<object> GetProviders(Type providerType);

        /// <summary>
        /// Get all provider types that can provide examples for the specified value type
        /// </summary>
        IEnumerable<Type> GetProviderTypes(Type? valueType);
    }
}
