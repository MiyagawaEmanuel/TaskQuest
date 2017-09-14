$(".integrante").on("click", function () {
    $.ajax({
        contentType: 'application/json;',
        type: "POST",
        url: "/Grupo/GetInfoIntegrante",
        data: $(this).data("id").id,
        async: false,
        success: function (data) {
            oi = data;
            if (data.Success === "true") {
                $("#NomeIntegrante").text(data.Nome);
                $("#EmailIntegrante").text(data.Email);
                $("#Telefones-container *").remove();
                if (data.Telefones.length > 0) {
                    $("#Telefones-container").append("<h3>Telefones</h3>");
                    $.each(data.Telefones, function (index, value) {
                        $("#Telefones-container").append("<p>" + value.Tipo + ": " + value.Numero + "</p>");
                    });
                }
                $("#modalIntegrante").modal('show');
            }
            else {
                showBalloon("Algo deu errado", "yellow-alert");
            }
        },
        error: function () {
            showBalloon("Algo deu errado", "yellow-alert");
        }
    });
});
