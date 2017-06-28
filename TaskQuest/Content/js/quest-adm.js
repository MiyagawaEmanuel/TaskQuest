/*
	Daí no razor isso vira um construtor:

	function Quest(){

		!!!!!!!!!!!!!fazer o get ou sei lá do Controller

		<o que está dentro de quest agora>
	}

	no evento ready:

	var quest = Quest();
*/


var quest = {

	Nome: 'Fazer as coisa',
	Descricao: 'Fazer as coisa',
	tasks: [],

	add: function(task){
		this.tasks.push(task);
	},

	remove: function(index){
		this.tasks.splice(index, 1);
	},

	get: function(index){
		return this.tasks[index];
	},

	set: function(index, doc){
		this.tasks[index] = doc;
	},

	setProp: function(index, prop, doc){
		this.tasks[index][prop] = doc;
	},

	getAll: function(){
		return this.tasks;
	},


	render: function(){
		$('#Nome').val(this.Nome);
		$('#Descricao').text(this.Descricao);
		$('#task-container div').remove();
		for(var x = 0; x < this.tasks.length; x++){
			var content = 	"<div class='margin-bottom item' id="+x+">"+
								"<a onclick='showAtualizarTaskModal("+x+")'><div class='tory-blue-bg filete'></div></a>"+
								"<div class='quest-body flex-properties-c'>"+
									"<div class='icon-black limit-lines'>"+
										"<a onclick='showAtualizarTaskModal("+x+")'>"+
											"<h4 class='Nome'>"+this.get(x)['Nome']+"</h4>"+
										"</a>"+
									"</div>"+
									"<div class='limit-lines'>"+
										"<h4 class='Descricao'>"+this.get(x)["Descricao"]+"</h4>"+
									"</div>"+
									"<div>"+
										"<h4 class='DataConclusao'>"+this.get(x)["DataConclusao"].split('-').reverse().join('/')+"</h4>"+
									"</div>"+
									"<div class='select-container'>"+
										"<select class='form-control Status' onchange='mudarStatus("+x+")'>"+ 
											"<option value='0'>A Fazer</option>"+
											"<option value='1'>Fazendo</option>"+
											"<option value='2'>Feito</option>"+
										"</select>"+
									"</div>"+
								"</div>"+
							"</div>";
			$("#task-container").append(content);
			$('#'+x+' .Status').val(this.get(x)["Status"]);
		}
	}
}

function mudarStatus(index){
	var status = $("#"+index+" .Status").val();
	quest.setProp(index, "Status", status);
	if(quest.get(index)['Feedback'] != undefined)
		if(status == 0 || status == 1)
			quest.setProp(index, 'Feedback', undefined);
}

function submit(id, action){
	$('#' + id).attr('action', action).submit();
}

function showAtualizarTaskModal(index){
	$("#NomeTaskAtualizar").val(quest.get(index)["Nome"]);
	$("#DescricaoTaskAtualizar").text(quest.get(index)["Descricao"]);
	$("#DataConclusaoAtualizar").val(quest.get(index)["DataConclusao"].split('/').reverse().join('-'));
	$("#DificuldadeAtualizar").val(quest.get(index)["Dificuldade"])
	$("#AtualizarTask").data('index', index);
	$("#ExcluirTask").data('index', index);

	$('#Feedback div').remove();
	if(quest.get(index)['Status'] === '2'){
		if(quest.get(index)['Feedback'] === undefined){
			var content = "<div class='icon-black'><a onclick='showFeedbackModal("+index+")'><h4>Criar Feedback<h4></a></div>";
			$("#Feedback").append(content);
		}
		else{
			var content = 	"<div><p>Feedback</p><div>"+
								"<div class='form-group'>"+
									"<textarea class='form-control' id='AtualizarTextoFeedback' name='AtualizarTextoFeedback' placeholder='Texto do Feedback' required></textarea>"+
									"<label for='AtualizarTextoFeedback'></label>"+
								"</div>"+
								"<div class='select-container'>"+
									"<select class='form-control' id='AtualizarNota'>"+
										"<option>0</option>"+
										"<option>1</option>"+
										"<option>2</option>"+
										"<option>3</option>"+
										"<option>4</option>"+
										"<option>5</option>"+
									"</select>"+
								"</div>";
			$('#Feedback').append(content);
			$('#AtualizarTextoFeedback').val(quest.get(index)['Feedback']['Texto']);
			$('#AtualizarNota').val(quest.get(index)['Feedback']['Nota']);
		}
	}

	$("#modalAtualizarTask").modal('show');
}

