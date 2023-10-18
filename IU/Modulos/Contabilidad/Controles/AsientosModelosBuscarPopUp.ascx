<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsientosModelosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.AsientosModelosBuscarPopUp" %>
<script type="text/javascript">

    function ShowModalBuscarAsientoModelo() {
        $("[id$='modalBuscarAsientoModelo']").modal('show');
    }

    function HideModalBuscarAsientoModelo() {
        $("[id$='modalBuscarAsientoModelo']").modal('hide');
    }

</script>

<%--<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Asiento Modelo" Style="display: none" CssClass="modalPopUpBuscarAfiliados">--%>
<div class="modal" id="modalBuscarAsientoModelo" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content modal-minHeight85">
            <div class="modal-header">
                <h5 class="modal-title">Buscar Asiento Modelo</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="ejercicio contable:" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEjercicioContable" ControlToValidate="ddlEjercicioContable" ValidationGroup="CuentasContablesBuscar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoAsiento" Text="Tipo Asiento" runat="server" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoAsiento" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">

                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="CuentasContablesBuscar" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                <Columns>
                                    <asp:BoundField HeaderText="Detalle" DataField="Detalle" SortExpression="Detalle" />
                                    <asp:TemplateField HeaderText="Tipo Asiento" SortExpression="TipoAsiento.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("TipoAsiento.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                                                AlternateText="Seleccionar" OnClientClick="HideModalBuscarAsientoModelo();" ToolTip="Seleccionar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" OnClientClick="HideModalBuscarAsientoModelo();" Text="Volver" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
