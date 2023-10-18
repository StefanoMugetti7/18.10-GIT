<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ValoresSistemaCuentasContablesListar.aspx.cs" Inherits="IU.Modulos.TGE.ValoresSistemaCuentasContablesListar" %>

<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="CuentasContables" TagPrefix="AUGE" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasValoresListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:Label CssClass="labelEvol" ID="lblParametro" runat="server" Text="Codigo Relacion" />
                <asp:TextBox CssClass="textboxEvol" ID="txtParametro" runat="server"></asp:TextBox>
           <div class="Espacio"></div>



                <asp:Label CssClass="labelEvol" ID="lblCodigoDetalle" runat="server" Text="Codigo Sistema Detalle" />
                <asp:TextBox CssClass="textboxEvol" ID="txtCodigoSistema" runat="server"></asp:TextBox>
             <div class="Espacio"></div>--%>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaValor" runat="server" Text="Lista Valor:"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlListaValor" runat="server">
                        </asp:DropDownList>
                    </div>


                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />

                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>

                <asp:UpdatePanel ID="upCuentasContables" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <AUGE:CuentasContables ID="ctrCuentasContables" MostrarEtiquetas="true" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>

                <br />

                <%-- <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server">
            </asp:DropDownList>
              <br /> --%>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                    AllowPaging="true" AllowSorting="true" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="IdListaValorSistemaDetalleCuentaContable" HeaderText="Codigo Relacion" SortExpression="CodigoRelacion" />
                        <asp:TemplateField HeaderText="Lista Valor Sistema" SortExpression="ListaValorSistemaDetalle.Descripcion">
                            <ItemTemplate>
                                <%# Eval("ListaValorSistemaDetalle.ListaValorSistema.ListaValor")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor Sys Det" SortExpression="ListaValorSistemaDetalle.Descripcion">
                            <ItemTemplate>
                                <%# Eval("ListaValorSistemaDetalle.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cuenta Contable" SortExpression="CuentaContable.Descripcion">
                            <ItemTemplate>
                                <%# Eval("CuentaContable.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cuenta Contable" SortExpression="CuentaContable.Descripcion">
                            <ItemTemplate>
                                <%# Eval("CuentaContable.NumeroCuenta")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
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
