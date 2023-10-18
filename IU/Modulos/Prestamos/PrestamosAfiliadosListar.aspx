<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosListar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosListar" Title=""%>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<div class="PrestamosAfiliadosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Solicitar" onclick="btnAgregar_Click" />
                <div class="Espacio"></div>                
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                  onclick="btnImprimir_Click"  AlternateText="Imprimir Análitico de Anticipos" ToolTip="Imprimir Análitico de Anticipos" />
            <br />
            <br />
                <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
                <asp:TabPanel runat="server" ID="tpPrestamosActivos" HeaderText="Prestamos Activos" >
                <ContentTemplate>
                    <div class="table-responsive">
                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="false" 
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdPrestamo, IdTipoOperacion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false" SortExpression="FechaEvento">
                                <ItemTemplate>
                                    <%# Eval("FechaPrestamo", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Autorizado" ItemStyle-Wrap="false" SortExpression="FechaAutorizado">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("FechaAutorizado")) > Convert.ToDateTime("1900/01/01") ?
                                        Eval("FechaAutorizado", "{0:dd/MM/yyyy}") : string.Empty %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="A partir de" ItemStyle-Wrap="false" SortExpression="FechaValidezAutorizado">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("FechaValidezAutorizado")) > Convert.ToDateTime("1900/01/01") ?
                                        Eval("FechaValidezAutorizado", "{0:dd/MM/yyyy}") : string.Empty %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Confirmado" ItemStyle-Wrap="false" SortExpression="FechaConfirmacion">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("FechaConfirmacion")) > Convert.ToDateTime("1900/01/01") ?
                                        Eval("FechaConfirmacion", "{0:dd/MM/yyyy}") : string.Empty %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField  HeaderText="#Prestamo" ItemStyle-Wrap="false" DataField="NroDeIdentificacion" SortExpression="NroDeIdentificacion" />
                            <asp:TemplateField HeaderText="Importe" SortExpression="ImportePrestamo" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("ImportePrestamo", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuota" SortExpression="ImporteCuota" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("ImporteCuota", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField  HeaderText="Cuotas" ItemStyle-Wrap="false" DataField="CantidadCuotas" SortExpression="CantidadCuotas" />
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <HeaderTemplate>
                                    <asp:Label ID="lblCantidadCuotasPendientes" runat="server" Text="Pendientes" ToolTip="Cantidad de Cuotas Pendientes"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# Eval("CantidadCuotasPendientes")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Deuda" SortExpression="SaldoDeuda" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("SaldoDeuda", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion">
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Forma de Cobro" SortExpression="FormaCobro">
                                <ItemTemplate>
                                    <%# Eval("FormaCobro")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inversor" SortExpression="Inversor" >
                                <ItemTemplate>
                                    <%# Eval("Inversor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vendedor" SortExpression="Vendedor" >
                                <ItemTemplate>
                                    <%# Eval("Vendedor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leg.Conf." SortExpression="LegajoConformado" >
                                <ItemTemplate>
                                    <%# Eval("LegajoConformado")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                        AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/9x.png" runat="server" CommandName="PreAutorizar" ID="btnPreAutorizar" 
                                        AlternateText="Pre Autorizar" ToolTip="Pre Autorizar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" ID="btnAutorizar" 
                                        AlternateText="Autorizar" ToolTip="Autorizar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Cancelar" ID="btnCancelar" 
                                    AlternateText="Cancelar" ToolTip="Cancelar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="AnularCancelar" ID="btnAnularCancelar" 
                                    AlternateText="Anular Cancelación Pendiente" ToolTip="Anular Cancelación Pendiente" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Anular" ID="btnAnular" 
                                        AlternateText="Anular" ToolTip="Anular" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" Visible="false" CommandName="AnularConfirmar" ID="btnAnularConfirmado" 
                                        AlternateText="Anular Prestamo Confirmado" ToolTip="Anular Prestamo Confirmado" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/AplicarCheque.png" runat="server" Visible="false" CommandName="AplicarCheque" ID="btnAplicarCheque" 
                                        AlternateText="Aplicar Cheque" ToolTip="Aplicar Cheque" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                    </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpPrestamosHistoricos" HeaderText="Prestamos Finalizados/Cancelados" >
                    <ContentTemplate>
                        <asp:GridView ID="gvPrestamosHistoricos" OnRowCommand="gvPrestamosHistoricos_RowCommand" AllowPaging="true" AllowSorting="false" 
                    OnRowDataBound="gvPrestamosHistoricos_RowDataBound" DataKeyNames="IdPrestamo"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    onpageindexchanging="gvPrestamosHistoricos_PageIndexChanging" onsorting="gvPrestamosHistoricos_Sorting" >
                        <Columns>
                            <%--<asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false" SortExpression="FechaPrestamo">
                                <ItemTemplate>
                                    <%# Eval("FechaPrestamo", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Confirmado" ItemStyle-Wrap="false" SortExpression="FechaConfirmacion">
                                <ItemTemplate>
                                    <%# Eval("FechaConfirmacion") == DBNull.Value || Convert.ToDateTime(Eval("FechaConfirmacion")) < Convert.ToDateTime("1900/01/01") ?
                                    string.Empty : Eval("FechaConfirmacion", "{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cancelado" ItemStyle-Wrap="false" SortExpression="FechaConfirmacionCancelacion">
                                <ItemTemplate>
                                    <%# Eval("FechaConfirmacionCancelacion") == DBNull.Value || Convert.ToDateTime(Eval("FechaConfirmacionCancelacion")) < Convert.ToDateTime("1900/01/01") ?
                                    string.Empty : Eval("FechaConfirmacionCancelacion", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finalizado" ItemStyle-Wrap="false" SortExpression="FechaFinalizacion">
                                <ItemTemplate>
                                    <%# Eval("FechaFinalizacion", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField  HeaderText="#Prestamo" ItemStyle-Wrap="false" DataField="NroDeIdentificacion" SortExpression="NroDeIdentificacion" />
                            <asp:BoundField  HeaderText="Importe" ItemStyle-Wrap="false" DataFormatString="{0:C2}" DataField="ImportePrestamo" SortExpression="ImportePrestamo" />
                            <asp:BoundField  HeaderText="Cuota" ItemStyle-Wrap="false" DataFormatString="{0:C2}" DataField="ImporteCuota" SortExpression="ImporteCuota" />
                            <asp:BoundField  HeaderText="Deuda" ItemStyle-Wrap="false" DataFormatString="{0:C2}" DataField="SaldoDeuda" SortExpression="SaldoDeuda" /> 
                            <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion">
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Inversor" SortExpression="Inversor" >
                                <ItemTemplate>
                                    <%# Eval("Inversor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vendedor" SortExpression="Vendedor" >
                                <ItemTemplate>
                                    <%# Eval("Vendedor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" >
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                        AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                    </asp:GridView>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
                <br />
                <center>
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>