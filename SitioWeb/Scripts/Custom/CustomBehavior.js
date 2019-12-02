$(".customCheck").change(function () {
    //debugger;
    var cost = 0.0;
    $(".customCheck").each(function (index, element) {
        var costValue = element.getAttribute("costo");
        if (element.checked) {

        cost += parseFloat(costValue);
        }
    });
    $("#lblCostoTotal").text(cost);

});

$(document).ready(function () {
    var cost = 0.0;
    $(".customCheck").each(function (index, element) {
        var costValue = element.getAttribute("costo");
        if (element.checked) {

            cost += parseFloat(costValue);
        }
    });
    $("#lblCostoTotal").text(cost);
});