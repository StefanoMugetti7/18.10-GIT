<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsientosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.AsientosDatos" %>

<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosModelosBuscarPopUp.ascx" TagName="popUpAsientosModelosBuscar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%--<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="buscarCuentasContables" TagPrefix="auge" %>--%>
<%--<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscarPopUp.ascx" tagname="popUpCuentasContablesBuscar" tagprefix="auge" %>--%>
<%--<%@ Register src="~/Modulos/Comunes/ImportarArchivo.ascx" tagname="ImportarArchivo" tagprefix="auge" %>--%>
<%--<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>--%>

<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>

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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);

        intiGridDetalle();
    });

    function InitControlFecha(desde, hasta) {
        var minDate = new Date(1970, 0, 1);
        minDate.setMilliseconds(desde);
        var maxDate = new Date(1970, 0, 1);
        maxDate.setMilliseconds(hasta);
        //txtFechaAsiento.destroy();
        var txtFechaAsiento = $('input:text[id$=txtFechaAsiento]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
            minDate: minDate,
            maxDate: maxDate
        });
    }

    function CalcularItem() {
        var totalDebe = 0.00;
        var totalHaber = 0.00;
        var cantidad = 0;
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            var importeDebe = $(this).find('input:text[id*="txtDebe"]').maskMoney('unmasked')[0];
            var importeHaber = $(this).find('input:text[id*="txtHaber"]').maskMoney('unmasked')[0];
            if (importeDebe) {
                totalDebe += parseFloat(importeDebe);
            }
            if (importeHaber) {
                totalHaber += parseFloat(importeHaber);
            }
        });
        $("#<%=gvDatos.ClientID %> [id*=lblDebe]").text(accounting.formatMoney(totalDebe, "$ ", 2, "."));
        $("#<%=gvDatos.ClientID %> [id*=lblHaber]").text(accounting.formatMoney(totalHaber, "$ ", 2, "."));
        $("#<%=gvDatos.ClientID %> [id*=lblCantidad]").text(cantidad);
        $("input[type=text][id*='txtDiferencia']").val(accounting.formatMoney(totalDebe - totalHaber, "", 2, "."));
    }

    function InvertirValoresDebeHaber() {
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            var importeDebe = $(this).find('input:text[id*="txtDebe"]').maskMoney('unmasked')[0];
            var importeHaber = $(this).find('input:text[id*="txtHaber"]').maskMoney('unmasked')[0];
            var debe = 0.00;
            var haber = 0.00;
            if (importeDebe) {
                haber = parseFloat(importeDebe);
            }
            if (importeHaber) {
                debe = parseFloat(importeHaber);
            }
            $(this).find('input:text[id*="txtDebe"]').val(accounting.formatMoney(debe, "$ ", 2, "."));
            $(this).find('input:text[id*="txtHaber"]').val(accounting.formatMoney(haber, "$ ", 2, "."));
        });
    }

    function UpdPanelUpdate() {
        __doPostBack("<%=button.UniqueID %>", "");
        //document.getElementById('<%= button.ClientID %>').click();        
    }

    function intiGridDetalle() {
        var rowindex = 0;
         var ddlEjercicioContable = $('select[id$="ddlEjercicioContable"] option:selected').val();
      
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
              var ddlCuentaContable = $(this).find('[id*="ddlCuentaContable"]');
              var lblCuentaContableDescripcion = $(this).find('[id*="lblProductoDescripcion"]');
              var hdfIdCuentaContable = $(this).find("input[id*='hdfIdCuentaContable']");
              var hdfCuentaContableDetalle = $(this).find("input:hidden[id*='hdfCuentaContableDescripcion']");
              var hdfCuentaContableNumeroCuenta = $(this).find("input:hidden[id*='hdfCuentaContableNumeroCuenta']");


              //if (hdfIdCuentaContable.val() > 0) {
              //    var newOption = new Option(hdfCuentaContableDetalle.val(), hdfIdCuentaContable.val(), false, true);
              //    ddlCuentaContable.append(newOption).trigger('change');
              //}

              ddlCuentaContable.select2({
                  placeholder: 'Ingrese el numero o nombre de la cuenta',
                  selectOnClose: true,
                  theme: 'bootstrap4',
                  width: '100%',
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
                            idEjercicioContable: ddlEjercicioContable
                        });
                        //var Productos = ObtenerProductosSeleccionadas();
                        //console.log(" array " + Productos);
                        //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                    },
                    beforeSend: function (xhr, opts) {

                        var algo = JSON.parse(this.data); // this.data.split('"');
                        //console.log(algo.filtro);
                        if (isNaN(algo.filtro)) {
                            if (algo.filtro.length < 4) {
                                xhr.abort();
                            }
                        }
                        else {
                        }
                      },
                    
                    processResults: function (data, params) {
                        //return { results: data.items };
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
                lblCuentaContableDescripcion.text(e.params.data.descripcion);
                hdfCuentaContableDetalle.val( e.params.data.descripcion);
                hdfIdCuentaContable.val(e.params.data.id);
                hdfCuentaContableNumeroCuenta.val(e.params.data.numeroCuenta);
            });
            ddlCuentaContable.on('select2:unselect', function (e) {
                lblCuentaContableDescripcion.text('');
                hdfCuentaContableDetalle.val('');
                hdfIdCuentaContable.val('0');
                hdfCuentaContableNumeroCuenta.val('');

            });

            rowindex++;
        });
    }

