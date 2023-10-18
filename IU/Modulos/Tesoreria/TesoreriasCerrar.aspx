<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TesoreriasCerrar.aspx.cs" Inherits="IU.Modulos.Tesoreria.TesoreriasCerrar" Title="" %>
<%@ Register src="~/Modulos/Tesoreria/Controles/TesoreriaCerrarDatos.ascx" tagname="TesoreriaCerrarDatos" tagprefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <auge:TesoreriaCerrarDatos ID="ctrTesoreriaDatos" runat="server" />
</asp:Content>