<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Turismo.ascx.cs" Inherits="IU.Modulos.Comunes.Turismo" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<script>

    $(document).ready(function () {
        $("input[type=submit][id$='btnAgregar']").click(function (e) {
            var txtFechaIda = $("input[id$='txtFechaSalida']").val();
            var txtFechaVuelta = $("input[id$='txtFechaRegreso']").val();
            if (txtFechaIda != '' && txtFechaVuelta != '') {
                var fechaIda = new Date(toDate(txtFechaIda));
                var fechaVuelta = new Date(toDate(txtFechaVuelta));
                if (fechaIda > fechaVuelta) {
                    e.preventDefault();
                    MostrarMensaje('La Fecha de regreso de la reserva debe ser mayor a la fecha de salida.', 'red');
                }
            }
        });
    });


</script>
<asp:UpdatePanel ID="pnlPrinc" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroReserva">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNroReserva" runat="server" Text="Nro. Reserva" />
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtNroReserva" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 border col-form-label" ID="lblFechaSalida" runat="server" Text="Fecha de Salida" />
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaSalida" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaSalida" ControlToValidate="txtFechaSalida" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 border col-form-label" ID="lblFechaRegreso" runat="server" Text="Fecha de Regreso" />
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRegreso" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-12 col-lg-12">
                <div class="row">
                    <asp:Label CssClass="col-sm-1 border col-form-label" ID="lblDetalle" runat="server" Text="Destino" />
                    <div class="col-sm-11">
                        <asp:TextBox CssClass="form-control" TextMode="MultiLine" Rows="2" ID="txtDetalle" runat="server" />
                    </div>
                </div>
            </div>
            <div class="w-100"></div>
            <asp:PlaceHolder ID="pnlCamposDinamicosTurismo" runat="server"></asp:PlaceHolder>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="divImpuestoPais">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImpuestoPais" runat="server" Text="Impuesto Pais" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImpuestoPais" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPercepcionRG4815">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPercepcionRG4815" runat="server" Text="Percepción RG 4815/20" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPercepcionRG4815" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPercepcionRG3819">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPercepcionRG3819" runat="server" Text="Percepción RG 3819/15" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPercepcionRG3819" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPercepcionRG5272">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPercepcionRG5272" runat="server" Text="Percepción RG 5272/22" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPercepcionRG5272" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteReintegrar" visible="false">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteReintegrar" runat="server" Text="Importe a Reintegrar" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteReintegrar" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteReintegrar" ControlToValidate="txtImporteReintegrar" Enabled="true" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCuentaAhorro" visible="false">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCuentaAhorro" runat="server" Text="Cuentas de Ahorro" />
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCuentasAhorros" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuentasAhorros" ControlToValidate="ddlCuentasAhorros" Enabled="true" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCostoCancelacion" visible="false">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCostoCancelacion" runat="server" Text="Costo Cancelación" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtCostoCancelar" runat="server" />
                    </div>
                </div>
            </div>
            <div class="w-100"></div>
            <div class="divider"></div>
        </div>
        <ul class="nav nav-tabs bg-info" id="myTab" role="tablist">
            <li class="nav-item  border border-dark border-bottom-0 bg-primary">
                <a class="nav-link active text-white" id="home-tab" data-toggle="tab" href="#home" role="tab" aria-controls="home" aria-selected="true">Servicios</a>
            </li>
            <li class="nav-item border border-dark border-bottom-0 bg-primary">
                <a class="nav-link text-white" id="profile-tab" data-toggle="tab" href="#profile" role="tab" aria-controls="profile" aria-selected="false">Pagos</a>
            </li>
            <li class="nav-item border border-dark border-bottom-0 bg-primary">
                <a class="nav-link text-white" id="contact-tab" data-toggle="tab" href="#contact" role="tab" aria-controls="contact" aria-selected="false">Facturacion y Cobros</a>
            </li>
        </ul>
        <div class="tab-content" id="myTabContent">
            <div class="tab-pane fade show active border border-dark "
                id="home" role="tabpanel" aria-labelledby="home-tab">

                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvServicio">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblServicio" runat="server" Text="Servicio" />
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlServicio" OnSelectedIndexChanged="ddlServicio_SelectedIndexChanged" AutoPostBack="true" Enabled="true" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvServicio" ControlToValidate="ddlServicio" ValidationGroup="TurismoCamposValores" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvProveedor">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor" />
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEntidad" Enabled="true" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEntidad" ControlToValidate="ddlEntidad" ValidationGroup="TurismoCamposValores" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                                <asp:HiddenField ID="hdfProveedor" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="w-100"></div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteServicio">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteServicio" runat="server" Text="Importe" />
                            <div class="col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteServicio" runat="server" />
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvImporteServicio" ControlToValidate="txtImporteServicio" ValidationGroup="TurismoCamposValores" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCostoServicio">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCostoServicio" runat="server" Text="Costo" />
                            <div class="col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCostoServicio" runat="server" />
                                <asp:HiddenField ID="hdfPorcentajeGananciaServicio" runat="server" />
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCostoServicio" ControlToValidate="txtCostoServicio" ValidationGroup="TurismoCamposValores" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvAgregar">
                        <div class="row">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                                OnClick="btnAgregar_Click" ValidationGroup="TurismoCamposValores" />
                        </div>
                    </div>
                    <asp:PlaceHolder ID="pnlCamposDinamicosTurismoServicios" runat="server"></asp:PlaceHolder>
                </div>
                <div class="overflow-auto" style="max-height: 400px">
                    <asp:HiddenField ID="hdfIdTurismo" runat="server" />
                    <asp:HiddenField ID="hdfTotalServicios" runat="server" />
                    <asp:GridView ID="gvDatos" OnRowCreated="gvDatos_RowCreated" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTurismoDetalle"
                        runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Tipo de Servicio">
                                <ItemTemplate>
                                    <%# Eval("TipoServicio")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Proveedor">
                                <ItemTemplate>
                                    <%# Eval("Proveedor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detalle">
                                <ItemTemplate>
                                    <asp:Literal ID="ltlDetalleCampos" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Costo" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right">
                                <ItemTemplate>
                                    <%# Eval("Costo", "{0:C2}")%>
                                    <asp:HiddenField ID="hdfCosto" Value='<%#Bind("Costo") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblCostoTotal" runat="server" Text="0.00" FooterStyle-Wrap="false" ItemStyle-Wrap="false"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:C2}")%>
                                    <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text="0.00" FooterStyle-Wrap="false" ItemStyle-Wrap="false"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                        AlternateText="Modificar" ToolTip="Modificar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja" Visible="false"
                                        AlternateText="Eliminiar" ToolTip="Eliminar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" Visible="false"
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante"  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdfIdTurismoDetalle" runat="server" />
                    <asp:HiddenField ID="hdfModificarDatos" runat="server" />
                </div>
            </div>
            <div class="tab-pane fade border border-dark" id="profile" role="tabpanel" aria-labelledby="profile-tab">
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div1">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblProveedoresPagos" Visible="false" runat="server" Text="Proveedor" />
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlProveedoresPagos" Visible="false" Enabled="true" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlProveedoresPagos_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvProveedoresPagos" ControlToValidate="ddlProveedoresPagos" ValidationGroup="AgregarPago" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div2">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteAnticipo" Visible="false" runat="server" Text="Importe" />
                            <div class="col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteAnticipo" NumberOfDecimals="2" Prefix="" Visible="false" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTasaInteres" runat="server" ControlToValidate="txtImporteAnticipo"
                                    ErrorMessage="*" ValidationGroup="AgregarPago" />
                            </div>
                        </div>
                    </div>
                    <div class="col-1 col-md-1 col-lg-1" runat="server" id="dvAgregarPago">
                        <div class="row">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarPago" runat="server" Text="Agregar Anticipo"
                                OnClick="btnAgregarPago_Click" ValidationGroup="AgregarPago" Visible="false" />
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-6"></div>
                    <div class="col-sm-6">
                        <div class="table-responsive">
                            <asp:GridView ID="gvPagosValores" OnRowDataBound="gvPagosValores_RowDataBound"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Proveedor" DataField="Proveedor" SortExpression="Proveedor" />
                                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Importe">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("Importe", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pago" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Pago">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("Pago", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Saldo" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Saldo">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("Saldo", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvPagos" OnRowCommand="gvPagos_RowCommand" AllowPaging="false" AllowSorting="false"
                        OnRowDataBound="gvPagos_RowDataBound" DataKeyNames="IdOrdenPago,IdSolicitudPago, IdTipoOperacion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <asp:TemplateField HeaderText="Número" SortExpression="IdOrdenPago">
                                <ItemTemplate>
                                    <%# Eval("IdOrdenPago")%>
                                    <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("IdTipoOperacion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Beneficiario" SortExpression="EntidadNombre">
                                <ItemTemplate>
                                    <%# Eval("EntidadNombre")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuit" SortExpression="EntidadCuit">
                                <ItemTemplate>
                                    <%# Eval("EntidadCuit")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                                <ItemTemplate>
                                    <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Pago" SortExpression="FechaPago">
                                <ItemTemplate>
                                    <%# Eval("FechaPago", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe Total" SortExpression="ImporteTotal">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnModificar"
                                        AlternateText="Modificar Solicitud" ToolTip="Modificar Solicitud" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" ID="btnAutorizar"
                                        AlternateText="Autorizar" ToolTip="Autorizar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                        AlternateText="Anular Orden Pago" ToolTip="Anular Orden Pago" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="tab-pane fade border border-dark border-top-0" id="contact" role="tabpanel" aria-labelledby="contact-tab">
                <div class="form-group row">
                    <div class="col-sm-3">
                        <div class="btn-group" role="group">
                            <button type="button" id="BtnAgregarFactura" class="botonesEvol dropdown-toggle"
                                data-toggle="dropdown" aria-expanded="false" runat="server" visible="false">
                                Agregar <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnAgregarComprobante" runat="server" Text="Agregar Cbte" OnClick="btnAgregarComprobante_Click" /></li>
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnAgregarOC" runat="server" Text="Agregar Cobro" OnClick="btnAgregarOC_Click" /></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-6"></div>
                    <div class="col-sm-6">
                        <div class="table-responsive">
                            <asp:GridView ID="gvValores" OnRowDataBound="gvTiposValores_RowDataBound"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Tipo Valor" DataField="TipoValor" SortExpression="TipoValor" />
                                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                                        <ItemTemplate>
                                            <%# Eval("Importe", "{0:C2}")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvCuentaCorriente" DataKeyNames="Orden" OnRowDataBound="gvCuentaCorriente_RowDataBound" OnRowCommand="gvCuentaCorriente_RowCommand"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                        <Columns>
                            <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                            <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                            <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacionTipoOperacion" SortExpression="TipoOp" />
                            <asp:BoundField HeaderText="Debito" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteDebito" SortExpression="ImporteDebito" />
                            <asp:BoundField HeaderText="Credito" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteCredito" SortExpression="ImporteCredito" />
                            <asp:BoundField HeaderText="Saldo Actual" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="SaldoActual" SortExpression="SaldoActual" />
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("TipoOperacionIdTipoOperacion") %>' />
                                    <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>