using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.PagSeguro
{
    public static class Status
    {
        public const string Pendente = "PENDING";

        public const string Ativa = "ACTIVE";

        //A assinatura foi cancelada pelo PagSeguro ou pela operadora do cartão 
        public const string Inválida = "CANCELLED";

        public const string Cancelada = "CANCELLED_BY_RECEIVER";

        public const string CanceladaPeloComprador = "CANCELLED_BY_SENDER";

        public const string Expirada = "EXPIRED";
    }
}