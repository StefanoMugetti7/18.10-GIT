<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CuentasMovimientosListar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasMovimientosListar" Title="" %>

<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
    <div class="CuentasMovimientosLsitar">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCuenta" runat="server" Text="Tipo Cuenta" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtTipoCuenta" Enabled="false" runat="server" />
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCuenta" runat="server" Text="Número Cuenta" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtNumeroCuenta" Enabled="false" runat="server" />
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Actual" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtSaldoActual" Enabled="false" runat="server" />
            </div>
        </div>
        <asp:UpdatePanel ID="upGeneral" UpdateMode="Conditional" runat="server">

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

                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Filtrar" OnClick="btnBuscar_Click" />
                        <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                            OnClick="btnImprimir_Click" AlternateText="Imprimir Movimientos" ToolTip="Imprimir Movimientos" />
                     <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
                    <asp:TabPanel runat="server" ID="tpMovimientos" HeaderText="Movimientos">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdCuentaMovimiento"
                                        runat="server" SkinID="GrillaBasicaFormalSinSticky" AutoGenerateColumns="false" ShowFooter="true"
                                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Fecha">
                                                <ItemTemplate>
                                                    <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha Confirmacion">
                                                <ItemTemplate>
                                                    <%# Eval("FechaConfirmacion", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo Operacion">
                                                <ItemTemplate>
                                                    <%# Eval("TipoOperacionTipoOperacion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo Valor">
                                                <ItemTemplate>
                                                    <%# Eval("TipoValorTipoValor")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField  HeaderText="Concepto" DataField="ConceptoGrilla"  />--%>

                                            <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# string.Concat(Eval("CuentaMonedaMoneda"), Eval("Importe", "{0:N2}"))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# string.Concat(Eval("CuentaMonedaMoneda"), Eval("SaldoActual", "{0:N2}"))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial">
                                                <ItemTemplate>
                                                    <%# Eval("FilialFilial")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                        Visible="false" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Evol:EvolGridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" ID="tpMovimientosPendientes" HeaderText="Movimientos Pendientes">
                        <ContentTemplate>
                            <asp:GridView ID="gvMovimientosPendientes" DataKeyNames="IdCuentaMovimiento"
                                OnRowCommand="gvMovimientosPendientes_RowCommand" OnRowDataBound="gvMovimientosPendientes_RowDataBound"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" SortExpression="FechaMovimiento">
                                        <ItemTemplate>
                                            <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacionTipoOperacion">
                                        <ItemTemplate>
                                            <%# Eval("TipoOperacionTipoOperacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:BoundField  HeaderText="Concepto" DataField="ConceptoGrilla" SortExpression="ConceptoGrilla" />--%>

                                    <asp:TemplateField HeaderText="Importe" SortExpression="ImportePrestamo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("CuentaMonedaMoneda"), Eval("Importe", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <%--<asp:BoundField  HeaderText="Saldo" DataField="SaldoActual" SortExpression="SaldoActual" />--%>
                                    <asp:TemplateField HeaderText="Filial" SortExpression="FilialFilial">
                                        <ItemTemplate>
                                            <%# Eval("FilialFilial")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                        <ItemTemplate>
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                Visible="false" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                                Visible="false" AlternateText="Anular Operación" ToolTip="Anular Operación" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" ID="tpChequesRechazados" HeaderText="Cheques Rechazados">
                        <ContentTemplate>
                            <asp:GridView ID="gvChequesRechazados" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" SortExpression="FechaMovimiento">
                                        <ItemTemplate>
                                            <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacionTipoOperacion">
                                        <ItemTemplate>
                                            <%# Eval("TipoOperacionTipoOperacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField  HeaderText="Concepto" DataField="ConceptoGrilla" SortExpression="ConceptoGrilla" />--%>
                                    <asp:BoundField HeaderText="Importe" DataField="Importe" DataFormatString="{0:C2}" SortExpression="Importe" />
                                    <%--<asp:BoundField  HeaderText="Saldo" DataField="SaldoActual" SortExpression="SaldoActual" />--%>
                                    <asp:TemplateField HeaderText="Filial" SortExpression="FilialFilial">
                                        <ItemTemplate>
                                            <%# Eval("FilialFilial")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
            </ContentTemplate>
             <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
</asp:Content>
