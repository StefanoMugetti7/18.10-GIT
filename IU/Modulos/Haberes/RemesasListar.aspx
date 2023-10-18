<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="RemesasListar.aspx.cs" Inherits="IU.Modulos.Haberes.RemesasListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RemesasListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <%--<asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            <br />
            <br />--%>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="True" AllowSorting="True" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="False" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:TemplateField HeaderText="Periodo" SortExpression="Periodo">
                            <ItemTemplate>
                                <%# Eval("Periodo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo" SortExpression="RemesaTipo.Descripcion">
                            <ItemTemplate>
                                <%# Eval("RemesaTipo.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" HeaderText="Cantidad Registros" DataField="CantidadRegistros" SortExpression="CantidadRegistros" />
                        <asp:BoundField ReadOnly="true" HeaderText="Importe Total" DataField="ImporteTotal" DataFormatString="{0:C2}" SortExpression="ImporteTotal" />
                        <asp:BoundField ReadOnly="true" HeaderText="Cantidad Depositar" DataField="CantidadDepositar" SortExpression="CantidadDepositar" />
                        <asp:BoundField ReadOnly="true" HeaderText="Importe Depositar" DataField="ImporteDepositar" DataFormatString="{0:C2}" SortExpression="ImporteDepositar" />
                        <asp:BoundField ReadOnly="true" HeaderText="Cantidad Depositada" DataField="CantidadDepositada" SortExpression="CantidadDepositada" />
                        <asp:BoundField ReadOnly="true" HeaderText="Importe Depositado" DataField="ImporteDepositado" DataFormatString="{0:C2}" SortExpression="ImporteDepositado" />
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
                                    AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/properties_f2.png" runat="server" CommandName="Movimientos" ID="btnMovimientos" 
                                    AlternateText="Movimientos" ToolTip="Movimientos" />--%>
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
