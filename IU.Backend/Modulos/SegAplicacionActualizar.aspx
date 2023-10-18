<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SegAplicacionActualizar.aspx.cs" Inherits="IU.Modulos.Seguridad.SegAplicacionActualizar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
        fnProcesando();
        fnEjecutarProceso();        
    }
    function fnEjecutarProceso() {
        $.ajax({
            type: "POST",
            url: "SeguridadWS.asmx/ObetenerMensajes",
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
            url: "SeguridadWS.asmx/Procesando",
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
            console.log("Error en APP");
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
        console.log("Finalizar Proceso");
        move(100);
        $("body").removeClass("loading");
        $("input[type=button][id$='btnProcesar']").hide();
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

    function fnResetBar() {
        $("#myBar").removeClass("myBarError");
        $("#myBar").removeClass("myBar");
        $("#myBar").width("0%");
        $("#myBar").html("");
        var divInicio = document.getElementById("divInicio");
        divInicio.innerHTML = "";
        var divStatus = document.getElementById("divStatus");
        divStatus.innerHTML = "";
    }

</script>
     <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-2 col-form-label" ID="Label2" runat="server" Text="Tipo de Versión"></asp:Label>
            <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoVersion" runat="server" OnSelectedIndexChanged="ddlTipoVersion_SelectedIndexChanged"
                        AutoPostBack="true" >
                        <asp:ListItem Text="Seleccione una opción" Value="" Selected="True" />
                        <asp:ListItem Text="Beta" Value="StartupBeta" />
                        <asp:ListItem Text="Produccion" Value="Startup3"/>
                    </asp:DropDownList>
                </div>    
        <asp:Label CssClass="col-sm-2 col-form-label" ID="Label1" runat="server" Text="Empresa"></asp:Label>
            <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEmpresa" runat="server" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged"
                        AutoPostBack="true" />
                </div>
    </div>
     <div class="form-group row">
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblDirectorio" runat="server" Text="Directorio de Actualizacion"></asp:Label>
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblDirectorioDatos" runat="server" Text=""></asp:Label>
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblDirAplicacion" runat="server" Text="Directorio de Aplicación"></asp:Label>
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblDirAplicacionDatos" runat="server" Text=""></asp:Label>
    </div>
     <div class="form-group row">
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblVersionNueva" runat="server" Text="Versión Nueva"></asp:Label>
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblVersionNuevaDatos" runat="server" Text=""></asp:Label>
           <asp:Label CssClass="col-sm-2 col-form-label" ID="lblVersion" runat="server" Text="Version Actual:"></asp:Label>
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblVersionDatos" runat="server" Text=""></asp:Label>
       
        </div>
        <asp:GridView ID="gvDatos" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="true" ShowFooter="true">
        </asp:GridView>
        </ContentTemplate>
         </asp:UpdatePanel>
     <div id="myProgress" class="myProgress">
              <div id="myBar" class="myProgress"></div>
        </div>

      <div class="row justify-content-md-center">
            <div class="col-md-auto">
            <div id="divInicio" ></div>
            <div id="divStatus" ></div>
            </div>
          </div>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
<div class="row justify-content-md-center">
            <div class="col-md-auto">
     <input type="button" runat="server" class="botonesEvol" id="btnProcesar" value="Actualizar" onclick="fnEmpezarProceso();" />
            <asp:Button CssClass="botonesEvol" ID="btnContinuar" OnClick="btnContinuar_Click" runat="server" CausesValidation="false" 
                     Text="Procesar Hidden" style="display:none"/>
            <asp:Button CssClass="botonesEvol" ID="btnFinalizar" OnClick="btnFinalizarProceso_Click" runat="server" CausesValidation="false" 
                     Text="Finalizar Hidden" style="display:none" />
            <asp:Button CssClass="botonesEvol" ID="btnVolver" OnClientClick="fnResetBar();" OnClick="btnVolver_Click" runat="server" CausesValidation="false" 
                     Text="Volver" Visible="false" />

    </div>
</div>
        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

