<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressBar.ascx.cs" Inherits="IU.ProgressBar" %>

<script type="text/javascript">
    function showPopupProgressBar() {
            //$("body").addClass("loading");
        $("[id$='modalProgressBar']").modal('show');
        $("body").css("cursor", "progress");
        }
        function hidePopupProgressBar()
        {
            //$("body").removeClass("loading");
            $("[id$='modalProgressBar']").modal('hide');
            $("body").css("cursor", "default");
        }
</script>
<div class="modal" id="modalProgressBar" tabindex="-1" role="dialog" data-keyboard="false" data-backdrop="static">
    <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">EVOL - SIM</h5>
      </div>
      <div class="modal-body">
        <div class="d-flex align-items-center">
        <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Procesando...</span>
        </div>
        <strong>&nbsp;&nbsp;Procesando...</strong>
        </div>
    <asp:Label CssClass="labelEvol" ID="lblMensajesOpcionales" runat="server" Text=" "></asp:Label>
        </div>
    </div>
    </div>
</div>