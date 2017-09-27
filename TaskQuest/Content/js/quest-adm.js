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
                "<a onclick='showAtualizarTaskModal(" + x + ")'>" +
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
                "<select class='form-control Status' onchange='mudarStatus(" + x + ")'>" +
                "<option value='0'>A Fazer</option>" +
                "<option value='1'>Fazendo</option>" +
                "<option value='2'>Feito</option>" +
                "</select>" +
                "</div>" +
                "</div>" +
                "</div>";
            $("#task-container").append(content);
            $('#' + x + ' .Status').val(this.get(x)["Status"]);
        }
        $('#Nome').val(quest.Nome);
        $("#Descricao").val(quest.Descricao);
        $("#Cor").val(quest.Cor);
    }

}

function mudarStatus(index) {
    var status = $("#" + index + " .Status").val();
    quest.setProp(index, "Status", parseInt(status));
    if (quest.get(index)['FeedbackViewModel'] != undefined)
        if (status == 0 || status == 1)
            quest.setProp(index, 'FeedbackViewModel', undefined);
}

function submit(id, action) {
    $('#' + id).attr('action', action).submit();
}

function showAtualizarTaskModal(index) {
    $("#NomeTaskAtualizar").val(quest.get(index)["Nome"]);
    $("#DescricaoTaskAtualizar").text(quest.get(index)["Descricao"]);
    $("#DataConclusaoAtualizar").val(quest.get(index)["DataConclusao"].split('/').reverse().join('-'));
    $("#DificuldadeAtualizar").val(quest.get(index)["Dificuldade"])
    $("#AtualizarTask").data('index', index);
    $("#ExcluirTask").data('index', index);

    $('#Feedback div').remove();
    if (quest.get(index)['Status'] === 2) {
        if (quest.get(index)['FeedbackViewModel'] === undefined || quest.get(index)['FeedbackViewModel'] === null) {
            var content = "<div class='icon-black'><a onclick='showFeedbackModal(" + index + ")'><h4>Criar Feedback<h4></a></div>";
            $("#Feedback").append(content);
        }
        else {
            var content = "<div><p>Feedback</p><div>" +
								"<div class='form-group'>" +
									"<textarea class='form-control' id='AtualizarTextoFeedback' name='AtualizarTextoFeedback' placeholder='Texto do Feedback' required></textarea>" +
									"<label for='AtualizarTextoFeedback'></label>" +
								"</div>" +
								"<div class='select-container'>" +
									"<select class='form-control' id='AtualizarNota'>" +
										"<option>0</option>" +
										"<option>1</option>" +
										"<option>2</option>" +
										"<option>3</option>" +
										"<option>4</option>" +
										"<option>5</option>" +
									"</select>" +
								"</div>";
            $('#Feedback').append(content);
            $('#AtualizarTextoFeedback').val(quest.get(index)['FeedbackViewModel']['Resposta']);
            $('#AtualizarNota').val(quest.get(index)['FeedbackViewModel']['Nota']);
        }
    }

    $("#modalAtualizarTask").modal('show');
}

