using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.Models.ViewModels;
using TaskQuest.ViewModels;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class QuestController : Controller
    {

        private DbContext db = new DbContext();
        
        public ActionResult CriarQuest()
        {
            return View("CriarQuest", null);
        }

        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CriarQuest(LinkViewModel model)
        {
            return View("CriarQuest", Convert.ToInt32(model.Hash));
        }

        [HttpPost]
        public string CriarQuestPost(QuestViewModel model)
        {
            try
            {
                var quest = new Quest()
                {
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    Cor = model.Cor,
                    DataCriacao = DateTime.Now
                };

                foreach (var tsk in model.Tasks)
                    quest.Tasks.Add(tsk);

                if (model.GrupoCriadorId == null)
                    quest.UsuarioCriadorId = User.Identity.GetUserId<int>();
                else
                    quest.GrupoCriadorId = model.GrupoCriadorId;

                db.Quest.Add(quest);
                db.SaveChanges();

                TempData["Alert"] = "Criado com sucesso";
                TempData["Class"] = "green-alert";

                return "true";
            }
            catch
            {
                return "false";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LinkViewModel model)
        {

            var quest = db.Quest.Find(Convert.ToInt32(model.Hash));
            int user_id = User.Identity.GetUserId<int>();

            if (quest.UsuarioCriadorId == user_id || quest.GrupoCriador.UsuarioGrupos.Where(q => q.UsuarioId == user_id && q.Administrador) != null)
            {
                return View("QuestAdm", quest.Id);
            }

            return View("Quest", quest.Id);
        }

        [HttpPost]
        public JsonResult GetQuests(string Hash)
        {
            var quest = db.Quest.Find(Convert.ToInt32(Hash));

            Debug.WriteLine(quest.Tasks.ToList()[0].GetType().ToString());

            var json = new {
                Nome = quest.Nome,
                Descricao = quest.Descricao,
                Cor = quest.Cor,
                Tasks = quest.Tasks
            };
            
            return Json(json);
        }

    }
}