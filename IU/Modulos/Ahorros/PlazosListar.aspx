<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlazosListar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosListar" Title="" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="PlazosListar">
                <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
                    <div class="col-sm-4"></div>
                        <div class="col-sm-4"> <%--ml-auto p-2--%>
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
                </div>
                    </div>
                <div class="table-responsive">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Código" DataField="IdPlazos" ItemStyle-Wrap="false" SortExpression="IdPlazos" />
<%--                        <asp:BoundField  HeaderText="Plazo Días" DataField="PlazoDias" SortExpression="PlazoDias" />
                        <asp:BoundField  HeaderText="Tasa Interes" DataField="TasaInteres" SortExpression="TasaInteres" />--%>
                        <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Moneda.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Descripcion" DataField="Descripcion" SortExpression="Descripcion" />
                        <asp:BoundField  HeaderText="Fecha Alta" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                        <asp:BoundField  HeaderText="Fecha Desde" DataField="FechaDesde" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
                    </div>
                <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false"  onclick="btnCancelar_Click" />
               </div>
                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>