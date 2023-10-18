<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TesTesoreriasAbrir.aspx.cs" Inherits="IU.Modulos.Tesoreria.TesTesoreriasAbrir" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="TesoreriasAbrir">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
                <br />
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaUltimaTesoreria" runat="server" Text="Fecha Ultima Tesoreria"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtFechaUltimaTesoreria" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAbrir" runat="server" Text="Fecha Abrir"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAbrir" runat="server"></asp:TextBox>
                    </div>
                </div>
                <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
