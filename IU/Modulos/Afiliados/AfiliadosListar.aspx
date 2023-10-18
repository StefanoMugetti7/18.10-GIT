<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadosListar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosListar" Title="" %>

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
                        <asp:TextBox CssClass="form-control text-right" ID="txtNumeroSocio" runat="server"></asp:TextBox>
                    </div>

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
                        <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMatricula" runat="server" Text="Legajo"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtMatricula" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAlertas" runat="server" Text="Alertas"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlAlertas" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-check">
                            <asp:CheckBox ID="chkFamiliares" CssClass="form-check-input" runat="server" />
                            <asp:Label CssClass="col-form-label form-check-label" ID="lblFamiliares" runat="server" Text="Incluir Familiares / Apoderado"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCategoria" runat="server" TabIndex="0">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4"></div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <div class="col-sm-12">
                    <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                        SkinID="GrillaBasicaFormalSticky" OnRowDataBound="gvDatos_RowDataBound" runat="server" ShowFooter="true"
                        DataKeyNames="IdAfiliado,IdAfiliadoRef,AfiliadoTipoIdAfiliadoTipo" AutoGenerateColumns="false" 
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Id Socio" SortExpression="NumeroSocio">
                                <ItemTemplate>
                                    <%# Eval("IdAfiliado")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Numero Socio" SortExpression="NumeroSocio">
                                <ItemTemplate>
                                    <%# Eval("NumeroSocio")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Legajo" SortExpression="MatriculaIAF">
                                <ItemTemplate>
                                    <%# Eval("MatriculaIAF")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumentoTipoDocumento">
                                <ItemTemplate>
                                    <%# Eval("TipoDocumentoTipoDocumento")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Numero" SortExpression="NumeroDocumento">
                                <ItemTemplate>
                                    <%# Eval("NumeroDocumento")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apellido y Nombre" SortExpression="ApellidoNombre">
                                <ItemTemplate>
                                    <%# Eval("ApellidoNombre")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Categoria" SortExpression="CategoriaCategoria">
                                <ItemTemplate>
                                    <%# Eval("CategoriaCategoria")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Participantes" DataField="CantidadParticipantes" SortExpression="CantidadParticipantes" />
                            <asp:TemplateField HeaderText="Tipo Socio" SortExpression="AfiliadoTipo.AfiliadoTipo">
                                <ItemTemplate>
                                    <%# Eval("AfiliadoTipoAfiliadoTipo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Ingresar a la cuenta" ToolTip="Ingresar a la cuenta" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Evol:EvolGridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
