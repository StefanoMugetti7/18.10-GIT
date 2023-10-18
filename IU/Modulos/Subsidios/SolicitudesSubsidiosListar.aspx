<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SolicitudesSubsidiosListar.aspx.cs" Inherits="IU.Modulos.Subsidios.SolicitudesSubsidiosListar" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpEnviarMail.ascx" tagname="popUpEnviarMail" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">




     <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
       
    
           <asp:TabPanel runat="server" ID="TabPanel1" TabIndex="0"
                HeaderText="Solicitudes de Subsidio">
                    <ContentTemplate>
<div class="SolicitudesSubsidiosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSolicitud" runat="server" Text="Número Solicitud"></asp:Label>
                <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSolicitud" runat="server"></AUGE:NumericTextBox>
            </div>
            <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoSolicitud" runat="server" Text="Tipo Solicitud" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlTipoSolicitud" runat="server" />
            <div class="Espacio"></div>--%>
                <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
                onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
            onclick="btnAgregar_Click" />
                    </div>
                <div class="col-sm-3"></div>
                </div>
           
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            >
                <Columns>
                    <asp:TemplateField HeaderText="Número Solicitud" SortExpression="NumeroSolicitud">
                            <ItemTemplate>
                                <%# Eval("IdSolicitudPago")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Subsidio" SortExpression="TipoSolicitudPago.Descripcion">
                        <ItemTemplate>
                            <%# Eval("TipoSolicitudPago.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observacion" SortExpression="Observacion">
                        <ItemTemplate>
                            <%# Eval("Observacion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("ImporteTotal", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Estado.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>    
                     <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="AnularSolicitud" ToolTip="AnularSolicitud" />
                               <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                        </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
                        </ContentTemplate>
                 </asp:TabPanel>
              

    <asp:TabPanel runat="server" ID="tpCuentaCorriente" TabIndex="0"
                HeaderText="Ordenes de Pago">
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
                                    <asp:Button CssClass="botonesEvol" ID="btnBuscarCtaCte" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                    <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                                    </div>
                                  <%--  <div class="col-sm-1">
                                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                                        OnClick="btnImprimir_Click" AlternateText="Imprimir" ToolTip="Imprimir" />
                                    <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                                    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail"
                                        OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />

                                </div>--%>
                                <div class="col-sm-1">
                                   
                                     
                                    
                                     <asp:Button CssClass="botonesEvol" ID="btnAgregarOP" runat="server" Text="Agregar OP" OnClick="btnAgregarOP_Click"  />
                                               
                                         
                               
                                </div>
                            </div>
                            <div class="form-group row">
                             <%--   <div class="col-sm-12">
                                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                                </div>--%>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                    <asp:GridView ID="gvDatosCuentaCorriente" AllowPaging="true" DataKeyNames="FechaMovimiento" OnRowDataBound="gvDatosCuentaCorriente_RowDataBound" OnRowCommand="gvDatosCuentaCorriente_RowCommand"
                                        OnPageIndexChanging="gvDatosCuentaCorriente_PageIndexChanging"
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
                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    <%--    <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportarExcel" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
          </asp:TabContainer>

</asp:Content>
