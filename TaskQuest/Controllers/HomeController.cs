using System.Collections.Generic;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using Teste;
using TaskQuest.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private DbContext db = new DbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Index(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }
            return View();
        }

        public ActionResult Inicio()
        {

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new InicioViewModel();

            foreach (var uxg in user.UsuarioGrupos)
                foreach (var gru in db.Grupo.Where(q => q.Id == uxg.GrupoId).ToArray())
                    model.Grupos.Add(gru);

            foreach (var gru in model.Grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        if (tsk.Status != 2)
                            model.Pendencias.Add(tsk);

            foreach (var gru in model.Grupos)
                foreach (var qst in gru.Quests)
                    foreach (var tsk in qst.Tasks)
                        if (tsk.Status != 2)
                            model.Pendencias.Add(tsk);

            foreach (var tsk in model.Pendencias)
                foreach (var feb in tsk.Feedbacks)
                    model.Feedbacks.Add(feb);

            return View(model);
        }

        public ActionResult Configuracao()
        {
            var model = new ConfiguracaoViewModel();
            model.usuario = db.Users.Find(User.Identity.GetUserId<int>());
            model.Cartoes = model.usuario.Cartoes.ToList();
            model.Telefones = model.usuario.Telefones.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Configuracao(ConfiguracaoViewModel model)
        {

            if(User.Identity.GetUserId<int>() == model.usuario.Id)
            {
                db.Entry(model.usuario).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Response"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }
            
            return RedirectToAction("Configuracao");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTelefone(Telefone model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Response"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Configuracao");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCartao(Cartao model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["Response"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }
            return RedirectToAction("Configurar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirTelefone(ButtonViewModel model)
        {
            var telefone = db.Telefone.Find(model.Id);
            if (User.Identity.GetUserId<int>() == telefone.UsuarioId)
            {
                db.Entry(telefone).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Response"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Configurar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirCartao(ButtonViewModel model)
        {
            var cartao = db.Cartao.Find(model.Id);
            if (User.Identity.GetUserId<int>() == cartao.UsuarioId)
            {
                db.Entry(cartao).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Response"] = "Atualizado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Configurar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarTelefone(Telefone model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Telefone.Add(model);
                db.SaveChanges();
                TempData["Response"] = "Adicionado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Configurar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCartao(Cartao model)
        {
            if (User.Identity.GetUserId<int>() == model.UsuarioId)
            {
                db.Cartao.Add(model);
                db.SaveChanges();
                TempData["Response"] = "Adicionado com sucesso";
                TempData["Class"] = "green-alert";
            }
            else
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }

            return RedirectToAction("Usuario");
        }
        
        public ActionResult Grupos()
        {
            List<Grupo> model = new List<Grupo>();
            foreach (var uxg in db.Users.Find(User.Identity.GetUserId()).UsuarioGrupos)
                model.Add(db.Grupo.Find(uxg.GrupoId));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarGrupo(Grupo model)
        {
            db.Grupo.Add(model);
            db.SaveChanges();

            UsuarioGrupo uxg = new UsuarioGrupo()
            {
                UsuarioId = User.Identity.GetUserId<int>(),
                GrupoId = model.Id
            };
            db.UsuarioGrupo.Add(uxg);
            db.SaveChanges();

            return RedirectToAction("Grupo", "Home", new ButtonViewModel(model.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grupo(ButtonViewModel model) 
        {

            Grupo grupo = db.Grupo.Find(model.Id);
            UsuarioGrupo usuariogrupo;

            try
            {
                usuariogrupo = grupo.UsuarioGrupos.Where(q => q.UsuarioId == User.Identity.GetUserId<int>()).First();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Voce nao pertence a esse grupo";
                return RedirectToAction("Grupos");
            }
            
            GrupoViewModel grupoViewModel = new GrupoViewModel();

            grupoViewModel.Grupo = grupo;

            foreach (var uxg in grupo.UsuarioGrupos)
                grupoViewModel.Integrantes.Add(uxg.Usuario);

            if (usuariogrupo.Administrador)
                return View("GrupoAdmin", model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGrupo(GrupoViewModel model)
        {

            if(db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.Grupo.Id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Você não pode executar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            db.Entry(model.Grupo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Grupo", new ButtonViewModel(model.Grupo.Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarUsuarioGrupo(AdicionarUsuarioGrupoViewModel model) 
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.gru_id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Você não pode executar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                User usuario = db.Users.Where(q => q.Email == model.Email).First();
                UsuarioGrupo usuarioGrupo = new UsuarioGrupo()
                {
                    UsuarioId = usuario.Id,
                    GrupoId = model.gru_id,
                    Administrador = false
                };
                db.UsuarioGrupo.Add(usuarioGrupo);
                db.SaveChanges();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Usuário não encontrado";
            }

            return RedirectToAction("Grupo", new ButtonViewModel(model.gru_id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirIntegranteGrupo(EditarIntegranteViewModel model) //V
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.gru_id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Você não pode executar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                db.Entry(db.UsuarioGrupo.Where(q => q.UsuarioId == model.usu_id && q.GrupoId == model.gru_id).First())
                    .State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Usuário não encontrado";
            }

            return RedirectToAction("Grupo", new ButtonViewModel(model.gru_id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TornarAdministrador(EditarIntegranteViewModel model)
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.gru_id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Você não pode executar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                var uxg = db.UsuarioGrupo.Where(q => q.UsuarioId == model.usu_id && q.GrupoId == model.gru_id).First();
                uxg.Administrador = true;
                db.Entry(uxg).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Algo deu errado";
            }

            return RedirectToAction("Grupo", new ButtonViewModel(model.gru_id));
        }

        public ActionResult ExcluirGrupo(ButtonViewModel model)
        {

            if (db.UsuarioGrupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>() && q.GrupoId == model.Id).First().Administrador)
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Você não pode executar essa ação";
                return RedirectToAction("Inicio", "Home");
            }

            try
            {
                db.Entry(db.Grupo.Find(model.Id)).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                TempData["Class"] = "yellow-alert";
                TempData["Result"] = "Algo deu errado";
            }

            return RedirectToAction("Grupos", "Home");

        }

    }
}
