using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static System.Web.HttpContext;

namespace TaskQuest.App_Code
{
    public static class Util
    {
        
        //compare -> Comparer<T>.Create((x, y) => typeof(Atributo).Compare(x.Atributo, y.Atributo))
        public static List<T> Sort<T>(List<T> list, Comparer<T> compare) where T : new()
        {
            Current.Session["list"] = list;
            Current.Session["compare"] = compare;

            Quicksort<T>(0, ((List<T>)Current.Session["list"]).Count - 1);

            list = ((List<T>)Current.Session["list"]);

            Current.Session["list"] = "";
            Current.Session["compare"] = "";

            return list;
        }

        private static void Quicksort<T>(int inicio, int fim) where T : new()
        {
            if (inicio < fim)
            {
                int pivo = Particionar<T>(inicio, fim);
                Quicksort<T>(inicio, pivo - 1);
                Quicksort<T>(pivo + 1, fim);
            }
        }

        private static int Particionar<T>(int inicio, int fim) where T : new()
        {
            T pivo = ((List<T>)Current.Session["list"])[fim];
            var i = inicio;
            T aux;
            for (var j = inicio; j <= fim; j++)
            {
                //list[j] < pivo
                if (((Comparer<T>)Current.Session["compare"]).Compare(((List<T>)Current.Session["list"])[j], pivo) < 0)
                {
                    aux = ((List<T>)Current.Session["list"])[i];
                    ((List<T>)Current.Session["list"])[i] = ((List<T>)Current.Session["list"])[j];
                    ((List<T>)Current.Session["list"])[j] = aux;
                    i++;
                }
            }
            aux = ((List<T>)Current.Session["list"])[i];
            ((List<T>)Current.Session["list"])[i] = ((List<T>)Current.Session["list"])[fim];
            ((List<T>)Current.Session["list"])[fim] = aux;
            return i;
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