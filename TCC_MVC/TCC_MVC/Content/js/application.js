(function ($, window) {

    $(document).ready(function () {
        $("input[name='OrderByType']").click(function () {
            if ($(this).attr("value") == "evolution") {
                $(".hidden-options").removeClass("hidden");
            }
            else {
                $(".hidden-options").addClass("hidden");
            }
        });
        $("#uncheck").click(function () {
            $("input[type='radio']").attr("checked", false);
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
    
    console.log(qualies);
    console.log(totals);
    for (var i = 0; i < qualies.length; i++) {
        console.log($(qualies[i]).val());
        console.log($(totals[i]).val());
        data.addRows([
            [$(qualies[i]).val(), parseInt($(totals[i]).val())]
        ]);
    }

    var options = {
        title: "Divisões dos Qualis"
    };
    //,'yellow','blue','gold','brown','black','purple','orange'
    var chart = new google.visualization.PieChart(document.getElementById('chart_qualis'));
    chart.draw(data, options);
}

        