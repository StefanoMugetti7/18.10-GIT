<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestamoSimulacionModificarDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrestamoSimulacionModificarDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>

<div class="PrestamoAfiliadoModificarDatos">
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
    <asp:Label CssClass="labelEvol" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
    <asp:TextBox CssClass="textboxEvol" ID="txtFechaAlta" runat="server" />    
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblEstado" runat="server" Text="Estado" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlEstado" runat="server" />
    <br />

    <asp:Label CssClass="labelEvol" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlTipoOperacion" runat="server" />

    <asp:UpdatePanel ID="upPlan" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
        <asp:Label CssClass="labelEvol" ID="lblPlan" runat="server" Text="Plan" />
        <asp:DropDownList CssClass="selectEvol" ID="ddlPlan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlan_SelectedIndexChanged"  />
        <div class="Espacio"></div>
        <asp:Label CssClass="labelEvol" ID="lblTasaInteres" runat="server" Text="Tasa Interes" />
        <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtTasaInteres" Enabled="false" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:Label CssClass="labelEvol" ID="lblMonto" runat="server" Text="Monto Solicitado" />
    <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtMonto" runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMonto" runat="server" InitialValue="" ControlToValidate="txtMonto" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    <div class="EspacioValidador"></div>
    <asp:Label CssClass="labelEvol" ID="lblCantidadCuotas" runat="server" Text="Cantidad de Cuotas" />
    <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtCantidadCuotas" runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidadCuotas" runat="server" InitialValue="" ControlToValidate="txtCantidadCuotas" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
    <br />
    
    <asp:Label CssClass="labelEvol" ID="lblMoneda" runat="server" Text="Moneda" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlMoneda" runat="server" />
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblTipoValor" runat="server" Text="Tipo Valor" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlTipoValor" runat="server" />
    <br />
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <asp:TabPanel runat="server" ID="TabPanel1" HeaderText="Detalle de Cuotas" >
            <ContentTemplate>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Cuota" DataField="CuotaNumero" SortExpression="CuotaNumero" />
                        <asp:TemplateField HeaderText="Vencimiento" SortExpression="CuotaFechaVencimiento">
                            <ItemTemplate>
                                <%# Eval("CuotaFechaVencimiento", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Importe" DataFormatString="{0:C2}" DataField="ImporteCuota" />
                        <asp:BoundField  HeaderText="Interes" DataFormatString="{0:C2}" DataField="ImporteInteres" />
                        <asp:BoundField  HeaderText="Amortizacion" DataFormatString="{0:C2}" DataField="ImporteAmortizacion" />
                        <asp:BoundField  HeaderText="Saldo" DataFormatString="{0:C2}" DataField="ImporteSaldo" />
             
                        <%--<asp:TemplateField HeaderText="Filial Pago" >
                            <ItemTemplate>
                                <%# Eval("FilialPago.Filial")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    
        <center>
            <br />
            <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
        </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>
