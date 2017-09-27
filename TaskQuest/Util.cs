using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using TaskQuest.Identity;
using TaskQuest.Models;
using TaskQuest.ViewModels;

namespace TaskQuest
{
    public static class Util
    {

        private static byte[] IV { get { return StringToByte(ConfigurationManager.AppSettings["AesIV"]); } }
        private static byte[] Key { get { return StringToByte(ConfigurationManager.AppSettings["AesKey"]); } }

        public static string ToHtmlDate(this HtmlHelper helper, DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToHtmlDate(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        public static string ToString(this HtmlHelper helper, string @string)
        {
            return @string;
        }

        public static DateTime StringToDateTime(this string @string)
        {
            var aux = @string.Split('-');
            return new DateTime(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]));
        }

        public static bool IsAdm(this IIdentity identity, int GrupoId)
        {
            using (var db = new DbContext())
            {
                var user = db.Users.Find(identity.GetUserId<int>());
                if (user.Claims.Any(q => q.ClaimType == GrupoId.ToString() && q.ClaimValue == "Adm") && user.Grupos.Any(q => q.Id == GrupoId))
                    return true;
                else
                    return false;
            }
        }

        /*
        public static string Hash(string @string)
        {
            StringBuilder sb = new StringBuilder();
            SHA512 sha = SHA512.Create();

            byte[] entrada = Encoding.ASCII.GetBytes(@string + ConfigurationManager.AppSettings["Salt"]);
            byte[] hash = sha.ComputeHash(entrada);

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }
        */

        public static bool HasQuest(this IIdentity identity, string questId)
        {
            int Id;
            if (identity.IsAuthenticated && Int32.TryParse(Decrypt(questId), out Id))
            {
                using(var db = new DbContext())
                {
                    if (db.Users.Find(identity.GetUserId<int>()).Quests.Where(q => q.Id == Id).Any())
                        return true;

                    foreach (var grupo in db.Users.Find(identity.GetUserId<int>()).Grupos)
                        if (grupo.Quests.Where(q => q.Id == Id).Any())
                            return true;
                }
            }
            return false;
        }

        public static string GetCor(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                using (var db = new DbContext())
                {
                    return db.Users.Find(identity.GetUserId<int>()).Cor;
                }
            }
            else
            {
                return null;
            }
        }


        private static byte[] StringToByte(string stringToConvert)
        {
            byte[] key = new byte[16];
            for (int i = 0; i < 16; i += 2)
            {
                byte[] unicodeBytes = BitConverter.GetBytes(stringToConvert[i % stringToConvert.Length]);
                Array.Copy(unicodeBytes, 0, key, i, 2);
            }
            return key;
        }

        public static string Encrypt(string plainText)
        {

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);

        }

        public static string Decrypt(string cipherText)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

    }
    
    public class Date : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null)
            {
                try
                {
                    var n = value.ToString().Split('-').Select(q => Convert.ToInt32(q)).ToList();
                    DateTime date = new DateTime(n[0], n[1], n[2]);
                    return ValidationResult.Success;
                }
                catch
                {
                    return new ValidationResult("Digite uma data válida");
                }
            }
            else
                return new ValidationResult("Digite uma data válida");
        }
    }
}