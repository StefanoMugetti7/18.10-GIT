<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" 
    AutoEventWireup="true" CodeBehind="CajasAfiliadosExtraer.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosExtraer" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%--<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>--%>

<%--<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />
        <br />
        <asp:Label CssClass="labelEvol" ID="Label1" runat="server" Text=""></asp:Label>
        <asp:Label CssClass="labelEvol" ID="Label2" runat="server" Text=""></asp:Label>
        <asp:Label CssClass="labelEvol" ID="Label3" runat="server" Text=""></asp:Label>
        <asp:Label CssClass="labelEvol" ID="lblSaldoActual" runat="server" Text="Saldo Actual"></asp:Label>
        <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtSaldoActual" Enabled="false" runat="server" />
        <br />
        <asp:Label CssClass="labelEvol" ID="Label5" runat="server" Text=""></asp:Label>
        <asp:Label CssClass="labelEvol" ID="Label6" runat="server" Text=""></asp:Label>
        <asp:Label CssClass="labelEvol" ID="Label7" runat="server" Text=""></asp:Label>
        <asp:Label CssClass="labelEvol" ID="lblImporteExtraer" runat="server" Text="Importe Extracción"></asp:Label>
        <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtImporteExtraer" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteExtraer" runat="server" ControlToValidate="txtImporteExtraer" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
        <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
            <br />
            <asp:Label CssClass="labelEvol" ID="lblImprimaRecibo" runat="server" Text="Imprima el Recibo y hagalo firmar"></asp:Label>
            <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" 
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Recibo" ToolTip="Imprimir Recibo" />
            <br />--%>
            <br />
            <center>
                <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>     
</asp:Content>
