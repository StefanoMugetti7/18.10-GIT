<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="EstudiosAnular.aspx.cs" Inherits="IU.Modulos.Medicina.EstudiosAnular" %>
<%@ Register Src="~/Modulos/Medicina/Controles/EstudiosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ctrModificarDatos" runat="server" />
</asp:Content>
