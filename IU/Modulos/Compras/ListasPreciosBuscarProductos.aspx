<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListasPreciosBuscarProductos.aspx.cs" Inherits="IU.Modulos.Compras.BuscarProductos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">




     <div class="ListasPrecios">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoProducto" runat="server" Text="Codigo de Producto"></asp:Label>
                    <div class="col-sm-3">
                         <Evol:CurrencyTextBox Prefix="" NumberOfDecimals="0" ThousandsSeparator="" CssClass="form-control" ID="txtCodigoProducto" runat="server" Enabled="true"></Evol:CurrencyTextBox>
                    </div>
                     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcionProducto" runat="server" Text="Descripción de Producto"></asp:Label>
                    <div class="col-sm-3">
                         <asp:TextBox CssClass="form-control" ID="txtDescripcionProducto" runat="server" Enabled="true"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIvas" runat="server" Text="IVA"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlIvas" Enabled="true" runat="server"/>

                    </div>
                    </div>
                <div class="form-group row">
                    <div class="col-sm-9"></div>
                    <div class="col-sm-3">

                         <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                    </div>
                </div>
                    
              

                  <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
                 <asp:GridView ID="gvItems" AllowPaging="true" PageSize="50" AllowSorting="true"
                                             DataKeyNames="IdListaPrecioDetalle" OnPageIndexChanging="gvItems_PageIndexChanging" OnSorting="gvItems_Sorting"
                                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                            >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Código" SortExpression="ProductoIdProducto" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%# Eval("ProductoIdProducto")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                
                                                <asp:TemplateField HeaderText="Producto" SortExpression="ProductoDescripcion">
                                                    <ItemTemplate>
                                                        <%# Eval("ProductoDescripcion")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Precio" SortExpression="Precio" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                          <%# string.Concat(Eval("Moneda"), Eval("Precio", "{0:N2}"))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Margen %" SortExpression="MargenPorcentaje" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                         <%# Eval("MargenPorcentaje", "{0:N2}") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="AliCuota" SortExpression="AliCuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <%# Eval("AliCuota", "{0:N2}") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Precio Final" SortExpression="PrecioConIva" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                         <%# string.Concat(Eval("Moneda"), Eval("PrecioConIva", "{0:N2}"))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Precio Total Pesos" SortExpression="PrecioTotalPesos" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" >
                                                    <ItemTemplate>
                                                           <%# string.Concat("$ ", Eval("PrecioTotalPesos", "{0:N2}"))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock Actual" SortExpression="StockActual" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                         <%# Eval("StockActual") %>
                                                      
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <%-- <asp:TemplateField HeaderText="Filial" SortExpression="IdFilial">
                                                    <ItemTemplate>
                                                         <%# Eval("IdFilial") %>
                                                      
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                               
                                               
                                            </Columns>
                                        </asp:GridView>
            
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
