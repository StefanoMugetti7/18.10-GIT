<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CajasAbrir.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAbrir" Title="" %>

<%--<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceEncabezado" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />--%>
    <div class="CajasAbrir">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtUsuario" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCaja" runat="server" Text="Número Caja"></asp:Label>
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroCaja" runat="server"></AUGE:NumericTextBox>
                <%--<asp:RequiredFieldValidator ID="rfvNumeroCaja" ValidationGroup="Aceptar" ControlToValidate="txtNumeroCaja" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
            </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtFecha" runat="server"></asp:TextBox>
        </div>
    </div>
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="row justify-content-md-center">
                    <div class="col-md-auto">
                        <asp:Label CssClass="labelEvol" ID="lblMsgReabrirCaja" runat="server" Width="100%" ForeColor="Red" Visible="false" Text="Se ha cerrado la Caja para el dia. Se reabrira la misma Caja"></asp:Label>
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Abrir"
                            OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                            OnClick="btnCancelar_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
