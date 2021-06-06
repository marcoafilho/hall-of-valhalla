using System;
using System.Text;

namespace HallOfValhalla.Helpers.Digest
{
    public static class MD5
    {
        public static string Hexdigest(string value)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
            byte[] hash = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);

            StringBuilder result = new();
            foreach (byte b in hash)
            {
                result.Append(b.ToString("X2").ToLower());
            }
            return result.ToString();
        }
    }
}
