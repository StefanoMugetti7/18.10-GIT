<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Mensajes.ascx.cs" Inherits="IU.Mensajes" %>


<%--<asp:Panel ID="pnlPopUp" GroupingText="EVOL - SIM" runat="server" Style='display:none; width:auto; min-width:300px' CssClass="modalPopup" >
    <asp:Label CssClass="labelEvol" ID="lblMensaje" runat="server" Text=""></asp:Label>
    <br />
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptarPnlMensajes" runat="server" Text="Aceptar" />
    </center>
</asp:Panel>--%>

<%--<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp" 
    BackgroundCssClass="modalBackground"
    OkControlID="btnAceptarPnlMensajes" 
    behaviorid="mdlPopupMensajes"
    >
</asp:ModalPopupExtender>--%>

<div class="modal" id="modalMensajes" tabindex="-1" role="dialog" >
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">EVOL - SIM</h5>
      </div>
      <div class="modal-body">
          <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
        </div>
        <div class="modal-footer" style="display:block;text-align:center;">
        <button type="button" class="botonesEvol" data-dismiss="modal" onclick="SetFocus();">Aceptar</button>
      </div>
    </div>
    </div>
</div>

<script type="text/javascript" lang="javascript">

    function SetFocus() {

       if (postBackControl != undefined && $get(postBackControl) != null) {        
          $get(postBackControl).focus();
       }
    }

    function ShowModal()
    {
        $("[id$='modalMensajes']").modal('show');
    }

    function MostrarMensaje(mensaje, color) {
        var msg = $("[id$='lblMensaje']");
        msg.css("color", color);
        msg.text( mensaje);
        ShowModal();
    }
        
        function MostrarMensaje(mensaje){
            var msg = $("[id$='lblMensaje']");
            msg.text(mensaje);
            ShowModal();
        }
        
</script>