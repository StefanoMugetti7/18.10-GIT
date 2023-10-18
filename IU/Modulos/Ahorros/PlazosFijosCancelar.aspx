<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PlazosFijosCancelar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosFijosCancelar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosFijosDatos.ascx" TagPrefix="AUGE" TagName="CancelarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:CancelarDatos ID="CancelarDatos" runat="server" />
</asp:Content>
