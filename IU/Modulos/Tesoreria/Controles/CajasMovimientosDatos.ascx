<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CajasMovimientosDatos.ascx.cs" Inherits="IU.Modulos.Tesoreria.Controles.CajasMovimientosDatos" %>
<%@ Register Src="~/Modulos/Tesoreria/Controles/CajasIngresosValores.ascx" TagName="CajasIngresos" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="TesoreriasMovimientosDatos">
    <auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvAnular" visible="false">
        <p class="h5">Anulación de Operación de Caja</p>
    </div>
    <asp:UpdatePanel ID="upTipoOperacionConceptos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                <div class="col-9 col-md-6 col-lg-3">
                    <asp:TextBox CssClass="form-control" ID="txtNumero" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <auge:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Movimiento" runat="server" />
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" Enabled="false" runat="server" OnSelectedIndexChanged="ddlTipoOperacion_OnSelectedIndexChanged"
                        AutoPostBack="true" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacion"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" Enabled="false" OnSelectedIndexChanged="ddlMoneda_OnSelectedIndexChanged" AutoPostBack="true"/>
                       <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMoneda" runat="server" ControlToValidate="ddlMoneda"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:HyperLink ID="hplVerDetalle" CssClass="botonesEvol" Font-Underline="false" ForeColor="white" Visible="false" runat="server">Ver Detalle</asp:HyperLink>

                </div>
            </div>

              <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
            <asp:Panel ID="pnlDetalleConceptos" GroupingText="Detalle de Conceptos" runat="server" Visible="false">

                <asp:Panel ID="pnlAltaDatos" runat="server" Visible="false">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptos" runat="server" Text="Concepto"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlConceptosContables" runat="server" OnSelectedIndexChanged="Concepto_OnSelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvConceptosContables" runat="server" ControlToValidate="ddlConceptosContables"
                                ErrorMessage="*" ValidationGroup="IngresarConcepto" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCentroCostos" runat="server" Text="Centro de Costos"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostos" runat="server" />
                           <%-- <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCentrosCostos" runat="server" ControlToValidate="ddlCentrosCostos"
                                ErrorMessage="*" ValidationGroup="IngresarConcepto" />--%>
                        </div>
                        <div class="col-sm-3">
                            <asp:Button CssClass="botonesEvol" ID="btnIngresarConcepto" runat="server" Text="Ingresar Concepto"
                                OnClick="btnIngresarConcepto_Click" ValidationGroup="IngresarConcepto" />
                        </div>
                    </div>

                     <AUGE:CamposValores ID="ctrCamposValoresConceptosContables" runat="server" />


                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                        <div class="col-sm-3">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte"
                                ErrorMessage="*" ValidationGroup="IngresarConcepto" />
                            <div class="col-sm-8"></div>
                        </div>
                    </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle Concepto"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtDetalle" TextMode="MultiLine" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-8"></div>
                        </div>
                </asp:Panel>
                <div class="table-responsive">
                <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvDatos_RowDataBound"
                    OnRowCommand="gvDatos_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Concepto">
                            <ItemTemplate>
                                <%# Eval("ConceptoContable.ConceptoContable")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle">
                            <ItemTemplate>
                                <%# string.Concat(Eval("Detalle")," ", Eval("DescripcionCampos")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Centro de Costos">
                            <ItemTemplate>
                                <%# Eval("CentroCostoProrrateo.CentroCostoProrrateo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                            <ItemTemplate>
                                <%# Eval("Importe", "{0:C2}")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                    </div>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <auge:CajasIngresos ID="ctrIngresosValores" runat="server" />
    <asp:UpdatePanel ID="upMovimientos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <auge:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                    <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                        OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" Visible="false" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
