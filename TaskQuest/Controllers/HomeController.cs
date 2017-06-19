using System;
using System.Collections.Generic;
using TaskQuest.App_Code;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Diagnostics;
using System.Linq;
using Teste;
using TaskQuest.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace TaskQuest.Controllers
{
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
        public ActionResult Index(string returnUrl=null)
        {
            if(returnUrl != null)
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }
            return View();
        }

        [Authorize]
        public ActionResult Inicio()
        {

            var model = new InicioViewModel();

            foreach (var uxg in user.UsuarioGrupos)
                foreach (var gru in db.Grupos.Where(q => q.Id == uxg.GrupoId).ToArray())
                    model.Grupos.Add(gru);

            foreach (var qst in db.Grupo.Where(q => q.UsuarioId == User.Identity.GetUserId<int>()).ToList())
                foreach (var tsk in qst.Tasks)
                    if (tsk.Status != 2)
                        model.Task.Add(tsk);
                    
            foreach (var gru in model.Grupos)
                foreach (var qst in db.Grupo.Where(q => q.GrupoId == gru.Id).ToList())
                    foreach (var tsk in qst.Tasks)
                        if (tsk.Status != 2)
                            model.Task.Add(tsk);
                        
            foreach (var tsk in model.Tasks)
                foreach (var feb in tsk.Feedbacks)
                    model.Feedback.Add(feb);
                    
            return View(model);
        }
        
        [Authorize]
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
        [Authorize]
        public ActionResult Configuracao(ConfiguracaoViewModel model)
        {
            db.Entry(model.Usuario).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Configuracao");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditarTelefone(Telefone model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;     
            db.SaveChanges();
            return RedirectToAction("Configuracao");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditarCartao(Cartao model)
        {
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Configurar");
        }
        
        [Authorize]
        public ActionResult ExcluirTelefone(IdViewModel model)
        {
            db.Entry(db.Telefone.Find(model.Id)).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Configurar");
        }

        [Authorize]
        public ActionResult ExcluirCartao(IdViewModel model)
        {
            db.Entry(db.Cartao.Find(model.Id)).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Configurar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AdicionarTelefone(Telefone model)
        {
            db.Telefone.Add(model);
            db.SaveChanges();
            return RedirectToAction("Configurar");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AdicionarCartao(Cartao model)
        {
            db.Cartao.Add(model);
            db.SaveChanges();
            return RedirectToAction("Usuario");
        }

        [Authorize]
        public ActionResult Grupos()
        {
            List<Grupo> model = new List<Grupo>();
            foreach (var uxg in db.User.Find(User.Identity.GetUserId()).UsuarioGrupos)
                model.Add(db.Grupo.Find(uxg.GrupoId));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarGrupo(Grupo model)
        {
            db.Grupos.Add(model);
            db.SaveChanges();

            UsuarioGrupo uxg = new UsuarioGrupo()
            {
                UsuarioId = User.Identity.GetUserId<int>(),
                GrupoId = gru.Id
            };
            db.UsuarioGrupo.Add(uxg);
            db.SaveChanges();

            return RedirectToAction("Inicio", "Home");
        }

        public ActionResult Grupo(IdViewModel model) //V
        {

            gru_grupo grupo = Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_id), id)[0];

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>("gru_id", grupo.gru_id);

            bool usuarioHasGrupo = false;
            int uxg_Index = 0;
            for (; uxg_Index < uxgs.Count; uxg_Index++)
            {
                if (uxgs[uxg_Index].usu_id == usuario.Id)
                {
                    usuarioHasGrupo = true;
                    break;
                }
            }

            if (!usuarioHasGrupo)
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao pertence a esse grupo";
                return RedirectToAction("Grupos");
            }

            var model = new GrupoViewModel()
            {
                Id = grupo.gru_id,
                Nome = grupo.gru_nome,
                Descricao = grupo.gru_descricao,
                Plano = grupo.gru_plano,
            };

            foreach (uxg_usuario_grupo uxg in uxgs)
            {
                usu_usuario usu = Cursor.Select<usu_usuario>(nameof(usu_usuario.usu_id), uxg.usu_id)[0];
                model.Integrantes.Add(new IntegranteViewModel()
                {
                    Id = usu.usu_id,
                    Nome = usu.usu_nome,
                    isAdm = uxg.uxg_administrador,
                });
            }

            if (uxgs[uxg_Index].uxg_administrador)
                return View("GrupoAdmin", model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGrupo(GrupoViewModel model)
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (!(
                   from x in
                   Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.Id)
                   where (x.gru_id == model.Id)
                   select x
                ).First().uxg_administrador)
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao pode executar essa acao";
                return RedirectToAction("Grupos");
            }

            gru_grupo gru = new gru_grupo()
            {
                gru_nome = model.Nome,
                gru_descricao = model.Descricao,
                gru_id = model.Id,
            };

            if (!Cursor.Update(gru))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Grupo", new { id = model.Id });
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Grupo editado com sucesso";
            return RedirectToAction("Grupo", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarUsuarioGrupo(AdicionarUsuarioGrupoViewModel model) //V 
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            usu_usuario usu;
            try
            {
                usu = Cursor.Select<usu_usuario>(nameof(usu_usuario.usu_email), model.Email)[0];
            }
            catch
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Usuario não encontrado";
                return RedirectToAction("Grupo", new { id = model.gru_id });
            }

            if (!Cursor.Insert(new uxg_usuario_grupo(usu.usu_id, model.gru_id, false), true))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Grupo", new { id = model.gru_id });
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = usu.usu_nome + " adicionado ao grupo com sucesso";
            return RedirectToAction("Grupo", new { id = model.gru_id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirIntegranteGrupo(EditarIntegranteViewModel model) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var uxg = (
                        from x
                        in Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.Id)
                        where (x.gru_id == model.gru_id)
                        select x
                      ).First();

            if (uxg.uxg_administrador)
            {
                if (!Cursor.Delete<uxg_usuario_grupo>(model.usu_id, model.gru_id))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Grupo", new { id = model.gru_id });
                }
            }
            else
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao pode fazer isso";
                return RedirectToAction("Index");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Usuario retirado com sucesso";
            return RedirectToAction("Grupo", new { id = model.gru_id });

        }

        public ActionResult ApagarUsuario() //V
        {
            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (!Cursor.Delete<usu_usuario>(usuario.Id))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            Session.Clear();
            return RedirectToAction("Index");

        }

        public ActionResult Sair() //V
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TornarAdministrador(EditarIntegranteViewModel model)
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var uxg = (
                        from x
                        in Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.Id)
                        where (x.gru_id == model.gru_id)
                        select x
                      ).First();

            if (uxg.uxg_administrador)
            {
                if (!Cursor.Update(new uxg_usuario_grupo(model.usu_id, model.gru_id, true), true))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Grupo", new { id = model.gru_id });
                }
            }
            else
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao pode fazer isso";
                return RedirectToAction("Index");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Usuario tornou-se Administrador do grupo";
            return RedirectToAction("Grupo", new { id = model.gru_id });
        }

        public ActionResult TornarIntegrante(int? id)
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (id == null)
            {
                return RedirectToAction("Grupos");
            }

            var uxgs = Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.gru_id), id);

            int adms = 0;
            foreach (var x in uxgs)
                if (x.uxg_administrador)
                    adms++;

            var uxg = (
                        from x
                        in uxgs
                        where (x.usu_id == usuario.Id)
                        select x
                      ).First();

            if (uxg.uxg_administrador)
            {
                if (adms >= 2)
                {
                    if (!Cursor.Update(new uxg_usuario_grupo(usuario.Id, id.Value, false), true))
                    {
                        TempData["ResultColor"] = "#EEEE00";
                        TempData["Result"] = "Algo deu errado";
                        return RedirectToAction("Grupo", new { id = id });
                    }
                }
                else
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Tem de existir ao menos um Administrador no grupo";
                    return RedirectToAction("Grupo", new { id = id });
                }
            }
            else
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao pode fazer isso";
                return RedirectToAction("Index");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Voce nao e mais Administrador desse grupo";
            return RedirectToAction("Grupo", new { id = id });
        }

        public ActionResult ExcluirGrupo(int? id)
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (id == null)
            {
                return RedirectToAction("Grupos");
            }

            var uxg = (
                        from x
                        in Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.gru_id), id)
                        where (x.usu_id == usuario.Id)
                        select x
                      ).First();

            if (uxg.uxg_administrador)
            {
                if (!Cursor.Delete<gru_grupo>(id.Value))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Grupo", new { id = id });
                }
            }
            else
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao pode fazer isso";
                return RedirectToAction("Index");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Grupo excluido com sucesso";
            return RedirectToAction("Grupos");
        }

    }
}
