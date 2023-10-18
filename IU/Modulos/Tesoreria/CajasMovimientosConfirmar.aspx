<%@ Page Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpCajas.master" AutoEventWireup="true" CodeBehind="CajasMovimientosConfirmar.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasMovimientosConfirmar" Title="" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/Tesoreria/Controles/CajasIngresosValores.ascx" TagName="CajasIngresos" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularValores);
        CalcularValores();
    });

        function CalcularValores() {
        var valores = gvValoresCalcularImporteTotal();
        var neto = $("input[type=text][id$='txtImporte']").maskMoney('unmasked')[0];
        var total = neto - valores;
        $("input[type=text][id$='txtDiferencia']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));   
    }
</script>

    <div class="form-group row">
        <auge:FechaCajaContable ID="ctrFechaCajaContable" runat="server" />
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaComprobante" runat="server" Text="Fecha Comprobante"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtFechaComprobante" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroReferencia" runat="server" Text="Numero Referencia"></asp:Label>
        <div class="col-sm-3">
            <auge:NumericTextBox CssClass="form-control" ID="txtNumeroReferencia" Enabled="false" runat="server"></auge:NumericTextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAfiliado" runat="server" Text="Nombre"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtAfiliado" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Numero Documento"></asp:Label>
        <div class="col-sm-3">
            <auge:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" Enabled="false" runat="server"></auge:NumericTextBox>
        </div>
        <div class="col-sm-3"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe"></asp:Label>
        <div class="col-sm-3">
            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server"></Evol:CurrencyTextBox>
        </div>
        <div class="col-sm-8"></div>
    </div>
    <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblPaga" runat="server" Text="Ingrese el Efectivo"></asp:Label>
    <AUGE:CurrencyTextBox CssClass="form-control" ID="txtEfectivo" runat="server"></AUGE:CurrencyTextBox>
    <br />
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVuelto" runat="server" Text="Vuelto"></asp:Label>
    <AUGE:CurrencyTextBox CssClass="form-control" ID="txtVuelto"  Enabled="false" runat="server"></AUGE:CurrencyTextBox>
    <br />--%>

    <auge:CajasIngresos ID="ctrIngresosValores" Visible="false" runat="server" />
     <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblDiferencia" runat="server" Text="Diferencia"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtDiferencia" runat="server" Enabled="false" Text="0.00" />
            </div>
        </div>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />
            <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
            <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Confirmar" 
                onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--<script type="text/javascript">
        $(function () {
            var efectivo = $('input:text[id$=#txtEfectivo]').change(CalcularVuelto);        
 
            function CalcularVuelto() {
                var value1 = efectivo.val();
                var value2 = $('input:text[id$=#txtImporte]').val();               
                var sum = add(value1, value2);
                $('input:text[id$=#txtVuelto]').val(sum);
            }
 
            function add() {
                var sum = 0;
                for (var i = 0, j = arguments.length; i < j; i++) {
                    if (IsNumeric(arguments[i])) {
                        sum += parseFloat(arguments[i]);
                    }
                }
                return sum;
            }
            function IsNumeric(input) {
                return (input - 0) == input && input.length > 0;
            }
        });
</script>--%>
</asp:Content>
