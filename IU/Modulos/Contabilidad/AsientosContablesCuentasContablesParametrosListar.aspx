<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosContablesCuentasContablesParametrosListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosContablesCuentasContablesParametrosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="BancosCuentasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo Valor" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValor" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiliales" runat="server" Text="Filial" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
                    </div>
                   <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDenominacion" runat="server" Text="Denominación" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDenominacion" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-9"></div>
                       <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAsientoContableCuentaContableParametro"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValorTipoValor">
                            <ItemTemplate>
                                <%# Eval("TipoValorTipoValor")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filial" SortExpression="FilialFilial">
                            <ItemTemplate>
                                <%# Eval("FilialFilial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Banco" SortExpression="BancoCuentaBancoDescripcion">
                            <ItemTemplate>
                                <%# Eval("BancoCuentaBancoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField HeaderText="Denominacion" SortExpression="Denominacion">
                            <ItemTemplate>
                                <%# Eval("BancoCuentaDenominacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cuenta Bancaria" SortExpression="BancoCuentaNumeroCuenta">
                            <ItemTemplate>
                                <%# Eval("BancoCuentaNumeroCuenta")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moneda" SortExpression="MonedamiMonedaDescripcion">
                            <ItemTemplate>
                                <%# Eval("MonedaDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cuenta Contable" SortExpression="CuentaContableNumeroCuenta">
                            <ItemTemplate>
                                <%# Eval("CuentaContableNumeroCuenta")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" Visible="false" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
