﻿$(function () {

    $("#DataNascimento").mask("00/00/0000", { placeholder: "Dia/Mes/Ano" });

    $("#RegisterForm").validate({
        rules: {
            Nome: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Sobrenome: {
                required: true,
                minlength: 3,
                maxlength: 20
            },
            Email: {
                required: true,
                email: true,
                maxlength: 50
            },
            ConfirmarEmail: {
                required: true,
                equalTo: '#RegisterEmail'
            },
            DataNascimento: {
                required: function (element) {
                    return new Date($("#DataNascimento").val().split("/").reverse().join("-")) < Date.now();
                },
                date: true
            },
            Senha: {
                required: true
            },
            ConfirmarSenha: {
                required: true,
                equalTo: "#Senha"
            },
            Sexo: {
                required: function (element) {
                    return $("#Sexo").val() === "M" || $("#Sexo").val() === "F";
                }
            }
        },
        messages: {
            ConfirmarEmail: {
                equalTo: "Os emails não são iguais."
            },
            ConfirmarSenha: {
                equalTo: "As senhas não são iguais."
            },
            DataNascimento: {
                required: "Data de nascimento inválida."
            },
            Sexo: {
                required: "Digite um gênero válido."
            }
        }
    });

    $("#LoginForm").validate({
        rules: {
            LoginEmail: {
                required: true,
                email: true,
                maxlength: 50
            },
            LoginSenha: {
                required: true,
                maxlength: 20
            }
        }
    });
});
