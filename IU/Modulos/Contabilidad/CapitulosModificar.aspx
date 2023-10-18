<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CapitulosModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.CapitulosModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CapitulosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
