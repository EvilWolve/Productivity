#if USE_ASSERTIONS
using UnityEngine.Assertions;
#else
using UnityEngine;
#endif

namespace framework.errorhandling
{
    public static class ErrorHandling
    {
        public static void AssertIsTrue(bool condition, string message)
        {
#if USE_ASSERTIONS
            Assert.IsTrue (condition, message);
#else
            if (condition == false)
            {
                Debug.LogError (message);
            }
#endif
        }

        public static void AssertIsFalse(bool condition, string message)
        {
            AssertIsTrue (!condition, message);
        }

        public static void AssertIsNotNull(object obj, string message)
        {
            AssertIsTrue (obj != null, message);
        }

        public static void AssertIsNull(object obj, string message)
        {
            AssertIsTrue (obj == null, message);
        }
    }
}