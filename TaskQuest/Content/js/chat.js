$(function () {

    // Função que cria o objeto que se comunica com o Hub

    // Declare a proxy to reference the hub.
    var chatHub = $.connection.chatHub;

    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {

        registerEvents(chatHub)

    });
});

function registerEvents(chatHub) {
    // Cria os eventos que chamam as funções no hub
    $("#btnSend").click(function () {
        chatHub.server.Test($("#textInput").val());
    });
}

function registerClientMethods(chatHub) {
    // Funções que são chamadas pelo Hub
    chatHub.client.sendFeedback = function (text) {
        $("#feedback").append("<h1>"+text+"</h1>");
    }
}