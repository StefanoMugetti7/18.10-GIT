<%@ Page Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpCajas.master" AutoEventWireup="true" CodeBehind="CajasCerrar.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasCerrar" Title="" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <div class="CajasCerrar">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtUsuario" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <%--<asp:Label CssClass="labelEvol" ID="lblCaja" runat="server" Text="Número Caja"></asp:Label>
        <asp:TextBox CssClass="textboxEvol" ID="txtNumeroCaja" runat="server"></asp:TextBox>
        <br />--%>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTraspasarFondos" runat="server" Text="Traspasar Fondos y dejar Saldos en cero"></asp:Label>
                <asp:CheckBox ID="chkTraspasarFondos" runat="server" />
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                        OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Cerrar"
                        onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                        onclick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
