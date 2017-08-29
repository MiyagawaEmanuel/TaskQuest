using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public static MvcHtmlString ToHtmlDate(this HtmlHelper helper, DateTime dateTime)
        {
            return new MvcHtmlString(dateTime.ToString("yyyy-MM-dd"));
        }

        public static string ToJavaScriptDate(this DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd");
        }

        public static LinkViewModel LinkViewModel(this HtmlHelper helper, string id, string hash, string action, bool requireHashing=true)
        {
            return new LinkViewModel(id, hash, action, requireHashing);
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

        public static string Hash(string @string)
        {
            StringBuilder sb = new StringBuilder();
            SHA512 sha = SHA512.Create();

            byte[] entrada = Encoding.ASCII.GetBytes(@string);
            byte[] hash = sha.ComputeHash(entrada);

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("X2"));

            return sb.ToString();
        }

        public static AdicionarIntegranteViewModel AdicionarIntegranteViewModel(this HtmlHelper helper, string Id)
        {
            var aux = new AdicionarIntegranteViewModel()
            {
                GrupoId = Id
            };
            return aux;
        }

        public static bool HasQuest(this IIdentity identity, string questId)
        {
            if (identity.IsAuthenticated)
            {
                using(var db = new DbContext())
                {
                    if (db.Users.Find(identity.GetUserId<int>()).Quests.Where(q => Util.Hash(q.Id.ToString()) == questId).Any())
                        return true;

                    foreach (var grupo in db.Users.Find(identity.GetUserId<int>()).Grupos)
                        if (grupo.Quests.Where(q => Util.Hash(q.Id.ToString()) == questId).Any())
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