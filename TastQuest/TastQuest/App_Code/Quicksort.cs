using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TastQuest.Models;

namespace TastQuest.App_Code
{
    public static class Utilities
    {

        private static List<gru_grupo> lista;

        public static List<gru_grupo> Sort(List<gru_grupo> grupos)
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
            string pivo = lista[fim].gru_nome;
            int i = inicio;
            gru_grupo aux;
            for (int j = inicio; j <= fim; j++)
            {
                if (string.Compare(lista[j].gru_nome, pivo, true) < 0)
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