$(function () {

    // Declare a proxy to reference the hub.
    var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {

        registerEvents(chatHub)

    });
});

function registerEvents(chatHub) {

    $("#btnStartChat").click(function () {
        // A função envia o nome do campo de texto para dentro do hub.
        var name = $("#txtNickName").val();
        if (name.length > 0) {
            chatHub.server.connect(name);
        }
        else {
            alert("Por Favor insira um nickname");
        }
    });


    $('#btnSendMsg').click(function () {
        // função responsável por chamar o envio de mensagem do SignalR
        var msg = $("#txtMessage").val();
        if (msg.length > 0) {

            var userName = $('#hdUserName').val();
            chatHub.server.sendMessageToAll(userName, msg);
            $("#txtMessage").val('');
        }
    });


    //$("#txtNickName").keypress(function (e) {
    //    if (e.which == 13) {
    //        $("#btnStartChat").click();
    //    }
    //});

    $("#txtMessage").keypress(function (e) {
        if (e.which == 13) {
            $('#btnSendMsg').click();
        }
    });
}

function registerClientMethods(chatHub) {

    // Chama quando o usuário logou com sucesso
    chatHub.client.onConnected = function (id, userName, allUsers, messages) {

        setScreen(true);

        $('#hdId').val(id);
        $('#hdUserName').val(userName);
        $('#spanUser').html(userName);

        // Adiciona todos os Usuários
        for (i = 0; i < allUsers.length; i++) {

            AddUser(chatHub, allUsers[i].ConnectionId, allUsers[i].UserName);
        }

        // Adiciona mensagens existentes
        for (i = 0; i < messages.length; i++) {

            AddMessage(messages[i].UserName, messages[i].Message);
        }
    }

    // Ao conectar adiciona usuário
    chatHub.client.onNewUserConnected = function (id, name) {

        AddUser(chatHub, id, name);
    }


    // Ao desconectar deverá modificar o ícone de conectado para desconectado.
    chatHub.client.onUserDisconnected = function (id, userName) {

        $('#nome-' + id).remove();

        //var ctrId = 'flex-' + id;
        //$('flex-' + ctrId).remove();


        var disc = $('<li class="disconnect">"' + userName + '" logged off.</li>');

        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);
    }

    chatHub.client.messageReceived = function (userName, message) {
        // Exite uma função no hub que chama essa função que passa o username e a mensagem
        AddMessage(userName, message);
    }


    chatHub.client.sendPrivateMessage = function (windowId, fromUserName, message) {

        var ctrId = 'Nome-' + windowId;


        if ($('#' + ctrId).length == 0) {

            createPrivateChatWindow(chatHub, windowId, ctrId, fromUserName);// não existe mais criação de janela privada
        }

        $('#' + ctrId).find('#divMessage').append('<div class="message"><span class="userName">' + fromUserName + '</span>: ' + message + '</div>');

        // set scrollbar
        var height = $('#' + ctrId).find('#divMessage')[0].scrollHeight;
        $('#' + ctrId).find('#divMessage').scrollTop(height);

    }
}



//Adicionar usuários
function AddUser(chatHub, id, name) {
    // coloca os novos usuarios logados na sessão de abas
    var userId = $('#hdId').val();

    var code = "";

    if (userId != id) {
        code = $('<li id="Nome' + id + '"><a href="#Nome-' + id + '">' + name + '</a></li>');

        $(code).dblclick(function () {

            var id = $(this).attr('id');

            if (userId != id)
                OpenPrivateChatWindow(chatHub, id, name);
        });
    }

    $("#divusers").append(code);
}

function AddMessage(userName, message) {
    $('#divChatWindow').append('<div class="message"><span class="userName">' + userName + '</span>: ' + message + '</div>');

    var height = $('#divChatWindow')[0].scrollHeight;
    $('#divChatWindow').scrollTop(height);
}

function OpenPrivateChatWindow(chatHub, id, userName) {

    var ctrId = 'private_' + id;

    if ($('#' + ctrId).length > 0) return;

    createPrivateChatWindow(chatHub, id, ctrId, userName);

}

function createPrivateChatWindow(chatHub, userId, ctrId, userName) {
    var div = '<div id="Nome-' + ctrId + '" class="tab-pane fade in active chat-home">' +
        '<div class="container-fluid">' +
        '<div id="divMessage" class="row campo-texto">' +
        '</div>' +
        '<!--Botões do chat principal-->' +
        '<div class="row chat-stuff">' +
        '<div class="center-block position-absolute-botton">' +
        '<div class="col-lg-11">' +
        '<input class="form-control" type="text" id="txtPrivateMessage" />' +
        '</div>' +
        '<div class="col-lg-1">' +
        '<input id="btnSendMessage" type="button" value="Enviar" class="btn btn-primary" />' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div';
    //div = '<div id="' + ctrId + '" class="ui-widget-content draggable" rel="0">' +
    //    '<div class="header">' +
    //    '<div  style="float:right; padding-right: 2px;">' +
    //    '<i class="fa fa-close" style="cursor:pointer;"></i>' +
    //    '</div>' +

    //    '<span class="selText" rel="0">' + userName + '</span>' +
    //    '</div>' +
    //    '<div id="divMessage" class="messageArea">' +

    //    '</div>' +
    //    '<div class="buttonBar">' +
    //    '<input id="txtPrivateMessage" class="form-control" type="text"   />' +
    //    '<input id="btnSendMessage" class="btn btn-primary" type="button" value="Enviar"   />' +
    //    '</div>' +
    //    '</div>';

    var $div = $(div);

    // Send Button event
    $div.find("#btnSendMessage").click(function () {

        $textBox = $div.find("#txtPrivateMessage");
        var msg = $textBox.val();
        if (msg.length > 0) {

            chatHub.server.sendPrivateMessage(userId, msg);
            $textBox.val('');
        }
    });

    // Text Box event
    $div.find("#txtPrivateMessage").keypress(function (e) {
        if (e.which == 13) {
            $div.find("#btnSendMessage").click();
        }
    });

    AddDivToContainer($div);

} // Delete

function AddDivToContainer($div) {
    $('#ChatTab').prepend($div);

} //Delete

//function () {
//    $(".nav-pills a").click(function () {
//        $(this).tab('show');
//    });
//    $('.nav-pills a').on('shown.bs.tab', function (event) {
//        var x = $(event.target).text();         // active tab
//        var y = $(event.relatedTarget).text();  // previous tab
//        $(".act span").text(x);
//        $(".prev span").text(y);
//    });
//}