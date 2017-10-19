function showBalloon(alert, classe) {
    $(".balloon").addClass(classe);
    $(".balloon").text(alert);
    $(".balloon").css("display", "block");
    $(".balloon").delay(4000).fadeOut(2000);
}

var quest = {

    TasksViewModel: [],

    add: function (task) {
        this.TasksViewModel.push(task);
    },

    remove: function (index) {
        this.TasksViewModel.splice(index, 1);
    },

    get: function (index) {
        return this.TasksViewModel[index];
    },

    set: function (index, doc) {
        this.TasksViewModel[index] = doc;
    },

    setProp: function (index, prop, doc) {
        this.TasksViewModel[index][prop] = doc;
    },

    getAll: function () {
        return this.TasksViewModel;
    },


    render: function () {
        $('#task-container div').remove();
        for (var x = 0; x < this.TasksViewModel.length; x++) {
            var content = "<div class='margin-bottom item' id=" + x + ">" +
                "<a onclick='showAtualizarTaskModal(" + x + ")'><div class='filete' style='background-color: " + quest.Cor + "'></div></a>" +
                "<div class='quest-body flex-properties-c'>" +
                "<div class='icon-black limit-lines'>" +
                "<a onclick='showTaskModal(" + x + ")'>" +
                "<h4 class='Nome'>" + this.get(x)['Nome'] + "</h4>" +
                "</a>" +
                "</div>" +
                "<div class='limit-lines'>" +
                "<h4 class='Descricao'>" + this.get(x)["Descricao"] + "</h4>" +
                "</div>" +
                "<div>" +
                "<h4 class='DataConclusao'>" + this.get(x)["DataConclusao"].split('-').reverse().join('/') + "</h4>" +
                "</div>" +
                "<div class='select-container'>" +
                "<select id='" + this.get(x)["Id"] + "' class='form-control Status' onchange='mudarStatus(this)'>" +
                "<option value='0'>A Fazer</option>" +
                "<option value='1'>Fazendo</option>" +
                "<option value='2'>Feito</option>" +
                "</select>" +
                "</div>" +
                "</div>" +
                "</div>";
            $("#task-container").append(content);
            $('#' + x + ' .Status').val(this.get(x)["Status"]);
            $('#Nome').text(quest.Nome);
            $("#Descricao").text(quest.Descricao);
        }
    }
}

function mudarStatus(index) {
    var status = $("#" + index + " .Status").val();
    quest.setProp(index, "Status", parseInt(status));
    if (quest.get(index)['Feedback'] != undefined)
        if (status == 0 || status == 1)
            quest.setProp(index, 'Feedback', undefined);
}

function submit(id, action) {
    $('#' + id).attr('action', action).submit();
}

function showTaskModal(index) {
    $("#NomeTask").text(quest.get(index)["Nome"]);
    $("#DescricaoTask").text(quest.get(index)["Descricao"]);
    $("#DataConclusao").text(quest.get(index)["DataConclusao"].split('-').reverse().join('/'));

    var dificuldade = "";
    switch (quest.get(index)["Dificuldade"]) {
        case 0:
            dificuldade = 'Fácil';
            break;
        case 1:
            dificuldade = 'Médio';
            break;
        case 2:
            dificuldade = 'Difícil';
            break;
    }
    $("#Dificuldade").text(dificuldade);
    $("#Responsavel").text(quest.get(index).ResponsavelNome);

    $("#modalTask").modal('show');
}

$(document).ready(function () {

    $.ajax({
        contentType: 'application/json;',
        type: "POST",
        url: "/Quest/GetQuests",
        data: JSON.stringify({
            "Hash": $("#Hash").val()
        }),
        success: function (result) {
            oi = result;
            quest.Id = result.data.Id
            quest.Nome = result.data.Nome;
            quest.Descricao = result.data.Descricao;
            quest.Cor = result.data.Cor;
            quest.TasksViewModel = result.data.TasksViewModel;

            $.each(quest.TasksViewModel, function (index, value) {
                quest.TasksViewModel[index].DataConclusao = new Date(parseInt(/^(\/)(Date\()(.+)(\))(\/)$/g.exec(value.DataConclusao)[3])).toLocaleDateString()
            });

            quest.render();
        },
        error: function () {
            showBalloon("Algo deu errado", "yellow-alert");
        }
    });

});