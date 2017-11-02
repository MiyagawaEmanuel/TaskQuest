﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {

        //        static List<Usuario> ConnectedUsers = new List<Usuario>();
        //        static List<Mensagem> CurrentMessage = new List<Mensagem>();

        //        #endregion

        //        #region Methods

        //        public void Connect(string userName)
        //        {
        //            var id = Context.ConnectionId;


        //            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
        //            {
        //                ConnectedUsers.Add(new Usuario { ConnectionId = id, UserName = userName });

        //                // send to caller
        //                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

        //                // send to all except caller client
        //                Clients.AllExcept(Convert.ToString(id)).onNewUserConnected(id, userName);

        //            }

        //        }

        //        public void SendMessageToAll(string userName, string message)
        //        {
        //            // store last 100 messages in cache
        //            AddMessageinCache(userName, message);
        //            // Broad cast message
        //            Clients.All.messageReceived(userName, message);
        //        }

        //        public void SendPrivateMessage(string toUserId, string message)
        //        {

        //            string fromUserId = Context.ConnectionId;

        //            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
        //            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);

        //            if (toUser != null && fromUser != null)
        //            {
        //                // send to 
        //                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

        //                // send to caller user
        //                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
        //            }

        //        }

        //        public override Task OnDisconnected(bool stopCalled)
        //        {
        //            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        //            if (item != null)
        //            {
        //                ConnectedUsers.Remove(item);

        //                var id = Context.ConnectionId;
        //                Clients.All.onUserDisconnected(id, item.UserName);

        //            }
        //            return base.OnDisconnected(stopCalled);
        //        }


        //        #endregion

        //        #region private Messages

        //        private void AddMessageinCache(string userName, string message)
        //        {
        //            CurrentMessage.Add(new Mensagem { NomeDeUsuario = userName, Texto = message });

        //            if (CurrentMessage.Count > 100)
        //                CurrentMessage.RemoveAt(0);
        //        }
        
    }
}