using System;

namespace Jail.Design.Annotations {
    /// <summary>This attribute can be used to explicitly 
    /// express the nullability of the value.</summary>
    /// <remarks>It is assumed that the absence of this 
    /// attribute means that the value cannot be null 
    /// (however an exception still can be thrown on 
    /// value calculation).</remarks>
    [AttributeUsage(
        AttributeTargets.GenericParameter |
        AttributeTargets.Parameter |
        AttributeTargets.Property |
        AttributeTargets.Field
    )]
    public sealed class CanBeNullAttribute : Attribute {
    }
}
