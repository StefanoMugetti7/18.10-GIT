<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesPagosAnticipos.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.SolicitudesPagosAnticipos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" tagname="popUpBuscarProveedor" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" tagname="AsientoMostrar" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>


       <AUGE:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></AUGE:BuscarProveedorAjax>

<div class="form-group row">
     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFactura" runat="server" Text="Fecha" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFactura" runat="server" />
    </div>
   
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacion"
            ErrorMessage="*" ValidationGroup="Aceptar" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control select2" ID="txtImporte" Enabled="false" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" Enabled="true" ControlToValidate="txtImporte" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
</div>
<div class="form-group row">
     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
    <div class="col-sm-7">
        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" MaxLength="500" Enabled="false" TextMode="MultiLine" />
    </div>
</div>

<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">

    <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
        <ContentTemplate>
            <AUGE:Comentarios ID="ctrComentarios" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>

    <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>

    <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>

</asp:TabContainer>


<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
        <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                    <br />
                </center>
    </ContentTemplate>
</asp:UpdatePanel>
