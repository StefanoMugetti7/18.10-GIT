<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="FondoSuplementarioListar.aspx.cs" Inherits="IU.Modulos.Haberes.FondoSuplementarioListar" %>
<%@ Register Src="~/Modulos/Haberes/Controles/FondoSuplementarioDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>



<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrModificarDatos" runat="server" />
</asp:Content>
