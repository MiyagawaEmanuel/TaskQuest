using System;
using System.Collections.Generic;
using TaskQuest.App_Code;
using TaskQuest.Models;
using System.Web.Mvc;
using TaskQuest.ViewModels;
using System.Linq;
using System.Diagnostics;

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

            usu_usuario usuario = Cursor.Select<usu_usuario>(GetType().GetProperty("usu_email"), model.Email)[0];

            if (usuario == null)
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewBag["Result"] = "Usuario não reconhecido";
                return RedirectToAction("Index");
            }

            else
            {
                Session["user"] = Hash.String(usuario.usu_id.ToString());
                return RedirectToAction("Inicio");
            }

        }
        
        public ActionResult Register(RegisterViewModel model) //V
        {

            Debug.WriteLine("Usou o Register controller");

            usu_usuario usuario = new usu_usuario()
            {
                usu_nome = model.Nome,
                usu_sobrenome = model.Sobrenome,
                usu_email = model.Email,
                usu_cor = model.Cor,
                usu_data_nascimento = model.DataNascimento,
                usu_sexo = model.Sexo,
                usu_senha = Hash.String(model.Senha),
            };

            if (!Cursor.Insert(usuario))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            tel_telefone telefone = new tel_telefone()
            {
                tel_tipo = model.TelTipo,
                tel_ddd = Convert.ToInt32(model.Telefone.Substring(1, 2)),
                tel_numero = Convert.ToInt32(model.Telefone.Substring(5, model.Telefone.Length)),
                usu_id = Cursor.Select<usu_usuario>(GetType().GetProperty("usu_email"), model.Email)[0].usu_id,
            };
            
            if (!Cursor.Insert(telefone))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", new LoginViewModel() { Email = model.Email, Senha = Hash.String(model.Senha) });
        }

        public ActionResult Inicio() //V
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var model = new InicioViewModel();

            foreach (var grupo in Cursor.Select<gru_grupo>(GetType().GetProperty("usu_id"), usuario.usu_id))
            {
                model.Grupos.Add(new Grupo()
                {
                    Nome = grupo.gru_nome,
                    Cor = grupo.gru_cor,
                });
            }

            return View(model);
        }

        public ActionResult Usuario() //V
        {
            
            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

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
            model.Senha = String.Concat(Enumerable.Repeat("*", 8));
            model.DataNascimento = usuario.usu_data_nascimento.ToString();
            model.Cor = usuario.usu_cor;
            model.Sexo = usuario.usu_sexo;

            foreach (var cartao in Cursor.Select<crt_cartao>())
            {
                model.Cartoes.Add(new CartaoViewModel()
                {
                    Bandeira = cartao.crt_bandeira,
                    Numero = cartao.crt_numero,
                    NomeTitular = cartao.crt_nome_titular,
                    DataVencimento = cartao.crt_data_vencimento,
                    CodigoSeguranca = cartao.crt_codigo_seguranca,
                });
            }

            foreach (var telefone in Cursor.Select<tel_telefone>())
            {
                model.Telefones.Add(new TelefoneViewModel()
                {
                    Tipo = telefone.tel_tipo,
                    Numero = "(" + telefone.tel_ddd + ")" + " " + telefone.tel_numero,
                });
            }

            ViewBag.ResultColor = ViewData["ResultColor"];
            ViewBag.Result = ViewData["Result"];

            return View(model);
            
        }

        [HttpPost]
        public ActionResult Usuario(UsuarioViewModel model) //V
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

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
                usu_cor = model.Cor,
                usu_data_nascimento = DateTime.ParseExact(model.DataNascimento, "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture),
            };

            if (!Cursor.Update(usuario))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            foreach (var telefone in model.Telefones)
            {
                var tel = new tel_telefone()
                {
                    tel_id = telefone.Id,
                    usu_id = usuario.usu_id,
                    tel_tipo = telefone.Tipo,
                    tel_ddd = Convert.ToInt32(telefone.Numero.Substring(1, 2)),
                    tel_numero = Convert.ToInt32(telefone.Numero.Substring(5, telefone.Numero.Length)),
                };
                if (!Cursor.Update(tel))
                {
                    ViewData["ResultColor"] = "#EEEE00";
                    ViewData["Result"] = "Algo deu errado";
                    return RedirectToAction("Usuario");
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
                    return RedirectToAction("Usuario");
                }
            }

            ViewBag.ResultColor = "#32CD32";
            ViewBag.Result = "Seus dados foram atualizados";
            return View();
        }

        [HttpPost]
        public ActionResult AdicionarTelefone(TelefoneViewModel model) //V
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

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
                tel_ddd = Convert.ToInt32(model.Numero.Substring(1, 2)),
                tel_numero = Convert.ToInt32(model.Numero.Substring(5, model.Numero.Length)),
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

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

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

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var model = new GruposViewModel();

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>(GetType().GetProperty("usu_id"), usuario.usu_id);

            foreach(var uxg in uxgs)
            {
                gru_grupo grupo = Cursor.Select<gru_grupo>(GetType().GetProperty("gru_id"), uxg.gru_id)[0];
                model.Grupos.Add(new Grupo()
                {
                    Id = grupo.gru_id,
                    Nome = grupo.gru_nome,
                    Cor = grupo.gru_cor,
                });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CriarGrupo(CriarGrupoViewModel model)
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            var gru = new gru_grupo()
            {
                gru_cor = model.Cor,
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

            var gruId = Cursor.Select<gru_grupo>(GetType().GetProperty("gru_data_criacao"), gru.gru_data_criacao)[0].gru_id;

            if (!Cursor.Insert(new uxg_usuario_grupo(usuario.usu_id, gruId, true)))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = "Grupo adicionado com sucesso";
            return RedirectToAction("Grupos");
        }

        public ActionResult Grupo(int id) //V
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            gru_grupo grupo = Cursor.Select<gru_grupo>(GetType().GetProperty("gru_id"), id)[0];

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>(GetType().GetProperty("gru_id"), grupo.gru_id);

            bool usuarioHasGrupo = false;
            foreach (var uxg in uxgs)
                if (uxg.usu_id == usuario.usu_id)
                    usuarioHasGrupo = true;

            if (!usuarioHasGrupo)
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não tem esse grupo";
                return RedirectToAction("Inicio");
            }

            var model = new GrupoViewModel()
            {
                Id = grupo.gru_id,
                Nome = grupo.gru_nome,
                Cor = grupo.gru_cor,
                Descricao = grupo.gru_descricao,
                Plano = grupo.gru_plano,
            };
            
            foreach(uxg_usuario_grupo uxg in uxgs)
            {
                usu_usuario usu = Cursor.Select<usu_usuario>(GetType().GetProperty("usu_id"), uxg.usu_id)[0];
                model.Integrantes.Add(new IntegranteViewModel()
                {
                    Id = usu.usu_id,
                    Nome = usu.usu_nome,
                    Cor = usu.usu_cor,
                });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Grupo(GrupoViewModel model)
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            gru_grupo gru = new gru_grupo()
            {
                gru_nome = model.Nome,
                gru_cor = model.Cor,
                gru_data_criacao = DateTime.Now,
                gru_descricao = model.Descricao,
                gru_plano = model.Plano,
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
        public ActionResult AdicionarUsuarioGrupo(AdicionarUsuarioGrupoViewModel model) //V
        {

            var usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];

            if (usuario == null)
            {
                Session.Clear();
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Você não está logado";
                return RedirectToAction("Index");
            }

            usu_usuario usu = Cursor.Select<usu_usuario>(GetType().GetProperty("usu_email"), model.Email)[0];

            if (usu == null)
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Usuario não encontrado";
                return RedirectToAction("Grupo", model.gru_id);
            }

            if (!Cursor.Insert(new uxg_usuario_grupo(usu.usu_id, model.gru_id, false)))
            {
                ViewData["ResultColor"] = "#EEEE00";
                ViewData["Result"] = "Algo deu errado";
                return RedirectToAction("Grupo", model.gru_id);
            }

            ViewData["ResultColor"] = "#32CD32";
            ViewData["Result"] = usu.usu_nome + " adicionado ao grupo com sucesso";
            return RedirectToAction("Grupo", model.gru_id);
        }

    }
}