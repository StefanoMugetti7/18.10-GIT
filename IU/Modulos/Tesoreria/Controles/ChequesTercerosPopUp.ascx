<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChequesTercerosPopUp.ascx.cs" Inherits="IU.Modulos.Tesoreria.Controles.ChequesTercerosPopUp" %>


<script lang="javascript" type="text/javascript">
    function ShowModalPopUpChequesTerceros() {
        $("[id$='modalPopUpChequesTerceros']").modal('show');
    }

    function HideModalPopUpChequesTerceros() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPopUpChequesTerceros']").modal('hide');
    }

</script>


<div class="modal" id="modalPopUpChequesTerceros" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Buscar Cheques Terceros</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">



                        <%--  <asp:Panel ID="pnlBuscar" runat="server">--%>


                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoCheque" runat="server" Text="Código Cheque"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCodigoCheque" runat="server"></AUGE:NumericTextBox>
    <div class="Espacio"></div>--%>

                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCheque" runat="server" Text="Numero Cheque"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                                    OnClick="btnBuscar_Click" />
                            </div>

                        </div>

                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaDesde" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaDesde" PopupButtonID="imgFechaDesde" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <div class="Espacio"></div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaHasta" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="cdFechaHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaHasta" PopupButtonID="imgFechaHasta" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <br />--%>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferidoDesde" runat="server" Text="Fecha Dif. Desde"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferidoDesde" runat="server"></asp:TextBox>
                            </div>

                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferidoHasta" runat="server" Text="Fecha dif. Hasta"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferidoHasta" runat="server"></asp:TextBox>
                            </div>

                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTitularCheque" runat="server" Text="Titular"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtTitularCheque" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlFiliales" runat="server">
    </asp:DropDownList>
    <div class="Espacio"></div>--%>

                        <%--    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlEstados" runat="server">
    </asp:DropDownList>--%>


                        <%--  </asp:Panel>--%>
                        <div class="table-responsive">
                        <asp:GridView ID="gvCheques" OnRowCommand="gvCheques_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvCheques_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvCheques_PageIndexChanging" OnSorting="gvCheques_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Codigo Cheque" SortExpression="IdCheque">
                                    <ItemTemplate>
                                        <%# Eval("IdCheque")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Numero Cheque" SortExpression="NumeroCheque">
                                    <ItemTemplate>
                                        <%# Eval("NumeroCheque")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                    <ItemTemplate>
                                            <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fecha Diferido" SortExpression="FechaDiferido">
                                    <ItemTemplate>
                                           <%# Eval("FechaDiferido", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Banco" SortExpression="Banco.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Banco.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Importe" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right"  SortExpression="Importe">
                                    <ItemTemplate>
                                        <%# Eval("Importe", "{0:C2}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                                            AlternateText="Seleccionar" ToolTip="Seleccionar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                            </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false"  class="close" data-dismiss="modal" runat="server" Text="Volver" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


<%--
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnVolver">
</asp:ModalPopupExtender>--%>
