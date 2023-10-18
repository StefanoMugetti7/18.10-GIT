<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="FormasCobrosAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.TGE.FormasCobrosAfiliadosAgregar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/FormasCobrosAfiliadosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrFormasCobroAfiliadoDatos" runat="server" />
</asp:Content>
