<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PosicionGlobal.aspx.cs" Inherits="IU.Modulos.Afiliados.PosicionGlobal" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpEnviarMail.ascx" tagname="popUpEnviarMail" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">

<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpCuentaCorrienteCargos"
        HeaderText="Cuenta Corriente Cargos">
        <ContentTemplate>
<asp:UpdatePanel ID="upDatos" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
            <asp:ImageButton ID="btnExportarCargosExcel" ImageUrl="~/Imagenes/Excel-icon.png" runat="server" onclick="btnExportarCargosExcel_Click" Visible="false" />
            <br />
<asp:GridView ID="gvDatosCargos" AllowPaging="true" DataKeyNames=""
        onpageindexchanging="gvDatosCargos_PageIndexChanging"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
        <Columns>
            <asp:BoundField  HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
            <asp:BoundField  HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
            <asp:TemplateField HeaderText="Tipo Cargo/Concepto" SortExpression="TipoCargoConcepto">
                    <ItemTemplate>
                        <%# Eval("TipoCargoConcepto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField  HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />--%>
             
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
<%--            <asp:TemplateField HeaderText="Tipo Movimiento" SortExpression="TipoOperacion.TipoMovimiento.TipoMovimiento">
                    <ItemTemplate>
                        <%# Eval("TipoOperacion.TipoMovimiento.TipoMovimiento")%>
                    </ItemTemplate>
            </asp:TemplateField>--%>
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
           <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("EstadoDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField> 
            
            </Columns>
    </asp:GridView>
     </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="btnExportarCargosExcel" />
      </Triggers>
        </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>




     <asp:TabPanel runat="server" ID="tpCuentaCorrienteCargosDolar"
        HeaderText="Cuenta Corriente Cargos Dolar">
        <ContentTemplate>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
            <asp:ImageButton ID="btnExportarCargosExcelDolar" ImageUrl="~/Imagenes/Excel-icon.png" runat="server" onclick="btnExportarCargosExcelDolar_Click" Visible="false" />
            <br />
<asp:GridView ID="gvDatosCargosDolar" AllowPaging="true" DataKeyNames=""
        onpageindexchanging="gvDatosCargosDolar_PageIndexChanging"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
        <Columns>
            <asp:BoundField  HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
            <asp:BoundField  HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
            <asp:TemplateField HeaderText="Tipo Cargo/Concepto" SortExpression="TipoCargoConcepto">
                    <ItemTemplate>
                        <%# Eval("TipoCargoConcepto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField  HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />--%>
             
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
<%--            <asp:TemplateField HeaderText="Tipo Movimiento" SortExpression="TipoOperacion.TipoMovimiento.TipoMovimiento">
                    <ItemTemplate>
                        <%# Eval("TipoOperacion.TipoMovimiento.TipoMovimiento")%>
                    </ItemTemplate>
            </asp:TemplateField>--%>
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
           <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("EstadoDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField> 
            
            </Columns>
    </asp:GridView>
     </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="btnExportarCargosExcelDolar" />
      </Triggers>
        </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>




    <asp:TabPanel runat="server" ID="tpCuentaCorriente"
        HeaderText="Cuenta Corriente Facturacion" >
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
                    <li><asp:Button CssClass="dropdown-item" ID="btnAgregarComprobante" Visible="false" runat="server" Text="Agregar Cbte" OnClick="btnAgregarComprobante_Click" /></li>
                    <li><asp:Button CssClass="dropdown-item" ID="btnAgregarOC" Visible="false" runat="server" Text="Agregar Cobro" OnClick="btnAgregarOC_Click" /></li>
                    <li><asp:Button CssClass="dropdown-item" ID="btnAgregarRemito" Visible="false" runat="server" Text="Agregar Remito" OnClick="btnAgregarRemito_Click" /></li>
                    <li><asp:Button CssClass="dropdown-item" ID="btnAgregarPresupuesto" Visible="false" runat="server" Text="Agregar Presupuesto" OnClick="btnAgregarPresupuesto_Click" /></li>
                                        </ul>
                            </div>
                    </div>
                    </div>
                    <asp:UpdatePanel ID="upImprimir" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoReporte" runat="server" Text="Tipo Reporte"></asp:Label>
                            <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoReporte" runat="server">
                            </asp:DropDownList>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoComprobante" runat="server" Text="Tipo Comprobante"></asp:Label>
                            <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoComprobante" runat="server">
                            </asp:DropDownList>
                            </div>
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
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("TipoOperacionIdTipoOperacion") %>' />
                                    <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                         <asp:HiddenField ID="hdfIdEstado" runat="server" Value='<%#Eval("EstadoIdEstado") %>' />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" Visible="true" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular" 
                                AlternateText="Anular Factura" ToolTip="Anular Factura"  Visible="false"/>
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

      <asp:TabPanel runat="server" ID="tpCuentaCorrienteDolar" TabIndex="0"
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
                    <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <AUGE:popUpComprobantes ID="PopUpComprobantes1" runat="server" />
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
                                    <AUGE:popUpEnviarMail ID="PopUpEnviarMail1" runat="server" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="ImageButton1"
                                        OnClick="btnImprimir_Click" AlternateText="Imprimir" ToolTip="Imprimir" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="ImageButton2"
                                        OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
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
                            </asp:TemplateField>                <%--<asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
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
</asp:TabContainer>

</asp:Content>