</script>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<asp:UpdatePanel ID="upEjercicioContable" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" OnSelectedIndexChanged="ddlEjercicioContable_SelectedIndexChanged" AutoPostBack="true" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEjercicio" runat="server" ErrorMessage="*" ControlToValidate="ddlEjercicioContable" ValidationGroup="Aceptar" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroAsiento" runat="server" Text="Número Asiento" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtNumeroAsiento" runat="server" Enabled="false" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAsiento" runat="server" Text="Fecha" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAsiento" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaAsiento" ControlToValidate="txtFechaAsiento" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upTipoAsiento" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFiliales" ControlToValidate="ddlFiliales" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoAsiento" runat="server" Text="Tipo de Asiento" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTiposAsientos" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlTiposAsientos_SelectedIndexChanged" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" Enabled="false" runat="server" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" Enabled="false" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRefTipoOperacion" runat="server" Text="Número Referencia" />
    <div class="col-sm-2">
        <asp:TextBox CssClass="form-control" ID="txtRefTipoOperacion" Enabled="false" runat="server" />
    </div>
    <div class="col-sm-1">
        <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarComprobante" Visible="false"
            AlternateText="Buscar Comprobante" ToolTip="Buscar Comprobante" OnClick="btnBuscarComprobante_Click" />
    </div>
    <div class="col-sm-4">
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
    <div class="col-sm-11">
        <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" TextMode="multiline" Rows="2" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDetalle" runat="server" ErrorMessage="*"
            ControlToValidate="txtDetalle" ValidationGroup="Aceptar" />
    </div>
