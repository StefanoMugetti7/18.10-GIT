<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="RequerimientosAfiliadosListar.aspx.cs" Inherits="IU.Modulos.CRM.RequerimientosAfiliadosListar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>


    <div class="RequerimientosListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPrioridad" runat="server" Text="Prioridad" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlPrioridad" runat="server" />
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblAsignadoA" runat="server" Text="Asignado a"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtAsignadoA" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>

                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                    runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                <br />
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdRequerimiento"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="IdRequerimiento">
                            <ItemTemplate>
                                <%# Eval("IdRequerimiento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Titulo" SortExpression="Nombre">
                            <ItemTemplate>
                                <%# Eval("Nombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cliente" SortExpression="Cliente">
                            <ItemTemplate>
                                <%# Eval("Cliente")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Apertura" SortExpression="FechaAlta">
                            <ItemTemplate>
                                <%# Eval("FechaAlta")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prioridad" SortExpression="Prioridad">
                            <ItemTemplate>
                                <%# Eval("Prioridad")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Asignado A" SortExpression="Asignado">
                            <ItemTemplate>
                                <%# Eval("Tecnico")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Categoria" SortExpression="Categoria">
                            <ItemTemplate>
                                <%# Eval("Categoria")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Limite" SortExpression="FechaRequerimiento">
                            <ItemTemplate>
                                <%# Eval("FechaRequerimiento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Usuario Alta">
                            <ItemTemplate>
                                <%# Eval("UsuarioAlta")%>
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
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

