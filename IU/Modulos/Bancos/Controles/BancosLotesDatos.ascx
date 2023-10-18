<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BancosLotesDatos.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.BancosLotesDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%--<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        //$("input[type=text][id$='txtCodigo']").focus();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindEvents);
        SetTabIndexInput();
    });
    //function bindEvents(sender, args) {
    //    $("input[type=text][id$='txtImporteAPagar']").blur(function () {
    //        CalcularItem();
    //    });
    //}
    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    var gridViewId = '#<%= gvDatos.ClientID %>';
    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);

        updateSelectionLabel();
    }

    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
        CalcularItem();
    }
    function ValidarShowConfirm(ctrl, msg) {
        //if (Page_ClientValidate("Aceptar")) {
            var mensaje = msg.replace('#CantidadRegistros#', $("input[type=text][id$='txtCantidadRegistrosConciliado']").val().toString());
            var mensajeFinal = mensaje.replace('#ImporteConciliar#', " " + $("input[type=text][id$='txtImporteTotalConciliado']").val().toString());
            showConfirm(ctrl, mensajeFinal);
        //}
    }

    function ModificarEstado() {
        var gestion = $("input[id$='hdfGestionConciliar']").val()
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {

            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var lblEstado = $(this).find("[id*='lblEstadoGrilla']");

            var mensaje1 = $("input[id$='hdfMensaje1']").val()
            var mensaje2 = $("input[id$='hdfMensaje2']").val()

            if (gestion == "1") {
                if (incluir) {
                    lblEstado.text(mensaje1);
                } else {
                    lblEstado.text(mensaje2);
                }
            }
        });
    }

    function CalcularItem() {
        var subTotal = 0.00;
        var contadorRegistros = 0; 
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {

            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");

            var importeItem = $(this).find("input[id*='hdfImporteParcial']").val().replaceAll(".", "").replace(',', '.');
            if (incluir && importeItem) {
                subTotal += parseFloat(importeItem);
                contadorRegistros++;
            }
        });
        var gestion = $("input[id$='hdfGestionConciliar']").val()
      //  var a = gridViewId.find("");
        if (gestion == "0") {
            var subTotalFormat = accounting.formatMoney(subTotal, "$ ", 2, ".");
            $("#<%=gvDatos.ClientID %> [id$=lblImporteSubTotal]").text(subTotalFormat);
            $("input[type=text][id$='txtCantidadRegistros']").val(contadorRegistros);
            $("input[type=text][id$='txtImporteTotal']").val(subTotalFormat);
        }
        else {
            var subTotalFormat = accounting.formatMoney(subTotal, "$ ", 2, ".");
            $("#<%=gvDatos.ClientID %> [id$=lblImporteSubTotal]").text(subTotalFormat);
            $("input[type=text][id$='txtCantidadRegistrosConciliado']").val(contadorRegistros);
            $("input[type=text][id$='txtImporteTotalConciliado']").val(subTotalFormat);
            //if (incluir) {
            //    //COLUMNA ESTADO ->CONCILIADO
            //} else {
            //    //COLUMNA ESTADO ->RECHAZADO
            //}
            ModificarEstado();
        }
    }

