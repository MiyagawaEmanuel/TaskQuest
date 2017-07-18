function submit(id, action) {
	$('#' + id).attr('action', action).submit();
}

$(function () {

	$("form").on('keyup keydown submit click focusout onfocus', function(){
		var errors = $('[aria-invalid=true]');
		if(errors[0]!=undefined)
			$('label[for='+$('[aria-invalid=true]')[0]['id']+"] span").css('display', 'inline');
		for(x = 1; x < errors.length; x++){
			$('label[for='+$('[aria-invalid=true]')[x]['id']+"] span").css('display', 'none');
		}
	});

	$("#formUsuario").validate({
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
		},
		errorElement: "span",
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
			DataNascimento: {
				required: true,
				date: true,
				pastDate: true
			},
			Email: {
				required: true,
				email: true,
			},
			Senha: {
				required: true,
				minlength: 5,
				pwd: true,
			},
			ConfirmarSenha: {
				required: true,
				minlength: 5,
				equalTo: "#Senha",
				pwd: true,
			},
			debug: {} 
		},
		messages: {
			Senha: {
				minlength: "Senha fraca, use no mínimo 5 caracteres."
			},
			ConfirmarSenha: {
				minlength: "Senha fraca, use no mínimo 5 caracteres."
			},
			debug: {}
		}
	});

    var options = {
        onKeyPress: function (cep, e, field, options) {
		    var masks = ['(00) 0000 00000', '(00) 00000 0000'];
		    mask = (cep.length > 14) ? masks[1] : masks[0];
		    $('.TelefoneNumero').mask(mask, options);
        },
        placeholder: "Digite seu número de telefone"
    };

    $('.TelefoneNumero').mask('(00) 0000 00000', options);

	var validateTelefone = {
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
		},
		errorElement: "span",
		rules: {
			TipoTelefone: {
				required: true,
				minlength: 3,
				maxlength: 20
			},
			TelefoneNumero: {
				required: true,
				minlength: 14
			}
		},
		messages: {
			TelefoneNumero: {
				minlength: "Telefone inválido."
			}
		}
	};

	var telefoneLen = $('[data-telefones-length]').attr('data-telefones-length');
	for(x = 0; x < telefoneLen; x++){
		$("#formTelefone"+x).validate(validateTelefone);
	}

	$("#formTelefoneModal").validate({
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
		},
		errorElement: "span",
		rules: {
			TipoTelefoneModal: {
				required: true,
				minlength: 3
			},
			TelefoneNumeroModal: {
				required: true,
				minlength: 14
			}
		},
		messages: {
			TelefoneNumeroModal: {
				minlength: "Telefone inválido."
			}
		}
	});

	$('.NumeroCartao').mask('0000 0000 0000 0000');

	$('.CodigoSeguranca').mask('000');

	var validateCartao = {
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
		},
		errorElement: "span",
		rules: {
			NomeCartao: {
				required: true,
				minlength: 3,
				maxlength: 40
			},
			Bandeira: {
				required: true,
				minlength: 3,
				maxlength: 20
			},
			Numero: {
				required: true,
				minlength: 19,
				maxlength: 19
			},
			CodigoSeguranca: {
				required: true,
				minlength: 3,
				maxlength: 3
			},
			DataVencimento: {
				required: true,
				date: true,
				futureDate: true
			},
			SenhaCartao: {
				required: true,
				minlength: 3,
				maxlength: 40
			}
		},
	};

	var cartaoLen = $('[data-cartoes-length]').attr('data-cartoes-length');
	for(x = 0; x < cartaoLen; x++){
		$('#formCartao'+x).validate(validateCartao);
	}

	$('#formCartaoModal').validate({
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
		},
		errorElement: "span",
		rules: {
			NomeCartaoModal: {
				required: true,
				minlength: 3,
				maxlength: 40
			},
			BandeiraModal: {
				required: true,
				minlength: 3,
				maxlength: 20
			},
			NumeroModal: {
				required: true,
				minlength: 19,
				maxlength: 19
			},
			CodigoSegurancaModal: {
				required: true,
				minlength: 3,
				maxlength: 3
			},
			DataVencimentoModal: {
				required: true,
				date: true,
				futureDate: true
			},
			SenhaCartaoModal: {
				required: true,
				minlength: 3,
				maxlength: 40
			}
		},
	});

});