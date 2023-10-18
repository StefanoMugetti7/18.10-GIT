function fnEmpezarProcesoEnviarMails() {
    $("body").addClass("loading");
    $("#myBar").removeClass("myBarError");
    $("#myBar").addClass("myBar");
    $("#myBar").width("0%");
    $("#myBar").html("");
    var divInicio = document.getElementById("divInicio");
    divInicio.innerHTML = "No salga de la pagina hasta que el proceso finalice";
    move(1);
    $("input[type=button][id$='btnEnviarMailAceptar']").hide();
    $("input[type=submit][id$='btnEnviarMailProceso']").click();
    fnEjecutarProceso();
}

function fnEjecutarProceso() {
    $.ajax({
        type: "POST",
        url: ResolveUrl("~/Modulos/Comunes/EnviarMailsWS.asmx/ObetenerProgressBar"),
        //data: "{ name: '" + message + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        error: function (xhr, errorType, exception) {
            procesando = "#ERROR";
            var responseText = $.parseJSON(xhr.responseText);
            var divStatus = document.getElementById("divStatus");
            divStatus.innerHTML = "<span style='color:red;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
            fnErrorValidaciones();
        },
        failure: function (xhr, errorType, exception) {
            procesando = "#ERROR";
            var responseText = $.parseJSON(xhr.responseText);
            var divStatus = document.getElementById("divStatus");
            divStatus.innerHTML = "<span style='color:red;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
            fnErrorValidaciones();
        }
    });
}

function OnSuccess(result) {
    var divStatus = $("[id$='divStatus']");
    var data = result.d;
    var html = data.text;
    var num = data.number;
    var result = data.result;
    divStatus.html(html);
    if (!result) {
        fnErrorValidaciones();
        return;
    }
    if (num < 100) {
        move(num);
        setTimeout('', 3000);
        fnEjecutarProceso();
    }
    else {
        fnFinalizarProceso();
    }
}

function fnFinalizarProceso() {
    move(100);
    var divInicio = document.getElementById("divInicio");
    divInicio.innerHTML = "El proceso finalizo de forma correcta";
    $("body").removeClass("loading");
    $("input[type=submit][id$='btnFinalizar']").click();
}

function move(width) {
    var elem = document.getElementById("myBar");
    var barWith = elem.style.width;
    barWith = barWith.replace('%', '');
    var id = setInterval(frame, 10);
    function frame() {
        if (parseInt(barWith) >= parseInt(width)) {
            clearInterval(id);
        } else {
            barWith++;
            elem.style.width = barWith + '%';
            elem.innerHTML = barWith * 1 + '%';
        }
    }
}

function fnErrorValidaciones() {
    $("body").removeClass("loading");
    $("#myBar").removeClass("myBar");
    $("#myBar").addClass("myBarError");
    move(100);
    $("input[type=button][id$='btnEnviarMailAceptar']").show();
}