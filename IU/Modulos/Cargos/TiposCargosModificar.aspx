<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="TiposCargosModificar.aspx.cs" Inherits="IU.Modulos.Cargos.TiposCargosModificar" Title="" %>
<%@ Register Src="~/Modulos/Cargos/Controles/TipoCargoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
