using Microsoft.AspNet.Identity;
using System;
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

        public static LinkViewModel LinkViewModel(this HtmlHelper helper, string id, string hash, string action)
        {
            return new LinkViewModel(id, hash, action);
        }

        public static Grupo Grupo(this HtmlHelper helper)
        {
            return new Grupo();
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

        public static bool HasClaim(this IIdentity identity, string type, string value)
        {
            if (identity.GetApplicationUser().Claims.Any(q => q.ClaimType == type && q.ClaimValue == value))
                return true;
            else
                return false;
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

        public static AdicionarIntegranteViewModel AdicionarIntegranteViewModel(this HtmlHelper helper, int Id)
        {
            var aux = new AdicionarIntegranteViewModel()
            {
                GrupoId = Util.Hash(Id.ToString())
            };
            return aux;
        }

        public static EditarUsuarioViewModel EditarUsuarioViewModel(this HtmlHelper helper, User user)
        {
            return new EditarUsuarioViewModel(user);
        }

        public static User GetApplicationUser(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                using (var db = new DbContext())
                {
                    var userManager = new ApplicationUserManager(new UserStore(db));
                    return userManager.FindByName(identity.Name);
                }
            }
            else
            {
                return null;
            }
        }

    }
}