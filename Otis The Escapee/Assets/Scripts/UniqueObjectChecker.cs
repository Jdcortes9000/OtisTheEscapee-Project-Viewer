using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace Assets.Scripts
{
    static class UniqueObjectChecker
    {
        public static Dictionary<int, bool> worldIdentifiers = new Dictionary<int, bool>();
        public static Dictionary<int, bool> enemyIdentifiers = new Dictionary<int, bool>();
        public static Dictionary<int, bool> groundObjectIdentifiers = new Dictionary<int, bool>();
    }

    public class IdentifierTakenException : Exception
    {
        private readonly string exceptionString;

        public IdentifierTakenException(string exceptionDetails)
        {
            exceptionString = exceptionDetails;
        }

        public override string ToString()
        {
            return exceptionString;
        }
    }
}
