<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.master" AutoEventWireup="true" CodeBehind="CajasAfiliadosListar.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosListar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscar.ascx" TagPrefix="AUGE" TagName="Afiliados" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:Afiliados ID="ctrAfiliados" runat="server" />
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnVolver" runat="server" Text="Volver a Caja" 
            onclick="btnVolver_Click" />
    </center>
</asp:Content>
