<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesComprasConfirmar.aspx.cs" Inherits="IU.Modulos.Compras.OrdenesComprasConfirmar" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="popUpBuscarProveedor" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="OrdenesComprasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoOrden" runat="server" Text="Codigo"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoOrdenCompra" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion de pago" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionPago" runat="server" />
                    </div>
                </div>
                <%-- <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
            onclick="btnAgregar_Click" />--%>
            <br />

            <asp:Label CssClass="labelEvol" ID="lblCodigoOrden" runat="server" Text="Codigo Orden Compra"></asp:Label>
            <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCodigoOrdenCompra" runat="server"></AUGE:NumericTextBox>
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblCondicionPago" runat="server" Text="Condicion de pago" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlCondicionPago" runat="server" />
            <div class="Espacio"></div>
                       
            <br />
            <asp:Label CssClass="labelEvol" ID="lblCodigo" runat="server" Text="Numero Proveedor"></asp:Label>
                <AUGE:NumericTextBox CssClass="txtCodigoBuscador" ID="txtCodigo" Enabled="false" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarProveedor"
                AlternateText="Buscar proveedor" ToolTip="Buscar" onclick="btnBuscarProveedor_Click"  />
                <asp:ImageButton ImageUrl="~/Imagenes/Baja.png" runat="server" ID="btnLimpiar"
                AlternateText="Limpiar" ToolTip="Limpiar" onclick="btnLimpiar_Click"  />
                <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />
            <div class="Espacio"></div>
            
            <asp:Label CssClass="labelEvol" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>

            <%--<div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server">
            </asp:DropDownList>--%>
            <br />

            <%--<asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                                runat="server" onclick="btnExportarExcel_Click" Visible="false" />--%>
            <br />
           <%-- <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />--%>
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            >
                <Columns>
                    <asp:TemplateField HeaderText="Código Orden" SortExpression="CodigoOrden">
                            <ItemTemplate>
                                <%# Eval("IdOrdenCompra")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Proveedor.RazonSocial")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Orden Compra" SortExpression="FechaOrden">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Detalle" SortExpression="Detalle">
                            <ItemTemplate>
                                <%# Eval("Detalle")%>
                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Importe Total" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("ImporteTotal", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField> 

                    <asp:TemplateField HeaderText="Cuotas" SortExpression="Cuotas">
                            <ItemTemplate>
                                <AUGE:CurrencyTextBox CssClass="gvTextBox" ID="txtCuotas" runat="server" Text='<%#Bind("CuotasDescuentoAfiliado") %>'></AUGE:CurrencyTextBox>
                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText ="ImporteDescontar" SortExpression ="">
                        <ItemTemplate>
                            <Evol:CurrencyTextBox CssClass="gvTextBox" ID="txtImporteDescontar" runat="server" Text='<%#Bind("ImporteDescontar", "{0:C2}") %>'></Evol:CurrencyTextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText ="Periodo Primer Vto." SortExpression ="">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="gvSelect" ID="ddlPeriodoProximosPeriodos" runat="server" /> 
                        </ItemTemplate>
                    </asp:TemplateField>

                   <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" Visible="true" Checked='<%#Eval("Check") %>' runat="server" CommandName="Incluir"/>
                        </ItemTemplate>
                     </asp:TemplateField>
                    </Columns>
            </asp:GridView>
        </ContentTemplate>
            <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>--%>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
        <center>
            <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Visible="false" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server"  Text="Volver" onclick="btnCancelar_Click" />
            <br />
        </center>
    </ContentTemplate>
</asp:UpdatePanel> 


</div>
</asp:Content>
