using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.Models
{
    public class Pagamento
    {

        public int Id { get; set; }

        public string Code { get; set; }

        public string Status { get; set; }

        public virtual Grupo Grupo { get; set; }

    }
}