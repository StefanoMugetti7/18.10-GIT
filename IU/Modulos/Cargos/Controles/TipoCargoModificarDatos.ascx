<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TipoCargoModificarDatos.ascx.cs" Inherits="IU.Modulos.Cargos.Controles.TipoCargoModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Cargos/Controles/CargoAfiliadoModificarDatosFormaCobro.ascx" TagPrefix="auge" TagName="CargoAfiliadoModificarDatosFormaCobro" %>


<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitProductoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitCuentaContable2);
        InitProductoSelect2();
        InitCuentaContable2();
    });


    function checkAllRow(objRef) {
        $('input:checkbox[id*="chkFormasCobros"]').each(function () {
            $(this).prop('checked', objRef.checked);
        });
    }


    function InitProductoSelect2() {
        var ddlProducto = $("select[name$='ddlProducto']");
        var hdfProductoDetalle = $("input:hidden[id*='hdfProductoDetalle']");
        var hdfIdProducto = $("input[id*='hdfIdProducto']");

        if (hdfIdProducto.val() > 0) {
            var newOption = new Option(hdfProductoDetalle.val(), hdfIdProducto.val(), false, true);
            ddlProducto.append(newOption).trigger('change');
        }

        ddlProducto.select2({
            placeholder: 'Ingrese el codigo o producto',
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
                url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/CMPProductosSeleccionarFiltro', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: ddlProducto.val(), // search term");
                        filtro: params.term, // search term");
                        proveedor: 0,
                    });
                },
                beforeSend: function (xhr, opts) {
                    var algo = JSON.parse(this.data); // this.data.split('"');
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
                                text: item.DescripcionCombo,
                                id: item.IdProducto,
                                productoDescripcion: item.Descripcion,
                                stockActual: item.StockActual,
                                precio: item.Precio,
                                //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        ddlProducto.on('select2:select', function (e) {
            hdfProductoDetalle.val(e.params.data.productoDescripcion);
            hdfIdProducto.val(e.params.data.id);
        });
        ddlProducto.on('select2:unselect', function (e) {
            hdfProductoDetalle.val('');
            hdfIdProducto.val('');
        });
    }

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
  <asp:UpdatePanel ID="updatos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Código" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigo" runat="server" ErrorMessage="*"
                ControlToValidate="txtCodigo" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo de Cargo" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtTipoCargo" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoCargo" runat="server" ErrorMessage="*"
                ControlToValidate="txtTipoCargo" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargoProceso" runat="server" Text="Tipo Cargo Proceso" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCargoProceso" runat="server" OnSelectedIndexChanged="ddlTipoCargoProceso_SelectedIndexChanged"
                AutoPostBack="true" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSumarizaConTipoCargo" runat="server" Text="Sumariza con Cargo" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlSumarizaConCargo" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCargoIrregular" runat="server" Text="Cargo Irregular" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkCargoIrregular" CssClass="form-control" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
        <div class="col-sm-3">
            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ErrorMessage="*"
                ControlToValidate="txtImporte" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPorcentaje" runat="server" Text="Porcentaje" />
        <div class="col-sm-3">
            <Evol:CurrencyTextBox CssClass="form-control" Prefix="" Enabled="false" ID="txtPorcentaje" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPorcentaje" runat="server" ErrorMessage="*"
                ControlToValidate="txtPorcentaje" ValidationGroup="Aceptar" Enabled="false" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPermiteCuotas" runat="server" Text="Permite cuotas" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkPermiteCuotas" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadMaximaCuotas" runat="server" Text="Maximo Cuotas" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadMaximaCuotas" runat="server" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAplicaSiTieneParticipantes" runat="server" Text="Aplica si tiene participantes" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkAplicaSiTieneParticipantes" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAplicaPorCantidadParticipantes" runat="server" Text="Aplica por cantidad de participantes" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkAplicaPorCantidadParticipantes" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDepositoCajaAhorro" runat="server" Text="Deposito en Caja de Ahorros" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkDepositoCajaAhorro" CssClass="form-control" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTiposOperaciones" runat="server" OnSelectedIndexChanged="ddlTiposOperaciones_SelectedIndexChanged"
                AutoPostBack="true"/>
        </div>
        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptos" runat="server" Text="Concepto"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlConceptosContables" runat="server">
    </asp:DropDownList>
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvConceptosContables" runat="server" ErrorMessage="*" 
        ControlToValidate="ddlConceptosContables" ValidationGroup="Aceptar"/>--%>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPrioridad" runat="server" Text="Prioridad de Descuento" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtPrioridad" runat="server" />
        </div>
              
    </div>
            <div class="form-group row">
                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaContable" runat="server" Text="Cuenta Contable" />
            <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlCuentaContable" runat="server"></asp:DropDownList>
        <asp:HiddenField ID="hdfIdCuentaContable" Value='<%#Bind("CuentaContable.IdCuentaContable") %>' runat="server" />
        <asp:HiddenField ID="hdfCuentaContableDescripcion" Value='<%#Bind("CuentaContable.Descripcion") %>' runat="server" />
        <asp:HiddenField ID="hdfCuentaContableNumeroCuenta" Value='<%#Bind("CuentaContable.Descripcion") %>' runat="server" />

                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Producto" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" />
                          <asp:HiddenField ID="hdfIdProducto" runat="server" />
                        <asp:HiddenField ID="hdfProductoDetalle" runat="server" />
                    </div>
            </div>

                  </ContentTemplate>
    </asp:UpdatePanel>
  
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtFechaAlta" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsuarioAlta" runat="server" Text="Usuario Alta" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtUsuarioAlta" runat="server" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpCargosCategorias" HeaderText="Importes por Categoria">
            <ContentTemplate>
                <asp:UpdatePanel ID="pnlCategorias" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCategorias" Visible="false" runat="server" Text="Categorias" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlCategorias" Visible="false" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCategorias" runat="server" ErrorMessage="*"
                                    ControlToValidate="ddlCategorias" ValidationGroup="Agregar" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" Visible="false" OnClick="btnAgregar_Click" ValidationGroup="Agregar" />
                            </div>
                            <div class="col-sm-3"></div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Categoria" SortExpression="Categoria.Categoria">
                                        <ItemTemplate>
                                            <%# Eval("Categoria.Categoria")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prec. Unitario" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" Text='<%#Bind("Importe", "{0:C2}") %>'></Evol:CurrencyTextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField  HeaderText="Importe" DataField="Importe" SortExpression="Importe" />--%>
                                    <asp:TemplateField HeaderText="Fecha Alta" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAltaGrilla" Enabled="false" Text='<%#Bind("FechaAlta", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Vigencia Desde" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVigenciaDesde" Text='<%#Bind("FechaVigenciaDesde", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaVigenciaDesde" runat="server" Display="Dynamic" ErrorMessage=""
                                                ControlToValidate="txtFechaVigenciaDesde" ValidationGroup="Aceptar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--                    <asp:BoundField  HeaderText="Fecha Alta" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                    <asp:BoundField  HeaderText="Fecha Vigencia Desde" DataField="FechaVigenciaDesde" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaVigenciaDesde" />       --%>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlEstados" Enabled="false" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpRangos" HeaderText="Rangos">
            <ContentTemplate>
                <asp:UpdatePanel ID="upRangos" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarRangos" runat="server" Text="Agregar" Visible="false" OnClick="btnAgregarRangos_Click" />
                            </div>
                            <div class="col-sm-8"></div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvRangos" OnRowCommand="gvRangos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvRangos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvRangos_PageIndexChanging" OnSorting="gvRangos_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo de Rango" SortExpression="">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTiposRangos" Enabled="false" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desde" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" Enabled="false" NumberOfDecimals="2" Prefix="" ID="txtDesde" runat="server" Text='<%#Bind("Desde", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hasta" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" Enabled="false" NumberOfDecimals="2" Prefix="" ID="txtHasta" runat="server" Text='<%#Bind("Hasta", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" Enabled="false" ID="txtImporte" runat="server" Text='<%#Bind("Importe", "{0:C2}") %>'></Evol:CurrencyTextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Inicio Vigencia" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" Enabled="false" ID="txtFechaVigenciaDesde" runat="server" Text='<%#Bind("FechaVigenciaDesde", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlEstadosRangos" Enabled="false" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpFormasCobros" HeaderText="Formas de Cobro">
            <ContentTemplate>
                <asp:CheckBox ID="chkTodos" Text="Todos" TextAlign="Right" runat="server" onclick="checkAllRow(this);" />
                <asp:CheckBoxList ID="chkFormasCobros" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                </asp:CheckBoxList>
            </ContentTemplate>
        </asp:TabPanel>
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
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

