<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposOperacionesConsultar.aspx.cs" Inherits="IU.Modulos.TGE.TiposOperacionesConsultar" %>
<%@ Register Src="~/Modulos/TGE/Control/TiposOperacionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
