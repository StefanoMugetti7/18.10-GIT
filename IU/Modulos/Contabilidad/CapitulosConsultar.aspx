<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CapitulosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.CapitulosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CapitulosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
