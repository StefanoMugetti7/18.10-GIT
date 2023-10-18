<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudPagoBuscarPopUp.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.SolicitudPagoBuscarPopUp" %>

<%--<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Comprobantes" Style="display: none" CssClass="modalPopUpBuscarAfiliados">--%>

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarSolicitudPago() {
       $('.modal-backdrop').remove();
        $("[id$='modalBuscarSolicitudPago']").modal('show');
    }

    function HideModalBuscarSolicitudPago() {
 $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarSolicitudPago']").modal('hide');

    }
</script>

<div class="modal" id="modalBuscarSolicitudPago" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content modal-minHeight85">
            <div class="modal-header">
                <h5 class="modal-title">Buscar Comprobantes</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
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
                                    OnClick="btnBuscar_Click" />
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
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" MaxLength="10" />
                            </div>
                        </div>
                        <%--<div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Numero Proveedor"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3"></div>
                </div>--%>

                        <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                        runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
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
                                            <asp:TemplateField HeaderText="Tipo y Nro Factura" SortExpression="NumeroFacturaCompleto">
                                                <ItemTemplate>
                                                    <%# Eval("TipoNumeroFacturaCompleto")%>
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
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                                                        AlternateText="Seleccionar" OnClientClick="HideModalBuscarSolicitudPago();" ToolTip="Seleccionar" />
                                                    <%--                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row justify-content-md-center">
                    <div class="col-md-auto">
                        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" OnClientClick="HideModalBuscarSolicitudPago();" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%--</asp:Panel>--%>

<%--<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnVolver">
</asp:ModalPopupExtender>--%>
