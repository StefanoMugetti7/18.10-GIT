<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadosGrupoListar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosGrupoListar" Title="" %>

<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="AfiliadosListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Número Socio"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSocio" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMatricula" runat="server" Text="Legajo"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtMatricula" runat="server"></AUGE:NumericTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-8">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                    </div>
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Empresa" SortExpression="Empresa" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("Empresa")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Numero Socio" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="NumeroSocio" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("NumeroSocio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Legajo" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="MatriculaIAF">
                            <ItemTemplate>
                                <%# Eval("MatriculaIAF")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento">
                            <ItemTemplate>
                                <%# Eval("TipoDocumento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Número" DataField="NumeroDocumento" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
                        <asp:BoundField HeaderText="Apellido y Nombre" DataField="ApellidoNombre" SortExpression="ApellidoNombre" />
                        <%--<asp:BoundField  HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" />--%>
                        <%--<asp:TemplateField HeaderText="Categoria" SortExpression="Categoria">
                <ItemTemplate>
                    <%# Eval("Categoria")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Tipo Socio" SortExpression="AfiliadoTipo">
                <ItemTemplate>
                    <%# Eval("AfiliadoTipo")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ultimo Cobro" ItemStyle-Wrap="false" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("FechaUltimoCobro", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ultimo Rechazo" ItemStyle-Wrap="false" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("FechaRechazo", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Motivo Rechazo" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("MotivoRechazo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cant.Prestamos" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("CantidadPrestamos")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Cargos Mes Tope" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("TotalCargosMesTope", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                         AlternateText="Ingresar a la cuenta" ToolTip="Ingresar a la cuenta" />
                </ItemTemplate>
            </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
