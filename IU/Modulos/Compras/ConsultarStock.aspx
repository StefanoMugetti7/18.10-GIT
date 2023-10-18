<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ConsultarStock.aspx.cs" Inherits="IU.Modulos.Compras.ConsultarStock" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ConsultarStock">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoProducto" runat="server" Text="Codigo" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCodigoProducto" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Producto" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiliales" runat="server" Text="Filial" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
                </div>
                <div class="col-sm-8"></div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                </div>
            </div>
        <asp:GridView ID="gvDatos"  AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" 
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Filial" DataField="Filial" SortExpression="filial" />
                        <asp:TemplateField HeaderText="Codigo" SortExpression="ProductoIdProducto">
                            <ItemTemplate>
                                <%# Eval("ProductoIdProducto")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto" SortExpression="Producto.Descripcion">
                            <ItemTemplate>
                                <%# Eval("ProductoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Familia" SortExpression="Producto.Familia.Descripcion">
                            <ItemTemplate>
                                <%# Eval("ProductoFamilia")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField  HeaderText="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="StockActual" SortExpression="Stock" />  
                         <asp:BoundField  HeaderText="Valorizacion por Compra" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="ValorizacionCompras" DataFormatString="{0:C}"  SortExpression="ValorizacionCompras" /> 
                        <asp:BoundField  HeaderText="Precio Unitario Compra" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="PrecioUnitarioCompras" DataFormatString="{0:C}" SortExpression="PrecioUnitarioCompras" /> 
                        <asp:BoundField  HeaderText="Valorizacion por Venta" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="ValorizacionVentas" DataFormatString="{0:C}"  SortExpression="ValorizacionVentas" /> 
                        <asp:BoundField  HeaderText="Precio Unitario Venta" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="Precio" DataFormatString="{0:C}" SortExpression="Precio" />
                        </Columns>
                </asp:GridView>

        </ContentTemplate>
        <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
    </asp:UpdatePanel>
</div>
</asp:Content>