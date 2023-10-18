<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SimulacionesListar.aspx.cs" Inherits="IU.Modulos.Prestamos.SimulacionesListar" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<div class="PrestamosAfiliadosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            <br />
            <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="false" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:TemplateField HeaderText="Fecha" SortExpression="FechaEvento">
                            <ItemTemplate>
                                <%# Eval("FechaPrestamo", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Simulación" DataField="IdSimulacion" SortExpression="IdSimulacion" />
                        <asp:BoundField  HeaderText="Importe"  DataFormatString="{0:C2}" DataField="ImporteSolicitado" SortExpression="ImporteSolicitado" /> 
                        <asp:BoundField  HeaderText="Cuota"  DataFormatString="{0:C2}" DataField="ImporteCuota" SortExpression="ImporteCuota" />
                        <asp:BoundField  HeaderText="Deuda"  DataFormatString="{0:C2}" DataField="SaldoDeuda" SortExpression="SaldoDeuda" /> 
                        <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion.TipoOperacion">
                            <ItemTemplate>
                                <%# Eval("TipoOperacion.TipoOperacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
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
