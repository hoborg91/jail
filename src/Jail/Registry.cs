using System.Runtime.CompilerServices;

#if DEBUG
    [assembly: InternalsVisibleTo("Jail.Tests")]
    // Required for Moq.
    [assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif
