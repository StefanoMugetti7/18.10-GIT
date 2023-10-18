<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CajasAfiliadosPagosHaberes.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosPagosHaberes" %>

<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<%--<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Actual"></asp:Label>
        <div class="col-sm-3">
            <AUGE:CurrencyTextBox CssClass="form-control" ID="txtSaldoActual" Enabled="false" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteExtraer" runat="server" Text="Neto a Pagar"></asp:Label>
        <div class="col-sm-3">
            <AUGE:CurrencyTextBox CssClass="form-control" ID="txtImporteExtraer" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteExtraer" runat="server" ControlToValidate="txtImporteExtraer"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
            <div class="form-group row">
                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblImprimaRecibo" runat="server" Text="Imprima el Recibo y hagalo firmar"></asp:Label>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                        OnClick="btnImprimir_Click" AlternateText="Imprimir Recibo" ToolTip="Imprimir Recibo" />
                </div>
            </div>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                    <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
