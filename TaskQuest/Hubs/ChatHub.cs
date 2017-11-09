using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TaskQuest.Hubs
{
    public class ChatHub : Hub
    {

        public void Teste(string text)
        {
            Clients.All.sendFeedback(text);
        }

        public void Connect(string userName)
        {
            var connectionId = Context.ConnectionId;
            // TODO: Executar o método Connect no OnLoad da página
            
            // TODO: Salvar essa connectionId no current user no banco,
            // dessa forma podemos identificar quais usuários estão conectados atualmente e
            // envia-los mensagens
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            var item = Context.ConnectionId;
            // TODO: Retirar a connectionId do current user, para informar que ele
            // não precisa receber a mensagem agora
            return base.OnDisconnected(stopCalled);
        }

        public void SendMessage(string id, string text, bool isContatoGrupo)
        {
            // TODO: Salvar a mensagem no banco, enviar a mensagem e
            // para o id do destinatário se ele estiver logado
            
            // TODO: Se isContatoGrupo for true, salvar no banco e
            // enviar a mensagem para todos os usuários do grupo (menos o usuário conectado)
            // conectados atualmente

            // TODO: Enviar um feedback para o usuário remetente de que
            // a mensagem foi enviada ou não
        }

    }
}