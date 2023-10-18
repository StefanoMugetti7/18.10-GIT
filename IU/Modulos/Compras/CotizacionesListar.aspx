<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CotizacionesListar.aspx.cs" Inherits="IU.Modulos.Compras.CotizacionesListar" %>

<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="popUpBuscarProveedor" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="CotizacionesListar">
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
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                                OnClick="btnBuscar_Click" />
                            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                                OnClick="btnAgregar_Click" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigoCotizacion" runat="server" Text="Codigo Cotización"></asp:Label>
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoCotizacion" runat="server"></AUGE:NumericTextBox>
                        </div>
                        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion de pago" />
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionPago" runat="server" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Numero Proveedor"></asp:Label>
                        <div class="col-lg-2 col-md-2 col-sm-7">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server" />
                        </div>
                        <div class="col-lg-1 col-md-1 col-sm-2">
                            <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarProveedor"
                                AlternateText="Buscar proveedor" ToolTip="Buscar" OnClick="btnBuscarProveedor_Click" />
                            <asp:ImageButton ImageUrl="~/Imagenes/Baja.png" runat="server" ID="btnLimpiar"
                                AlternateText="Limpiar" ToolTip="Limpiar" OnClick="btnLimpiar_Click" />
                            <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />
                        </div>
                        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Código Cotización" SortExpression="CodigoCotizacion">
                                    <ItemTemplate>
                                        <%# Eval("IdCotizacion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                                    <ItemTemplate>
                                        <%# Eval("Proveedor.RazonSocial")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud">
                                    <ItemTemplate>
                                        <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="EstadoCotizacion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar"
                                            AlternateText="Autorizar Cotización" ToolTip="Autorizar Cotización" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnular"
                                            AlternateText="Anular Cotización" ToolTip="Anular Cotización" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnModificar"
                                            AlternateText="Modificar Cotización" ToolTip="Modificar Cotización" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>