using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace HigalaApp.Utility
{
    public class UtilityProvider
    {
        public string getEncodeString(string item)
        {
            byte[] encodedBytes;

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var originalBytes = Encoding.Default.GetBytes(item);
                encodedBytes = md5.ComputeHash(originalBytes);
            }
            var stringyHash = Convert.ToBase64String(encodedBytes);

            return stringyHash;
        }
    }
}
