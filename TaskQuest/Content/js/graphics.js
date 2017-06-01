window.onload = function () {
  var chart = new CanvasJS.Chart("chartContainer",{
	  title: {
		text: "Gráfico de Gantt das últimas tarefas"
	  },
	  axisX: {
		minimum: 5,
		maximum: 120
	  },
	  data: [
	  {
		type: "spline",
		dataPoints: [
			{ x: 10, y: 71 },
			{ x: 20, y: 55 },
			{ x: 30, y: 50 },
			{ x: 40, y: 65 },
			{ x: 50, y: 95 },
			{ x: 60, y: 68 },
			{ x: 70, y: 28 },
			{ x: 80, y: 34 },
			{ x: 90, y: 14 }
		  ]
		}					
	  ]
	});
	chart.render();
	
	jQuery(".canvasjs-chart-canvas").last().on("click", 
		function(e){
			var parentOffset = $(this).parent().offset();
			var relX = e.pageX - parentOffset.left;
			var relY = e.pageY - parentOffset.top
			var xValue = Math.round(chart.axisX[0].convertPixelToValue(relX));
			var yValue = Math.round(chart.axisY[0].convertPixelToValue(relY));
		
			chart.data[0].addTo("dataPoints", {x: xValue, y: yValue});
			chart.axisX[0].set("maximum", Math.max(chart.axisX[0].maximum, xValue + 30));
		});
}