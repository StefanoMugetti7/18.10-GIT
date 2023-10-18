<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BancosCuentasMovimientosMultiplesDatos.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.BancosCuentasMovimientosMultiplesDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="BancosCuentasMovimientosDatos">
    <asp:UpdatePanel ID="upTodo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
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
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtFechaMovimiento" Enabled="false" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaConciliacionBanco" runat="server" Text="Fecha Banco" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaConciliacionBanco" Enabled="false" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaConciliacionBanco" Enabled="false" ControlToValidate="txtFechaConciliacionBanco" ValidationGroup="BancosCuentasMovimientosDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:UpdatePanel ID="upTipoOperacionConceptos" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server" OnSelectedIndexChanged="ddlTipoOperacion_OnSelectedIndexChanged" AutoPostBack="true" />
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacion" ErrorMessage="*" ValidationGroup="BancosCuentasMovimientosDatos" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptos" runat="server" Text="Concepto"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlConceptosContables" runat="server" OnSelectedIndexChanged="ddlConceptosContables_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvConceptosContables" runat="server" ControlToValidate="ddlConceptosContables" ErrorMessage="*" ValidationGroup="BancosCuentasMovimientosDatos" />
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="pnlFilialDestino" Visible="false">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDestino" runat="server" Text="Filial destino"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" OnSelectedIndexChanged="ddlFiliales_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFiliales" Enabled="false" runat="server" ControlToValidate="ddlFiliales" ErrorMessage="*" ValidationGroup="BancosCuentasMovimientosDatos" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlCuentaInternaDestino" Visible="false">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoDestino" runat="server" Text="Banco destino"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" OnSelectedIndexChanged="ddlBancos_OnSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancos" Enabled="false" runat="server" ControlToValidate="ddlBancos" ErrorMessage="*" ValidationGroup="BancosCuentasMovimientosDatos" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Cuenta destino"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentas" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentas" Enabled="false" runat="server" ControlToValidate="ddlBancosCuentas" ErrorMessage="*" ValidationGroup="BancosCuentasMovimientosDatos" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroTipoOperacion" runat="server" Text="Numero Operacion" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtNumeroTipoOperacion" MaxLength="50" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte" ErrorMessage="*" ValidationGroup="BancosCuentasMovimientosDatos" />
                </div>
            </div>
            <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control" ID="txtDetalle" TextMode="MultiLine" MaxLength="1000" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregarMovimiento" runat="server" Text="Agregar Movimiento" OnClick="btnAgregarMovimiento_Click" ValidationGroup="BancosCuentasMovimientosDatos" />
                </div>
            </div>
            <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
                <asp:TabPanel runat="server" ID="TabPanel1" HeaderText="Movimientos">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvDatos" DataKeyNames="IdBancoCuentaMovimiento" AllowPaging="false" AllowSorting="false"
                                        runat="server" SkinID="GrillaResponsive"
                                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo Operacion">
                                                <ItemTemplate>
                                                    <%# Eval("TipoOperacion.TipoOperacion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concepto">
                                                <ItemTemplate>
                                                    <%# Eval("ConceptoContable.ConceptoContable")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Filial Destino">
                                                <ItemTemplate>
                                                    <%# Eval("FilialDestino")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cuenta Bancaria Destino">
                                                <ItemTemplate>
                                                    <%# Eval("BancoCuentaDestino.Denominacion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha Movimiento" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("FechaConfirmacionBanco", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Numero Operacion" FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("NumeroTipoOperacion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                                FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("Importe", "{0:C2}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detalle" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <%# Eval("Detalle")%>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblTotalImporte" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                        AlternateText="Modificar" ToolTip="Modificar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                        AlternateText="Eliminar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
     <%--           <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
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
                </asp:TabPanel>--%>
            </asp:TabContainer>
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
                    <center>
                        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                        <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                        <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                            onclick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
