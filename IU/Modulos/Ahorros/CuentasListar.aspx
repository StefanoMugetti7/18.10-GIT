<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CuentasListar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasListar" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<div class="CuentasListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            <br />
            <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                               <asp:BoundField  HeaderText="Número" DataField="NumeroCuenta" SortExpression="NumeroCuenta" />
                        <asp:BoundField  HeaderText="Denominación" DataField="Denominacion"  SortExpression="Denominacion" />
                        <asp:TemplateField HeaderText="Tipo de Cuenta" SortExpression="CuentaTipo.Descripcion">
                            <ItemTemplate>
                                <%# Eval("CuentaTipo.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.miMonedaDescripcion">
                            <ItemTemplate>
                                <%# Eval("Moneda.miMonedaDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                 
                         <asp:TemplateField HeaderText="Saldo Actual" SortExpression="ImportePrestamo" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda.Moneda"), Eval("SaldoActual", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/movimientos.png" runat="server" CommandName="Movimientos" ID="btnMovimientos" 
                                    AlternateText="Movimientos" ToolTip="Movimientos" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
                <br />
                <center>
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
