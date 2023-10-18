<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="DepartamentosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.DepartamentosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/DepartamentosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
