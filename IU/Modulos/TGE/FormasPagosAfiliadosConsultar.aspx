<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FormasPagosAfiliadosConsultar.aspx.cs" Inherits="IU.Modulos.TGE.FormasPagosAfiliadosConsultar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/FormasPagosAfiliadosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrFormasPagoAfiliadoDatos" runat="server" />
</asp:Content>
