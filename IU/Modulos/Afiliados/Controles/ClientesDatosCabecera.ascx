<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientesDatosCabecera.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.ClientesDatosCabecera" %>

<asp:Panel ID="pnlDatosDelSocio" GroupingText="Datos del Cliente" runat="server">  
<asp:Label CssClass="labelEvol" ID="lblIdAfiliado" runat="server" Text="Codigo Cliente"></asp:Label>
<asp:TextBox CssClass="textboxEvol" ID="txtIdAfiliado" Enabled="false" runat="server"></asp:TextBox>
<div class="Espacio"></div>
<asp:Label CssClass="labelEvol" ID="lblApellido" runat="server" Text="Razon Social"></asp:Label>
<asp:TextBox CssClass="textboxEvol" ID="txtApellido" Enabled="false" runat="server"></asp:TextBox>
<div class="Espacio"></div>
<asp:Label CssClass="labelEvol" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
<asp:TextBox CssClass="textboxEvol" ID="txtEstado" Enabled="false" runat="server"></asp:TextBox>
<br />
        
<asp:Label CssClass="labelEvol" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
<asp:TextBox CssClass="textboxEvol" ID="txtTipoDocumento" Enabled="false" runat="server"></asp:TextBox>
<div class="Espacio"></div>
<asp:Label CssClass="labelEvol" ID="lblNumeroDocumento" runat="server" Text="Número documento"></asp:Label>
<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroDocumento" Enabled="false" runat="server"></AUGE:NumericTextBox>
<div class="Espacio"></div>
<asp:Label CssClass="labelEvol" ID="lblCondicionesFiscales" runat="server" Text="Condicion Fiscal"></asp:Label>
<asp:TextBox CssClass="textboxEvol" ID="txtCondicionesFiscales" Enabled="false" runat="server"></asp:TextBox>
<br />

<asp:Label CssClass="labelEvol" ID="lblCorreoElectronico" runat="server" Text="Correo electronico"></asp:Label>
<asp:TextBox CssClass="textboxEvol" ID="txtCorreoElectronico" runat="server"></asp:TextBox>
</asp:Panel>
