using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSwag.Examples.Core.Generation;

/// <summary>
/// Helper to work with response type attributes from different frameworks
/// (ProducesResponseTypeAttribute from ASP.NET Core or ResponseTypeAttribute from Web API)
/// </summary>
internal static class ResponseTypeAttributeHelper
{
    public static List<IResponseTypeInfo> GetAttributesWithSameStatusCode(MemberInfo memberInfo, int responseStatusCode)
    {
        var result = new List<IResponseTypeInfo>();

        foreach (var attribute in memberInfo.GetCustomAttributes(true))
        {
            var attrType = attribute.GetType();

            // Check for StatusCode property (both ProducesResponseTypeAttribute and ResponseTypeAttribute have this)
            var statusCodeProp = attrType.GetProperty("StatusCode");
            if (statusCodeProp == null || statusCodeProp.PropertyType != typeof(int))
                continue;

            var statusCode = (int)statusCodeProp.GetValue(attribute);
            if (statusCode != responseStatusCode)
                continue;

            // Get the Type property (both attributes have this)
            var typeProp = attrType.GetProperty("Type");
            var type = typeProp?.GetValue(attribute) as Type;

            result.Add(new ResponseTypeInfo(statusCode, type));
        }

        return result;
    }

    private class ResponseTypeInfo : IResponseTypeInfo
    {
        public ResponseTypeInfo(int statusCode, Type? type)
        {
            StatusCode = statusCode;
            Type = type;
        }

        public int StatusCode { get; }
        public Type? Type { get; }
    }
}

internal interface IResponseTypeInfo
{
    int StatusCode { get; }
    Type? Type { get; }
}
