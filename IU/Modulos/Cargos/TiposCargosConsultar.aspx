<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposCargosConsultar.aspx.cs" Inherits="IU.Modulos.Cargos.TiposCargosConsultar" Title="" %>
<%@ Register Src="~/Modulos/Cargos/Controles/TipoCargoModificarDatos.ascx" TagPrefix="AUGE" TagName="MostrarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:MostrarDatos ID="MostrarDatos" runat="server" />
</asp:Content>
