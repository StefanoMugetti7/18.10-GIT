<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosContablesCuentasContablesParametrosModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosContablesCuentasContablesParametrosModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosContablesCuentasContablesParametrosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="ctrDatos" runat="server" />
</asp:Content>
