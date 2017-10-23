using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskQuest.Controllers
{
    public class PagSeguroController: Controller
    {

        //Criar requisição de pagamento
        public ActionResult CreatePreApproval()
        {
            return Content("");
        }

        //Recebe as requisições de modificação de status do PagSeguro
        public ActionResult ReceiveStatusChange()
        {
            return Content("");
        }

        //Busca com o PagSeguro os dados de pagamento do Usuário
        public ActionResult VerifyStatus()
        {
            return Content("");
        }

    }
}