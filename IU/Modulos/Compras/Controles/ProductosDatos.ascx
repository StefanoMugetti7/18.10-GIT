<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductosDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.ProductosDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


<%--<script lang="javascript" type="text/javascript">
$(document).keypress(function(e)
{
    if(e.keyCode === 13)
    {
        e.preventDefault();
        return false;
    }
    });
  
</script>--%>

<div class="FamiliasDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" ControlToValidate="txtDescripcion" ValidationGroup="FamiliasDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFamilia" runat="server" Text="Familia"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFamilias" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFamilias" ControlToValidate="ddlFamilias" ValidationGroup="FamiliasDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoProducto" runat="server" Text="Tipo Producto"></asp:Label>
        <div class="col-sm-3">
            <asp:UpdatePanel ID="upTipoProducto" UpdateMode="Conditional" RenderMode="Inline" runat="server">
                <ContentTemplate>

                    <asp:DropDownList CssClass="form-control select2" ID="ddlTiposProductos" runat="server" OnSelectedIndexChanged="ddlTiposProductos_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUnidadMedida" runat="server" Text="Unidad de Medida"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlUnidadMedida" runat="server"></asp:DropDownList>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStockMinimo" runat="server" Text="Stock Minimo"></asp:Label>
        <div class="col-sm-3">
            <auge:NumericTextBox CssClass="form-control" ID="txtStockMinimo" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStockRecomendado" runat="server" Text="Stock Recomendado"></asp:Label>
        <div class="col-sm-3">
            <auge:NumericTextBox CssClass="form-control" ID="txtStockRecomendado" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStockMaximo" runat="server" Text="Stock Maximo"></asp:Label>
        <div class="col-sm-3">
            <auge:NumericTextBox CssClass="form-control" ID="txtStockMaximo" runat="server" />
        </div>
    </div>
    <div class="form-group row">
          <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIva" runat="server" Text="Iva"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlIva" runat="server"></asp:DropDownList>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCompra" runat="server" Text="Compra" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkCompra" CssClass="form-control" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVenta" runat="server" Text="Venta" />
        <div class="col-sm-3">
            <asp:CheckBox ID="chkVenta" CssClass="form-control" runat="server" />
        </div>
        <div class="col-sm-3"></div>
        <%--   <asp:CheckBox ID="chkCompra" runat="server" Text="Compra" />
            <asp:CheckBox ID="chkVenta" runat="server" Text="Venta" />--%>
    </div>
    <div class="form-group row">

        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoBarras" runat="server" Text="Código de Barras"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoBarras" MaxLength="13"  OnTextChanged="CrearImagenCodigoBarra" AutoPostBack="true" runat="server" />
        </div>
        <div class="col-sm-1"></div>
        <div class="col-sm-3">
            <asp:Image ID="imgCodigoBarras" runat="server" />
        </div>
    </div>
</div>
<div class="form-group row">
    <div class="col-sm-12">
        <auge:CamposValores ID="ctrCamposValores" runat="server" />
    </div>
</div>
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
    SkinID="MyTab">

    <asp:TabPanel runat="server" ID="tpUltimasComprasProveedores" TabIndex="2"
        HeaderText="Ultimas Compras Proveedores">
        <ContentTemplate>
            <asp:UpdatePanel ID="upUltimasComprasProveedores" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:GridView ID="gvProveedores" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nro." SortExpression="IdProveedor">
                                            <ItemTemplate>
                                                <%# Eval("IdProveedor")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Razon Social" SortExpression="RazonSocial">
                                            <ItemTemplate>
                                                <%# Eval("RazonSocial")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Ultima Compra" SortExpression="FechaUltimaCompra">
                                            <ItemTemplate>
                                                <%# Eval("FechaUltimaCompra", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Precio Ultima Compra" SortExpression="PrecioUltimaCompra" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("PrecioUltimaCompra", "{0:C2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>

    <asp:TabPanel runat="server" ID="tpHistorial"
        HeaderText="Auditoria">
        <ContentTemplate>
            <auge:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <center>
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="FamiliasDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
                </center>
    </ContentTemplate>
</asp:UpdatePanel>