</script>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>    
<div class="deshabilitarControles">
    <asp:HiddenField ID="hdfGestionConciliar" Value="0" runat="server" />
    <asp:HiddenField ID="hdfMensaje1" Value="Mensaje1" runat="server" />
    <asp:HiddenField ID="hdfMensaje2" Value="Mensaje2" runat="server" />
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNroLote" runat="server" Text="Numero de Lote" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNumeroLote" Enabled="false" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaAlta" runat="server" ErrorMessage="*"
                ControlToValidate="txtFechaAlta" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" runat="server" ErrorMessage="*"
                ControlToValidate="ddlEstado" ValidationGroup="Aceptar" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAutorizado" runat="server" Text="Fecha Autorizado" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAutorizado" Enabled="false" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaPago" runat="server" Text="Fecha Pago" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaPago" Enabled="false" runat="server" />
                   <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaPago" runat="server" ErrorMessage="*"
                ControlToValidate="txtFechaPago" ValidationGroup="Aceptar" Enabled="false"/>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Banco Cuenta" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentas" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentas" runat="server" ErrorMessage="*"
                ControlToValidate="ddlBancosCuentas" ValidationGroup="Aceptar" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadRegistros" runat="server" Text="Cantidad de Registros" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCantidadRegistros" Enabled="false" Text="0" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidadRegistros" runat="server" ErrorMessage="*"
                ControlToValidate="txtCantidadRegistros" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTotal" runat="server" Text="Importe Total" />
        <div class="col-sm-3">
            <evol:currencytextbox cssclass="form-control" id="txtImporteTotal" enabled="false" runat="server" text="0.00" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteTotal" runat="server" ErrorMessage="*"
                ControlToValidate="txtImporteTotal" ValidationGroup="Aceptar" />
            <asp:HiddenField ID="hdfImporteAPagar" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoArchivo" runat="server" Text="Tipo Registro" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoArchivo" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoArchivo" runat="server" ErrorMessage="*"
                ControlToValidate="ddlTipoArchivo" Enabled="false" ValidationGroup="Aceptar" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadRegistrosConciliados" runat="server" Visible="false" Text="Registros Conciliados" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCantidadRegistrosConciliado" Text="0" Enabled="false" Visible="false" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTotalConciliado" runat="server" Visible="false" Text="Importe Conciliado" />
        <div class="col-sm-3">
            <evol:currencytextbox cssclass="form-control" id="txtImporteTotalConciliado" enabled="false" visible="false" runat="server" text="0.00" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
        <div class="col-sm-7">
            <asp:TextBox CssClass="form-control" ID="txtObservacion" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNroTramite" runat="server" Visible="false" Text="Numero de Tramite" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNroTramite" Enabled="false" Visible="false" runat="server" />
                      <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNroTramite" Enabled="false" runat="server" ErrorMessage="*"
                ControlToValidate="txtNroTramite" ValidationGroup="Aceptar" />
        </div>
    </div>
          </ContentTemplate>
        </asp:UpdatePanel>
    <asp:UpdatePanel ID="upPanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>     
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Detalles">
            <contenttemplate>
                <div class="form-group row">
                    <%--         <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiltroDetalles" visible="false" runat="server" Text="FiltroDetalles"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox  CssClass="form-control" ID="txtFiltro" visible="false" runat="server"></asp:TextBox>
            </div>--%>
                    <div class="col-sm-4">
                        <div class="row">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblFechaDesde" runat="server" Text="Desde"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblFechaHasta" runat="server" Text="Hasta"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Operacion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscarFiltro" runat="server" Text="Buscar"
                            OnClick="btnBuscarFiltro_Click" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                    <columns>
                        <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion">
                            <itemtemplate>
                                <%# Eval("TipoOperacion")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nro" SortExpression="IdRefTipoOperacion">
                            <itemtemplate>
                                <%# Eval("IdRefTipoOperacion")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Comprobante" ItemStyle-Wrap="false" SortExpression="FechaComprobante">
                            <itemtemplate>
                                <%# Eval("FechaComprobante", "{0:dd/MM/yyyy}")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle" SortExpression="Detalle">
                            <itemtemplate>
                                <%# Eval("Detalle")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CBU" SortExpression="CBU">
                            <itemtemplate>
                                <%# Eval("CBU")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle CBU" SortExpression="CBUDetalle">
                            <itemtemplate>
                                <%# Eval("CBUDetalle")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cuit" SortExpression="Cuit">
                            <itemtemplate>
                                <%# Eval("Cuit")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <itemtemplate>
                                <asp:Label CssClass="col-form-label" ID="lblEstadoGrilla" Visible="true" Enabled="false" Text='<%# Eval("Estado.Descripcion")%>' runat="server"></asp:Label>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" SortExpression="ImporteTotal" ItemStyle-HorizontalAlign="Right">
                            <itemtemplate>
                                <%# Eval("ImporteTotal","{0:C2}")%>
                                <%--<evol:currencytextbox cssclass="form-control" id="txtImporteParcial" allownegative="true" itemstyle-wrap="false" runat="server" enabled="false" text='<%#Bind("ImporteTotal", "{0:C2}") %>'></evol:currencytextbox>--%>
                                <asp:HiddenField ID="hdfImporteParcial" Value='<%#Bind("ImporteTotal","{0:N2}") %>' runat="server" />
                            </itemtemplate>
                            <footertemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteSubTotal" runat="server" Text="0.00" FooterStyle-Wrap="false" ItemStyle-Wrap="false"></asp:Label>
                            </footertemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <headertemplate>
                                <asp:CheckBox ID="chkIncluirTodos" runat="server" onclick="checkAllRow(this); CalcularItem();" Visible="false" Text="Todo" TextAlign="Left" />
                            </headertemplate>
                            <itemtemplate>
                                <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this); CalcularItem();" Visible="false" Checked='<%# Convert.ToBoolean (Eval("IncluirEnLote")) %>' runat="server" />
                            </itemtemplate>
                        </asp:TemplateField>
                    </columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
        </ContentTemplate>
        </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <%--<auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                <asp:ImageButton ID="btnExportarTxt" CausesValidation="true" Visible="false" ImageUrl="~/Imagenes/txt-icon.png" runat="server" onclick="btnExportarTxt_Click" ToolTip="Descargar en formato Texto" AlternateText="Descargar en formato Texto" />
                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" runat="server" OnClick="btnExportarExcel_Click" Visible="false" ToolTip="Descargar en formato Excel" AlternateText="Descargar en formato Excel"/>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarTxt" />
             <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>

