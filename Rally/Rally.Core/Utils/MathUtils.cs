using System;
using System.Collections.Generic;
using System.Text;

namespace Rally.Core.Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// 判断是否为质数
        /// </summary>
        public static bool IsPrime(long num)
        {
            if (num < 2)
                return false;
            if (num == 2 || num == 3)
            {
                return true;
            }
            if (num % 6 != 1 && num % 6 != 5)
            {
                return false;
            }
            int sqr = (int)Math.Sqrt(num);
            for (int i = 5; i <= sqr; i += 6)
            {
                if (num % i == 0 || num % (i + 2) == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
