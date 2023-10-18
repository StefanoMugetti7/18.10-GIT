<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SolicitudesPagosListar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            InitApellidoSelect2();
        });


        function InitApellidoSelect2() {
            var control = $("select[name$='ddlNumeroProveedor']");
            //var lblCUIT = $(this).find("input:text[id*='lblCUIT']");
            control.select2({
                placeholder: 'Ingrese el codigo o Razón Social',
                selectOnClose: true,
                theme: 'bootstrap4',
                minimumInputLength: 1,
                //width: '100%',
                language: 'es',
                //tags: true,
                //allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Proveedores/ProveedoresWS.asmx/ProveedoresCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: control.val(), // search term");
                            filtro: params.term // search term");
                        });
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

                                    id: item.IdProveedor,
                                    text: item.CodigoProveedor,
                                    Cuit: item.TipoDocumentoDescripcion,
                                    NumeroDocumento: item.NumeroDocumento,
                                    RazonSocial: item.RazonSocial,
                                    IdCondicionFiscal: item.IdCondicionFiscal,
                                    Estado: item.EstadoDescripcion,
                                    Beneficiario: item.Beneficiario


                                    //Apellido: item.Apellido,
                                    //Nombre: item.Nombre,
                                    //IdTipoDocumento: item.IdTipoDocumento,
                                    //TipoDocumento: item.DescripcionTipoDocumento,
                                    //NumeroDocumento: item.NumeroDocumento,
                                    //IdAfiliadoTipo: item.IdAfiliadoTipo,
                                    //IdCondicionFiscal: item.IdCondicionFiscal,
                                    //CondicionFiscalDescripcion: item.CondicionFiscalDescripcion
                                }
                            })
                        };
                        cache: true
                    }
                }
            });

            control.on('select2:select', function (e) {
                $("input[id*='hdfIdProveedor']").val(e.params.data.id);//.trigger("change");
                $("input[id*='hdfRazonSocial']").val(e.params.data.text);
                AfiliadoSeleccionar();
            });

            control.on('select2:unselect', function (e) {
                control.val(null).trigger('change');
                $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
                $("input[id*='hdfRazonSocial']").val('');
            });

        }

        function AfiliadoSeleccionar() {
            __doPostBack("<%=button.UniqueID %>", "");
        }



        $(document).ready(function () {
            $("input[type=text][id$='txtFechaDesde']").focus();
        });
    </script>
    <div class="FacturasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimientoDesde" runat="server" Text="Fecha Vto."></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimientoDesde" Placeholder="Desde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimientoHasta" Placeholder="Hasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSolicitud" runat="server" Text="Número Solicitud"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSolicitud" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoSolicitud" runat="server" Text="Tipo Solicitud" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoSolicitud" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Número Factura"></asp:Label>
                    <div class="col-sm-1">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroFactura" runat="server" MaxLength="5" />
                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblGuionMedio" runat="server" Text="-"  Width="10"></asp:Label>--%>
                    </div>
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" MaxLength="8" />
                    </div>
                </div>

                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroProveedor">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroProveedor" runat="server" Text="Proveedor"></asp:Label>
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroProveedor" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvNumeroProveedor" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroProveedor" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                                <asp:HiddenField ID="hdfRazonSocial" runat="server" />

                            </div>
                        </div>
                    </div>
                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />

                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCUIT" runat="server" Text="CUIT"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                        </asp:DropDownList>

                    </div>
                </div>

                <div class="form-group row">

                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3"></div>
                    <div class="col-sm-3"></div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdSolicitudPago,EntidadIdEntidad,EntidadIdRefEntidad"
                            runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Número Solicitud" SortExpression="NumeroSolicitud">
                                    <ItemTemplate>
                                        <%# Eval("IdSolicitudPago")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                                    <ItemTemplate>
                                        <%# Eval("EntidadNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacionTipoOperacion">
                                    <ItemTemplate>
                                        <%# Eval("TipoOperacionTipoOperacion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaAlta">
                                    <ItemTemplate>
                                        <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                    <ItemTemplate>
                                        <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Factura" SortExpression="TiposFacturasDescripcion">
                                    <ItemTemplate>
                                        <%# Eval("TiposFacturasDescripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nro Factura" SortExpression="NumeroFacturaCompleto">
                                    <ItemTemplate>
                                        <%# Eval("NumeroFacturaCompleto")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                                    <ItemTemplate>
                                        <%# Eval("ImporteSinIVA", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteSinIVA" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Iva" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                    <ItemTemplate>
                                        <%# Eval("IvaTotal", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                    <ItemTemplate>
                                        <%# Eval("ImporteTotal", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                    <ItemTemplate>
                                        <%# Eval("EstadoDescripcion")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnModificar"
                                            AlternateText="Modificar Solicitud" ToolTip="Modificar Solicitud" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar"
                                            AlternateText="Autorizar Solicitud" ToolTip="Autorizar Solicitud" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/payment-icon-chico.gif" runat="server" CommandName="AgregarOP" Visible="false" ID="btnAgregarOP"
                                            AlternateText="Generar Orden de Pago" ToolTip="Generar Orden de Pago" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnular"
                                            AlternateText="Anular Solicitud" ToolTip="Anular Solicitud" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Evol:EvolGridView>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
