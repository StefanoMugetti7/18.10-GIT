<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SolicitudesPagosAfiliadosListar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosAfiliadosListar" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" tagname="popUpBuscarProveedor" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<div class="FacturasListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
           <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
            </div>
                <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
                onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
            onclick="btnAgregar_Click" />
            </div>
                </div>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSolicitud" runat="server" Text="Número Solicitud"></asp:Label>
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSolicitud" runat="server"></AUGE:NumericTextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoSolicitud" runat="server" Text="Tipo Solicitud" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoSolicitud" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Número Factura"></asp:Label>
            <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtPrefijoNumeroFactura" runat="server" maxlength="4"/>--%>
            <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblGuionMedio" runat="server" Text="-"  Width="10"></asp:Label>--%>
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" maxlength="10" />
             </div>
                </div>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Numero Proveedor"></asp:Label>
                <div class="col-sm-2">
                <AUGE:NumericTextBox ID="txtCodigo" CssClass="form-control" AutoPostBack="true" onTextChanged="txtCodigo_TextChanged" Enabled="true" runat="server" />
                </div>
                <div class="col-sm-1">
                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarProveedor"
                AlternateText="Buscar proveedor" ToolTip="Buscar" onclick="btnBuscarProveedor_Click"  />
                <asp:ImageButton ImageUrl="~/Imagenes/Baja.png" runat="server" ID="btnLimpiar"
                AlternateText="Limpiar" ToolTip="Limpiar" onclick="btnLimpiar_Click" Visible="false"  />
                <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>
                </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados"  runat="server">
            </asp:DropDownList>
            </div>
                </div>
            <div class="form-group row">
                <div class="col-sm-3">
            <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                                runat="server" onclick="btnExportarExcel_Click" Visible="false" />
                    </div>
                <div class="col-sm-8"></div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Número Solicitud" SortExpression="NumeroSolicitud">
                            <ItemTemplate>
                                <%# Eval("IdSolicitudPago")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Entidad.Nombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaAlta">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nro Factura" SortExpression="NumeroFacturaCompleto">
                            <ItemTemplate>
                                <%# Eval("NumeroFacturaCompleto")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Total" SortExpression="ImporteTotal">
                            <ItemTemplate>
                                <%# Eval("ImporteTotal", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoSolicitud">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnAutorizar"
                                    AlternateText="Autorizar Solicitud" ToolTip="Autorizar Solicitud" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnModificar"
                                    AlternateText="Anular Solicitud" ToolTip="Anular Solicitud" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
        </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
    </asp:UpdatePanel>
</div>
</asp:Content>
