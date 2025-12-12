using System;

namespace NSwag.Examples.Core.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ExampleAnnotationAttribute : Attribute
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public ExampleType ExampleType { get; set; } = ExampleType.Both;
}