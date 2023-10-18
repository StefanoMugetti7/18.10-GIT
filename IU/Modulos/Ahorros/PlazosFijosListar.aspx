<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PlazosFijosListar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosFijosListar" Title="" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>--%>
<%--<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <%--<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div >
        <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" onclick="btnAgregar_Click" Text="Agregar" />

        <div class="table-responsive">
        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdPlazoFijo"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
            <Columns>
                <asp:BoundField  HeaderText="Número" DataField="IdPlazoFijo" SortExpression="IdPlazoFijo" />
                <asp:TemplateField HeaderText="Cuenta" SortExpression="Cuenta.IdCuenta">
                    <ItemTemplate>
                        <%# Eval("CuentaIdCuenta")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha" SortExpression="FechaInicioVigencia">
                    <ItemTemplate>
                        <%# Eval("FechaInicioVigencia", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Moneda" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" SortExpression="Moneda.Moneda">
                    <ItemTemplate>
                        <%# Eval("MonedaMoneda")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  HeaderText="Plazo" DataField="PlazoDias" SortExpression="PlazoDias" />
                <asp:TemplateField HeaderText="Vencimiento" SortExpression="FechaVencimiento">
                    <ItemTemplate>
                        <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  HeaderText="Tasa Interes" DataField="TasaInteres" SortExpression="TasaInteres" />
                 <asp:TemplateField HeaderText="Capital" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right"  SortExpression="Capital">
                    <ItemTemplate>
                        <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteCapital", "{0:N2}"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Interes " ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" SortExpression="Interes">
                    <ItemTemplate>
                        <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteInteres", "{0:N2}"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" SortExpression="Total">
                    <ItemTemplate>
                        <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteTotal", "{0:N2}"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Renovacion" SortExpression="TipoRenovacionTipoRenovacion">
                    <ItemTemplate>
                        <%# Eval("TipoRenovacionTipoRenovacion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nro. Renov." ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-right" SortExpression="IdPlazoFijoAnterior">
                    <ItemTemplate>
                        <%# Eval("IdPlazoFijoAnterior")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                    <ItemTemplate>
                        <%# Eval("EstadoDescripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"  >
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" Visible="false" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Consultar" ToolTip="Consultar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" Visible="false" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" ToolTip="Modificar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Anular" ID="btnAnular" 
                                AlternateText="Anular" ToolTip="Anular" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Cancelar" ID="btnCancelar" 
                                AlternateText="Cancelar" ToolTip="Cancelar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/baja.png" runat="server" Visible="false" CommandName="AnularCancelacion" ID="btnAnularCancelacion" 
                                AlternateText="Anular Cancelacion" ToolTip="Anular Cancelacion" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" Visible="false" CommandName="Renovar" ID="btnRenovar" 
                                AlternateText="Renovar" ToolTip="Renovar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/pesos.png" runat="server" Visible="false" CommandName="Pagar" ID="btnPagar" 
                                AlternateText="Pagar Plazo Fijo" ToolTip="Pagar Plazo Fijo" />
                               <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/reutilizar.png" runat="server" Visible="false" CommandName="AnularRenovacion" ID="btnAnularRenovacion" 
                                AlternateText="Renovacion Anticipada" ToolTip="Renovacion Anticipada" />
                        </ItemTemplate>
                    </asp:TemplateField>          
            </Columns>
        </asp:GridView>
            </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
