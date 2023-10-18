<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasAnticiposDatos.ascx.cs" Inherits="IU.Modulos.Cobros.Controles.OrdenesCobrosFacturasAnticiposDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" tagname="popUpBuscarCliente" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<asp:UpdatePanel ID="upEntidades" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
        <asp:Panel ID="pnlDatosDelSocio" GroupingText="Datos del Cliente" runat="server"> 
            <asp:Label ID="lblCodigoSocio" runat="server" Text="Codigo Cliente" />
            <AUGE:NumericTextBox ID="txtCodigoSocio" AutoPostBack="true"  onTextChanged="txtCodigoSocio_TextChanged" runat="server" />
            <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCliente" ID="btnBuscarSocio" Visible="true"
            AlternateText="Buscar socio" ToolTip="Buscar" onclick="btnBuscarCliente_Click"  />
            <asp:Label ID="lblRazonSocial" runat="server" Text="Razon Social"></asp:Label>
            <asp:TextBox ID="txtRazonSocial" Enabled="false" runat="server"></asp:TextBox>
            <div class="Espacio"></div>
            <asp:Label ID="lblCuil" runat="server" Text="CUIT"></asp:Label>
            <asp:TextBox ID="txtCuil" Enabled="false" runat="server"></asp:TextBox>         
        </asp:Panel>
        <AUGE:popUpBuscarCliente ID="ctrBuscarClientePopUp" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<asp:Label ID="lblOrdenCobro" runat="server" Text="Numero"></asp:Label>
<asp:TextBox ID="txtOrdenCobro" Enabled="false" runat="server"></asp:TextBox>
<div class="Espacio"></div>
<asp:Label ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
<asp:TextBox ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
<br />
<asp:Label ID="lblFilialCobro" runat="server" Text="Filial Cobro" />
<asp:DropDownList ID="ddlFilialCobro" runat="server" />        
<div class="Espacio"></div>                                        
<asp:Label ID="lblTotalCobrar" runat="server" Text="Total"></asp:Label>                   
<Evol:CurrencyTextBox ID="txtTotalCobrar" runat="server" Text="0.00"></Evol:CurrencyTextBox>
<br />
<br />         
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
    <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios" >
        <ContentTemplate>
            <AUGE:Comentarios ID="ctrComentarios" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>
<br />

<asp:UpdatePanel ID="upAcciones" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
        <center>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="ordendepago" onclick="btngrabar_Click"/>
            <asp:Button ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false" onclick="btncancelar_Click" />
        </center>
    </ContentTemplate>
</asp:UpdatePanel>