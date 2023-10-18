<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="EjerciciosContablesAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.EjerciciosContablesAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/EjerciciosContablesDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
