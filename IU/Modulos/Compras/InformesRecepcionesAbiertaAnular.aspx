<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master"  AutoEventWireup="true" CodeBehind="InformesRecepcionesAbiertaAnular.aspx.cs" Inherits="IU.Modulos.Compras.InformesRecepcionesAbiertaAnular" %>
<%@ Register Src="~/Modulos/Compras/Controles/InformesRecepcionesAbiertoDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
