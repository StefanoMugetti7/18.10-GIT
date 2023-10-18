<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CajasAfiliadosInicio.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosInicio" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/MensajesAlertasListarPopUp.ascx" TagPrefix="AUGE" TagName="MensajesAlertas" %>



<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:MensajesAlertas ID="ctrMensajesAlertas" runat="server" />
    <div class="card">
    <div class="card-header">
        Cargos Pendientes
    </div>
         <div class="card-body">
              <asp:UpdatePanel ID="upGrilla" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvCargosMensuales" OnRowCommand="gvCargosMensuales_RowCommand" AllowPaging="true"
                        OnRowDataBound="gvCargosMensuales_RowDataBound" DataKeyNames="IdCuentaCorriente"
                        runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                        onpageindexchanging="gvCargosMensuales_PageIndexChanging" >
                            <Columns>
                                <asp:BoundField  HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                <asp:BoundField  HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="Periodo" />
                                <asp:BoundField  HeaderText="Tipo Cargo / Concepto" DataField="TipoCargoConcepto" SortExpression="TipoCargoConcepto" />
                               <%-- <asp:BoundField  HeaderText="Tipo Cargo / Concepto" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:C2}" DataField="TipoCargoConcepto" SortExpression="TipoCargoConcepto" />--%>

                                <asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                                        <ItemTemplate>
                                            <%# Eval("TipoValorTipoValor")%>
                                        </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Forma Cobro" SortExpression="FormaCobro.FormaCobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobroFormaCobro")%>
                                        </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("Importe", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cobrado" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("ImporteCobrado", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteCobrado" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe Enviar" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                                    FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <%# Eval("ImporteEnviar", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteEnviar" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField  HeaderText="Importe" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />
                                <asp:BoundField  HeaderText="Cobrado" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteCobrado" SortExpression="ImporteCobrado" />
                                <asp:BoundField  HeaderText="Importe Enviar" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteEnviar" SortExpression="ImporteEnviar" />--%>
                                <asp:TemplateField HeaderText="Estado" ItemStyle-Wrap="false" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("EstadoDescripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Motivo Rechazo" SortExpression="MotivoRechazo">
                                    <ItemTemplate>
                                        <%# Eval("MotivoRechazo")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                           
                                </Columns>
                            </asp:GridView>


                    </ContentTemplate>
                    </asp:UpdatePanel>

             </div>
    </div>
</asp:Content>
