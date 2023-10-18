<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestamosCesionesListar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosCesionesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="PrestamosCesionesListar">
                   <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor" />
                   <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlProveedores" runat="server" />
              </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
               <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" /></div>
               <div class="col-sm-3">  <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
              </div></div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Código" DataField="IdPrestamoCesion" ItemStyle-Wrap="false" SortExpression="IdPrestamoCesion" />
                        <asp:TemplateField HeaderText="Proveedor" SortExpression="Cesionario.Proveedor.RazonSocial">
                            <ItemTemplate>
                                <%# Eval("Cesionario.Proveedor.RazonSocial")%>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField  HeaderText="Descripcion" DataField="Descripcion" SortExpression="Descripcion" />
                        <asp:BoundField  HeaderText="Fecha Alta" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                        <asp:BoundField  HeaderText="Tasa" DataField="Tasa" SortExpression="Tasa" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField  HeaderText="VAN" DataField="VAN" SortExpression="VAN" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField  HeaderText="Cantidad" DataField="Cantidad" SortExpression="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField  HeaderText="Total Amortizacion" DataField="TotalAmortizacion" SortExpression="TotalAmortizacion" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField  HeaderText="Total Interes" DataField="TotalInteres" SortExpression="TotalInteres" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"/>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" ID="btnAutorizar" 
                                        AlternateText="Autorizar" ToolTip="Autorizar" Visible="false" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular" 
                                    AlternateText="Anular" ToolTip="Anular" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

