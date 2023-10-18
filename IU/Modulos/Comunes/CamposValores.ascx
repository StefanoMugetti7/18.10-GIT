<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CamposValores.ascx.cs" Inherits="IU.Modulos.Comunes.CamposValores" %>
<%--<%@ Register Src="~/Modulos/Comunes/CamposValoresPopUp.ascx" TagPrefix="AUGE" TagName="ModificarDatosPopUp" %>--%>


    <asp:UpdatePanel ID="upCamposValores" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="pnlCamposDinamicos" runat="server">
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
