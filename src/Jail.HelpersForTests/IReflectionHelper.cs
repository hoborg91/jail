using System;
using System.Reflection;

namespace Jail.HelpersForTests {
    internal interface IReflectionHelper {
        TAbstraction New<TAbstraction>(Type type, params object[] constructorArguments);
    }
}