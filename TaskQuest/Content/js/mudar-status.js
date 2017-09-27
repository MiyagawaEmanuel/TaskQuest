function showBalloon(alert, classe) {
    $("#BalloonStatus").addClass(classe);
    $("#BalloonStatus").text(alert);
    $("#BalloonStatus").css("display", "block");
    $("#BalloonStatus").delay(4000).fadeOut(2000);
}

function mudarStatus(status) {
    $.ajax({
        contentType: 'application/json;',
        type: "POST",
        url: "/Quest/MudarStatus",
        data: JSON.stringify({
            "Id": status.id,
            "Status": status.value
        }),
        success: function (response) {
            if (response == "true")
                showBalloon("Atualizado com sucesso", "green-alert")
            else
                showBalloon("Algo deu errado", "yellow-alert");
        },
        error: function () {
            showBalloon("Algo deu errado", "yellow-alert");
        }
    });
}