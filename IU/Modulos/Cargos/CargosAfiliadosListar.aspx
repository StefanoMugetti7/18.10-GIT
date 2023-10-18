<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CargosAfiliadosListar.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosListar" Title="" %>

<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Auditoria/Control/popUpAuditoria.ascx" tagname="popUpAuditoria" tagprefix="auge" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <div class="CargosAfiliadosListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                    <div class="col-sm-9"></div>
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
                    <asp:TabPanel runat="server" ID="tpCargosMensuales" HeaderText="Analitico de Cargos Procesados">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodo" runat="server" Text="Periodo"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlPeriodo" runat="server" />
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma de Cobro"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server" />
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estados"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" CausesValidation="false" runat="server" Text="Filtrar" OnClick="btnBuscar_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" />
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSeleccionar" runat="server" Text="Reporte"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlComprobantes" runat="server" />
                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvComprobantes" ValidationGroup="Imprimir" ControlToValidate="ddlComprobantes" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConcepto" runat="server" Text="Concepto"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtConepto" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" ValidationGroup="Imprimir"
                                        OnClick="btnImprimir_Click" AlternateText="Imprimir Reporte" ToolTip="Imprimir Reporte" />
                                    <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" ValidationGroup="Imprimir"
                                        OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
                                </div>
                            </div>
                            <Evol:EvolGridView ID="gvCargosMensuales" OnRowCommand="gvCargosMensuales_RowCommand" AllowPaging="true"
                                OnRowDataBound="gvCargosMensuales_RowDataBound" DataKeyNames="IdCuentaCorriente"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvCargosMensuales_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                    <asp:BoundField HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="Periodo" />
                                    <asp:BoundField HeaderText="Tipo Cargo / Concepto" DataField="TipoCargoConcepto" SortExpression="TipoCargoConcepto" />
                                    <asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                                        <ItemTemplate>
                                            <%# Eval("TipoValorTipoValor")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Forma Cobro" SortExpression="FormaCobro.FormaCobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobroFormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("Importe", "{0:N2}"))%>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Visible="false" Text=""></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cobrado" SortExpression="Cobrado" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteCobrado", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteCobrado" runat="server" Visible="false" Text=""></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteCobradoDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe Enviar" SortExpression="ImporteEnviar" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteEnviar", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteEnviar" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteEnviarDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" ItemStyle-Wrap="false" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Motivo Rechazo" SortExpression="MotivoRechazo">
                                        <ItemTemplate>
                                            <%# Eval("MotivoRechazo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comprobante" SortExpression="Comprobante">
                                        <ItemTemplate>
                                            <%# Eval("Comprobante")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Consultar" ToolTip="Consultar" Visible="false" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" Visible="false" CommandName="DesimputarCobro" ID="btnDesimputarCobro"
                                                AlternateText="Desimputar Cobro" ToolTip="Desimputar Cobro" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/reutilizar.png" runat="server" Visible="false" CommandName="RevertirCobro" ID="btnRevertirCobro"
                                                AlternateText="Revertir Cobro" ToolTip="Revertir Cobro" />
                                            <asp:HiddenField ID="hdfIdEstado" runat="server" Value='<%#Eval("EstadoIdEstado")%>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" Visible="false" runat="server" Text=""></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblRegistrosDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" ID="tcCuotasPendientes" HeaderText="Cuotas Pendientes">
                        <ContentTemplate>
                            <Evol:EvolGridView ID="gvCuotasPendientes" AllowPaging="true" Paginacion="false"
                                OnRowDataBound="gvCuotasPendientes_RowDataBound" DataKeyNames=""
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvCuotasPendientes_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Tipo Cargo / Concepto" DataField="TipoCargoConcepto" />
                                    <asp:TemplateField HeaderText="Forma Cobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Proximo Vto." SortExpression="TotalProximoVto" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImporteCuota", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporte" Visible="false" runat="server" Text=""></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Cuotas" DataField="CantidadCuotas" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderText="Total Futuro." SortExpression="TotalProximoVto" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImporteRestante", "{0:N2}"))%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteRestante" runat="server" Visible="false" Text=""></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteRestanteDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Visible="false" Text=""></asp:Label>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblRegistrosDolar" runat="server" Visible="false" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" ID="tpCargosAutomaticos" TabIndex="0" HeaderText="Cargos Automaticos">
                        <ContentTemplate>
                            <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" Paginacion="false"
                                 AllowSorting="true" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                <Columns>
                                    <asp:BoundField HeaderText="Nro." DataField="IdTipoCargoAfiliadoFormaCobro" ItemStyle-Wrap="false" SortExpression="IdTipoCargoAfiliadoFormaCobro" />
                                    <asp:TemplateField HeaderText="Tipo de Cargo" SortExpression="TipoCargo.TipoCargo">
                                        <ItemTemplate>
                                            <%# Eval("TipoCargo.TipoCargo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Forma de Cobro" SortExpression="FormaCobroAfiliado.FormaCobro.FormaCobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobroAfiliado.FormaCobro.FormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" SortExpression="TipoCargo.Importe" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# Eval("ImporteCuota", "{0:C2}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Fecha Alta" DataField="FechaAlta" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                     <asp:TabPanel runat="server" ID="tpTurismo" TabIndex="1" HeaderText="Turismo">
                        <ContentTemplate>

                             <Evol:EvolGridView ID="gvReservasTurismo" OnRowCommand="gvReservasTurismo_RowCommand" AllowPaging="true"
                                OnRowDataBound="gvReservasTurismo_RowDataBound" DataKeyNames="IdTipoCargoAfiliadoFormaCobro"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvReservasTurismo_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Nro. de Reserva" DataField="NumeroReserva" ItemStyle-Wrap="false" SortExpression="NumeroReserva" />
                                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Detalle" DataField="Detalle"/>
                                    <asp:TemplateField HeaderText="Forma de Cobro" >
                                        <ItemTemplate>
                                            <%# Eval("FormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                             <%# string.Concat(Eval("Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pagado" SortExpression="TipoCargo.Importe" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                             <%# string.Concat(Eval("Moneda"), Eval("ImportePagado", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cobrado" SortExpression="CantidadCuotas">
                                        <ItemTemplate>
                                             <%# string.Concat(Eval("Moneda"), Eval("ImporteCobrado", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfIdEstado" runat="server" Value='<%#Eval("IdEstado")%>' />
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Consultar" ToolTip="Consultar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        </ItemTemplate>
                                          <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" ID="tpCargosAdministrables" TabIndex="2" HeaderText="Administracion de Cargos">
                        <ContentTemplate>
                            <Evol:EvolGridView ID="gvCargosAdministrables" OnRowCommand="gvCargosAdministrables_RowCommand" Paginacion="false" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvCargosAdministrables_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvCargosAdministrables_PageIndexChanging" OnSorting="gvCargosAdministrables_Sorting">
                                <Columns>
                                    <asp:BoundField HeaderText="Nro." DataField="IdTipoCargoAfiliadoFormaCobro" ItemStyle-Wrap="false" SortExpression="IdTipoCargoAfiliadoFormaCobro" />
                                    <asp:BoundField HeaderText="Fecha Carga" DataField="FechaAltaEvento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaAltaEvento" />
                                    <asp:BoundField HeaderText="Fecha Inicio" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaAltaEvento" />
                                    <asp:BoundField HeaderText="Periodo Inicio" ItemStyle-Wrap="false" DataField="FechaAlta" DataFormatString="{0:yyyyMM}" SortExpression="FechaAlta" />
                                    <asp:TemplateField HeaderText="Tipo Cargo / Concepto" SortExpression="TipoCargo.TipoCargo">
                                        <ItemTemplate>
                                            <%# Eval("TipoCargo.TipoCargoConcepto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Forma de Cobro" SortExpression="FormaCobroAfiliado.FormaCobro.FormaCobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobroAfiliado.FormaCobro.FormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" SortExpression="TipoCargo.Importe" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                             <%# string.Concat(Eval("Moneda.Moneda"), Eval("ImporteCuota", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cuotas" SortExpression="CantidadCuotas">
                                        <ItemTemplate>
                                            <%# Eval("CantidadCuotas")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Porcentaje" SortExpression="Porcentaje">
                                        <ItemTemplate>
                                            <%# Eval("Porcentaje")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                                       <ItemTemplate>
                                             <%# string.Concat(Eval("Moneda.Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" ID="tpCargosHistoricos" TabIndex="3" HeaderText="Cargos Historicos">
                        <ContentTemplate>
                            <Evol:EvolGridView ID="gvCargosHistoricos" OnRowCommand="gvCargosHistoricos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvCargosHistoricos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvCargosHistoricos_PageIndexChanging" OnSorting="gvCargosHistoricos_Sorting">
                                <Columns>
                                    <asp:BoundField HeaderText="Nro." DataField="IdTipoCargoAfiliadoFormaCobro" ItemStyle-Wrap="false" SortExpression="IdTipoCargoAfiliadoFormaCobro" />
                                    <asp:BoundField HeaderText="Fecha Alta" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaAlta" />
                                    <asp:TemplateField HeaderText="Tipo de Cargo" SortExpression="TipoCargo.TipoCargo">
                                        <ItemTemplate>
                                            <%# Eval("TipoCargo.TipoCargo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Forma de Cobro" SortExpression="FormaCobroAfiliado.FormaCobro.FormaCobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobroAfiliado.FormaCobro.FormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe" SortExpression="TipoCargo.Importe" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# Eval("ImporteCuota")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Fecha Alta" ItemStyle-Wrap="false" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Consultar" ToolTip="Consultar" Visible="false" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
