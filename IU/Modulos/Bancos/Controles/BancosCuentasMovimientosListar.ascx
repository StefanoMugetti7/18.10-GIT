<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BancosCuentasMovimientosListar.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.BancosCuentasMovimientosListar" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<style>
    .GridViewHeaderStyle {
        position: -webkit-sticky; /* this is for all Safari (Desktop & iOS), not for Chrome*/
        position: sticky;
        top: 0;
        z-index: 1; /* any positive value, layer order is global*/
    }
</style>

<script language="javascript" type="text/javascript">
    function ModificarFechaBancoEstado(control) {
        if ($(control).val().length > 0) {
            $(control).closest("tr").find('[id*="ddlEstados"]').val(12).change(); //Confirmado
        } else {
            $(control).closest("tr").find('[id*="ddlEstados"]').val(44).change(); //Pendiente Confirmacion
        }
    }
</script>

<%--<div class="Banco sCuetnasMovimientosLsitar">   --%>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtBanco" Enabled="false" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCuenta" runat="server" Text="Numero Cuenta" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" Enabled="false" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server" />
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDenominacion" runat="server" Text="Denominacion" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDenominacion" Enabled="false" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroBancoSucursal" runat="server" Text="Numero Banco Sucursal" />
    <div class="col-sm-3">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroBancoSucursal" Enabled="false" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuentaTipo" runat="server" Text="Tipo Cuenta" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtTipoCuenta" Enabled="false" runat="server" />
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiliales" runat="server" Text="Filial" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtFilial" Enabled="false" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMonedas" runat="server" Text="Monedas" />
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtMoneda" Enabled="false" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteDescubierto" runat="server" Text="Importe Descubierto" />
    <div class="col-sm-3">
        <AUGE:CurrencyTextBox CssClass="form-control" ID="txtImporteDescubierto" Enabled="false" runat="server" />
    </div>
</div>
<div class="form-group row" runat="server" id="botoneraAgregar">
    <div class="col-sm-3">
        <div class="btn-group" role="group" runat="server">
            <button type="button" class="botonesEvol dropdown-toggle"
                data-toggle="dropdown" aria-expanded="false">
                Agregar <span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu">
                <li>
                    <asp:Button CssClass="dropdown-item" ID="btnAgregar" runat="server" Visible="false" Text="Movimiento" OnClick="btnAgregar_Click" />
                <li>
                    <asp:Button CssClass="dropdown-item" ID="btnAgregarMultiplesMovimientos" Visible="false" runat="server" Text="Multiples Movimientos" OnClick="btnAgregarMultiplesMovimientos_Click" />
            </ul>
        </div>
    </div>
    <div class="col-sm-9"></div>
</div>
<asp:UpdatePanel ID="upMovimientos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpMovimientos" HeaderText="Movimientos">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upFiltrar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                                <div class="col-sm-3">
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaDesde" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtFechaDesde" ValidationGroup="Buscar" />
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaHasta" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtFechaHasta" ValidationGroup="Buscar" />
                                        </div>
                                    </div>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" ValidationGroup="Buscar" Text="Filtrar" OnClick="btnBuscar_Click" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                                        OnClick="btnImprimir_Click" AlternateText="Imprimir Movimientos" ToolTip="Imprimir Movimientos" />
                                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                                        runat="server" OnClick="btnExportarExcel_Click" />
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <Evol:EvolGridView ID="gvDatos" AllowPaging="true"
                                    OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand" DataKeyNames="IdBancoCuentaMovimiento"
                                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                                    OnPageIndexChanging="gvDatos_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fecha Carga" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Banco" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("FechaConfirmacionBanco", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detalle" FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("Detalle")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Convert.ToDecimal(Eval("ImporteDebe")) > 0 ? Eval("ImporteDebe", "{0:C2}") : string.Empty%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label CssClass="labelFooterEvol" ID="lblSaldoOperativo" runat="server" Text="Saldo Operativo:"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Haber" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Convert.ToDecimal(Eval("ImporteHaber")) > 0 ? Eval("ImporteHaber", "{0:C2}") : string.Empty%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Saldo" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <%# Eval("Saldo", "{0:C2}")%>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label CssClass="labelFooterEvol" ID="lblTotalImporte" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </Evol:EvolGridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportarExcel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpPendientes" HeaderText="Movimientos Pendientes">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetallePendiente" runat="server" Text="Detalle"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtDetallePendiente" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <asp:Button CssClass="botonesEvol" ID="btnBuscarPendientes" runat="server" Text="Filtrar" OnClick="btnBuscarPendientes_Click" />
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                    <div class="col-sm-12">
                        <div class="overflow-auto" style="max-height: 400px">
                            <Evol:EvolGridView ID="gvPendientes" AllowPaging="false" OnRowDataBound="gvPendientes_RowDataBound" runat="server"
                                OnRowCommand="gvPendientes_RowCommand" DataKeyNames="IdBancoCuentaMovimiento" ShowFooter="true"
                                SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" OnPageIndexChanging="gvPendientes_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Detalle" DataField="Detalle" />
                                    <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaDescripcion"), Eval("Importe", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Banco" ItemStyle-Wrap="false" SortExpression="">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaBanco" Enabled="false" runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" Enabled="false" Width="200px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Mostrar" ToolTip="Mostrar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </div>
                    </div>
                    <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                    <center>
                        <asp:Button CssClass="botonesEvol" ID="btnConciliar" runat="server" Visible="false" Text="Conciliar Movimientos" onclick="btnConciliar_Click" />
                    </center>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpRechazados" HeaderText="Movimientos Rechazados">
                <ContentTemplate>
                    <div class="col-sm-12">
                        <Evol:EvolGridView ID="gvRechazados" AllowPaging="true"
                            OnRowDataBound="gvRechazados_RowDataBound"
                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvRechazados_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("FechaConfirmacionBanco", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detalle" FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Detalle")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deuda" SortExpression="SaldoDeuda" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# string.Concat(Eval("Moneda"), Eval("Importe", "{0:N2}"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Evol:EvolGridView>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
