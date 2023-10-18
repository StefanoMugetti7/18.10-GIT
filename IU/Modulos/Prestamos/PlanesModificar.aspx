<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlanesModificar.aspx.cs" Inherits="IU.Modulos.Prestamos.PlanesModificar" Title="" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PlanesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>