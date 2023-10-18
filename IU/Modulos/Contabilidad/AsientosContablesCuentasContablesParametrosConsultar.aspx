<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosContablesCuentasContablesParametrosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosContablesCuentasContablesParametrosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosContablesCuentasContablesParametrosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="ctrDatos" runat="server" />
</asp:Content>