using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSwag.Examples.Core.Generation;

/// <summary>
/// Reads response type metadata from attributes using reflection to avoid compile-time dependencies.
/// Supports:
/// - ProducesResponseTypeAttribute (ASP.NET Core) - StatusCode: int, Type: Type
/// - ResponseTypeAttribute (Web API) - StatusCode: int, Type: Type
/// - SwaggerResponseAttribute (NSwag.Annotations) - StatusCode: string, Type: Type
/// </summary>
internal static class ResponseTypeAttributeHelper
{
    public static List<IResponseTypeInfo> GetAttributesWithSameStatusCode(MemberInfo memberInfo, int responseStatusCode)
    {
        var result = new List<IResponseTypeInfo>();

        foreach (var attribute in memberInfo.GetCustomAttributes(true))
        {
            var responseType = TryExtractResponseTypeInfo(attribute, responseStatusCode);
            if (responseType != null)
            {
                result.Add(responseType);
            }
        }

        return result;
    }

    private static IResponseTypeInfo? TryExtractResponseTypeInfo(object attribute, int targetStatusCode)
    {
        var attrType = attribute.GetType();
        var statusCodeProp = attrType.GetProperty("StatusCode");

        if (statusCodeProp == null)
            return null;

        // Handle both int and string StatusCode properties
        int statusCode;
        if (statusCodeProp.PropertyType == typeof(int))
        {
            statusCode = (int)statusCodeProp.GetValue(attribute);
        }
        else if (statusCodeProp.PropertyType == typeof(string))
        {
            var statusCodeString = (string)statusCodeProp.GetValue(attribute);
            if (!int.TryParse(statusCodeString, out statusCode))
                return null;
        }
        else
        {
            return null;
        }

        if (statusCode != targetStatusCode)
            return null;

        // Extract the response type
        var typeProp = attrType.GetProperty("Type");
        var type = typeProp?.GetValue(attribute) as Type;

        return new ResponseTypeInfo(statusCode, type);
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
