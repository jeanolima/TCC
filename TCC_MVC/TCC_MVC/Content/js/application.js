(function ($, window) {

    $(document).ready(function () {
        $("input[name='OrderByType']").click(function () {
            if ($(this).attr("value") == "evolution") {
                $(".orderBy").removeClass("hidden");
            }
            else {
                $(".orderBy").addClass("hidden");
            }
        });

        $("input[name='KeyType']").click(function () {
            if ($(this).attr("value") == "group") {
                $(".keyType").removeClass("hidden");
            }
            else {
                $(".keyType").addClass("hidden");
            }
        });

        $("#uncheck").click(function () {
            $("input[type='radio']").attr("checked", false);
            $("input[type='checkbox']").attr("checked", false);
        });
    });

    $(window).load(init);
})(jQuery, window);

function init() {
    google.load("visualization", "1", { 'packages': ['corechart'], "callback": drawChartEvolution });
    google.setOnLoadCallback(drawChartEvolution);

    google.load("visualization", "1", { 'packages': ['corechart'], "callback": drawChartQualis });
    google.setOnLoadCallback(drawChartQualis);

}

function drawChartEvolution() {

    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Year');
    data.addColumn('number', $("#chartKeyword").val());

    var years = $("input[name='chartYear']");
    var totals = $("input[name='chartTotal']");
    var yearsArray = [];
    
    $("input[name='chartYear']").each(function () {
        yearsArray.push($(this).val()); 
    });
    
    var smalestYear = Math.min.apply(Math,yearsArray);
    
    for (var i = 0; i < years.length; i++){
        data.addRows([
            [$(years[i]).val(), parseInt($(totals[i]).val())]
        ]);
    }

    var options = {
        title: $("#chartTitle").val(),
        hAxis: { title: 'Ano', titleTextStyle: { color: '#333' }, viewWindow: { min: 0.5 } },
        vAxis: { viewWindow: { min: 0, } }
    };

    var chart = new google.visualization.LineChart(document.getElementById('chart_evolution'));
    chart.draw(data, options);
}

function drawChartQualis() {

    var data = new google.visualization.DataTable();

    data.addColumn('string', 'Qualis'); 
    data.addColumn('number', "Total");
    
    var qualies = $("input[name='qualis']");
    var totals = $("input[name='totalQualis']");
    var colors = ['#cc3333', '#5ca028n', '#0000ff', '#191919', '#ffcb05', '#646464', '#800080', '#f6a7bb', '#ffd700', '#006600'];

    for (var i = 0; i < qualies.length; i++) {
        //var intColor = Math.floor(Math.random() * colors.length);
        //var color = colors[intColor];
        //colors.splice(intColor, 1);
        data.addRows([
            [$(qualies[i]).val(), parseInt($(totals[i]).val())]
        ]);
    }

    var view = new google.visualization.DataView(data);
    view.setColumns([0, 1,
                     {
                         calc: "stringify",
                         sourceColumn: 1,
                         type: "string",
                         role: "annotation"
                     }]);
    var width = i * 95;
    var options = {
        colors: ["#CC3333", "#006600"],
        title: "Divisões dos Qualis",
        width: width,
        height: 500, 
        bar: { groupWidth: "95%" },
        legend: { position: "none" },
        vAxis: { viewWindow: { min: 0, } },
        isStacked: true
    };
    //,'yellow','blue','gold','brown','black','purple','orange'
    var chart = new google.visualization.ColumnChart(document.getElementById('chart_qualis'));
    chart.draw(view, options);
}

        