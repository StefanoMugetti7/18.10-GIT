<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FormasPagosAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.TGE.FormasPagosAfiliadosAgregar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/FormasPagosAfiliadosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrFormasPagoAfiliadoDatos" runat="server" />
</asp:Content>
