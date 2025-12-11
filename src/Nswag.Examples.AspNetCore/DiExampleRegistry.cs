using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NSwag.Examples.Core;

namespace Nswag.Examples.AspNetCore
{
    internal class DiExampleRegistry : IExampleRegistry
    {
        private readonly SwaggerExampleTypeMapper _typeMapper;
        private readonly IServiceProvider _serviceProvider;

        public DiExampleRegistry(SwaggerExampleTypeMapper typeMapper, IServiceProvider serviceProvider)
        {
            _typeMapper = typeMapper;
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<object> GetProviders(Type providerType)
        {
            return _serviceProvider.GetServices(providerType);
        }

        public IEnumerable<Type> GetProviderTypes(Type? valueType)
        {
            return _typeMapper.GetProviderTypes(valueType);
        }
    }
}
