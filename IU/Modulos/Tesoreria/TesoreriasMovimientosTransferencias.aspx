<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpTesorerias.master" AutoEventWireup="true" CodeBehind="TesoreriasMovimientosTransferencias.aspx.cs" Inherits="IU.Modulos.Tesoreria.TesoreriasMovimientosTransferencias" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/FechaCajaContable.ascx" tagname="FechaCajaContable" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" tagname="popUpGrillaGenerica" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="TesoreraiasMovimientosTransferencias">
<br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
            <div class="table-responsive">
    <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false" 
        OnRowDataBound="gvDatos_RowDataBound"  OnRowCommand="gvDatos_RowCommand" DataKeyNames="IndiceColeccion"
        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
        onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
            <Columns>
                <asp:TemplateField HeaderText="Transaccion" SortExpression="Transaccion">
                    <ItemTemplate>
                        <%# Eval("Transaccion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Movimiento" SortExpression="TipoOperacion.TipoMovimiento.TipoMovimiento">
                    <ItemTemplate>
                        <%# Eval("TipoOperacion.TipoMovimiento.TipoMovimiento")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Moneda" SortExpression="TesoreriaMoneda.Moneda.Descripcion">
                    <ItemTemplate>
                        <%# Eval("TesoreriaMoneda.Moneda.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:BoundField  HeaderText="Caja" DataField="" SortExpression="" />
                <asp:BoundField  HeaderText="Usuario" DataField="" SortExpression="" />--%>
                <asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                    <ItemTemplate>
                        <%# Eval("TipoValor.TipoValor")%>
                    </ItemTemplate>
                </asp:TemplateField>
      <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                           <ItemTemplate>
                                    <%# string.Concat(Eval("TesoreriaMoneda.Moneda.Moneda"), Eval("Importe", "{0:N2}"))%>
                                </ItemTemplate>
                          </asp:TemplateField>          
                
                <asp:TemplateField HeaderText="Fecha Movimiento" SortExpression="" >
                    <ItemTemplate>
                        <auge:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="" DivCajaContabilizacion="col-sm-9" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                        AlternateText="Modificar" ToolTip="Confirmar" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Anular" ID="btnAnular" 
                                        AlternateText="Anular" ToolTip="Anular" />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label CssClass="labelFooterEvol" ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
