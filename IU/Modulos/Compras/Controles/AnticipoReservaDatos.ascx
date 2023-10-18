<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnticipoReservaDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.AnticipoReservaDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitReservaDestino);
        InitReservaDestino();
        SetTabIndexInput();
        initFooterGrilla();
        initDiferencia();
        $.fn.addLeadingZeros = function (length) {
            for (var el of this) {
                _value = el.value.replace(/^0+/, '');
                length = length - _value.length;
                if (length > 0) {
                    while (length--) _value = '0' + _value;
                }
                el.value = _value;
            }
        };
    });

    $(document).keypress(function (e) {
        $(":input").each(function () {
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }
        }); //quita la funcion de enter a todos los imputs
    });
    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function initFooterGrilla(){
        $("#<%=gvItems.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(0.00, gblSimbolo, gblCantidadDecimales, "."));
    }
    function initDiferencia() {
        var importeAnticipo = $("input[type=text][id$='txtTotalConIva']").maskMoney('unmasked')[0];
        $("input[type=text][id$='txtDiferencia']").val(accounting.formatMoney(importeAnticipo, gblSimbolo, gblCantidadDecimales, "."));
    }
    function CalcularItem() {
        var acumulador = 0;
        var total = 0;
        var importeAnticipo = $("input[type=text][id$='txtTotalConIva']").maskMoney('unmasked')[0];

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var importe = $(this).find("input:text[id*='txtImporteReimputar']").maskMoney('unmasked')[0];
            acumulador += importe;
        });
        $("#<%=gvItems.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(acumulador, gblSimbolo, gblCantidadDecimales, "."));
        total = importeAnticipo - acumulador;
        $("input[type=text][id$='txtDiferencia']").val(accounting.formatMoney(total, gblSimbolo, gblCantidadDecimales, "."));
    }
    function InitReservaDestino() {
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var control = $(this).find("[id*='ddlReservaDestino']");
            var hdfTabla = $(this).find("input[id*='hdfReservaDestino']");
            var hdfIdRefTabla = $(this).find("input[id*='hdfIdReservaDestino']");
            control.select2({
                placeholder: 'Ingrese algun dato de la reserva...',
                selectOnClose: true,
                minimumInputLength: 2,
                language: 'es',
                allowClear: true,
                theme: 'bootstrap4',
                width: '100%',
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/ObtenerReservasActivas', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        filtro: params.term,// search term");
                    });
                },
                beforeSend: function (xhr, opts) {
                    var algo = JSON.parse(this.data); // this.data.split('"');
                    if (isNaN(algo.filtro)) {
                        if (algo.filtro.length < 3) {
                            xhr.abort();
                        }
                    }
                    else {
                    }
                },
                processResults: function (data, params) {
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.text,
                                id: item.id,
                            }
                        })
                    };
                    cache: true
                }
            }
        });
            control.on('select2:select', function (e) {
                hdfIdRefTabla.val(e.params.data.id);
                hdfTabla.val(e.params.data.text);
            });
            control.on('select2:unselect', function (e) {
                hdfIdRefTabla.val("");
                hdfTabla.val("");
            });
        });
    }
</script>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<AUGE:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></AUGE:BuscarProveedorAjax>

<div class="card">
    <div class="card-header">
        Datos del Anticipo
    </div>
    <div class="card-body">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaContable" runat="server" Text="Fecha Contable"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaContable" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialPago" runat="server" Text="Filial Pago"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPago" Enabled="false" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilialPago" Enabled="false" ControlToValidate="ddlFilialPago" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
            <div class="col-sm-11">
                <asp:TextBox CssClass="form-control" ID="txtObservacion" runat="server" MaxLength="500" TextMode="MultiLine" />
            </div>
        </div>
    </div>
</div>
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpDetalles" HeaderText="Detalles">
        <ContentTemplate>
            <asp:UpdatePanel ID="upExcepciones" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad: "></asp:Label>
                        <Evol:CurrencyTextBox CssClass="form-control col-sm-2" ID="txtCantidadAgregar" Prefix="" NumberOfDecimals="0" ThousandsSeparator="" runat="server"></Evol:CurrencyTextBox>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvItems" OnRowCommand="gvItems_RowCommand"
                            OnRowDataBound="gvItems_RowDataBound" ShowFooter="true" DataKeyNames="IdSolicitudPago"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Reimputar a Reserva" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlReservaDestino" runat="server"></asp:DropDownList>
                                        <asp:HiddenField ID="hdfIdReservaDestino" Value='<%#Bind("IdRefTabla") %>' runat="server" />
                                        <asp:HiddenField ID="hdfReservaDestino" Value='<%#Bind("Tabla")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe a Reimputar" SortExpression="" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" >
                                    <ItemTemplate>
                                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteReimputar" NumberOfDecimals="2" ThousandsSeparator="." runat="server"></Evol:CurrencyTextBox>
                                    </ItemTemplate>
                                      <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text="$0,00"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                            AlternateText="Eliminar" ToolTip="Eliminar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
        <ContentTemplate>
            <AUGE:Comentarios ID="ctrComentarios" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>
<asp:UpdatePanel ID="pnTotales" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotal" runat="server" Text="Total Anticipo" />
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server" />
                <asp:HiddenField ID="hdfTotalConIva" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblDiferencia" runat="server" Text="Diferencia" />
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtDiferencia" Enabled="false" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
        <center>
            <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                onclick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </center>
    </ContentTemplate>
</asp:UpdatePanel>
