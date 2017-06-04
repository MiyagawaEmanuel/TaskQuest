using System;
using System.Collections.Generic;
using TaskQuest.App_Code;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Diagnostics;
using System.Linq;

namespace TaskQuest.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index() //V
        {

            ViewBag.ResultColor = ViewData["ResultColor"];
            ViewBag.Result = ViewData["Result"];

            return View();

        }

        public ActionResult Login(LoginViewModel model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.Select<usu_usuario>(nameof(usu_usuario.usu_email), model.Email)[0];
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Usuario não reconhecido";
                return RedirectToAction("Index");
            }
            
            if (!String.Equals(Util.Hash(model.Senha), usuario.usu_senha, StringComparison.OrdinalIgnoreCase))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Usuario não reconhecido";
                return View("Index");
            }

            else
            {
                Session["user"] = Util.Hash(usuario.usu_id.ToString());
                return RedirectToAction("Inicio");
            }

        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model) //V
        {

            usu_usuario usuario = new usu_usuario()
            {
                usu_nome = model.Nome,
                usu_sobrenome = model.Sobrenome,
                usu_email = model.Email,
                usu_data_nascimento = model.DataNascimento,
                usu_sexo = model.Sexo.ToString(),
                usu_senha = Util.Hash(model.Senha),
            };

            if (!Cursor.Insert(usuario))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", new LoginViewModel() { Email = model.Email, Senha = model.Senha });
        }

        public ActionResult Inicio() //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var model = new InicioViewModel();

            foreach (var uxg in Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.usu_id))
            {
                foreach (var grupo in Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_id), uxg.gru_id))
                {
                    model.Grupos.Add(new Grupo()
                    {
                        Nome = grupo.gru_nome,
                        Id = grupo.gru_id,
                    });
                }
            }

            return View(model);
        }

        public ActionResult Usuario() //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var model = new UsuarioViewModel();

            model.Nome = usuario.usu_nome;
            model.Sobrenome = usuario.usu_sobrenome;
            model.Email = usuario.usu_email;
            model.Senha = usuario.usu_senha;
            model.DataNascimento = usuario.usu_data_nascimento;
            model.Sexo = usuario.usu_sexo;

            foreach (var cartao in Cursor.Select<crt_cartao>(nameof(usu_usuario.usu_id), usuario.usu_id))
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

            foreach (var telefone in Cursor.Select<tel_telefone>(nameof(tel_telefone.usu_id), usuario.usu_id))
            {
                model.Telefones.Add(new TelefoneViewModel()
                {
                    Id = telefone.tel_id,
                    Tipo = telefone.tel_tipo,
                    Ddd = telefone.tel_ddd,
                    Numero = telefone.tel_numero,
                });
            }

            ViewBag.ResultColor = ViewData["ResultColor"];
            ViewBag.Result = ViewData["Result"];

            return View(model);

        }

        [HttpPost]
        public ActionResult Usuario(UsuarioViewModel model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var usu = new usu_usuario()
            {
                usu_id = usuario.usu_id,
                usu_nome = model.Nome,
                usu_sobrenome = model.Sobrenome,
                usu_senha = model.Senha,
                usu_email = model.Email,
                usu_data_nascimento = model.DataNascimento,
                usu_sexo = model.Sexo,
            };

            if (!Cursor.Update(usu))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            foreach (var telefone in model.Telefones)
            {
                var tel = new tel_telefone()
                {
                    tel_id = telefone.Id,
                    usu_id = usuario.usu_id,
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
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
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
                    usu_id = usuario.usu_id,
                };
                if (!Cursor.Update(crt))
                {
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
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
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
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
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
        public ActionResult EditarCartao(List<CartaoViewModel> model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

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
                    usu_id = usuario.usu_id,
                };
                if (!Cursor.Update(crt))
                {
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");
        }

        public ActionResult ExcluirTelefone(int? id) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return RedirectToAction("Inicio");
            }

            if (!Cursor.Delete<tel_telefone>(id ?? 0))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");

        }

        public ActionResult ExcluirCartao(int? id) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return RedirectToAction("Inicio");
            }

            if (!Cursor.Delete<crt_cartao>(id ?? 0))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Seus dados foram atualizados";
            return RedirectToAction("Usuario");

        }

        [HttpPost]
        public ActionResult AdicionarTelefone(TelefoneViewModel model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var tel = new tel_telefone()
            {
                usu_id = usuario.usu_id,
                tel_tipo = model.Tipo,
                tel_ddd = model.Ddd,
                tel_numero = model.Numero,
            };

            if (!Cursor.Insert(tel))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = "Telefone adicionado com sucesso";
            return RedirectToAction("Usuario");
        }

        [HttpPost]
        public ActionResult AdicionarCartao(CartaoViewModel model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var crt = new crt_cartao()
            {
                usu_id = usuario.usu_id,
                crt_bandeira = model.Bandeira,
                crt_codigo_seguranca = model.CodigoSeguranca,
                crt_data_vencimento = model.DataVencimento,
                crt_nome_titular = model.NomeTitular,
                crt_numero = model.Numero,
            };

            if (!Cursor.Insert(crt))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = "Cartão adicionado com sucesso";
            return RedirectToAction("Usuario");
        }

        public ActionResult Grupos() //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var model = new GruposViewModel();

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.usu_id);

            foreach (var uxg in uxgs)
            {
                gru_grupo grupo = Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_id), uxg.gru_id)[0];
                model.Grupos.Add(new Grupo()
                {
                    Id = grupo.gru_id,
                    Nome = grupo.gru_nome,
                });
            }

            return View(model);
        }

        public ActionResult CriarGrupo() //V
        {
            return View(new CriarGrupoViewModel());
        }

        [HttpPost]
        public ActionResult CriarGrupo(CriarGrupoViewModel model) //V
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var gru = new gru_grupo()
            {
                gru_data_criacao = DateTime.Now,
                gru_descricao = model.Descricao,
                gru_nome = model.Nome,
                gru_plano = model.Plano,
            };

            if (!Cursor.Insert(gru))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            int? gru_id = null;
            foreach (var grupo in Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_nome), gru.gru_nome))
            {
                Console.WriteLine(grupo.gru_id + ", " + grupo.gru_nome + ", " + grupo.gru_data_criacao.ToString());
                if (grupo.gru_data_criacao.ToString().CompareTo(gru.gru_data_criacao.ToString()) == 0)
                    gru_id = grupo.gru_id;
            }

            if(gru_id != null)
            {
                if (!Cursor.Insert(new uxg_usuario_grupo(usuario.usu_id, gru_id.Value, true), true))
                {
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
                    return RedirectToAction("Usuario");
                }
            }
            
            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = "Grupo adicionado com sucesso";
            return RedirectToAction("Grupos");
        }

        public ActionResult Grupo(int? id) 
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return RedirectToAction("Inicio");
            }

            gru_grupo grupo = Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_id), id)[0];

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>("gru_id", grupo.gru_id);

            bool usuarioHasGrupo = false;
            int uxg_Index = 0;
            for (; uxg_Index < uxgs.Count; uxg_Index++)
            {
                if (uxgs[uxg_Index].usu_id == usuario.usu_id)
                {
                    usuarioHasGrupo = true;
                    break;
                }
            }

            if (!usuarioHasGrupo)
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não pertence a esse grupo";
                return RedirectToAction("Index");
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
                });
            }

            if (uxgs[uxg_Index].uxg_administrador)
                return View("GrupoAdmin", model);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditarGrupo(GrupoViewModel model)
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            
            if ((
                   from x in
                   Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.usu_id)
                   where (x.gru_id == model.Id)
                   select x
                ).First().uxg_administrador)
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não pode executar essa ação";
                return RedirectToAction("Index");
            }

            gru_grupo gru = new gru_grupo()
            {
                gru_nome = model.Nome,
                gru_descricao = model.Descricao,
                gru_id = model.Id,
            };

            if (!Cursor.Update(gru))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Grupo", model.Id);
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Grupo editado com sucesso";
            return View(model);
        }

        [HttpPost]
        public ActionResult AdicionarUsuarioGrupo(AdicionarUsuarioGrupoViewModel model) 
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            usu_usuario usu = Cursor.Select<usu_usuario>(nameof(usu_usuario.usu_email), model.Email)[0];

            if (usu == null)
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Usuario não encontrado";
                return RedirectToAction("Index");
            }

            Debug.WriteLine(usu.usu_id);
            Debug.WriteLine(model.gru_id);

            if (!Cursor.Insert(new uxg_usuario_grupo(usu.usu_id, model.gru_id, false), true))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Inicio");
            }

            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = usu.usu_nome + " adicionado ao grupo com sucesso";
            return RedirectToAction("Grupo", model.gru_id);
        }

        [HttpPost]
        public ActionResult ExcluirUsuarioGrupo(ExcluirUsuarioGrupo model)
        {

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var uxg = (
                        from x
                        in Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.usu_id)
                        where (x.gru_id == model.gru_id)
                        select x
                      ).First();

            if (uxg.uxg_administrador)
            {
                if (!Cursor.Delete<uxg_usuario_grupo>(model.usu_id, model.gru_id))
                {
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
                    return RedirectToAction("Grupo", model.gru_id);
                }
            }
            else
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não tem privilégios para fazer isso";
                return RedirectToAction("Index");
            }

            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = "Usuário retirado com sucesso";
            return RedirectToAction("Grupo", model.gru_id);

        }

        public ActionResult ApagarUsuario() //V
        {
            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }
            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            if (!Cursor.Delete<usu_usuario>(usuario.usu_id))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
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

    }
}