<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposFuncionalidadesListasValoresDetallesModificar.aspx.cs" Inherits="IU.Modulos.TGE.TiposFuncionalidadesListasValoresDetallesModificar" %>
<%@ Register Src="~/Modulos/TGE/Control/TiposFuncionalidadesListasValoresDetallesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ctrModifDatos" runat="server" />
</asp:Content>
