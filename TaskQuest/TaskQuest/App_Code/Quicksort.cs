using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskQuest.Models;

namespace TaskQuest.App_Code
{
    public static class Utilities
    {
        private static List<Grupo> lista;

        public static List<Grupo> Sort(List<Grupo> grupos)
        {
            lista = grupos;
            Quicksort(0, lista.Count - 1);
            return lista;
        }

        public static void Quicksort(int inicio, int fim)
        {
            if (inicio < fim)
            {
                int pivo = Particionar(inicio, fim);
                Quicksort(inicio, pivo - 1);
                Quicksort(pivo + 1, fim);
            }
        }

        public static int Particionar(int inicio, int fim)
        {
            string pivo = lista[fim].Nome;
            var i = inicio;
            Grupo aux;
            for (var j = inicio; j <= fim; j++)
            {
                if (String.Compare(lista[j].Nome, pivo, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    aux = lista[i];
                    lista[i] = lista[j];
                    lista[j] = aux;
                    i++;
                }
            }
            aux = lista[i];
            lista[i] = lista[fim];
            lista[fim] = aux;
            return i;
        }
    }
}