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
				maxlength: 40
			},
			Sobrenome: {
				required: true,
				minlength: 3,
				maxlength: 40
			},
			DataNascimento: {
				required: true,
				date: true,
				pastDate: true
			},
			Email: {
				required: true,
				email: true,
				minlength: 10,
				maxlength: 50
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
				maxlength: 40
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
				minlength: 3,
				maxlength: 40
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
});