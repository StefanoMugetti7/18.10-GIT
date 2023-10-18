$(document).ready(function() {
    $(":submit").addClass("glow");
    $("submit[name$='MeNecesitaElExtender']").css("display", "none");
    $("submit[name$='btnSalir']").css("display", "none");
    $('.glow').glowbuttons();
});