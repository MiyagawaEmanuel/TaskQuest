using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace TaskQuest.App_Code
{
    public static class Util
    {

        public static DateTime StringToDateTime(this string @string)
        {
            var aux = @string.Split('/');
            return new DateTime(Convert.ToInt32(aux[2]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[0]));
        }

        public static string DateTimeToString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string Hash(string s)
        {
            StringBuilder sb = new StringBuilder();
            MD5 md5 = MD5.Create();

            byte[] entrada = Encoding.ASCII.GetBytes(s);
            byte[] hash = md5.ComputeHash(entrada);

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }

    }
}