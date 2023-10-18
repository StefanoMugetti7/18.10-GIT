<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrePrestamosLotesModificar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrePrestamosLotesModificar" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrePrestamosLotesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>