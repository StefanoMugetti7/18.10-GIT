<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadosBuscarPopUp" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscar.ascx" TagPrefix="AUGE" TagName="Afiliados" %>

<script type="text/javascript" lang="javascript">

    function ShowModalBuscarSocio()
    {
        $("[id$='modalBuscarSocio']").modal('show');
    }

    function HideModalBuscarSocio() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarSocio']").modal('hide');
    }
</script>

<div class="modal" id="modalBuscarSocio" tabindex="-1" role="dialog" >
    <form class="needs-validation" novalidate>
    <div class="modal-dialog modal-dialog-scrollable modal-xl modal-xl modal-minHeight85" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Buscar Socio</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">
            <AUGE:Afiliados ID="ctrAfiliados" runat="server" />
        </div>
            <div class="modal-footer">
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
        </div>
    </div>
        </div>
        </form>
</div>