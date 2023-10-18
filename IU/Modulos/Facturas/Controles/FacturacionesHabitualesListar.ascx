<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacturacionesHabitualesListar.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturacionesHabitualesListar" %>
<asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
<div class="table-responsive">
<asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="false"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                       <asp:TemplateField HeaderText="Descripcion" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Tipo" SortExpression="TipoFacturacionHabitual.Descripcion">
                            <ItemTemplate>
                                <%# Eval("TipoFacturacionHabitual.Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="% incremento" SortExpression="IncrementoPorcentaje">
                        <ItemTemplate>
                            <%# Eval("IncrementoPorcentaje", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Incremento Periodo Meses" SortExpression="IncrementoPeriodoMeses">
                        <ItemTemplate>
                            <%# Eval("IncrementoPeriodoMeses")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Periodo Inicio" SortExpression="PeriodoInicio">
                        <ItemTemplate>
                            <%# Eval("PeriodoInicio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Periodo Fin" SortExpression="PeriodoFin">
                        <ItemTemplate>
                            <%# Eval("PeriodoFin")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Factura Dia Vto" SortExpression="PeriodoFin">
                        <ItemTemplate>
                            <%# Eval("FacturaDiaVencimiento")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                        </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                        </div>