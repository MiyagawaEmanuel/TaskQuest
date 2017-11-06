using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web.Mvc;
using TaskQuest.Data;
using TaskQuest.Models;

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
                var oi = user.Grupos.Any(q => q.Id == GrupoId);
                var io = user.Claims.Any(q => q.ClaimType == GrupoId.ToString() && q.ClaimValue == "Adm");
                if (user.Claims.Any(q => q.ClaimType == GrupoId.ToString() && q.ClaimValue == "Adm") && user.Grupos.Any(q => q.Id == GrupoId))
                    return true;
                else
                    return false;
            }
        }

        public static bool HasQuest(this IIdentity identity, string questId)
        {
            int Id;
            if (identity.IsAuthenticated && Int32.TryParse(Decrypt(questId), out Id))
            {
                using (var db = new DbContext())
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

        public static bool HasQuest(this IIdentity identity, int questId)
        {
            if (identity.IsAuthenticated)
            {
                using (var db = new DbContext())
                {
                    if (db.Users.Find(identity.GetUserId<int>()).Quests.Where(q => q.Id == questId).Any())
                        return true;

                    foreach (var grupo in db.Users.Find(identity.GetUserId<int>()).Grupos)
                        if (grupo.Quests.Where(q => q.Id == questId).Any())
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

            if (string.IsNullOrEmpty(cipherText))
                return "";

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                try
                {
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
                catch (Exception) { return ""; }
            }

            return plaintext;

        }

        public static void Delete<T>(this DbContext db, Expression<Func<T, object>> prop, string where) where T : class
        {
            db.Database.ExecuteSqlCommand(string.Format(@"delete from task_quest.{0} where {1} = {2}", db.GetTableName<T>(), db.GetColumnName<T>(prop), where));
        }

        private static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).First();
        }

        private static string GetTableName<T>(this DbContext context) where T : class
        {
            string result = "";
            var itens = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace.GetItems<EntityContainerMapping>(DataSpace.CSSpace);

            foreach (EntityContainerMapping ecm in itens)
            {
                EntitySet entitySet;
                if (ecm.StoreEntityContainer.TryGetEntitySetByName(typeof(T).Name, true, out entitySet))
                    return result = entitySet.Table;
            }
            return "";
        }

        private static string GetColumnName<T>(this DbContext context, Expression<Func<T, object>> expression)
        {
            var propertyName = ((MemberExpression)((UnaryExpression)expression.Body).Operand).Member.Name;
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            // Get the part of the model that contains info about the actual CLR types
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

            // Get the entity type from the model that maps to the CLR type
            var entityType = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == typeof(T));

            // Get the entity set that uses this entity type
            var entitySet = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .Single()
                .EntitySets
                .Single(s => s.ElementType.Name == entityType.Name);

            // Find the mapping between conceptual and storage model for this entity set
            var mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                    .Single()
                    .EntitySetMappings
                    .Single(s => s.EntitySet == entitySet);

            // Find the storage property (column) that the property is mapped
            var columnName = mapping
                .EntityTypeMappings.Single()
                .Fragments.Single()
                .PropertyMappings
                .OfType<ScalarPropertyMapping>()
                .Single(m => m.Property.Name == propertyName)
                .Column
                .Name;

            return columnName;
        }
        
    }

    public class Date : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
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