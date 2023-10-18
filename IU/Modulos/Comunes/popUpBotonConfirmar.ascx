<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpBotonConfirmar.ascx.cs" Inherits="IU.Controles.popUpBotonConfirmar" %>

<div class="modal" id="modalBotonConfirmarPopUp" tabindex="-1" role="dialog" >
    <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
            <h5 class="modal-title">EVOL - SIM</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    <div class="modal-body">
        <div class="form-group row">
  <asp:Label CssClass="col" ID="lblPopUpConfirmarMensaje" runat="server" Text=""></asp:Label>

</div>
    </div>
        <div class="modal-footer">
        <button type="button" class="botonesEvol" onclick="okClick();" value="Aceptar">Aceptar</button>
        <button type="button" class="botonesEvol" data-dismiss="modal" value="Volver">Volver</button>
<%--        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" data-dismiss="modal" />--%>
        </div>
    </div>
    </div>
</div>



<script type="text/javascript" language="javascript">
  
        //  keeps track of the delete button for the row
        //  that is going to be removed
        var _source;
        // keep track of the popup div
        var _popup;

        function showConfirm(source, mensaje) {
            this._source = source;
            //this._popup = $find('mdlPopup');
            //  find the confirm ModalPopup and show it
            var msg = document.getElementById('<%=lblPopUpConfirmarMensaje.ClientID%>');
            msg.innerHTML = mensaje;
            $("[id$='modalBotonConfirmarPopUp']").modal('show');
            //this._popup.show();
        }
        
        function okClick(){
             $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBotonConfirmarPopUp']").modal('hide');

            //  use the cached button as the postback source
            if (this._source != null) {
                __doPostBack(this._source.name, '');
            }            
        }
        
    </script>