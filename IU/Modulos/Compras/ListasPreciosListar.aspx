<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListasPreciosListar.aspx.cs" Inherits="IU.Modulos.Compras.ListasPreciosListar" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>--%>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasPrecios">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoLista" runat="server" Text="Codigo"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCodigoLista" runat="server" Enabled="true"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtRazonSocial" runat="server" Enabled="true"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" TextMode="MultiLine" Enabled="true"></asp:TextBox>
                    </div>
                    <div class="col-sm-8"></div>
                </div>
                <%--<auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Codigo" SortExpression="IdListaPrecio">
                            <ItemTemplate>
                                <%# Eval("IdListaPrecio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Inicio Vigencia" SortExpression="FechaInicio">
                            <ItemTemplate>
                                <%# Eval("FechaInicioVigencia", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Fin Vigencia" SortExpression="FechaFin">
                            <ItemTemplate>
                                <%# Eval("FechaFinVigencia", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />--%>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" Visible="true" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                    AlternateText="Anular Lista Precio" ToolTip="Anular Lista Precio" Visible="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
