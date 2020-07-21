using System;

namespace Shared.ECS
{
    [AttributeUsage(System.AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class ComponentAttribute : Attribute { }
}