function showFeedbackModal(index){
	$('#modalAtualizarTask').modal('hide');
	$('#modalAtualizarTask').on('hidden.bs.modal', function(){
		if(index != undefined){
			$('#modalCriarFeedback').modal('show');
			$('#CriarFeedback').data('index', index);
			index = undefined;
		}
		$("#AtualizarTask").data('index', undefined);	
	});
}

$(document).ready(function() {

	quest.render();

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

	$('#modalCriarFeedback').on('hidden.bs.modal', function(){
		document.getElementById("formCriarFeedback").reset();
		$("#NomeTaskAtualizar").val('');
		$("#DescricaoTaskAtualizar").text('');
		$("#DataConclusaoAtualizar").val('');
		$("#DificuldadeAtualizar").val(0);
	})

	$('#formQuest').on("submit",function(e) {
		e.preventDefault();
		alert('oi');
        //Colocar a lógica do ajax aqui!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    });

	$('#AdicionarTask').click(function(event) {
		event.preventDefault();
		if($("#formAdicionarTaskModal").valid()){
			quest.add({
				'Nome': $('#NomeTask').val(),
				'Descricao': $('#DescricaoTask').val(),
				'DataConclusao': $('#DataConclusao').val(),
				'Dificuldade' : $('#Dificuldade').val(),
				'Status': 0
			});
			quest.render()
			$('#modalAdicionarTask').modal('hide');		
		}
	});

	$('#AtualizarTask').click(function(event) {
		event.preventDefault();
		if($("#formAtualizarTaskModal").valid()){
			var index = $("#AtualizarTask").data('index');
			if(quest.get(index)['Feedback'] == undefined){
				quest.set(index, {
					'Nome': $('#NomeTaskAtualizar').val(),
					'Descricao': $('#DescricaoTaskAtualizar').val(),
					'DataConclusao': $('#DataConclusaoAtualizar').val(),
					'Dificuldade' : $('#DificuldadeAtualizar').val(),
					'Status' : $('#'+index+' .Status').val() 
				})
			}
			else{
				quest.set(index, {
					'Nome': $('#NomeTaskAtualizar').val(),
					'Descricao': $('#DescricaoTaskAtualizar').val(),
					'DataConclusao': $('#DataConclusaoAtualizar').val(),
					'Dificuldade' : $('#DificuldadeAtualizar').val(),
					'Status' : $('#'+index+' .Status').val(),
					'Feedback': {
						'Texto': $('#AtualizarTextoFeedback').val(),
						'Nota': $('#AtualizarNota').val()
					}
				})
			}
			quest.render();
			$('#modalAtualizarTask').modal('hide');
		}
	});

	$('#ExcluirTask').click(function(){
		event.preventDefault();
		quest.remove($('#ExcluirTask').data('index'));
		quest.render();
		$('#modalAtualizarTask').modal('hide');	
	});

	$('#CriarFeedback').click(function(){
		event.preventDefault();
		quest.setProp($('#CriarFeedback').data('index'), 'Feedback', {
			'Texto': $('#TextoFeedback').val(),
			'Nota': $('#Nota').val()
		});
		$('#modalCriarFeedback').modal('hide');
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

	$('formCriarFeedback').validate({
		errorPlacement: function(error, element) {
			$( element )
			.closest( "form" )
			.find( "label[for='" + element.attr( "id" ) + "']" )
			.append( error );
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