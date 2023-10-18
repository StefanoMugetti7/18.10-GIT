<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="DepartamentosAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.DepartamentosAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/DepartamentosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
