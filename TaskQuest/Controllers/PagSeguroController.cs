using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using TaskQuest.PagSeguro;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class PagSeguroController: Controller
    {

        //Criar requisição de pagamento
        public ActionResult CriarAssinatura()
        {
            Assinatura a = new Assinatura()
            {
                Email = "taskquest01@gmail.com",
                Token = "85E43107188E4237B2E284181B9E673E",
                PreApprovalDetails = "Todo certo dia será cobrado certo valor",
                PreApprovalName = "Teste",
                ReceiverEmail = "teste@mail.com",
                Reference = "Deu certo?",
                RedirectURL = "https://google.com",
                SenderEmail = "comprador@uol.com.br",
                SenderName = "Joao Comprador",
                SenderPhone = "56273440",
                SenderAreaCode = "11",
                ReviewURL = "https://google.com",
            };
            var data = a.CriarAssinatura();
            return Content(data.DictToString());
            //return Content(string.Format("https://pagseguro.uol.com.br/v2/pre-approvals/request.html?code={0}", data["code"]));
        }

        [HttpPost]
        //Recebe as requisições de modificação de status do PagSeguro
        public ActionResult ReceberNotificacao(string notificationCode, string notificationType)
        {
            //TODO: fazer o get no bando dos dados

            var IsSandBox = ConfigurationManager.AppSettings["IsSandBox"];
            var path = "";
            if (IsSandBox == "True")
                path = "https://ws.sandbox.pagseguro.uol.com.br/v2/pre-approvals/{preApprovalCode}?email={email}&token={token}";
            else
                path = "https://ws.pagseguro.uol.com.br/v2/pre-approvals/{preApprovalCode}?email={email}&token={token}";

            //if (notificationCode exists in db)
            IDictionary<string, string> data = GetResponse(path, notificationCode);

            return Content(data.DictToString());
        }

        //Cancela a assinatura do plano Premium
        public ActionResult CancelarAssinatura()
        {
            //TODO: fazer o get no bando dos dados

            var IsSandBox = ConfigurationManager.AppSettings["IsSandBox"];
            var path = "";
            if (IsSandBox == "True")
                path = "https://ws.sandbox.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}";
            else
                path = "https://ws.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}";

            string code = "";
            IDictionary<string, string> data = GetResponse(path, code);

            return Content(data.DictToString());
        }

        private IDictionary<string, string> GetResponse(string path, string code)
        {
            path = path.Replace("{preApprovalCode}", code);
            path = path.Replace("{email}", "taskquest01@gmail.com");
            path = path.Replace("{token}", "85E43107188E4237B2E284181B9E673E");

            try
            {
                var response = Service.Request(urlPath: path, query: null, method: Service.Get);
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                    return Service.ReadXml(XDocument.Load(response.GetResponseStream()));
            }
            catch (WebException ex)
            {
                using (var reader = XmlReader.Create(ex.Response.GetResponseStream()))
                    return Service.ReadXml(XDocument.Load(ex.Response.GetResponseStream()));
            }
            return null;
        }

    }
}