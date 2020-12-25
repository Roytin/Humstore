using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AntX.Utils
{
    public static class SHAUtils
    {
        public static string Sha1(this string str)
        {
            byte[] StrRes = Encoding.Default.GetBytes(str);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder enstr = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                enstr.AppendFormat("{0:x2}", iByte);
            }
            return enstr.ToString();
        }
    }
}
