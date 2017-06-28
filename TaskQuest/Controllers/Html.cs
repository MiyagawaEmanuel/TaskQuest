using System;
using System.Web.Mvc;

namespace TaskQuest
{
    public static class Html
    {
        public static MvcHtmlString ToHtmlDate(this HtmlHelper helper, DateTime dateTime)
        {
            var aux = dateTime.ToString().Split('/');
            return new MvcHtmlString(aux[2] + "/" + aux[1] + "/" + aux[0]);
        }
    }
}