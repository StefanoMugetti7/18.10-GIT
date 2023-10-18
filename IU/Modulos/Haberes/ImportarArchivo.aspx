<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ImportarArchivo.aspx.cs" Inherits="IU.Modulos.Cardex.ImportarArchivo" Title="" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script type="text/javascript">
    var procesando=""; ;
    function fnEmpezarProceso() {
        procesando = "";
        $("body").addClass("loading");
        $("#myBar").removeClass("myBarError");
        $("#myBar").addClass("myBar");
        $("#myBar").width("0%");
        $("#myBar").html("");
        var divInicio = document.getElementById("divInicio");
        divInicio.innerHTML = "No salga de la pagina hasta que el proceso finalice";
        move(1);
        $("input[type=button][id$='btnProcesar']").hide();
        $("input[type=submit][id$='btnContinuar']").click();
        fnEjecutarProceso();        
    }
    function fnEjecutarProceso() {
        $.ajax({
            type: "POST",
            url: ResolveUrl("~/Modulos/ProcesosDatos/ProcesosDatosWS.asmx/ObetenerMensajes"),
            //data: "{ name: '" + message + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            error: function (xhr, errorType, exception) {
                procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                $("body").removeClass("loading");
                $("input[type=button][id$='btnProcesar']").show();
            },
            failure: function (xhr, errorType, exception) {
                procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                $("body").removeClass("loading");
                $("input[type=button][id$='btnProcesar']").show();
            }
        });
    }

    function fnProcesando() {
        $.ajax({
            type: "POST",
            url: ResolveUrl("~/Modulos/ProcesosDatos/ProcesosDatosWS.asmx/Procesando"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                procesando = r.d;
            },
            error: function (xhr, errorType, exception) {
                procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                $("body").removeClass("loading");
                $("input[type=button][id$='btnProcesar']").show();
            },
            failure: function (xhr, errorType, exception) {
                procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                $("body").removeClass("loading");
                $("input[type=button][id$='btnProcesar']").show();
            }
        });
    }

    var procPendiente = 0;
    function OnSuccess(result) {
        var divStatus = document.getElementById("divStatus");
        var data = result.d;
        var html = "";
        var num = 0;
        for (var i = 0; i < data.length; i ++) {
            html = html + data[i].text + "<BR />"
            num = data[i].number;
        }
        divStatus.innerHTML = html;
        move(num);
        setTimeout('', 2000);
        fnProcesando();
        if (procesando == "#FINALIZADO") {
            fnFinalizarProceso();
        } else if (procesando == "#PROCESANDO") {
            fnEjecutarProceso();
        } else if (procesando == "#ERROR") {
            $("#myBar").removeClass("myBar");
            $("#myBar").addClass("myBarError");
            $("input[type=button][id$='btnProcesar']").show();
            fnFinalizarProceso();
        } else {
            procPendiente++;
            if (parseInt(procPendiente) == 40) {
                procPendiente = 0;
                fnFinalizarProceso()
            } else {
                fnEjecutarProceso();
            }
        }
    }

    function fnFinalizarProceso() {
        move(100);
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
        $("input[type=button][id$='btnProcesar']").show();
    }

</script>

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAnio" runat="server" Text="Anio" />
        <div class="col-sm-2">
            <asp:DropDownList CssClass="form-control select2" ID="ddlAnios" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMes" runat="server" Text="Mes" />
        <div class="col-sm-2">
            <asp:DropDownList CssClass="form-control select2" ID="ddlMeses" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipo" runat="server" Text="Tipo" />
        <div class="col-sm-2">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipo" runat="server" />
        </div>
    </div>
    <AUGE:Archivos ID="ctrArchivos" runat="server" />
    <div class="row justify-content-md-center">
        <div class="col-md-auto w-100">
            <div id="myProgress" class="myProgress">
                <div id="myBar" class="myProgress"></div>
            </div>
        </div>
    </div>
    <div class="row justify-content-md-center">
        <div class="col-md-auto">
            <div id="divInicio"></div>
            <div id="divStatus"></div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
    <div class="row justify-content-md-center">
        <div class="col-md-auto">
            <input type="button" runat="server" class="botonesEvol" id="btnProcesar" value="Importar Archivo IAF" onclick="fnEmpezarProceso();" />
            <asp:Button CssClass="botonesEvol" ID="btnContinuar" OnClick="btnContinuar_Click" runat="server" CausesValidation="false" 
                     Text="Procesar Hidden" style="display:none"/>
            <asp:Button CssClass="botonesEvol" ID="btnFinalizar" OnClick="btnFinalizarProceso_Click" runat="server" CausesValidation="false" 
                     Text="Finalizar Hidden" style="display:none" />
        </div>
    </div>
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

