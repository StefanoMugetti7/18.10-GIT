<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadosBuscar.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadosBuscar" %>

<div class="AfiliadosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBuscar" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Número Socio"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSocio" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>

                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                        OnClick="btnBuscar_Click" />
                </div></div>
                <div class="form-group row">

                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                    </div>

                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox></div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox></div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Incluir Familiares / Apoderado"></asp:Label>

                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkFamiliares" CssClass="form-control" runat="server" /></div>
                </div>
            </asp:Panel>
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Numero Socio" SortExpression="NumeroSocio">
                        <ItemTemplate>
                            <%# Eval("NumeroSocio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento.TipoDocumento">
                        <ItemTemplate>
                            <%# Eval("TipoDocumento.TipoDocumento")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
                    <asp:BoundField HeaderText="Apellido y Nombre" DataField="ApellidoNombre" SortExpression="ApellidoNombre" />
                    <%--<asp:BoundField  HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" />--%>
                    <asp:TemplateField HeaderText="Categoria" SortExpression="Categoria.Categoria">
                        <ItemTemplate>
                            <%# Eval("Categoria.Categoria")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Socio" SortExpression="AfiliadoTipo.AfiliadoTipo">
                        <ItemTemplate>
                            <%# Eval("AfiliadoTipo.AfiliadoTipo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Parentesco" SortExpression="Parentesco.Parentesco">
                <ItemTemplate>
                    <%# Eval("Parentesco.Parentesco")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Seleccionar" ToolTip="Seleccionar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
