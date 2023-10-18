<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpGrillaGenerica.ascx.cs" Inherits="IU.Modulos.Comunes.popUpGrillaGenerica" %>
<script lang="javascript" type="text/javascript">
   function ShowModalPopUpGrillaGenerica() {
        $("[id$='ModalPopUpGrillaGenerica']").modal('show');
    }

    function HideModalPopUpGrillaGenerica() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='ModalPopUpGrillaGenerica']").modal('hide');
    }

</script>
<div class="modal" id="ModalPopUpGrillaGenerica" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 id="h5Titulo" runat="server" class="modal-title">Sistema de gestión para mutuales</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label text-danger" ID="lblDetalle" runat="server" Text="Detalle" />
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblDetalleMsg"  runat="server" Width="100%" Text="" />
                        </div>

                        <div class="data-table">
                            <asp:GridView ID="gvDatos" runat="server" SkinID="GrillaResponsive" ShowFooter="true" AutoGenerateColumns="true"
                                OnRowDataBound="gvDatos_RowDataBound">
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <button type="button" class="botonesEvol" data-dismiss="modal">Aceptar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
