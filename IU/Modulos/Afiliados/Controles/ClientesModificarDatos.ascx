<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientesModificarDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.ClientesModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosDomicilioPopUp.ascx" TagName="popUpDomicilio" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosTelefonoPopUp.ascx" TagName="popUpTelefono" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="popUpAfiliadosBuscar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/FacturacionesHabitualesListar.ascx" TagName="FacturasHabitualesListar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/EnviarMails.ascx" TagName="EnviarMails" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script lang="javascript" type="text/javascript">

    function InitControlsAgregar() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControls);
        InitControls();
    }

    function InitControls() {
        $("input[type=text][id$='txtCuit']").blur(btnTxtCuitOnBlur)
    }

    function btnTxtCuitOnBlur() {
        __doPostBack("<%=btntxtNumeroDocumentoBlur.UniqueID %>", "");
    }

</script>
<div class="card">
    <div class="card-header">
        Datos del Cliente
    </div>
    <div class="card-body">
        <asp:UpdatePanel ID="upDatosCliente" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIdAfiliado" runat="server" Text="Codigo Cliente"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtIdAfiliado" Enabled="false" ValidationGroup="afiliado" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvIdAfiliado" ControlToValidate="txtIdAfiliado" ValidationGroup="Filtrar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número documento"></asp:Label>
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvtNumeroDocumento" ControlToValidate="txtNumeroDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-1">
                        <asp:Button CssClass="botonesEvol" ID="btntxtNumeroDocumentoBlur" OnClick="btntxtNumeroDocumentoBlur_Click" runat="server" Text="Validar" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Razon Social"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtApellido" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvApellido" ControlToValidate="txtApellido" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoAfiliado" runat="server" Text="Tipo"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtTipoAfiliado" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionesFiscales" runat="server" Text="Condicion Fiscal"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" TabIndex="18" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCondicionFiscal" ControlToValidate="ddlCondicionFiscal" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblComprobantesExentos" runat="server" Text="Solo IVA Exento"></asp:Label>
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkComprobanteExento" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Correo electronico"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-9"></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    </div>
</div>
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
    SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpCuentaCorriente" TabIndex="0"
        HeaderText="Cuenta Corriente">
        <ContentTemplate>
            <asp:UpdatePanel ID="upCtaCte" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnBuscarCtaCte" runat="server" Text="Filtrar" ValidationGroup="Filtrar" OnClick="btnFiltrar_Click" />
                        </div>
                        <div class="col-sm-3">
                            <div class="btn-group" role="group">
                                <button type="button" class="botonesEvol dropdown-toggle"
                                    data-toggle="dropdown" aria-expanded="false">
                                    Agregar <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarComprobante" Visible="false" runat="server" Text="Agregar Cbte" OnClick="btnAgregarComprobante_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarOC" Visible="false" runat="server" Text="Agregar Cobro" OnClick="btnAgregarOC_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarRemito" Visible="false" runat="server" Text="Agregar Remito" OnClick="btnAgregarRemito_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarPresupuesto" Visible="false" runat="server" Text="Agregar Presupuesto" OnClick="btnAgregarPresupuesto_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarNotaPedido" Visible="false" runat="server" Text="Agregar Nota de Pedido" OnClick="btnAgregarNotaPedido_Click" /></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="upImprimir" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoReporte" runat="server" Text="Tipo Reporte"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoReporte" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <%--                             <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoComprobante" runat="server" Text="Tipo Comprobante"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoComprobante" runat="server">
                                    </asp:DropDownList>
                                </div>--%>
                                <div class="col-sm-4">
                                    <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                                        OnClick="btnImprimir_Click" AlternateText="Imprimir" ToolTip="Imprimir" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail"
                                        OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                                runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvDatos" AllowPaging="true" DataKeyNames="Orden" OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand"
                                OnPageIndexChanging="gvDatos_PageIndexChanging"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                    <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                                    <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacionTipoOperacion" SortExpression="TipoOp" />
                                    <%--<asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOpe">
                                        <ItemTemplate>
                                            <%# Eval("TipoOperacion.TipoOperacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>
                                    <asp:TemplateField HeaderText="Debito" SortExpression="ImporteDebito" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteDebito", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credito" SortExpression="ImporteCredito" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteCredito", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("SaldoActual", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" Visible="true" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Mostrar" ToolTip="Mostrar" />
                                            <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("TipoOperacionIdTipoOperacion") %>' />
                                            <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                            <asp:HiddenField ID="hdfIdEstado" runat="server" Value='<%#Eval("EstadoIdEstado") %>' />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                                AlternateText="Anular Factura" ToolTip="Anular Factura" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportarExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpCuentaCorrienteDolar" TabIndex="1"
        HeaderText="Cuenta Corriente Dolar">
        <ContentTemplate>
            <asp:UpdatePanel ID="upCtaCteDolar" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesdeDolar" runat="server" Text="Fecha Desde"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesdeDolar" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHastaDolar" runat="server" Text="Fecha Hasta"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHastaDolar" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnBuscarCtaCteDolar" runat="server" Text="Filtrar" ValidationGroup="FiltrarDolar" OnClick="btnFiltrarDolar_Click" />
                        </div>
                        <div class="col-sm-3">
                            <div class="btn-group" role="group">
                                <button type="button" class="botonesEvol dropdown-toggle"
                                    data-toggle="dropdown" aria-expanded="false">
                                    Agregar <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarComprobanteDolar" Visible="false" runat="server" Text="Agregar Cbte" OnClick="btnAgregarComprobanteDolar_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarOCDolar" Visible="false" runat="server" Text="Agregar Cobro" OnClick="btnAgregarOCDolar_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarRemitoDolar" Visible="false" runat="server" Text="Agregar Remito" OnClick="btnAgregarRemitoDolar_Click" /></li>
                                    <li>
                                        <asp:Button CssClass="dropdown-item" ID="btnAgregarPresupuestoDolar" Visible="false" runat="server" Text="Agregar Presupuesto" OnClick="btnAgregarPresupuestoDolar_Click" /></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpImprimirDolar" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoReporteDolar" runat="server" Text="Tipo Reporte"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoReporteDolar" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoComprobanteDolar" runat="server" Text="Tipo Comprobante"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoComprobanteDolar" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4">
                                    <AUGE:popUpEnviarMail ID="PopUpEnviarMailDolar" runat="server" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimirDolar"
                                        OnClick="btnImprimirDolar_Click" AlternateText="Imprimir" ToolTip="Imprimir" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMailDolar"
                                        OnClick="btnEnviarMailDolar_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:ImageButton ID="btnExportarExcelDolar" ImageUrl="~/Imagenes/Excel-icon.png"
                                runat="server" OnClick="btnExportarExcelDolar_Click" Visible="false" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvDatosDolar" AllowPaging="true" DataKeyNames="Orden" OnRowDataBound="gvDatosDolar_RowDataBound" OnRowCommand="gvDatosDolar_RowCommand"
                                OnPageIndexChanging="gvDatosDolar_PageIndexChanging"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                    <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                                    <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacionTipoOperacion" SortExpression="TipoOp" />
                                    <%--<asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOpe">
                                        <ItemTemplate>
                                            <%# Eval("TipoOperacion.TipoOperacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>
                                    <asp:TemplateField HeaderText="Debito" SortExpression="ImporteDebito" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteDebito", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credito" SortExpression="ImporteCredito" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteCredito", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("SaldoActual", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField> --%>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultarDolar"
                                                AlternateText="Mostrar" ToolTip="Mostrar" />
                                            <asp:HiddenField ID="hdfIdTipoOperacionDolar" runat="server" Value='<%#Eval("TipoOperacionIdTipoOperacion") %>' />
                                            <asp:HiddenField ID="hdfIdRefTipoOperacionDolar" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" Visible="false" runat="server" CommandName="Impresion" ID="btnImprimirDolar"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnularDolar"
                                                AlternateText="Anular Factura" ToolTip="Anular Factura" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportarExcelDolar" />
                </Triggers>

            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpMercaderiaPendienteEntrega" TabIndex="2"
        HeaderText="Mercaderia Pte. de Entrega">
        <ContentTemplate>
            <asp:UpdatePanel ID="upMercaderiaPendienteEntrega" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:PlaceHolder ID="phDetalleAcopio" Visible="false" runat="server">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:GridView ID="gvAcopios" DataKeyNames="IdFactura"
                                    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nro." SortExpression="IdFactura">
                                            <ItemTemplate>
                                                <%# Eval("IdFactura")%>
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
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:ImageButton ID="btnMercaderiaPendienteEntrega" ImageUrl="~/Imagenes/Excel-icon.png"
                                runat="server" OnClick="btnMercaderiaPendienteEntrega_Click" Visible="false" />
                        </div>
                        <div class="col-sm-12">
                            <asp:GridView ID="gvRemitosPendientes" AllowPaging="false" AllowSorting="false"
                                OnRowDataBound="gvRemitosPendientes_RowDataBound" DataKeyNames="IdFacturaDetalle"
                                runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nro." SortExpression="IdFactura">
                                        <ItemTemplate>
                                            <%# Eval("IdFactura")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Codigo" SortExpression="IdProducto">
                                        <ItemTemplate>
                                            <%# Eval("IdProducto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("ProductoDescripcion")%>
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
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                        <ItemTemplate>
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
                                        <ItemTemplate>
                                            <%# Eval("Cantidad")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad Pendiente" SortExpression="CantidadPendiente">
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
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnMercaderiaPendienteEntrega" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpMercaderiaPendienteFacturar" TabIndex="3"
        HeaderText="Mercaderia Pte. de Facturar">
        <ContentTemplate>
            <asp:UpdatePanel ID="upMercaderiaPendienteFacturar" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:ImageButton ID="btnExportarExcelMercaderiaPendienteFacturar" ImageUrl="~/Imagenes/Excel-icon.png"
                                runat="server" OnClick="btnExportarExcelMercaderiaPendienteFacturar_Click" Visible="false" />
                        </div>
                        <div class="col-sm-12">
                            <asp:GridView ID="gvFacturasPendientes" AllowPaging="false" AllowSorting="false"
                                DataKeyNames="IdFacturaDetalle"
                                runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nro." SortExpression="IdFactura">
                                        <ItemTemplate>
                                            <%# Eval("IdRemito")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Codigo" SortExpression="IdProducto">
                                        <ItemTemplate>
                                            <%# Eval("ProductoIdProducto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("ProductoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Remito" SortExpression="FechaFactura">
                                        <ItemTemplate>
                                            <%# Eval("FechaRemito", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Numero Remito" SortExpression="NumeroFactura">
                                        <ItemTemplate>
                                            <%# Eval("NumeroRemitoCompleto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
                                        <ItemTemplate>
                                            <%# Eval("Cantidad")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportarExcelMercaderiaPendienteFacturar" />
                </Triggers>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpRemitos" TabIndex="4"
        HeaderText="Remitos">
        <ContentTemplate>
            <asp:UpdatePanel ID="upRemitos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesdeRemitos" runat="server" Text="Fecha Desde"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesdeRemitos" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHastaRemitos" runat="server" Text="Fecha Hasta"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHastaRemitos" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnFiltrarRemitos" runat="server" Text="Filtrar" ValidationGroup="FiltrarRemitos" OnClick="btnFiltrarRemitos_Click" />
                        </div>
                    </div>
                    <div class="col-sm-12 ">
                        <asp:ImageButton ID="btnExportarExcelRemitos" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcelRemitos_Click" Visible="false" />
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <AUGE:popUpEnviarMail ID="PopUpEnviarMail1" runat="server" />
                            <asp:GridView ID="gvRemitos" OnRowCommand="gvRemitos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvRemitos_RowDataBound" DataKeyNames="IdRemito,NumeroRemitoCompleto"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvRemitos_PageIndexChanging" OnSorting="gvRemitos_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion">
                                        <ItemTemplate>
                                            <%# Eval("TipoOperacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Codigo Remito" SortExpression="IdRemito">
                                        <ItemTemplate>
                                            <%# Eval("IdRemito")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Remito" SortExpression="FechaRemito">
                                        <ItemTemplate>
                                            <%# Eval("FechaRemito", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Numero Remito" SortExpression="NumeroRemitoCompleto">
                                        <ItemTemplate>
                                            <%# Eval("NumeroRemitoCompleto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valorizacion" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                        <ItemTemplate>
                                            <%# Eval("ImporteTotal")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                        <ItemTemplate>
                                            <%# Eval("Estado")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="Producto">
                                        <ItemTemplate>
                                            <%# Eval("Producto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
                                        <ItemTemplate>
                                            <%# Eval("Cantidad")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" Visible="false" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/sendmail.png" runat="server" visible="false" CommandName="EnviarMail" ID="btnEnviarMail"
                                                AlternateText="Enviar Mail" ToolTip="Enviar Mail" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Ver" Visible="false" ToolTip="Mostrar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" Visible="false" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                                AlternateText="Anular" Visible="false" ToolTip="Anular" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportarExcelRemitos" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upEnviarMails" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <AUGE:EnviarMails ID="ctrEnviarMails" Visible="false" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpNotasPedidos" TabIndex="5"
        HeaderText="Notas de pedidos">
        <ContentTemplate>
            <asp:UpdatePanel ID="upNotasPedidos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvNotasPedidos" OnRowCommand="gvNotasPedidos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvNotasPedidos_RowDataBound" DataKeyNames="IdNotaPedido"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvNotasPedidos_PageIndexChanging" OnSorting="gvNotasPedidos_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Numero" SortExpression="IdNotaPedido">
                                        <ItemTemplate>
                                            <%# Eval("IdNotaPedido")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                                        <ItemTemplate>
                                            <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaDescripcion"), Eval("ImporteTotal", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                                        <ItemTemplate>
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" Visible="false" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Ver" Visible="false" ToolTip="Mostrar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" Visible="false" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                                AlternateText="Anular" Visible="false" ToolTip="Anular" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpPresupuestos" TabIndex="6"
        HeaderText="Presupuestos">
        <ContentTemplate>
            <asp:UpdatePanel ID="upPresupuestos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvPresupuestos" OnRowCommand="gvPresupuestos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvPresupuestos_RowDataBound" DataKeyNames="IdPresupuesto"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvPresupuestos_PageIndexChanging" OnSorting="gvPresupuestos_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="# Cliente" SortExpression="IdAfiliado">
                                        <ItemTemplate>
                                            <%# Eval("IdAfiliado")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Razon Social" SortExpression="RazonSocial">
                                        <ItemTemplate>
                                            <%# Eval("RazonSocial")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="# Presupuesto" SortExpression="IdPresupuesto">
                                        <ItemTemplate>
                                            <%# Eval("IdPresupuesto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha" SortExpression="FechaFactura">
                                        <ItemTemplate>
                                            <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImporteSinIVA", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Iva" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="IvaTotal">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("IvaTotal", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe Total" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
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
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Mostrar" ToolTip="Mostrar" />
                                            <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar" 
                                AlternateText="Autorizar Comprobante" ToolTip="Autorizar Comprobante" />--%>
                                            <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpContratosConveniosServicios" TabIndex="7"
        HeaderText="Contratos - Convenios - Servicios">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <AUGE:FacturasHabitualesListar ID="ctrFacturasHabitualesListar" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpDomicilios" TabIndex="8"
        HeaderText="Domicilios">
        <ContentTemplate>
            <asp:UpdatePanel ID="upDomicilios" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarDomicilio" runat="server" Text="Agregar domicilio"
                                OnClick="btnAgregarDomicilio_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <AUGE:popUpDomicilio ID="ctrDomicilios" runat="server" />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvDomicilios" OnRowCommand="gvDomicilios_RowCommand"
                                OnRowDataBound="gvDomicilios_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo domicilio" SortExpression="DomicilioTipo.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("DomicilioTipo.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Calle" DataField="Calle" SortExpression="Calle" />
                                    <asp:BoundField HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                                    <asp:BoundField HeaderText="Piso" DataField="Piso" SortExpression="Piso" />
                                    <asp:BoundField HeaderText="Dpto" DataField="Departamento" SortExpression="Departamento" />
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
                                    <asp:BoundField HeaderText="Codigo Postal" DataField="CodigoPostal" SortExpression="CodigoPostal" />
                                    <asp:BoundField HeaderText="Predeterminado" DataField="Predeterminado" SortExpression="Predeterminado" />
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
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
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpTelefonos" TabIndex="9"
        HeaderText="Telefonos">
        <ContentTemplate>
            <asp:UpdatePanel ID="upTelefonos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarTelefono" runat="server" Text="Agregar telefono"
                                OnClick="btnAgregarTelefono_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <AUGE:popUpTelefono ID="ctrTelefonos" runat="server" />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvTelefonos" OnRowCommand="gvTelefonos_RowCommand"
                                OnRowDataBound="gvTelefonos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo telefono" SortExpression="TelefonoTipo.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("TelefonoTipo.Descripcion")%>
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
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpComentarios" TabIndex="10"
        HeaderText="Comentarios">
        <ContentTemplate>
            <AUGE:Comentarios ID="ctrComentarios" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpArchivos" TabIndex="11"
        HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpHistorial" TabIndex="12"
        HeaderText="Auditoria">
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
