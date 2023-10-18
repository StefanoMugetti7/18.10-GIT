<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CargoAfiliadoModificarDatos.ascx.cs" Inherits="IU.Modulos.Cargos.Controles.CargoAfiliadoModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        SetTabIndexInput();

    });

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function SetearImporteNichos(importe) {
        $("input[type=hidden][id$='hdfImporteCargo']").val(importe);
        $("input[type=text][id$='txtImporteCargo']").val(accounting.formatMoney(importe, gblSimbolo, 2, "."));
 
    }

    function txtControlChange() {
        $("input[type=hidden][id$='hdfImporteCargo']").val($("input[type=text][id$='txtImporteCargo']").maskMoney('unmasked')[0]);
        $("input[type=hidden][id$='hdfCantidadCuotas']").val($("input[type=text][id$='txtCantidadCuotas']").val());
        $("input[type=hidden][id$='hdfTasaInteres']").val($("input[type=text][id$='txtTasaInteres']").maskMoney('unmasked')[0]);
        CalcularTotal();
      
    }
        
    function CalcularTotal() {
        var total = 0.00;
        var interes = 0.00;
        var cuota = 0.00;
        //var hdfImporteCargo = $("input[type=hidden][id$='hdfImporteCargo']");
        var importe = $("input[type=hidden][id$='hdfImporteCargo']").val();//.maskMoney('unmasked')[0]; //.val().replace('.', '').replace(',', '.'); //remplazo . por nada y , por .;
        var cantidadCuotas = $("input[type=hidden][id$='hdfCantidadCuotas']").val();//.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .;
        var tasa = $("input[type=hidden][id$='hdfTasaInteres']").val();//.maskMoney('unmasked')[0]; //.val().replace('.', '').replace(',', '.'); //remplazo . por nada y , por .;
        if (cantidadCuotas < 1) {
            cantidadCuotas = 1;
            $("input[type=hidden][id$='hdfCantidadCuotas']").val(cantidadCuotas);
            $("input[type=text][id$='txtCantidadCuotas']").val(cantidadCuotas);
        }

        //if (cantidadCuotas && importe && tasa) {
        interes = parseFloat(parseFloat(cantidadCuotas) * parseFloat(importe) * parseFloat(tasa) / 100).toFixed(2);
        //if (parseFloat(cantidadCuotas) == 1) {
        //    interes = 0.00;
        //    tasa = 0.00;
        //}
        total = parseFloat(importe) + parseFloat(interes);
        cuota = parseFloat(parseFloat(total) / cantidadCuotas).toFixed(2);
        //}
        var txtImporteInteres = $("input[type=text][id$='txtImporteInteres']");
        if(txtImporteInteres)
            txtImporteInteres.val(accounting.formatMoney(interes, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtImporteTotal']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtImporteCuota']").val(accounting.formatMoney(cuota, gblSimbolo, 2, "."));
    }

    function CalcularTotalBonificacion() {
        var total = 0.00;
        var importe = $("input[type=hidden][id$='hdfImporteCargo']").val();//.maskMoney('unmasked')[0]; //.val().replace('.', '').replace(',', '.'); //remplazo . por nada y , por .;
        var porcentaje = $("input[type=hidden][id$='hdfPorcentaje']").val();//.maskMoney('unmasked')[0];
        var importeCargoReferencia = $("input[type=hidden][id$='hdfImporteCargoReferencia']").val(); //tomar valor del hdf
        var cantidadCuotas = $("input[type=hidden][id$='hdfCantidadCuotas']").val();//.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .;

        if (cantidadCuotas < 1) {
            cantidadCuotas = 1;
            $("input[type=hidden][id$='hdfCantidadCuotas']").val(cantidadCuotas);
            $("input[type=text][id$='txtCantidadCuotas']").val(cantidadCuotas);
        }

        if (porcentaje > 0) {
            if (porcentaje && importeCargoReferencia) {
                total = parseFloat(parseFloat(importeCargoReferencia) * parseFloat(porcentaje) / 100).toFixed(2) + parseFloat(importe);

                $("input[type=text][id$='txtImporteTotal']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));
                $("input[type=text][id$='txtImporteCuota']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));
                $("input[type=text][id$='txtImporteCargo']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));
            }
        }
    }

    function CalcularBonificacion() {
        var ddlCargoReferencia = $("select[name$='ddlCargoReferencia']");
        var idAfiliado = $("input[id$='hdfIdAfiliado']")
        $.ajax({
            type: "POST",
            url: '<%=ResolveClientUrl("~")%>/Modulos/Cargos/CargosWS.asmx/CargosAfiliadosObtenerABonificar',
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ 'filtro': ddlCargoReferencia.val(), 'idAfiliado': idAfiliado.val() }),
            beforeSend: function (xhr, opts) {
                if (ddlCargoReferencia.val() == '') {
                    xhr.abort();
                }
            },
            success: function (res) {
                $("input[id$='hdfImporteCargoReferencia']").val(res.d.ImporteCargo);
                CalcularTotalBonificacion();
            }
        });
    }
</script>

<div id="deshabilitarControles">

<asp:HiddenField ID="hdfIdAfiliado" runat="server" />
<asp:HiddenField ID="hdfIdTipoCargoAfiliadoFormaCobro" runat="server" />
<asp:HiddenField ID="hdfIdTipoCargo" runat="server" />

<asp:UpdatePanel ID="upTipoCargo" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Inicio Cargo" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" runat="server" Enabled="false" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"  />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTasaInteres" runat="server" Text="Tasa Interes/Comisión" />
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaInteres" Prefix="" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTasaInteres" runat="server" ControlToValidate="txtTasaInteres"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo de Cargo" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCargo" runat="server" OnSelectedIndexChanged="ddlTipoCargo_SelectedIndexChanged"
                    AutoPostBack="true" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoCargo" runat="server" InitialValue="" ControlToValidate="ddlTipoCargo"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma de Cobro" />
            <div class="col-sm-2">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server" OnSelectedIndexChanged="ddlFormaCobro_SelectedIndexChanged" AutoPostBack="true" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFromaCobro" runat="server" InitialValue="" ControlToValidate="ddlFormaCobro"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
            <div class="col-sm-1">
                <button class="botonesEvol" type="button" id="btnDetalleFormaCobro" data-toggle="collapse" data-target="#collapseExampleDetalleCobros" aria-expanded="false" aria-controls="collapseExample">
                    Detalle
                </button>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" Enabled="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvMoneda" CssClass="Validador" runat="server" ControlToValidate="ddlMoneda"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
        </div>
        <div class="collapse" id="collapseExampleDetalleCobros">
            <div class="card card-body row">
                <div class="col">
                    <div class="col-sm-6">
                        <asp:GridView ID="gvDetalleFormaCobro" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="true">
                        </asp:GridView>
                    </div>
                    <div class="col-sm-6">
                        <asp:GridView ID="gvFormasCobrosCodigosConceptos" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderText="Codigo Concepto" DataField="CodigoConcepto" />
                                <asp:BoundField HeaderText="Codigo Concepto Plan" DataField="CodigoConceptoPrestamoPlan" />
                                <asp:BoundField HeaderText="Sub Concepto" DataField="CodigoSubConcepto" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="pnlCargoReferencia" Visible="false" runat="server">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCargoReferencia" runat="server" Text="Cargo a Bonificar" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCargoReferencia" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCargoReferencia" runat="server" ControlToValidate="ddlCargoReferencia"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                    <asp:HiddenField ID="hdfImporteCargoReferencia" runat="server" />
                </div>
            </div>
            </asp:PlaceHolder>
        <div class="form-group row">
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteCargo">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteCargo" runat="server" Text="Importe Cargo" />
            <div class="col-sm-9">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCargo" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteCargo" runat="server" ControlToValidate="txtImporteCargo"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
                    </div>
            </div>
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPorcentaje">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPorcentaje" runat="server" Text="Porcentaje" />
            <div class="col-sm-9">
                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtPorcentaje" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPorcentaje" runat="server" ControlToValidate="txtPorcentaje"
                    ErrorMessage="*" ValidationGroup="Aceptar" Enabled="false" />
            </div>
                    </div>
                    </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteInteres">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteInteres" runat="server" Text="Importe Interes/Comisión" />
            <div class="col-sm-9">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteInteres" Enabled="false" runat="server" />
            </div>
        </div>
            </div>

        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteCuota">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporte" runat="server" Text="Importe Cuota" />
            <div class="col-sm-9">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCuota" Enabled="false" runat="server" />
            </div>
                    </div>
            </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCantidadCuotas">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cantidad de Cuotas" />
            <div class="col-sm-9">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadCuotas" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidadCuotas" runat="server" ControlToValidate="txtCantidadCuotas"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
                    </div>
            </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteTotal">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteTotal" runat="server" Text="Importe Total" />
            <div class="col-sm-9">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteTotal" Enabled="false" runat="server" />
            </div>
        </div>
            </div>
            </div>
        <asp:HiddenField ID="hdfImporteCargo" runat="server" />
        <asp:HiddenField ID="hdfCantidadCuotas" runat="server" />
        <asp:HiddenField ID="hdfTasaInteres" runat="server" />
        <asp:HiddenField ID="hdfPorcentaje" runat="server" />


        <asp:Panel ID="pnlCamposValores" Visible="false" GroupingText="Datos adicionales" runat="server">
            <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
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
    </div>
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptarContinuar" ToolTip="Guardar y Continuar Modificando" runat="server" Text="Aplicar" OnClick="btnAceptarContinuar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
