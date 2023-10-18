<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CuentasContablesBuscar.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.CuentasContablesBuscar" %>
<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscarPopUp.ascx" tagname="popUpCuentasContablesBuscar" tagprefix="auge" %>

<div class="CuentasContablesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCuenta" runat="server" Text="Número Cuenta" />
                <div class="col-sm-2">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" runat="server" OnTextChanged="txtNumeroCuenta_TextChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCuenta" runat="server" ErrorMessage="*" ControlToValidate="txtNumeroCuenta" Enabled="false" />
                </div>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscar"
                        AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" OnClick="btnBuscar_Click" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Cuenta" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" runat="server" />
                    <%--<asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" CausesValidation="false" />--%>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnEliminar" runat="server" Text="Eliminar" Visible="false" OnClick="btnEliminar_Click" CausesValidation="false" />
                    <asp:HiddenField ID="hfIdCuentaContable" runat="server" />
                    <asp:HiddenField ID="hdfIdIndiceColeccion" runat="server" />
                </div>
            </div>
            <AUGE:popUpCuentasContablesBuscar ID="puCuentasContables" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
