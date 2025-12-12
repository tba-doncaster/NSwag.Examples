using System;
using System.Collections.Generic;
using System.Linq;

namespace NSwag.Examples.Core;

public class SwaggerExampleTypeMapper
{
    private readonly List<KeyValuePair<Type, Type>> _mapper = new List<KeyValuePair<Type, Type>>();

    public void Add(Type valueType, Type providerType) {
        _mapper.Add(new KeyValuePair<Type, Type>(valueType, providerType));
    }

    public IEnumerable<Type> GetProviderTypes(Type valueType) {
        return _mapper.Where(x => x.Key == valueType).Select(x => x.Value);
    }
}