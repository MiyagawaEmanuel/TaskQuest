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
        public ActionResult Index(string returnUrl=null) //V
        {
            if(returnUrl != null)
            {
                TempData["Response"] = "Acesso não autorizado";
                TempData["Class"] = "yellow-alert";
            }
            return View();
        }

        [Authorize]
        public ActionResult Inicio() //V
        {

            User user = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new InicioViewModel();

            foreach (var uxg in user.UsuarioGrupos)
                foreach (var gru in db.Grupos.Where(q => q.Id == uxg.GrupoId).ToArray())
                    model.Grupos.Add(gru);

            return View(model);
        }

        public ActionResult Usuario() //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new UsuarioViewModel();

            model.Nome = usuario.Nome;
            model.Sobrenome = usuario.Sobrenome;
            model.Email = usuario.Email;
            model.Senha = usuario.PasswordHash;
            model.DataNascimento = usuario.DataNascimento.DateTimeToString();
            model.Sexo = usuario.Sexo;

            foreach (var cartao in Cursor.Select<crt_cartao>(nameof(usu_usuario.usu_id), usuario.Id))
            {
                model.Cartoes.Add(new CartaoViewModel()
                {
                    Id = cartao.crt_id,
                    Bandeira = cartao.crt_bandeira,
                    Numero = cartao.crt_numero,
                    NomeTitular = cartao.crt_nome_titular,
                    DataVencimento = cartao.crt_data_vencimento,
                    CodigoSeguranca = cartao.crt_codigo_seguranca,
                });
            }

            foreach (var telefone in Cursor.Select<tel_telefone>(nameof(tel_telefone.usu_id), usuario.Id))
            {
                model.Telefones.Add(new TelefoneViewModel()
                {
                    Id = telefone.tel_id,
                    Tipo = telefone.tel_tipo,
                    Ddd = telefone.tel_ddd,
                    Numero = telefone.tel_numero,
                });
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usuario(UsuarioViewModel model) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var usu = new usu_usuario()
            {
                usu_id = usuario.Id,
                usu_nome = model.Nome,
                usu_sobrenome = model.Sobrenome,
                usu_senha = model.Senha,
                usu_email = model.Email,
                usu_data_nascimento = model.DataNascimento.StringToDateTime(),
                usu_sexo = model.Sexo,
            };

            if (!Cursor.Update(usu))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            foreach (var telefone in model.Telefones)
            {
                var tel = new tel_telefone()
                {
                    tel_id = telefone.Id,
                    usu_id = usuario.Id,
                    tel_tipo = telefone.Tipo,
                    tel_ddd = telefone.Ddd,
                    tel_numero = telefone.Numero,

                };

                foreach (var prop in tel.GetType().GetProperties())
                {
                    Debug.WriteLine(prop.GetValue(tel));
                }

                if (!Cursor.Update(tel))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            foreach (var cartao in model.Cartoes)
            {
                var crt = new crt_cartao()
                {
                    crt_id = cartao.Id,
                    crt_numero = cartao.Numero,
                    crt_bandeira = cartao.Bandeira,
                    crt_codigo_seguranca = cartao.CodigoSeguranca,
                    crt_data_vencimento = cartao.DataVencimento,
                    crt_nome_titular = cartao.NomeTitular,
                    usu_id = usuario.Id,
                };
                if (!Cursor.Update(crt))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTelefone(List<TelefoneViewModel> model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voca nao esta logado";
                return RedirectToAction("Index");
            }

            foreach (var telefone in model)
            {
                var tel = new tel_telefone()
                {
                    tel_id = telefone.Id,
                    usu_id = usuario.usu_id,
                    tel_tipo = telefone.Tipo,
                    tel_ddd = telefone.Ddd,
                    tel_numero = telefone.Numero,
                };

                if (!Cursor.Update(tel))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCartao(List<CartaoViewModel> model) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            foreach (var cartao in model)
            {
                var crt = new crt_cartao()
                {
                    crt_id = cartao.Id,
                    crt_numero = cartao.Numero,
                    crt_bandeira = cartao.Bandeira,
                    crt_codigo_seguranca = cartao.CodigoSeguranca,
                    crt_data_vencimento = cartao.DataVencimento,
                    crt_nome_titular = cartao.NomeTitular,
                    usu_id = usuario.Id,
                };
                if (!Cursor.Update(crt))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");
        }

        public ActionResult ExcluirTelefone(int? id) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (id == null)
            {
                return RedirectToAction("Grupos");
            }

            if (!Cursor.Delete<tel_telefone>(id ?? 0))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");

        }

        public ActionResult ExcluirCartao(int? id) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (id == null)
            {
                return RedirectToAction("Grupos");
            }

            if (!Cursor.Delete<crt_cartao>(id ?? 0))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarTelefone(TelefoneViewModel model) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var tel = new tel_telefone()
            {
                usu_id = usuario.Id,
                tel_tipo = model.Tipo,
                tel_ddd = model.Ddd,
                tel_numero = model.Numero,
            };

            if (!Cursor.Insert(tel))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Telefone adicionado com sucesso";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarCartao(CartaoViewModel model) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var crt = new crt_cartao()
            {
                usu_id = usuario.Id,
                crt_bandeira = model.Bandeira,
                crt_codigo_seguranca = model.CodigoSeguranca,
                crt_data_vencimento = model.DataVencimento,
                crt_nome_titular = model.NomeTitular,
                crt_numero = model.Numero,
            };

            if (!Cursor.Insert(crt))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Cartao adicionado com sucesso";
            return RedirectToAction("Usuario");
        }

        public ActionResult Grupos() //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            var model = new GruposViewModel();

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.Id);

            foreach (var uxg in uxgs)
            {
                gru_grupo grupo = Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_id), uxg.gru_id)[0];
                model.Grupos.Add(new Grupo()
                {
                    Id = grupo.gru_id,
                    Nome = grupo.gru_nome,
                    Descricao = grupo.gru_descricao,
                });
            }

            return View(model);
        }

        [Authorize, Autenticar]
        public ActionResult CriarGrupo() //V
        {
            return View(new CriarGrupoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CriarGrupo(Grupo model) //V
        {
            Grupo gru = new Grupo()
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                Plano = false,
                Cor = model.Cor,
            };
            db.Grupos.Add(gru);
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

        public ActionResult Grupo(int? id) //V
        {

            User usuario = db.Users.Find(User.Identity.GetUserId<int>());

            if (id == null)
            {
                return RedirectToAction("Grupos");
            }

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