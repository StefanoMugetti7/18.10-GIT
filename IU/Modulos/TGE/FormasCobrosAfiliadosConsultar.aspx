<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FormasCobrosAfiliadosConsultar.aspx.cs" Inherits="IU.Modulos.TGE.FormasCobrosAfiliadosConsultar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/FormasCobrosAfiliadosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrFormasCobroAfiliadoDatos" runat="server" />
</asp:Content>
