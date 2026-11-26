using System.ComponentModel;

// .NET Standard 2.0 does not ship IsExternalInit, which the compiler requires
// for init-only setters and records. Provide the standard internal polyfill.
namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit
    {
    }
}
