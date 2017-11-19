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
using TaskQuest.Data;
using TaskQuest.Models;
using TaskQuest.PagSeguro;
using Microsoft.AspNet.Identity;
using TaskQuest.ViewModels;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class PremiumController: Controller
    {
        private static DbContext db = new DbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PremiumPropaganda(LinkViewModel model)
        {
            int Id;
            if (int.TryParse(Util.Decrypt(model.Hash), out Id))
                return View(new Tuple<string>(model.Hash));

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Inicio", "Home");
        }

        //Criar requisição de pagamento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarAssinatura(LinkViewModel model)
        {
            int Id;
            if (int.TryParse(Util.Decrypt(model.Hash), out Id))
            {
                var grupo = db.Grupo.Find(Id);
                var user = db.Users.Find(User.Identity.GetUserId<int>());

                Assinatura a = new Assinatura()
                {
                    Email = ConfigurationManager.AppSettings["PagSeguroEmail"],
                    Token = ConfigurationManager.AppSettings["PagSeguroToken"],
                    PreApprovalDetails = "Assinatura de R$ 40,00 cobrada mensalmente pelos serviços extras prestados pela plataforma TaskQuest",
                    PreApprovalName = Util.CreateRandomString(),
                    ReceiverEmail = ConfigurationManager.AppSettings["PagSeguroEmail"],
                    Reference = Util.CreateRandomString(),
                    RedirectURL = Url.Action("PremiumAguardandoPagamento"),
                    SenderEmail = user.Email,
                    SenderName = user.Nome + " " + user.Sobrenome,

                    ReviewURL = Url.Action("ReceberNotificacao"),
                };
                var data = a.CriarAssinatura();

                Pagamento pag = new Pagamento()
                {
                    Code = data["code"],
                    Grupo = grupo,
                    Status = Status.Pendente
                };
                db.Pagamento.Add(pag);
                db.SaveChanges();

                return View(new Tuple<string>(string.Format("https://pagseguro.uol.com.br/v2/pre-approvals/request.html?code={0}", data["code"])));
            }
            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Inicio", "Home");
        }

        public ActionResult PremiumAguardandoPagamento()
        {
            return View();
        }

        [HttpPost]
        //Recebe as requisições de modificação de status do PagSeguro
        public static void ReceberNotificacao(string notificationCode, string notificationType)
        {
            var pag = db.Pagamento.Where(e => e.Code == notificationCode).FirstOrDefault();

            var IsSandBox = ConfigurationManager.AppSettings["IsSandBox"];
            var path = "";
            if (IsSandBox == "True")
                path = "https://ws.sandbox.pagseguro.uol.com.br/v2/pre-approvals/{preApprovalCode}?email={email}&token={token}";
            else
                path = "https://ws.pagseguro.uol.com.br/v2/pre-approvals/{preApprovalCode}?email={email}&token={token}";

            IDictionary<string, string> data;
            if (pag != null)
            {
                data = GetResponse(path, notificationCode);
                pag.Status = data["status"];
                db.Entry(pag).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ActionResult PremiumCancelar(LinkViewModel model)
        {
            int Id;
            if (int.TryParse(Util.Decrypt(model.Hash), out Id))
                return View(new Tuple<string>(model.Hash));

            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Inicio", "Home");
        }

        //Cancela a assinatura do plano Premium
        [HttpPost]
        public ActionResult CancelarAssinatura(LinkViewModel model)
        {
            int Id;
            if (int.TryParse(Util.Decrypt(model.Hash), out Id))
            {
                var grupo = db.Grupo.Find(Id);
                var IsSandBox = ConfigurationManager.AppSettings["IsSandBox"];
                var path = "";
                if (IsSandBox == "True")
                    path = "https://ws.sandbox.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}";
                else
                    path = "https://ws.pagseguro.uol.com.br/v2/pre-approvals/cancel/{preApprovalCode}?email={email}&token={token}";

                var pag = grupo.Pagamento;

                IDictionary<string, string> data = GetResponse(path, pag.Code);
                pag.Status = Status.Cancelada;
                db.Entry(pag).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("PremiumPropaganda");
            }
            TempData["Alerta"] = "Algo deu errado";
            TempData["Classe"] = "yellow-alert";
            return RedirectToAction("Inicio", "Home");
        }

        private static IDictionary<string, string> GetResponse(string path, string code)
        {
            path = path.Replace("{preApprovalCode}", code);
            path = path.Replace("{email}", ConfigurationManager.AppSettings["PagSeguroEmail"]);
            path = path.Replace("{token}", ConfigurationManager.AppSettings["PagSeguroToken"]);

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