using System;
using System.Collections.Generic;

namespace Jail.HelpersForTests {
    internal interface IAssemblyTypesFinder {
        Type FindType(
            string typeName,
            string typeNamespace = null
        );

        IReadOnlyCollection<Type> GetAllTypes();
    }
}
