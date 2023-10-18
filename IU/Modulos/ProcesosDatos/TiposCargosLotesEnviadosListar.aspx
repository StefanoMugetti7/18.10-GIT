<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposCargosLotesEnviadosListar.aspx.cs" Inherits="IU.Modulos.ProcesosDatos.TiposCargosLotesEnviadosListar" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceEncabezado" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ProcesosDatos">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodo" runat="server" Text="Periodo"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlPeriodo" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma de Cobro"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                    </div>
                </div>
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTipoCargoLoteEnviado"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Nro. Lote">
                            <ItemTemplate>
                                <%# Eval("IdTipoCargoLoteEnviado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Forma Cobro">
                            <ItemTemplate>
                                <%# Eval("FormaCobro")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Periodo">
                            <ItemTemplate>
                                <%# Eval("Periodo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cant. de Registros">
                            <ItemTemplate>
                                <%# Eval("CantidadRegistros")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Enviado">
                            <ItemTemplate>
                                <%# Eval("ImporteEnviado", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Cobrado">
                            <ItemTemplate>
                                <%# Eval("ImporteCobrado", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
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