</div>
<asp:UpdatePanel ID="upTabControl" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="TabPanel1" HeaderText="Asiento Detalle">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upCuentasContables" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <AUGE:popUpAsientosModelosBuscar ID="puAsientosModelos" runat="server" />
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarCuenta" runat="server" Text="Agregar Cuenta" OnClick="btnAgregarCuenta_Click" CausesValidation="false" />
                                    <asp:Button CssClass="botonesEvol" ID="btnBuscarAsientoModelo" runat="server" Text="Buscar Asiento Modelo" OnClick="btnBuscarAsientoModelo_Click" CausesValidation="false" />
                                    <asp:Button CssClass="botonesEvol" ID="btnGenerarAsientoCierre" runat="server" Text="Generar Asiento de Cierre" OnClick="btnGenerarAsientoCierre_Click" Visible="false" CausesValidation="false" />
                                    <asp:Button CssClass="botonesEvol" ID="btnInvertirValores" runat="server" Text="Invertir Valores" Visible="false" OnClientClick="InvertirValoresDebeHaber(); return false;" />
                                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                                </div>
                            </div>
                         <%--   <AUGE:popUpCuentasContablesBuscar ID="puCuentasContables" runat="server" />--%>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                        runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Código - Producto / Descripcion" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:DropDownList CssClass="form-control select2" ID="ddlCuentaContable" runat="server"></asp:DropDownList>
                                                    <asp:HiddenField ID="hdfIdCuentaContable" Value='<%#Bind("CuentaContable.IdCuentaContable") %>' runat="server" />
                                                     <asp:HiddenField ID="hdfCuentaContableDescripcion" Value='<%#Bind("CuentaContable.Descripcion") %>' runat="server" />
                                                     <asp:HiddenField ID="hdfCuentaContableNumeroCuenta" Value='<%#Bind("CuentaContable.Descripcion") %>' runat="server" />
                                                    <asp:Label CssClass="col-form-label" ID="lblCuentaContable" Visible="false" Enabled="false" Text='<%#Bind("CuentaContable.Descripcion") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="Nro. Cuenta">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfIdCuentaContable" runat="server" />
                                                    <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" runat="server" OnTextChanged="txtNumeroCuenta_TextChanged" AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCuenta" runat="server" ErrorMessage="" ControlToValidate="txtNumeroCuenta" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCuentaContable" ID="btnBuscarCuenta"
                                                        AlternateText="Buscar Cuenta Contable" ToolTip="Buscar Cuenta Contable" />
                                                    </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <%--<asp:TemplateField HeaderText="Cuenta Contable">
                                                <ItemTemplate>
                                                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" runat="server" />
                                                    </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="EliminarCuentaContable" ID="btnEliminarCuenta"
                                                        AlternateText="Eliminar Cuenta Contable" ToolTip="Eliminar Cuenta Contable" Visible="false" />
                                                    <%--<AUGE:buscarCuentasContables ID="buscarCuenta" runat="server" MostrarEliminar="false" MostrarEtiquetas="false" AnchoTextBoxs="30%" OnCuentasContablesBuscarIniciar="buscarCuenta_CuentasContablesBuscarIniciar" OnCuentasContablesBuscarSeleccionar="buscarCuenta_CuentasContablesBuscarSeleccionar"/>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción">
                                                <ItemTemplate>
                                                    <asp:Label CssClass="col-form-label" ID="lblDetalle" Visible="false" Text='<%#Bind("Detalle")%>' runat="server"></asp:Label>
                                                    <asp:TextBox CssClass="form-control" ID="txtDetalle" Visible="false" Text='<%#Bind("Detalle")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Debe" SortExpression="Debe">
                                                <ItemTemplate>
                                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtDebe" runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblDebe" runat="server" Text="0.00" Style="text-align: right" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Haber" SortExpression="Haber">
                                                <ItemTemplate>
                                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtHaber" runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblHaber" runat="server" Text="0.00" Style="text-align: right" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Centro de Costo" SortExpression="" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostos" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text="Registros: " />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-6"></div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDiferencia" runat="server" Text="Diferencia" />
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control" ID="txtDiferencia" runat="server" Enabled="false" Text="0.00" />
                                </div>
                                <div class="col-sm-2"></div>
                            </div>
                        </ContentTemplate>
                        <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="button" EventName="Click" />
            </Triggers>--%>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpImportarArchivo" HeaderText="Importar Archivo">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:Button CssClass="botonesEvol" ID="btnDescargarPlanCuentas" runat="server" Text="Descargar Plantilla" OnClick="btnDescargarPlantilla_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-12 col-form-label" ID="lblColumnas" runat="server" Width="100%" Text="Nombre de Columnas Obligatorias"></asp:Label>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-12 col-form-label" ID="lblColumnasDetalles" runat="server" Width="100%" Text="IdEjercicioContable, EjercicioContable, IdCuentaContable, NumeroCuenta, Descripcion, Debe, Haber"></asp:Label>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblArchivo" runat="server" Text="Adjuntar archivo"></asp:Label>
                        <asp:AsyncFileUpload ID="afuArchivo" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server"
                            UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
                        <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
                <ContentTemplate>
                    <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="tcDatos$tpImportarArchivo$btnDescargarPlanCuentas" />
    </Triggers>
</asp:UpdatePanel>

<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnCopiarAsiento" runat="server" Text="Copiar Asiento" OnClick="btnCopiarAsiento_Click" Visible="false" CausesValidation="false" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

