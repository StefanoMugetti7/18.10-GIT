<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TesoreriaCerrarDatos.ascx.cs" Inherits="IU.Modulos.Tesoreria.Controles.TesoreriaCerrarDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="TesoreriasCerrarDatos">
            <asp:GridView ID="gvDatosCabecera" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="false" >
                <Columns>
                    <asp:BoundField  HeaderText="Fecha Abrir" DataField="FechaAbrir" />
                    <asp:TemplateField HeaderText="Usuario Abrir" >
                        <ItemTemplate>
                            <%# string.Concat(Eval("UsuarioAbrir.Apellido"), Eval("UsuarioAbrir.Nombre") )%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Cerrar" >
                        <ItemTemplate>
                            <%#  (DateTime)Eval("FechaCerrar") <= Convert.ToDateTime("1753/01/01") ? string.Empty :  Eval("FechaCerrar") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Usuario Cerrar" >
                        <ItemTemplate>
                            <%# string.Concat(Eval("UsuarioCerrar.Apellido"), Eval("UsuarioCerrar.Nombre") )%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" >
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="false" >
            <Columns>
                <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Moneda.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  HeaderText="Saldo Inicial" DataField="SaldoInicial" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="SaldoInicial" />
                <asp:BoundField  HeaderText="Ingresos" DataField="Ingreso" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Ingreso" />
                <asp:BoundField  HeaderText="Egresos" DataField="Egreso" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Egreso" />
                <asp:BoundField  HeaderText="Saldo Final" DataField="SaldoFinal" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="SaldoFinal" />
            </Columns>
            </asp:GridView>
            <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" 
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Cerrar" 
                    onclick="btnAceptar_Click" CausesValidation="false" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
    </div>