function showFeedbackModal(index) {
    $('#modalAtualizarTask').modal('hide');
    $('#modalAtualizarTask').on('hidden.bs.modal', function () {
        if (index != undefined) {
            $('#modalCriarFeedback').modal('show');
            $('#CriarFeedback').data('index', index);
            index = undefined;
        }
        $("#AtualizarTask").data('index', undefined);
    });
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
            
            quest.Id = result.data.Id
            quest.Nome = result.data.Nome;
            quest.Descricao = result.data.Descricao;
            quest.Cor = result.data.Cor;
            quest.TasksViewModel = result.data.TasksViewModel;

            quest.render();
            $("#Cor").spectrum({
                preferredFormat: "hex",
                allowEmpty: false,
                chooseText: "Selecionar",
                cancelText: "",
                move: function (color) {
                    $("#Cor").val(color.toHex());
                },
                color: quest.Cor,
            });
        },
        error: function () {
            showBalloon("Algo deu errado", "yellow-alert");
        }
    });

    $("form").on('keyup keydown submit click focusout onfocus', function () {
        var errors = $('[aria-invalid=true]');
        if (errors[0] != undefined)
            $('label[for=' + $('[aria-invalid=true]')[0]['id'] + "] span").css('display', 'inline');
        for (x = 1; x < errors.length; x++) {
            $('label[for=' + $('[aria-invalid=true]')[x]['id'] + "] span").css('display', 'none');
        }
    });

    $('#modalAdicionarTask').on('hidden.bs.modal', function () {
        document.getElementById("formAdicionarTaskModal").reset();
        $("#NomeTask").val('');
        $("#DescricaoTask").text('');
        $("#DataConclusao").val('');
        $("#Dificuldade").val(0);
    })

    $('#modalCriarFeedback').on('hidden.bs.modal', function () {
        document.getElementById("formCriarFeedback").reset();
        $("#NomeTaskAtualizar").val('');
        $("#DescricaoTaskAtualizar").text('');
        $("#DataConclusaoAtualizar").val('');
        $("#DificuldadeAtualizar").val(0);
    })

    $('#formQuest').on("submit", function (e) {
        e.preventDefault();
        quest.Nome = $("#Nome").val();
        quest.Descricao = $("#Descricao").val();
        quest.Cor = $("#Cor").val();
        $.ajax({
            contentType: 'application/json;',
            type: "POST",
            url: "/Quest/AtualizarQuest",
            data: JSON.stringify(quest),
            success: function (result) {
                alert(JSON.stringify(quest));
                if (result == "true")
                    window.location.href = "/Home/Inicio";
                else
                    showBalloon(result, "yellow-alert");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR + ", " + textStatus + ", " + errorThrown);
                showBalloon("Algo deu errado", "yellow-alert");
            }
        });
    });

    $('#AdicionarTask').click(function (event) {
        event.preventDefault();
        if ($("#formAdicionarTaskModal").valid()) {
            quest.add({
                'Nome': $('#NomeTask').val(),
                'Descricao': $('#DescricaoTask').val(),
                'DataConclusao': $('#DataConclusao').val(),
                'Dificuldade': $('#Dificuldade').val(),
                'Status': 0
            });
            quest.Nome = $("#Nome").val();
            quest.Descricao = $("#Descricao").val();
            quest.Cor = $("#Cor").val();
            quest.render()
            $('#modalAdicionarTask').modal('hide');
        }
    });

    $('#AtualizarTask').click(function (event) {
        event.preventDefault();
        if ($("#formAtualizarTaskModal").valid()) {
            var index = $("#AtualizarTask").data('index');
            if (quest.get(index)['FeedbackViewModel'] != undefined && quest.get(index)['FeedbackViewModel'] != null) {
                quest.TasksViewModel[index].FeedbackViewModel.Resposta = $('#AtualizarTextoFeedback').val();
                quest.TasksViewModel[index].FeedbackViewModel.Nota = parseInt($('#AtualizarNota').val());
            }

            quest.TasksViewModel[index].Nome = $('#NomeTaskAtualizar').val();
            quest.TasksViewModel[index].Descricao = $('#DescricaoTaskAtualizar').val();
            quest.TasksViewModel[index].DataConclusao = $('#DataConclusaoAtualizar').val();
            quest.TasksViewModel[index].Dificuldade = $('#DificuldadeAtualizar').val();

            quest.Nome = $("#Nome").val();
            quest.Descricao = $("#Descricao").val();
            quest.Cor = $("#Cor").val();
            quest.render();
            $('#modalAtualizarTask').modal('hide');
        }
    });

    $('#ExcluirTask').click(function () {
        event.preventDefault();
        quest.remove($('#ExcluirTask').data('index'));
        quest.Nome = $("#Nome").val();
        quest.Descricao = $("#Descricao").val();
        quest.Cor = $("#Cor").val();
        quest.render();
        $('#modalAtualizarTask').modal('hide');
    });

    $('#CriarFeedback').click(function () {
        event.preventDefault();
        quest.setProp($('#CriarFeedback').data('index'), 'FeedbackViewModel', {
            'Resposta': $('#TextoFeedback').val(),
            'Nota': parseInt($('#Nota').val())
        });
        $('#modalCriarFeedback').modal('hide');
    });

    $("#formQuest").validate({
        errorPlacement: function (error, element) {
            $(element)
			.closest("form")
			.find("label[for='" + element.attr("id") + "']")
			.append(error);
        },
        errorElement: "span",
        rules: {
            Nome: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Descricao: {
                required: true,
                maxlength: 120
            }
        }
    });

    $('#formAdicionarTaskModal').validate({
        errorPlacement: function (error, element) {
            $(element)
			.closest("form")
			.find("label[for='" + element.attr("id") + "']")
			.append(error);
        },
        errorElement: "span",
        rules: {
            NomeTask: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            DescricaoTask: {
                required: true,
                minlength: 3,
                maxlength: 120
            },
            DataConclusao: {
                required: true,
                date: true,
                futureDate: true
            }
        },
    });

    $('#formAtualizarTaskModal').validate({
        errorPlacement: function (error, element) {
            $(element)
			.closest("form")
			.find("label[for='" + element.attr("id") + "']")
			.append(error);
        },
        errorElement: "span",
        rules: {
            NomeTaskAtualizar: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            DescricaoTaskAtualizar: {
                required: true,
                minlength: 3,
                maxlength: 120
            },
            DataConclusaoAtualizar: {
                required: true,
                date: true,
                futureDate: true
            }
        },
    });

    $('formCriarFeedback').validate({
        errorPlacement: function (error, element) {
            $(element)
			.closest("form")
			.find("label[for='" + element.attr("id") + "']")
			.append(error);
        },
        errorElement: "span",
        rules: {
            TextoFeedback: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
        },
    });

});
