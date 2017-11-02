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
    public class PagSeguroController: Controller
    {

        //Criar requisição de pagamento
        public ActionResult CriarAsssinatura()
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

        //Recebe as requisições de modificação de status do PagSeguro
        public ActionResult ReceberAlteracao()
        {
            return Content("");
        }

        //Cancela a assinatura do plano Premium
        public ActionResult CancelarAssinatura()
        {
            //TODO: fazer o get no bando dos dados

            //https://ws.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}

            var IsSandBox = ConfigurationManager.AppSettings["IsSandBox"];
            var path = "";
            if (IsSandBox == "True")
                path = "https://ws.sandbox.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}";
            else
                path = "https://ws.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}";

            path = path.Replace("{preApprovalCode}", "38399D4DD5D5FE7994610FBC1F6E7544");
            path = path.Replace("{email}", "taskquest01@gmail.com");
            path = path.Replace("{token}", "85E43107188E4237B2E284181B9E673E");

            IDictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                var response = Service.Request(urlPath: path, query: null, method: Service.Get);
                if (HttpStatusCode.OK.Equals(response.StatusCode))
                    data = Service.ReadXml(XDocument.Load(response.GetResponseStream()));
            }
            catch (WebException ex)
            {
                using (var reader = XmlReader.Create(ex.Response.GetResponseStream()))
                    data = Service.ReadXml(XDocument.Load(ex.Response.GetResponseStream()));
            }

            return Content(data.DictToString());
        }

    }
}