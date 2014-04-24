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
    google.load("visualization", "1", { 'packages': ['corechart'], "callback": drawChart });
    google.setOnLoadCallback(drawChart);
}

function drawChart() {

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

    var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
    chart.draw(data, options);
}

        