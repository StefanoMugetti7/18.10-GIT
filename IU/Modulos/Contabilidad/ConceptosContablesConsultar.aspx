<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ConceptosContablesConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.ConceptosContablesConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/ConceptosContablesDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
