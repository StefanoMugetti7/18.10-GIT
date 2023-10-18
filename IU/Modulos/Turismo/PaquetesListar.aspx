<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PaquetesListar.aspx.cs" Inherits="IU.Modulos.Turismo.PaquetesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PaquetesListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Filtro"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdPaquete"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Nro. Paquete" SortExpression="IdPaquete">
                            <ItemTemplate>
                                <%# Eval("IdPaquete")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                            <ItemTemplate>
                                <%# Eval("Nombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de Cargo" SortExpression="TipoCargo">
                            <ItemTemplate>
                                <%# Eval("TipoCargo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad" SortExpression="CantidadServicio">
                            <ItemTemplate>
                                <%# Eval("CantidadServicio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad Vendida" SortExpression="CantidadVendida">
                            <ItemTemplate>
                                <%# Eval("CantidadVendida")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad Disponible" SortExpression="CantidadDisponible">
                            <ItemTemplate>
                                <%# Eval("CantidadDisponible")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Consultar" ToolTip="Consultar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
