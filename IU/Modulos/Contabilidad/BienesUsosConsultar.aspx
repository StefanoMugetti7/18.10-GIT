<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="BienesUsosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.BienesUsosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/BienesUsosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
