using System;
using System.Linq;

namespace Jail.Design.Internal {
    internal static class ReflectionExtensions {
        public static object GetPropertyValue(
            this Type genericTypeDefinition,
            object instance,
            string propertyName
        ) {
            var instanceType = instance.GetType();
            var implementedIface = instanceType.GetInterfaces()
                .SingleOrDefault(i => true
                    && i.IsGenericType 
                    && i.GetGenericTypeDefinition() == genericTypeDefinition
                );
            if (implementedIface == null)
                throw new Exception($"The specified type {instanceType} does not " +
                    $"impement the specified interface {genericTypeDefinition}.");
            var map = instanceType.GetInterfaceMap(implementedIface);
            var getMethod = map.TargetMethods.SingleOrDefault(x => true 
                && x.Name == "get_" + propertyName 
                && !x.GetParameters().Any()
            );
            if (getMethod == null)
                throw new Exception("Cannot find getter for specified property " +
                    $"{genericTypeDefinition}.{propertyName} in the type {instanceType}.");
            var result = getMethod.Invoke(instance, null);
            return result;
        }
    }
}
