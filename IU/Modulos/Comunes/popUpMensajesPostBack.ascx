<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpMensajesPostBack.ascx.cs" Inherits="IU.Modulos.Comunes.popUpMensajesPostBack" %>

<div  class="modal"  data-backdrop="static"   data-keyboard="false"   aria-labelledby="staticBackdropLabel"
  aria-hidden="true" id="modalMensajesPostBack" tabindex="-1" role="dialog" >
    <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">EVOL - SIM</h5>
      </div>
      <div class="modal-body">
          <asp:Label ID="lblPopUpConfirmarMensaje" runat="server" Text=""></asp:Label>
        </div>
        <div class="modal-footer">
         <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" OnClientClick="HideModalMensajesPostBack();" CausesValidation="false" runat="server" Text="Aceptar"/>
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" Visible="false" runat="server" Text="Cancelar" data-dismiss="modal" />
      </div>
    </div>
    </div>
</div>

<script type="text/javascript" lang="javascript">

    function ShowModalMensajesPostBack()
    {
        $("[id$='modalMensajesPostBack']").modal('show')
    }

    function HideModalMensajesPostBack() {
        $("[id$='modalMensajesPostBack']").modal('hide')
    }
</script>

<%--<asp:Panel ID="pnlPopUp" runat="server" GroupingText="EVOL - SIM" Style='display:none; width:auto; min-width:300px' CssClass="modalPopup" >
    <asp:Label CssClass="MensajesPostBack" ID="lblPopUpConfirmarMensaje" runat="server" Text="" Width="100%"  ></asp:Label>
    <br />
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" CausesValidation="false" runat="server" Text="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" Visible="false" runat="server" Text="Cancelar" />
    </center>
</asp:Panel>
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender 
    ID="mpePopUp" runat="server" 
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" 
    BehaviorID="bhidMpePopUpMensajesPostBack"
    >
</asp:ModalPopupExtender>--%>