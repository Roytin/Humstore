using System;
using System.Collections.Generic;
using System.Text;

namespace Rally.Core
{
    public static class TypeUtils
    {
        public static bool IsActor(this Type type)
        {
            return type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Actor));
        }
    }
}
