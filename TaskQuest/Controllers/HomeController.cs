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

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Usuario nao reconhecido";
                return RedirectToAction("Index");
            }

            if (!String.Equals(Util.Hash(model.Senha), usuario.usu_senha, StringComparison.OrdinalIgnoreCase))
            {
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Usuario nao reconhecido";
                return View("Index");
            }

            else
            {
                Session["user"] = Util.Hash(usuario.usu_id.ToString());
                return RedirectToAction("Grupos");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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

            model.Grupos = Util.Sort(model.Grupos, Comparer<Grupo>.Create((x, y) => String.Compare(x.Nome, y.Nome)));

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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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

            model.Cartoes = Util.Sort(model.Cartoes, Comparer<CartaoViewModel>.Create((x, y) => String.Compare(y.DataVencimento, x.DataVencimento)));

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

            model.Telefones = Util.Sort(model.Telefones, Comparer<TelefoneViewModel>.Create((x, y) => x.Numero.CompareTo(y.Numero)));

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
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
                    usu_id = usuario.usu_id,
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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

            var model = new GruposViewModel();

            List<uxg_usuario_grupo> uxgs = Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.usu_id);

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

            model.Grupos = Util.Sort(model.Grupos, Comparer<Grupo>.Create((x, y) => String.Compare(x.Nome, y.Nome)));

            return View(model);
        }

        public ActionResult CriarGrupo() //V
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
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

            return View(new CriarGrupoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Algo deu errado";
                return RedirectToAction("Usuario");
            }

            int? gru_id = null;
            foreach (var grupo in Cursor.Select<gru_grupo>(nameof(gru_grupo.gru_nome), gru.gru_nome))
            {
                Console.WriteLine(grupo.gru_id + ", " + grupo.gru_nome + ", " + grupo.gru_data_criacao.ToString());
                if (grupo.gru_data_criacao.ToString().CompareTo(gru.gru_data_criacao.ToString()) == 0)
                    gru_id = grupo.gru_id;
            }

            if (gru_id != null)
            {
                if (!Cursor.Insert(new uxg_usuario_grupo(usuario.usu_id, gru_id.Value, true), true))
                {
                    TempData["ResultColor"] = "#EEEE00";
                    TempData["Result"] = "Algo deu errado";
                    return RedirectToAction("Usuario");
                }
            }

            TempData["ResultColor"] = "#32CD32";
            TempData["Result"] = "Grupo adicionado com sucesso";
            return RedirectToAction("Grupos");
        }

        public ActionResult Grupo(int? id) //V
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
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

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
                if (uxgs[uxg_Index].usu_id == usuario.usu_id)
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

            model.Integrantes = Util.Sort(model.Integrantes, Comparer<IntegranteViewModel>.Create((x, y) => String.Compare(x.Nome, y.Nome)));

            if (uxgs[uxg_Index].uxg_administrador)
                return View("GrupoAdmin", model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

            if (!(
                   from x in
                   Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.usu_id), usuario.usu_id)
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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
            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

            if (!Cursor.Delete<usu_usuario>(usuario.usu_id))
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

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
                        where (x.usu_id == usuario.usu_id)
                        select x
                      ).First();

            if (uxg.uxg_administrador)
            {
                if (adms >= 2)
                {
                    if (!Cursor.Update(new uxg_usuario_grupo(usuario.usu_id, id.Value, false), true))
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

            usu_usuario usuario = new usu_usuario();
            try
            {
                usuario = Cursor.SelectMD5<usu_usuario>(Session["user"].ToString())[0];
            }
            catch (Exception)
            {
                Session.Clear();
                TempData["ResultColor"] = "#EEEE00";
                TempData["Result"] = "Voce nao esta logado";
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return RedirectToAction("Grupos");
            }

            var uxg = (
                        from x
                        in Cursor.Select<uxg_usuario_grupo>(nameof(uxg_usuario_grupo.gru_id), id)
                        where (x.usu_id == usuario.usu_id)
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