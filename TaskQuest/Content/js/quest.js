var quest = {

	Nome: 'Fazer as coisa',
	Descricao: 'Fazer as coisa',
	tasks: [],

	add: function(task){
		this.tasks.push(task);
	},

	get: function(index){
		return this.tasks[index];
	},

	render: function(){
		$('#Nome').val(this.Nome);
		$('#Descricao').text(this.Descricao);
		$('#task-container div').remove();
		var len = this.tasks.length - 1;
		for(var x = 0; x < this.tasks.length; x++){
			var content = 	"<div class='margin-bottom item' id="+len+">"+
								"<a onclick='showTaskModal("+len+")'><div class='tory-blue-bg filete'></div></a>"+
								"<div class='quest-body flex-properties-c'>"+
									"<div class='icon-black limit-lines'>"+
										"<a onclick='showTaskModal("+len+")'>"+
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

function showTaskModal(index){
	$("#NomeTask").text(quest.get(index)["Nome"]);
	$("#Descricao").text(quest.get(index)["Descricao"]);
	$("#DataConclusao").text(quest.get(index)["DataConclusao"]);
	
	switch (quest.get(index)["Dificuldade"]){
		case '0':
			dificuldade = 'Fácil';
			break;
		case '1':
			dificuldade = 'Médio';
			break;
		case '2':
			dificuldade = 'Difícil';
			break;
	}
	$("#Dificuldade").text(dificuldade);
	
	$("#modalTask").modal('show');
}

$(document).ready(function() {

	quest.render();

});