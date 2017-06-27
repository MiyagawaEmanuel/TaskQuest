function submit(id, action){
	$('#' + id).attr('action', action).submit();
}

function showAtualizarTaskModal(index){
	$("#NomeTaskAtualizar").val(get(index)["Nome"]);
	$("#DescricaoTaskAtualizar").text(get(index)["Descricao"]);
	$("#DataConclusaoAtualizar").val(get(index)["DataConclusao"].split('/').reverse().join('-'));
	$("#DificuldadeAtualizar").val(get(index)["Dificuldade"])
	$("#AtualizarTask").data("index", index);
	$("#ExcluirTask").data("index", index);
	$("#modalAtualizarTask").modal('show');
}

$(document).ready(function() {

	$("form").on('keyup keydown submit click focusout onfocus', function(){
		var errors = $('[aria-invalid=true]');
		if(errors[0]!=undefined)
			$('label[for='+$('[aria-invalid=true]')[0]['id']+"] span").css('display', 'inline');
		for(x = 1; x < errors.length; x++){
			$('label[for='+$('[aria-invalid=true]')[x]['id']+"] span").css('display', 'none');
		}
	});

	$('#modalAdicionarTask').on('hidden.bs.modal', function(){
		document.getElementById("formAdicionarTaskModal").reset();
		$("#NomeTask").val('');
		$("#DescricaoTask").text('');
		$("#DataConclusao").val('');
		$("#Dificuldade").val(0);
	})

	$('#modalAtualizarTask').on('hidden.bs.modal', function(){
		document.getElementById("formAtualizarTaskModal").reset();
		$("#NomeTaskAtualizar").val('');
		$("#DescricaoTaskAtualizar").text('');
		$("#DataConclusaoAtualizar").val('');
		$("#DificuldadeAtualizar").val(0);
	})

	len = $('[data-length]').attr('data-length') | 0;

	var tasks = Array()

	add = function(task){
		tasks.push(task);
	}

	remove = function(index){
		tasks.splice(0, index+1);
		len--;
	}

	get = function(index){
		return tasks[index];
	}

	set = function(index, doc){
		tasks[index] = doc;
	}

	getAll = function(){
		return tasks;
	}

	$('#formQuest').on("submit",function(e) {
		e.preventDefault();
        //Colocar a lógica do ajax aqui!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    });

	$('#AdicionarTask').click(function(event) {
		event.preventDefault();
		if($("#formAdicionarTaskModal").valid()){
			add({
				'Nome': $('#NomeTask').val(),
				'Descricao': $('#DescricaoTask').val(),
				'DataConclusao': $('#DataConclusao').val(),
				'Dificuldade' : $('#Dificuldade').val()
			});

			var content = 	"<div class='margin-bottom item' id="+len+">"+
								"<a onclick='showAtualizarTaskModal("+len+")'><div class='tory-blue-bg filete'></div></a>"+
								"<div class='quest-body flex-properties-c'>"+
									"<div class='icon-black'>"+
										"<a onclick='showAtualizarTaskModal("+len+")'>"+
											"<h4 class='Nome'>"+get(len)['Nome']+"</h4>"+
										"</a>"+
									"</div>"+
									"<div>"+
										"<h4 class='Descricao'>"+get(len)["Descricao"]+"</h4>"+
									"</div>"+
									"<div>"+
										"<h4 class='DataConclusao'>"+get(len)["DataConclusao"].split('-').reverse().join('/')+"</h4>"+
									"</div>"+
									"<div class='select-container'>"+
										"<select id='Status' class='form-control'>"+ 
											"<option value='0'>A Fazer</option>"+
											"<option value='1'>Fazendo</option>"+
											"<option value='2'>Feito</option>"+
										"</select>"+
									"</div>"+
								"</div>"+
							"</div>";

			$('#task-container').append(content);
			$('#modalAdicionarTask').modal('hide');			
			len++;
		}
	});

	$('#AtualizarTask').click(function(event) {
		event.preventDefault();
		if($("#formAtualizarTaskModal").valid()){
			
			set($('#AtualizarTask').data('index'), {
				'Nome': $('#NomeTaskAtualizar').val(),
				'Descricao': $('#DescricaoTaskAtualizar').val(),
				'DataConclusao': $('#DataConclusaoAtualizar').val(),
				'Dificuldade' : $('#DificuldadeAtualizar').val()
			})

			$("#"+$('#AtualizarTask').data('index')+" .Nome").text($('#NomeTaskAtualizar').val());
			$("#"+$('#AtualizarTask').data('index')+" .Descricao").text($('#DescricaoTaskAtualizar').val());
			$("#"+$('#AtualizarTask').data('index')+" .DataConclusao").text($('#DataConclusaoAtualizar').val());

			$('#modalAtualizarTask').modal('hide');	
		}
	});

	$('#ExcluirTask').click(function(){
		event.preventDefault();
		remove($('#AtualizarTask').data('index'));
		$("#"+$('#AtualizarTask').data('index')).remove();

		$('#modalAtualizarTask').modal('hide');	
	});

	$("#formQuest").validate({
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
			Descricao: {
				required: true,
				maxlength: 120
			}
		}
	});

	$('#formAdicionarTaskModal').validate({		
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
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
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
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

	for(x = 0;; x++){
		if($('#'+x)[0] != undefined){
			add({
				'Nome': $('#'+x+' '+'.NomeTask').text(),
				'Descricao': $('#'+x+' '+'.DescricaoTask').text(),
				'DataConclusao': $('#'+x+' '+'.DataConclusao').text(),
				'Dificuldade' : $('#'+x+' '+'.Dificuldade').val()
			});
		}
		else
			break;
	}

});