<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="BienesUsosModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.BienesUsosModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/BienesUsosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
