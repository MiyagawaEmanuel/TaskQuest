using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskQuest.Data;
using TaskQuest.Models;
using TaskQuest.ViewModels;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {

        private DbContext db = new DbContext();

        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId<int>());

            List<ContatoViewModel> model = new List<ContatoViewModel>();

            ContatoViewModel contatoViewModel;
            foreach (var grupo in user.Grupos)
            {

                contatoViewModel = new ContatoViewModel(grupo);
                
                foreach (var msg in grupo.Mensagens)
                    contatoViewModel.Mensagens.Add(new MensagemViewModel(msg, IsUserRemetente: ((msg.UsuarioRemetenteId == user.Id)? true : false), IsDestinatarioGrupo: true));

                contatoViewModel.Mensagens.OrderBy(e => e.DataMensagem);
                model.Add(contatoViewModel);

                foreach (var usu in grupo.Users)
                {
                    if (usu.Id != user.Id)
                    {
                        contatoViewModel = new ContatoViewModel(usu);

                        foreach (var msg in usu.DestinatarioMensagens)
                            if (msg.UsuarioRemetenteId == user.Id)
                                contatoViewModel.Mensagens.Add(new MensagemViewModel(msg, IsUserRemetente: true, IsDestinatarioGrupo: false));

                        foreach (var msg in usu.RemetenteMensagens)
                            if (msg.UsuarioDestinatarioId == user.Id)
                                contatoViewModel.Mensagens.Add(new MensagemViewModel(msg, IsUserRemetente: false, IsDestinatarioGrupo: false));

                        contatoViewModel.Mensagens.OrderBy(e => e.DataMensagem);
                        model.Add(contatoViewModel);
                    }
                }
            }

            return View(model);
        }

    }
}