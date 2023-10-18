<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SalirSistema.aspx.cs" Inherits="IU.SalirSistema" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
    <center>
        Muchas gracias por utilizar el Sistema.
        <br /><br />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
    </center>
    </div>
</asp:Content>
