using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
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
using System.Text;
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
            using (var db = new TaskQuest.Data.DbContext())
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
                using (var db = new TaskQuest.Data.DbContext())
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
                using (var db = new TaskQuest.Data.DbContext())
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
                using (var db = new TaskQuest.Data.DbContext())
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

        public static async void CreateNotificacaoAsync(IEnumerable<DbEntityEntry> entries)
        {
            using (var db = new TaskQuest.Data.DbContext())
            {
                foreach (var entry in entries)
                {
                    if (entry.State != EntityState.Unchanged)
                    {

                        if (entry.Entity is NotificacaoMetaData)
                        {

                            var notificacao = new Notificacao();
                            bool IsValid = false;

                            notificacao.TipoNotificacao = entry.State;
                            notificacao.EntidadeModificada = entry.Entity.GetType().ToString();
                            notificacao.DataNotificacao = DateTime.Now;

                            if (entry.Entity.GetType() == typeof(Grupo))
                            {
                                var grupo = ((Grupo)entry.Entity);
                                notificacao.Grupo = grupo;
                                notificacao.GrupoId = grupo.Id;
                                notificacao.Texto = "";
                                IsValid = true;
                            }
                            else if (entry.Entity.GetType() == typeof(Quest))
                            {
                                var quest = ((Quest)entry.Entity);
                                if (quest.GrupoCriador != null)
                                {
                                    notificacao.Grupo = quest.GrupoCriador;
                                    notificacao.GrupoId = quest.GrupoCriador.Id;
                                    notificacao.Texto = "";
                                    IsValid = true;
                                }
                            }
                            else if (entry.Entity.GetType() == typeof(Task))
                            {
                                var task = ((Task)entry.Entity);
                                if (task.Quest.GrupoCriador != null)
                                {
                                    notificacao.Grupo = task.Quest.GrupoCriador;
                                    notificacao.GrupoId = task.Quest.GrupoCriador.Id;
                                    notificacao.Texto = "";
                                    IsValid = true;
                                }
                            }
                            else if (entry.Entity.GetType() == typeof(Feedback))
                            {
                                var feedback = ((Feedback)entry.Entity);
                                if (feedback.Task.Quest.GrupoCriador != null)
                                {
                                    notificacao.Grupo = feedback.Task.Quest.GrupoCriador;
                                    notificacao.GrupoId = feedback.Task.Quest.GrupoCriador.Id;
                                    notificacao.Texto = "";
                                    IsValid = true;
                                }
                            }
                            if (IsValid)
                                db.Notificacao.Add(notificacao);
                        }
                    }
                }
                db.SaveWithoutThreads();
            }
        }

        public static async void CreateBackupAsync(IEnumerable<DbEntityEntry> entries)
        {
            using (var db = new TaskQuest.Data.DbContext())
            {
                foreach (var entry in entries)
                {
                    if (entry.State != EntityState.Unchanged)
                    {
                        var bkp = new Backup();

                        bkp.TableName = entry.Entity.GetType().ToString();
                        bkp.QueryType = entry.State.ToString();

                        StringBuilder data = new StringBuilder();

                        foreach (var prop in entry.Entity.GetType().GetProperties())
                        {
                            if (data.Length > 0)
                                data.Append("&");
                            data.Append(prop.Name);
                            data.Append("=");
                            data.Append(prop.GetValue(entry.Entity));
                        }

                        bkp.Data = data.ToString();
                        db.Backup.Add(bkp);
                    }
                    db.SaveWithoutThreads();
                }
            }
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