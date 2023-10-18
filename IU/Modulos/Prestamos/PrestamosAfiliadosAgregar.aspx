<%@ Page Language="C#"  MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosAgregar" Title=""%>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosAfiliadosModificarDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>

