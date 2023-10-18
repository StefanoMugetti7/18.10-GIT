<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MensajesAlertasListarPopUp.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.MensajesAlertasListarPopUp" %>
<script lang="javascript" type="text/javascript">
    function ShowModalPopUpMensajesAlertas() {
        $("[id$='modalPopUpMensajesAlertas']").modal('show');
    }
    function HideModalPopUpMensajesAlertas() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPopUpMensajesAlertas']").modal('hide');
    }

</script>

<div class="modal" id="modalPopUpMensajesAlertas" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Mensajes Alertas</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                          <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Label ID="lblMensajesAlertas" CssClass="MensajesAlertas" Width="100%" Text="Verifique el detalle de pendientes del socio." runat="server"></asp:Label>
                            </div>
                        </div>
                      
                        <div class="table-responsive">
                            <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                        <ItemTemplate>
                                            <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mensaje" SortExpression="Mensaje">
                                        <ItemTemplate>
                                            <%# Eval("Mensaje")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </div></div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" OnClick="btnVolver_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>


