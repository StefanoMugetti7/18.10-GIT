<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ConceptosContablesModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.ConceptosContablesModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/ConceptosContablesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
