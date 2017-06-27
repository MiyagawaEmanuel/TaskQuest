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
		this.tasks.splice(0, index+1);
	},

	get: function(index){
		return this.tasks[index];
	},

	set: function(prop, doc){
		this[prop] = doc;
	},

	set: function(index, prop, doc){
		this.tasks[index][prop] = doc;
	},

	getAll: function(){
		return this.tasks;
	},


	render: function(){
		$('#Nome').val(this.Nome);
		$('#Descricao').text(this.Descricao);
		$('#task-container div').remove();
		var len = this.tasks.length - 1;
		for(var x = 0; x < this.tasks.length; x++){
			var content = 	"<div class='margin-bottom item' id="+len+">"+
								"<a onclick='showAtualizarTaskModal("+len+")'><div class='tory-blue-bg filete'></div></a>"+
								"<div class='quest-body flex-properties-c'>"+
									"<div class='icon-black limit-lines'>"+
										"<a onclick='showAtualizarTaskModal("+len+")'>"+
											"<h4 class='Nome'>"+this.get(len)['Nome']+"</h4>"+
										"</a>"+
									"</div>"+
									"<div class='limit-lines'>"+
										"<h4 class='Descricao'>"+this.get(len)["Descricao"]+"</h4>"+
									"</div>"+
									"<div>"+
										"<h4 class='DataConclusao'>"+this.get(len)["DataConclusao"].split('-').reverse().join('/')+"</h4>"+
									"</div>"+
									"<div class='select-container'>"+
										"<select id='Status' class='form-control' onchange='mudarStatus("+len+")'>"+ 
											"<option value='0'>A Fazer</option>"+
											"<option value='1'>Fazendo</option>"+
											"<option value='2'>Feito</option>"+
										"</select>"+
									"</div>"+
								"</div>"+
							"</div>";
			$("#task-container").append(content);
			$('#Status').val(this.get(len)["Status"]);
		}
	}
}

function mudarStatus(index){
	quest.set(index, "Status", $("#"+index+" #Status").val());
}

function submit(id, action){
	$('#' + id).attr('action', action).submit();
}

function showAtualizarTaskModal(index){
	$("#NomeTaskAtualizar").val(quest.get(index)["Nome"]);
	$("#DescricaoTaskAtualizar").text(quest.get(index)["Descricao"]);
	$("#DataConclusaoAtualizar").val(quest.get(index)["DataConclusao"].split('/').reverse().join('-'));
	$("#DificuldadeAtualizar").val(quest.get(index)["Dificuldade"])
	$("#AtualizarTask").val(index);
	$("#ExcluirTask").val(index);
	$("#modalAtualizarTask").modal('show');
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

	$('#modalAtualizarTask').on('hidden.bs.modal', function(){
		document.getElementById("formAtualizarTaskModal").reset();
		$("#NomeTaskAtualizar").val('');
		$("#DescricaoTaskAtualizar").text('');
		$("#DataConclusaoAtualizar").val('');
		$("#DificuldadeAtualizar").val(0);
	})

	$('#formQuest').on("submit",function(e) {
		e.preventDefault();
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
			quest.set(this.value, {
				'Nome': $('#NomeTaskAtualizar').val(),
				'Descricao': $('#DescricaoTaskAtualizar').val(),
				'DataConclusao': $('#DataConclusaoAtualizar').val(),
				'Dificuldade' : $('#DificuldadeAtualizar').val()
			})
			quest.render();
			$('#modalAtualizarTask').modal('hide');	
		}
	});

	$('#ExcluirTask').click(function(){
		event.preventDefault();
		quest.remove(this.value);
		quest.render();
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

});