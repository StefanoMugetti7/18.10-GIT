<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcesosDatosModificarDatos.ascx.cs" Inherits="IU.Modulos.ProcesosDatos.Controles.ProcesosDatosModificarDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" tagname="popUpGrillaGenerica" tagprefix="auge" %>

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
            url: "ProcesosDatosWS.asmx/ObetenerMensajes",
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
            url: "ProcesosDatosWS.asmx/Procesando",
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
            html = data[i].text;
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

<div class="ProcesosDatosModificarDatos">
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
         <div class="card">
            <div class="card-header">
                <div class="form-group row">
                    <div class="col-sm-1"></div>
                    <asp:Label CssClass="col-sm-3 col-form-label font-weight-bold text-right" ID="lblNombre" runat="server" Text="Proceso"></asp:Label>
                    <asp:Label CssClass="col-sm-4 col-form-label font-weight-bold" ID="lblNombreProceso" runat="server"></asp:Label>
                    <div class="col-sm-3"></div>
                </div>
            </div>
             <div class="card-body">     
        <asp:Panel ID="tablaParametros" CssClass="form-group" runat="server">
        </asp:Panel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="pnlArchivo" runat="server">
        <asp:Label CssClass="labelEvol" ID="Label1" runat="server" Text="Seleccione un archivo"></asp:Label>
        <asp:AsyncFileUpload ID="AsyncFileUpload1" Width="211px" runat="server" 
              OnUploadedComplete="FileUploadComplete" Height="21px" Font-Size="Larger"   />
        </asp:Panel>
        <br />
        <div id="myProgress" class="myProgress">
              <div id="myBar" class="myProgress"></div>
        </div>
        <center>
            <div id="divInicio" ></div>
            <div id="divStatus" ></div>
            <br />
        </center>
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
        <center>
            <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
            <auge:popUpMensajesPostBack ID="ctrMensajesPostBack" runat="server" />
            <asp:ImageButton ID="btnExportarExcel" Visible="false" ImageUrl="~/Imagenes/Excel-icon.png" 
                            runat="server" onclick="btnExportarExcel_Click" ToolTip="Descargar en formato Excel" AlternateText="Descargar en formato Excel"/>
            &nbsp;&nbsp;
            <input type="button" runat="server" class="botonesEvol" id="btnProcesar" value="Procesar" onclick="fnEmpezarProceso();" />
            <asp:Button CssClass="botonesEvol" ID="btnContinuar" OnClick="btnContinuar_Click" runat="server" CausesValidation="false" 
                     Text="Procesar Hidden" style="display:none"/>
            <asp:Button CssClass="botonesEvol" ID="btnFinalizar" OnClick="btnFinalizarProceso_Click" runat="server" CausesValidation="false" 
                     Text="Finalizar Hidden" style="display:none" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" Text="Volver" CausesValidation="false"            
                    />
        </center>
    </ContentTemplate>
    <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
</asp:UpdatePanel>
<asp:Accordion ID="Accordion1" runat="server"
    SelectedIndex="-1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
    ContentCssClass="accordionContent" AutoSize="None" FadeTransitions="true"
    TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>
        <asp:AccordionPane ID="AccordionPane1" runat="server"
        HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected"
        ContentCssClass="accordionContent"
        >
        <Header>Detalle de estructura del Archivo</Header>
        <Content>
            <asp:GridView ID="gvArchivo" runat="server" AutoGenerateColumns="false"  
                DataKeyNames="IndiceColeccion" SkinID="GrillaBasicaFormal">
                <Columns>
                    <asp:BoundField DataField="RowDelimitator" HeaderText="Limitador Fila" />
                    <asp:BoundField DataField="FieldDelimitator" HeaderText="Limitador Campo"  />
                    <asp:BoundField DataField="CabLines" HeaderText="CabLines" Visible="false" />
                    <asp:BoundField DataField="TrailLines" HeaderText="TrailLines" Visible="false"  />
                    <asp:BoundField DataField="NombreArchivo" HeaderText="Nombre Archivo" />
                    <asp:BoundField DataField="Type" HeaderText="Tipo" />
                    <asp:BoundField DataField="SheetName" HeaderText="Nombre Hoja (Excel)" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false"  
                DataKeyNames="IndiceColeccion" SkinID="GrillaBasicaFormal">
                <Columns>
                    <asp:BoundField DataField="TableField" HeaderText="Campo Tabla" />
                    <asp:BoundField DataField="DefaultValue" HeaderText="Valor por defecto" Visible="false" />
                    <asp:BoundField DataField="FileField" HeaderText="Campo Archivo" />
                    <asp:BoundField DataField="FromChar" HeaderText="Posicion desde" />
                    <asp:BoundField DataField="ToChar" HeaderText="Posicion hasta" />
                    <asp:BoundField DataField="FieldsOrder" HeaderText="Campo orden" />
                </Columns>
            </asp:GridView>
        </Content>
        </asp:AccordionPane>
    </Panes>               
</asp:Accordion>
</div>