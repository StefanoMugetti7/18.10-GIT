<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientesBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.ClientesBuscarPopUp" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesBuscar.ascx" TagPrefix="AUGE" TagName="Afiliados" %>

<script type="text/javascript" lang="javascript">

    function ShowModalBuscarCliente() {
        $("[id$='modalBuscarCliente']").modal('show');
    }

    function HideModalBuscarCliente() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarCliente']").modal('hide');
    }
</script>

<div class="modal" id="modalBuscarCliente" tabindex="-1" role="dialog">
    <form class="needs-validation" novalidate>
        <div class="modal-dialog modal-dialog-scrollable modal-xl modal-minHeight85" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Buscar Cliente</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <AUGE:Afiliados ID="ctrAfiliados" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:Button CssClass="botonesEvol" ID="btnVolver" runat="server" Text="Volver" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </form>
</div>
