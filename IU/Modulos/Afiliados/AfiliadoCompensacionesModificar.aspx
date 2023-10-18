<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AfiliadoCompensacionesModificar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadoCompensacionesModificar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoCompensacionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>