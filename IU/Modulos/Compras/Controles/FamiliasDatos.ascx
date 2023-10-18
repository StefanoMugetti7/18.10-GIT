<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FamiliasDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.FamiliasDatos" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="AUGE" %>
<%--<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" tagname="CuentasContables" tagprefix="AUGE" %>--%>
<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscarPopUp.ascx" tagname="popUpCuentasContablesBuscar" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<div class="FamiliasDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoFamilia" runat="server" Text="Codigo"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoFamilia" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" ControlToValidate="txtDescripcion" ValidationGroup="FamiliasDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <div class="col-sm-3"></div>
    </div>
    <asp:UpdatePanel ID="upCuentasContables" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <auge:popUpCuentasContablesBuscar ID="puCuentasContables" runat="server" />
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaContableGastos" runat="server" Text="Nr Cuenta Modulo Compras"></asp:Label>
                <div class="col-sm-2">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroCuentaContableGastos" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarCuentaContableGastos"
                        AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" OnClick="btnBuscarCuentaContableGastos_Click" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcionGastos" runat="server" Text="Descripcion" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCuentaContableGastos" Enabled="false" runat="server" />
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaContableGanancia" runat="server" Text="Nro Cuenta Modulo Ventas (Ganancia)"></asp:Label>
                <div class="col-sm-2">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroCuentaContableGanancia" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarCuentaContableGanancia"
                        AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" OnClick="btnBuscarCuentaContableGanancia_Click" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcionGanancia" runat="server" Text="Descripcion" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCuentaContableGanancia" Enabled="false" runat="server" />
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaContableActivo" runat="server" Text="Nro Cuenta Modulo Ventas (Activo)"></asp:Label>
                <div class="col-sm-2">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroCuentaContableActivo" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarCuentaContableActivo"
                        AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" OnClick="btnBuscarCuentaContableActivo_Click" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcionActivo" runat="server" Text="Descripcion" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCuentaContableActivo" Enabled="false" runat="server" />
                </div>
                <div class="col-sm-3"></div>
                            <%--<AUGE:CuentasContables ID="ctrCuentasContables" LabelNumeroCuenta="Nro. Cuenta de Gastos" MostrarEtiquetas="true" runat="server" />
                    <AUGE:CuentasContables ID="ctrCuentasContablesGanancia" LabelNumeroCuenta="Nro. Cuenta de Ganancia" MostrarEtiquetas="true" runat="server" />
                    <AUGE:CuentasContables ID="ctrCuentasContablesActivo" LabelNumeroCuenta="Nro. Cuenta de Activo" MostrarEtiquetas="true" runat="server" />--%>
            </div>
             <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaCMV" runat="server" Text="Nro Cuenta Modulo Ventas (Asiento de Costo)"
                    ToolTip="Cuenta Contable para Costo de Mercaderia Vendida"></asp:Label>
                <div class="col-sm-2">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroCuentaContableCMV" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="ImageButton1"
                        AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" OnClick="btnBuscarCuentaContableCostoMercaderiaVendida_Click" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcionCMV" runat="server" Text="Descripcion" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCuentaContableCMV" Enabled="false" runat="server" />
                </div>
                <div class="col-sm-3"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRegimenRetencionIIGG" runat="server" Text="Regimen Retencion IIGG"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlRegimenRetencionIIGG" runat="server"></asp:DropDownList>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRetieneSUSS" runat="server" Text="Retiene SUSS"></asp:Label>
        <div class="col-sm-3">
            <asp:CheckBox ID="chkRetieneSUSS" runat="server" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStockeable" runat="server" Text="Stockeable"></asp:Label>
        <div class="col-sm-3">
            <asp:CheckBox ID="chkStockeable" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
        </div>
      
    </div>
      <div class="form-group row">
        <div class="col-sm-12">
            <auge:CamposValores ID="ctrCamposValores" runat="server" />
        </div>
    </div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
        SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpHistorial"
            HeaderText="Auditoria">
            <ContentTemplate>
                <auge:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="FamiliasDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
