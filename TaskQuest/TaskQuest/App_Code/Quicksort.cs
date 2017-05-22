using System;
using HttpContext;
using System.Reflection;
using System.Collections.Generic;

namespace TaskQuest.App_Code
{
    public static class Utilities
    {
        //The PropertyInfo of a specific property may be get through this function: 
        //T.GetType().GetProperty("")
        public static List<T> Sort<T>(List<T> list, string propertyName) where T : new()
        {
            Session["list"] = list; 
            try
            {
                PropertyInfo property = T.GetType().GetProperty(propertyName);
            }
            catch(Exception e){
                throw new System.ArgumentException("Invalid property name");
            }
            if(property == null)
                throw new System.ArgumentException("Invalid property name");
            Quicksort(0, _lista.Count - 1, property);
            list = (List<T>)Session["list"];
            HttpContext.Session.Clear();
            return list;
        }

        private static void Quicksort<T>(int inicio, int fim, PropertyInfo property) where T : new()
        {
            if (inicio < fim)
            {
                int pivo = Particionar<T>(inicio, fim, property);
                Quicksort<T>(inicio, pivo - 1, property);
                Quicksort<T>(pivo + 1, fim, property);
            }
        }

        private static int Particionar<T>(int inicio, int fim, PropertyInfo property)
        {
            string pivo = property.GetValue((List<T>)Session["list"][fim]);
            var i = inicio;
            T aux;
            for (var j = inicio; j <= fim; j++)
            {
                if (property.GetValue((List<T>)Session["list"][j] < pivo))
                {
                    aux = (List<T>)Session["list"][i];
                    (List<T>)Session["list"][i] = (List<T>)Session["list"][j];
                    (List<T>)Session["list"][j] = aux;
                    i++;
                }
            }
            aux = (List<T>)Session["list"][i];
            (List<T>)Session["list"][i] = (List<T>)Session["list"][fim];
            (List<T>)Session["list"][fim] = aux;
            return i;
        }
    }
}
