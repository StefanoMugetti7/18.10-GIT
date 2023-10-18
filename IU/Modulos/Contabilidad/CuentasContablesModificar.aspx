<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CuentasContablesModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.CuentasContablesModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
