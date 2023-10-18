<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BancosCuentasDatos.ascx.cs" Inherits="IU.Modulos.Tesoreria.Controles.BancosCuentasDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


<script lang="javascript" type="text/javascript">

    //$(document).ready(function () {
    //    //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
    //    var txtFechaAsiento;
    //    txtFechaAsiento = $('input:text[id$=txtFechaAsiento]').datepicker({
    //        showOnFocus: true,
    //        uiLibrary: 'bootstrap4',
    //        locale: 'es-es',
    //        format: 'dd/mm/yyyy'
    //    });
    //});

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitCuentaContable2);
        InitCuentaContable2();
    });
    function InitCuentaContable2() {
        var ddlCuentaContable = $("select[name$='ddlCuentaContable']");
        ddlCuentaContable.select2({
            placeholder: 'Ingrese el numero o nombre de la cuenta',
            selectOnClose: true,
            //theme: 'bootstrap',
            minimumInputLength: 1,
            language: 'es',
            //tags: true,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Contabilidad/ContabilidadWS.asmx/ContabilidadSeleccionarAjaxComboCuentas', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: ddlCuentaContable.val(), // search term");
                        filtro: params.term, // search term");
                        idEjercicioContable: "0" //Auxiliar para pegarle al mismo WS que lo requiere. No se puede elegir en el abm
                    });
                },
                beforeSend: function (xhr, opts) {
                    var algo = JSON.parse(this.data); // this.data.split('"');
                    if (isNaN(algo.filtro)) {
                        if (algo.filtro.length < 4) {
                            xhr.abort();
                        }
                    }
                },
                processResults: function (data, params) {
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.text,
                                id: item.id,
                                numeroCuenta: item.numeroCuenta,
                                descripcion: item.text,
                                //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                            }
                        })
                    };
                    cache: true
                }
            },
        });
        ddlCuentaContable.on('select2:select', function (e) {
            $("input[id*='hdfCuentaContableDetalle']").val(e.params.data.descripcion);
            $("input[id*='hdfIdCuentaContable']").val(e.params.data.id);
            $("input[id*='hdfCuentaContableNumeroCuenta']").val(e.params.data.numeroCuenta);
        });
        ddlCuentaContable.on('select2:unselect', function (e) {
            $("input[id*='hdfCuentaContableDetalle']").val('');
            $("input[id*='hdfIdCuentaContable']").val('');
            $("input[id*='hdfCuentaContableNumeroCuenta']").val('');
        });
    }

</script>

<div class="CategoriasModificarDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancos" runat="server" ErrorMessage="*"
                ControlToValidate="ddlBancos" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCuenta" runat="server" Text="Numero Cuenta" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCuenta" runat="server" ErrorMessage="*"
                ControlToValidate="txtNumeroCuenta" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDenominacion" runat="server" Text="Denominacion" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDenominacion" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDenominacion" runat="server" ErrorMessage="*"
                ControlToValidate="txtDenominacion" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroBancoSucursal" runat="server" Text="Numero Banco Sucursal" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroBancoSucursal" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroBancoSucursal" runat="server" ErrorMessage="*"
                ControlToValidate="txtNumeroBancoSucursal" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuentaTipo" runat="server" Text="Tipo Cuenta" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasTipos" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasTipos" runat="server" ErrorMessage="*"
                ControlToValidate="ddlBancosCuentasTipos" ValidationGroup="Aceptar" />
        </div>

    </div>
    <div class="form-group row">

        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiliales" runat="server" Text="Filial" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMonedas" runat="server" Text="Monedas" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMonedas" runat="server" ErrorMessage="*"
                ControlToValidate="ddlMonedas" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteDescubierto" runat="server" Text="Importe Descubierto" />
        <div class="col-sm-3">
            <AUGE:CurrencyTextBox CssClass="form-control" ID="txtImporteDescubierto" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteDescubierto" runat="server" ErrorMessage="*"
                ControlToValidate="txtImporteDescubierto" ValidationGroup="Aceptar" />
        </div>

    </div>
    <div class="form-group row">

        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaContable" runat="server" Text="Cuenta Contable" />
            <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlCuentaContable" runat="server"></asp:DropDownList>
        <asp:HiddenField ID="hdfIdCuentaContable" Value='<%#Bind("CuentaContable.IdCuentaContable") %>' runat="server" />
        <asp:HiddenField ID="hdfCuentaContableDescripcion" Value='<%#Bind("CuentaContable.Descripcion") %>' runat="server" />
        <asp:HiddenField ID="hdfCuentaContableNumeroCuenta" Value='<%#Bind("CuentaContable.Descripcion") %>' runat="server" />

                </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtFechaAlta" Enabled="false" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsuarioAlta" runat="server" Text="Usuario Alta" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtUsuarioAlta" Enabled="false" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCBU" runat="server" Text="CBU" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCbu" runat="server" />
                    <%--    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCbu" runat="server" ErrorMessage="*"
                ControlToValidate="txtCbu" ValidationGroup="Aceptar" />--%>
                     </div>
        </div>
    </div>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />
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
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

