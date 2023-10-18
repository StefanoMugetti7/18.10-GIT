<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FormasPagosAfiliadosModificar.aspx.cs" Inherits="IU.Modulos.TGE.FormasPagosAfiliadosModificar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/FormasPagosAfiliadosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrFormasPagoAfiliadoDatos" runat="server" />
</asp:Content>
