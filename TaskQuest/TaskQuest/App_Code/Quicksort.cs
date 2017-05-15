using System;
using System.Collections.Generic;

namespace TaskQuest.App_Code
{
    public static class Utilities
    {
        private static List<gru_grupo> _lista;

        public static List<gru_grupo> Sort(List<gru_grupo> grupos)
        {
            _lista = grupos;
            Quicksort(0, _lista.Count - 1);
            return _lista;
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
            string pivo = _lista[fim].gru_nome;
            var i = inicio;
            gru_grupo aux;
            for (var j = inicio; j <= fim; j++)
            {
                if (String.Compare(_lista[j].gru_nome, pivo, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    aux = _lista[i];
                    _lista[i] = _lista[j];
                    _lista[j] = aux;
                    i++;
                }
            }
            aux = _lista[i];
            _lista[i] = _lista[fim];
            _lista[fim] = aux;
            return i;
        }
    }
}