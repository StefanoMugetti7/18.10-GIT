﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SegGestionarUsuariosConsultar.aspx.cs" Inherits="IU.Modulos.Seguridad.SegGestionarUsuariosConsultar" %>
<%@ Register Src="~/Modulos/Seguridad/SegGestionarUsuariosDatos.ascx" TagPrefix="ctr" TagName="UsuarioDatos" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ctr:UsuarioDatos ID="ModificarDatos" runat="server" />
</asp:Content>
