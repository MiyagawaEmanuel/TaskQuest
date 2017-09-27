function showBalloon(alert, classe) {
    $(".balloon").addClass(classe);
    $(".balloon").text(alert);
    $(".balloon").css("display", "block");
    $(".balloon").delay(4000).fadeOut(2000);
}

var quest = {

    tasks: [],

    add: function (task) {
        this.tasks.push(task);
    },

    remove: function (index) {
        this.tasks.splice(index, 1);
    },

    get: function (index) {
        return this.tasks[index];
    },

    set: function (index, doc) {
        this.tasks[index] = doc;
    },

    setProp: function (index, prop, doc) {
        this.tasks[index][prop] = doc;
    },

    render: function () {
        $('#task-container div').remove();
        var len = this.tasks.length - 1;
        var cor = $("#Cor").val();
        for (var x = 0; x < this.tasks.length; x++) {
            var content =   "<div class='margin-bottom item' id=" + x + ">" +
                                "<a onclick='showAtualizarTaskModal("+x+")'><div class='filete' style='background-color: "+cor+"'></div></a>" +
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
            $('#Status').val(this.get(len)["Status"]);
        }
    }
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
    $("#modalAtualizarTask").modal('show');
}

$(document).ready(function () {

    quest.render();

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

    $('#modalAtualizarTask').on('hidden.bs.modal', function () {
        document.getElementById("formAtualizarTaskModal").reset();
        $("#NomeTaskAtualizar").val('');
        $("#DescricaoTaskAtualizar").text('');
        $("#DataConclusaoAtualizar").val('');
        $("#DificuldadeAtualizar").val(0);
    })

    $('#formQuest').on("submit", function (e) {
        e.preventDefault();
        $.each(quest.tasks, function (index, value) {
            quest.setProp(index, "DataConclusao", new Date(value.DataConclusao));
        });
        $.ajax({
            contentType: 'application/json;',
            type: "POST",
            url: "/Quest/CriarQuestPost",
            data: JSON.stringify({
                "Nome": $("#Nome").val(),
                "Descricao": $("#Descricao").val(),
                "Cor": $("#Cor").val(),
                "GrupoCriadorId": $("#GrupoCriadorId").val(),
                "TasksViewModel": quest.tasks
            }),
            success: function (response) {
                if (response == "true")
                    window.location.href = "/Home/Inicio";
                else
                    showBalloon(response, "yellow-alert");
            },
            error: function () {
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
            quest.render()
            $('#modalAdicionarTask').modal('hide');
        }
    });

    $('#AtualizarTask').click(function (event) {
        event.preventDefault();
        if ($("#formAtualizarTaskModal").valid()) {
            quest.set($("#AtualizarTask").data('index'), {
                'Nome': $('#NomeTaskAtualizar').val(),
                'Descricao': $('#DescricaoTaskAtualizar').val(),
                'DataConclusao': $('#DataConclusaoAtualizar').val(),
                'Dificuldade': $('#DificuldadeAtualizar').val(),
                'Status': $('#' + $("#AtualizarTask").data('index') + ' .Status').val()
            })
            quest.render();
            $('#modalAtualizarTask').modal('hide');
        }
    });

    $('#ExcluirTask').click(function () {
        event.preventDefault();
        quest.remove($('#ExcluirTask').data('index'));
        quest.render();
        $('#modalAtualizarTask').modal('hide');
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

});