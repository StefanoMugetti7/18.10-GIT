<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProveedoresDatos.ascx.cs" Inherits="IU.Modulos.Proveedores.Controles.ProveedoresDatos" %>

<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Proveedores/Controles/ProveedorModificarDatosDomicilioPopUp.ascx" tagname="popUpDomicilio" tagprefix="auge" %>
<%@ Register src="~/Modulos/Proveedores/Controles/ProveedorModificarDatosTelefonoPopUp.ascx" tagname="popUpTelefono" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpEnviarMail.ascx" tagname="popUpEnviarMail" tagprefix="auge" %>
<%@ Register src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" tagname="CuentasContables" tagprefix="AUGE" %>
<%@ Register Src="~/Modulos/Reportes/GestionarReportesDatosParametros.ascx" TagPrefix="AUGE" TagName="ReportesDatosParametros" %>

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        $("input[type=text][id$='txtCuit']").focus();
    });

    function InitControlsAgregar() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControls);
        InitControls();
    }

    function InitControls() {
        $("input[type=text][id$='txtCuit']").blur(btnTxtCuitOnBlur)
    }

    function btnTxtCuitOnBlur() {
        __doPostBack("<%=btnTxtCuitBlur.UniqueID %>", "");
    }

</script>

<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIdProveedor" runat="server" Text="Codigo"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtIdProveedor" Enabled="false" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuit" runat="server" Text="CUIT" />
            <div class="col-sm-2">
                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ThousandsSeparator="" NumberOfDecimals="0" ID="txtCuit" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuit" runat="server" ControlToValidate="txtCuit"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            <div class="col-sm-1">
                <asp:Button CssClass="botonesEvol" ID="btnTxtCuitBlur" Text="Validar"  OnClick="btnTxtCuitBlur_Click" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtRazonSocial" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvRazonSocial" runat="server" ControlToValidate="txtRazonSocial"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBeneficiarioDelCheque" runat="server" Text="Beneficiario del cheque" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtBeneficiarioDelCheque" runat="server" />
            </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado " />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionesFiscales" runat="server" Text="Condición Fiscal " />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionesFiscales" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="CUIT Vencimiento" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimiento" runat="server" />
            </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPaginaWeb" runat="server" Text="Pagina Web   " />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtPaginaWeb" runat="server" />
            </div>
        </div>
        <div class="form-group row">
                     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCBU" runat="server" Text="CBU" />
            <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCBU" MinLength = "22" MaxLength="22" AutoPostBack="true" OnTextChanged="txtCBU_TextChanged" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCBU" runat="server" ControlToValidate="txtCBU"
                ErrorMessage="*" enabled="true" ValidationGroup="IngresarTransferencia" />
            </div>
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblDatosCbu" runat="server" Text="."></asp:Label>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <asp:UpdatePanel ID="upCuentasContables" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <AUGE:CuentasContables ID="ctrCuentasContables" MostrarEtiquetas="true" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />

        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpCuentaCorriente" TabIndex="0"
                HeaderText="Cuenta Corriente">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upCtaCte" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                                </div>
                                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalleFiltro" runat="server" Text="Detalle"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox CssClass="form-control" ID="txtDetalleFiltro" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button CssClass="botonesEvol" ID="btnBuscarCtaCte" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                    <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                                    </div>
                                    <div class="col-sm-1">
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                                        OnClick="btnImprimir_Click" AlternateText="Imprimir" ToolTip="Imprimir" />
                                    <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail"
                                        OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />

                                </div>
                                <div class="col-sm-1">
                                    <div class="btn-group" role="group">
                                        <asp:PlaceHolder ID="phBotones" runat="server">
                                        <button type="button" class="botonesEvol dropdown-toggle"
                                            data-toggle="dropdown" aria-expanded="false">
                                            Agregar <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:Button ID="btnAgregarComprobante" CssClass="dropdown-item" Visible="false" runat="server" Text="Comprobante" OnClick="btnAgregarComprobante_Click" /></li>
                                            <li>
                                                <asp:Button ID="btnAgregarOP" CssClass="dropdown-item" Visible="false" runat="server" Text="Orden de Pago" OnClick="btnAgregarOP_Click" /></li>
                                            <li>
                                                <asp:Button ID="btnAgregarRemtio" CssClass="dropdown-item" runat="server" Text="Remito" OnClick="btnAgregarRemito_Click" /></li> 
                                            <li>
                                                <asp:Button ID="btnAgregarAnticipo" CssClass="dropdown-item" runat="server" Text="Anticipo" OnClick="btnAgregarAnticipo_Click" /></li>
                                        </ul>
                                        </asp:PlaceHolder>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                    <asp:GridView ID="gvDatos" AllowPaging="true" DataKeyNames="FechaMovimiento" OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand"
                                        OnPageIndexChanging="gvDatos_PageIndexChanging"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                        <Columns>
                                            <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                            <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                                            <%--<asp:BoundField  HeaderText="Tipo Operacion" DataField="TipoOperacion.TipoOperacion" SortExpression="TipoOp" />--%>
                                            <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacionTipoOperacion" SortExpression="TipoOp" />
                                            <%--<asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOpe">
                                        <ItemTemplate>
                                            <%# Eval("TipoOperacionTipoOperacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                            <asp:BoundField HeaderText="Debito" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteDebito" SortExpression="ImporteDebito" />
                                            <asp:BoundField HeaderText="Credito" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteCredito" SortExpression="ImporteCredito" />
                                            <asp:BoundField HeaderText="Saldo Actual" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="SaldoActual" SortExpression="SaldoActual" />
                                            <%--<asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>
                                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                           <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                                         <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                                      <asp:HiddenField ID="hdfEstado" runat="server" Value='<%#Eval("EstadoIdEstado") %>' />
                                                    <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("TipoOperacionIdTipoOperacion") %>' />
                                                    <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                                      <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnModificar"
                                            AlternateText="Modificar Solicitud" ToolTip="Modificar Solicitud" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar"
                                            AlternateText="Autorizar Solicitud" ToolTip="Autorizar Solicitud" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/payment-icon-chico.gif" runat="server" CommandName="AgregarOP" Visible="false" ID="btnAgregarOP"
                                            AlternateText="Generar Orden de Pago" ToolTip="Generar Orden de Pago" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnular"
                                            AlternateText="Anular Solicitud" ToolTip="Anular Solicitud" />

                                                      <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="AnularConfirmar" Visible="false" ID="btnAnularConfirmar"
                                            AlternateText="Anular Solicitud" ToolTip="Anular Solicitud" />
                                                        <%--     <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView></div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportarExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpInformesRecepcionPendientes"
                HeaderText="Mercaderia Pendiente">
                <ContentTemplate>
                    <asp:PlaceHolder ID="phDetalleAcopio" Visible="false" runat="server">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                                <asp:GridView ID="gvAcopios" DataKeyNames="IdSolicitudPago"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nro. Sol. Pago" SortExpression="IdSolicitudPago">
                                            <ItemTemplate>
                                                <%# Eval("IdSolicitudPago")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                            <ItemTemplate>
                                                <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tipo y Numero Factura" SortExpression="NumeroFactura">
                                            <ItemTemplate>
                                                <%# Eval("TipoNumeroFactura")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe" SortExpression="ImporteSinIVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("ImporteSinIVA", "{0:C2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe Pendiente" SortExpression="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("ImportePendiente", "{0:C2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView></div>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="form-group row">
                        <div class="col-sm-12">
                             <div class="table-responsive">
                            <asp:GridView ID="gvInformesRecepcionPendientes" ShowHeader="true" OnRowDataBound="gvInformesRecepcionPendientes_RowDataBound"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nro. Sol. Pago" SortExpression="IdSolicitudPago">
                                        <ItemTemplate>
                                            <%# Eval("IdSolicitudPago")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Codigo" SortExpression="IdProducto">
                                        <ItemTemplate>
                                            <%# Eval("IdProducto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                        <ItemTemplate>
                                            <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo y Numero Factura" SortExpression="NumeroFactura">
                                        <ItemTemplate>
                                            <%# Eval("TipoNumeroFactura")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
                                        <ItemTemplate>
                                            <%# Eval("Cantidad")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad Pendiente" SortExpression="CantidadRecibida" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Eval("CantidadPendiente")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubTotal" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                        <ItemTemplate>
                                            <%# Eval("SubTotalItem", "{0:C2}")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView></div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpInformesRecepcion"
                HeaderText="Remitos" TabIndex="2">
                <ContentTemplate> <div class="table-responsive">
                    <asp:GridView ID="gvRemitos" OnRowCommand="gvRemitos_RowCommand" AllowPaging="true" AllowSorting="false"
                        OnRowDataBound="gvRemitos_RowDataBound" DataKeyNames="IdInformeRecepcion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvRemitos_PageIndexChanging" OnSorting="gvRemitos_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Código" SortExpression="CodigoInforme">
                                <ItemTemplate>
                                    <%# Eval("IdInformeRecepcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fecha Remito" ItemStyle-Wrap="false" SortExpression="FechaRemito">
                                <ItemTemplate>
                                    <%# Eval("FechaEmision", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Numero" ItemStyle-Wrap="false" SortExpression="NumeroRemito">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("NumeroRemitoPrefijo"), "-", Eval("NumeroRemitoSufijo"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Observacion" SortExpression="Observacion">
                                <ItemTemplate>
                                    <%# Eval("Observacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Recepcion" ItemStyle-Wrap="false"  SortExpression="FechaRecepcion">
                                <ItemTemplate>
                                    <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Producto" SortExpression="Producto">
                                <ItemTemplate>
                                    <%# Eval("Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Cantidad" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right"  SortExpression="Cantidad">
                                <ItemTemplate>
                                    <%# Eval("CantidadRecibida")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Importe" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" SortExpression="Importe">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:c2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" Visible="false" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnular"
                                        AlternateText="Anular Informe Recepcion" ToolTip="Anular Informe Recepcion" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" Visible="false" ID="btnConsultarAbierta"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnularAbierta"
                                        AlternateText="Anular Informe Recepcion" ToolTip="Anular Informe Recepcion" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView></div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpDomicilios"
                HeaderText="Domicilios">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDomicilios" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarDomicilio" runat="server" Text="Agregar domicilio"
                                        OnClick="btnAgregarDomicilio_Click" CausesValidation="false" />
                                    <AUGE:popUpDomicilio ID="ctrDomicilios" runat="server" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12"> <div class="table-responsive">
                                    <asp:GridView ID="gvDomicilios" OnRowCommand="gvDomicilios_RowCommand" ShowHeader="true"
                                        OnRowDataBound="gvDomicilios_RowDataBound" DataKeyNames="IndiceColeccion"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo domicilio" SortExpression="TipoDomicilio.Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("TipoDomicilio.Descripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Calle" DataField="Calle" SortExpression="Calle" />
                                            <asp:BoundField HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                                            <asp:BoundField HeaderText="Piso" DataField="Piso" SortExpression="Piso" />
                                            <asp:BoundField HeaderText="Dpto" DataField="DeptoOficina" SortExpression="Departamento" />
                                            <asp:TemplateField HeaderText="Localidad" SortExpression="Localidad.Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("Localidad.Descripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Provincia" SortExpression="Localidad.Provincia.Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("Localidad.Provincia.Descripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField  HeaderText="Codigo Postal" DataField="CodigoPostal" SortExpression="CodigoPostal" />--%>
                                            <asp:TemplateField HeaderText="Código Postal" SortExpression="CodigoPostal.CodigoPostal">
                                                <ItemTemplate>
                                                    <%#Eval("Localidad.CodigoPostal")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                        AlternateText="Consultar" ToolTip="Consultar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                        AlternateText="Modificar" ToolTip="Modificar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpTelefonos"
                HeaderText="Telefonos">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upTelefonos" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarTelefono" runat="server" Text="Agregar telefono"
                                        OnClick="btnAgregarTelefono_Click" CausesValidation="false" />
                                       <AUGE:ReportesDatosParametros ID="RepDatosParametros" runat="server" />
                                    <AUGE:popUpTelefono ID="ctrTelefonos" runat="server" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12"> <div class="table-responsive">
                                    <asp:GridView ID="gvTelefonos" OnRowCommand="gvTelefonos_RowCommand"
                                        OnRowDataBound="gvTelefonos_RowDataBound" DataKeyNames="IndiceColeccion"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo telefono" SortExpression="TipoTelefono.Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("TipoTelefono.Descripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Prefijo" DataField="Prefijo" SortExpression="Prefijo" />
                                            <asp:BoundField HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                                            <asp:BoundField HeaderText="Interno" DataField="Interno" SortExpression="Interno" />
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                        AlternateText="Consultar" ToolTip="Consultar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                        AlternateText="Modificar" ToolTip="Modificar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpComentarios"
                HeaderText="Comentarios">
                <ContentTemplate>
                    <AUGE:Comentarios ID="ctrComentarios" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpArchivos"
                HeaderText="Archivos">
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
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
