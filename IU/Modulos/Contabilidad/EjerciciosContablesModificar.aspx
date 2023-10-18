<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="EjerciciosContablesModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.EjercicioContableModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/EjerciciosContablesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
