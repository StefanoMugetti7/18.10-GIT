<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MailingProcesamientosPlantillas.aspx.cs" Inherits="IU.Modulos.Mailing.MailingProcesamientosPlantillas" %>
<%@ Register Src="~/Modulos/Mailing/Controles/MailingProcesamientosPlantillasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>