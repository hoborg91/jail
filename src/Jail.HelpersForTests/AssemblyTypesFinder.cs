using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jail.HelpersForTests {
    internal class AssemblyTypesFinder : IAssemblyTypesFinder {
        private readonly Assembly _assembly;

        public AssemblyTypesFinder(Assembly assembly) {
            this._assembly = assembly ??
                throw new ArgumentNullException(nameof(assembly));
        }

        public Type FindType(
            string typeName,
            string typeNamespace = null
        ) {
            if (typeName == null)
                throw new ArgumentNullException(nameof(typeName));
            if (string.IsNullOrWhiteSpace(typeName))
                throw new ArgumentException($"The empty \"{nameof(typeName)}\" parameter " +
                    $"is not allowed.");
            var types = this._assembly.GetTypes()
                .Where(t => true
                    && t.Name == typeName
                    && (typeNamespace == null || t.Namespace == typeNamespace)
                )
                .ToList();
            if (types.Count == 0)
                throw new Exception("Cannot find a type with " +
                    $"the given name \"{typeName}\" in the assembly \"{this._assembly}\".");
            if (types.Count > 1)
                throw new Exception($"There are {types.Count} " +
                    $"types with name \"{typeName}\" in the assembly \"{this._assembly}\" " +
                     "(in different namespaces). Cannot choose the right one. Consider " +
                     "specifying the namespace parameter.");
            var type = types.Single();
            return type;
        }

        public IReadOnlyCollection<Type> GetAllTypes() {
            return this._assembly.GetTypes();
        }
    }
}
