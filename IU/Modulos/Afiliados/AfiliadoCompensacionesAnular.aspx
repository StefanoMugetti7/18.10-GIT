<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadoCompensacionesAnular.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadoCompensacionesAnular" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoCompensacionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>