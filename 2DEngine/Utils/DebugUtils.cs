using System.Diagnostics;

namespace _2DEngine
{
    public static class DebugUtils
    {
        public static void AssertNotNull(object nullableObject)
        {
            Debug.Assert(nullableObject != null);
        }
    }
}
