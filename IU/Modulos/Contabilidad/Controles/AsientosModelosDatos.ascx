<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsientosModelosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.AsientosModelosDatos" %>

<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%--<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="buscarCuentasContables" TagPrefix="auge" %>--%>
<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscarPopUp.ascx" tagname="popUpCuentasContablesBuscar" tagprefix="auge" %>


<script lang="javascript" type="text/javascript">

    function InitControlFecha(desde, hasta) {
        var minDate = new Date(1970, 0, 1);
        minDate.setMilliseconds(desde);
        var maxDate = new Date(1970, 0, 1);
        maxDate.setMilliseconds(hasta);
        var txtFechaAsiento = $('input:text[id$=txtFechaAsiento]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
            minDate: minDate,
            maxDate: maxDate
        });
    }

</script>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

     <asp:UpdatePanel ID="upDatos" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" OnSelectedIndexChanged="ddlEjercicioContable_SelectedIndexChanged" AutoPostBack="true" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEjercicio" runat="server" ErrorMessage="*" ControlToValidate="ddlEjercicioContable" ValidationGroup="Aceptar" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDetalle" runat="server" ErrorMessage="*" 
        ControlToValidate="txtDetalle" ValidationGroup="Aceptar"/>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
    </div>
        </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoAsiento" Text="Tipo Asiento" runat="server" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoAsiento" OnSelectedIndexChanged="ddlTipoAsiento_SelectedIndexChanged" 
                AutoPostBack="true" runat="server"  />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operacion" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposOperaciones" runat="server" />
    </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upCuentasContables" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-sm-3">
                <asp:Button CssClass="botonesEvol" ID="btnAgregarCuenta" runat="server" Text="Agregar Cuenta" onclick="btnAgregarCuenta_Click" CausesValidation="false" />
                </div>
            </div>
            <AUGE:popUpCuentasContablesBuscar ID="puCuentasContables" runat="server" />
            <div class="form-group row">
                <div class="col-sm-12">
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion" 
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                <Columns>
                     <asp:TemplateField HeaderText="Nro. Cuenta">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfIdCuentaContable" runat="server" />
                            <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" runat="server" OnTextChanged="txtNumeroCuenta_TextChanged" AutoPostBack="true"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCuenta" runat="server" ErrorMessage="*" ControlToValidate="txtNumeroCuenta" Enabled="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCuentaContable" ID="btnBuscarCuenta"
                                    AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="EliminarCuentaContable" ID="btnEliminarCuenta"
                                    AlternateText="Eliminar Cuenta Contable" ToolTip="Eliminar Cuenta Contable" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cuenta Contable">
                        <ItemTemplate>
                            <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <%--<asp:TemplateField HeaderText="Descripción" SortExpression="CuentaContable.Descripcion">
                        <ItemTemplate>
                            <%# Eval("CuentaContable.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Tipo Imputación" SortExpression="TipoImputacion.Descripcion">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoImputacion" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoImputacion" runat="server" ErrorMessage="" 
                                ControlToValidate="ddlTipoImputacion" ValidationGroup="Aceptar"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Codigo" SortExpression="CodigoAsientoModeloDetalle">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlCodigoAMD" runat="server"/>
                            <%--<asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"/>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <asp:UpdatePanel ID="upModificarAsientos" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <asp:Panel CssClass="form-group row" ID="pnlModificarAsientos" Visible="false" runat="server">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblModificarAsientos" runat="server" Text="Modificar Asientos Contables" />
                <div class="col-sm-3">
                <asp:CheckBox ID="chkModificarAsientos" CssClass="form-control" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha Posterior a"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAsiento" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvFechaAsiento" ControlToValidate="txtFechaAsiento" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-4"></div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>