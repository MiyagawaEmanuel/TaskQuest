using System;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;

namespace TaskQuest
{
    public static class Html
    {
        public static MvcHtmlString ToHtmlDate(this HtmlHelper helper, DateTime dateTime)
        {
            var aux = dateTime.ToString().Split('/');
            return new MvcHtmlString(aux[2] + "/" + aux[1] + "/" + aux[0]);
        }

        public static LinkViewModel LinkViewModel(this HtmlHelper helper, string id, string hash, string action)
        {
            return new LinkViewModel(id, hash, action);
        }

        public static Grupo Grupo(this HtmlHelper helper)
        {
            return new Grupo();
        }
    }
}