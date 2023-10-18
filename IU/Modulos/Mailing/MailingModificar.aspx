<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingModificar.aspx.cs" Inherits="IU.Modulos.Mailing.MailingModificar" %>
<%@ Register Src="~/Modulos/Mailing/Controles/MailingDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>