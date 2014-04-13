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