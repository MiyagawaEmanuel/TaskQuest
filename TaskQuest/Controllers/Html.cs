using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using TaskQuest.Identity;
using TaskQuest.Models;
using TaskQuest.ViewModels;

namespace TaskQuest
{
    public static class Html
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

        public static string LinkId(this HtmlHelper helper, string @string)
        {
            return @string;
        }

        public static AdicionarIntegranteViewModel AdicionarIntegranteViewModel(this HtmlHelper helper, int Id)
        {
            var aux = new AdicionarIntegranteViewModel()
            {
                GrupoId = Id
            };
            return aux;
        }

        public static EditarUsuarioViewModel EditarUsuarioViewModel(this HtmlHelper helper, User user)
        {
            return new EditarUsuarioViewModel(user);
        }

        public static User GetApplicationUser(this System.Security.Principal.IIdentity identity)
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