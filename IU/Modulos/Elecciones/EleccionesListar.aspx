<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="EleccionesListar.aspx.cs" Inherits="IU.Modulos.Elecciones.EleccionesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="EleccionesListar">
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
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdEleccion"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="IdEleccion">
                            <ItemTemplate>
                                <%# Eval("IdEleccion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año" SortExpression="Anio">
                            <ItemTemplate>
                                <%# Eval("Anio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observacion" SortExpression="Eleccion">
                            <ItemTemplate>
                                <%# Eval("Eleccion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <ItemTemplate>
                                <%# Eval("Estado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Consultar" ToolTip="Consultar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Listar" Visible="false" ID="btnDetalle" 
                                AlternateText="Detalle" ToolTip="Detalle" />
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
