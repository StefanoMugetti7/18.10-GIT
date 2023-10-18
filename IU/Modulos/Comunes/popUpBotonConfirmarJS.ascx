<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpBotonConfirmarJS.ascx.cs" Inherits="IU.Modulos.Comunes.popUpBotonConfirmarJS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Panel ID="pnlPopUpAceptarConfirmarJS" GroupingText="EVOL - SIM" runat="server" Style='display:none; width:auto' CssClass="modalPopup" >
    <asp:Label CssClass="labelEvol" ID="lblPopUpConfirmarMensajejs" runat="server" Text=""></asp:Label>
    <br />
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"  />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" />
    </center>
</asp:Panel>
<asp:modalpopupextender id="md2" 
    runat="server" backgroundcssclass="modalBackground"
    behaviorid="mdlPopupJS" 
    cancelcontrolid="btnCancelar" okcontrolid="btnAceptar" 
    oncancelscript="fnCancelar();" onokscript="fnAceptar();" 
    popupcontrolid="pnlPopUpAceptarConfirmarJS" targetcontrolid="pnlPopUpAceptarConfirmarJS"> 
</asp:modalpopupextender>

<script type="text/javascript" language="javascript">

    // keep track of the popup div
    var _popup;
    var myCallback;
    function fnConfirmarJS(mensaje, callback) {
        this._popup = $find('mdlPopupJS');
        myCallback = callback;
        $('span[id$="lblPopUpConfirmarMensajejs"]').text(mensaje);
        this._popup.show();
    }

    function fnAceptar() {
        if (this._popup == null)
            this._popup = $find('mdlPopupJS');
        this._popup.hide();
        this._popup = null;
        myCallback(true);
    }

    function fnCancelar() {
        if (this._popup == null)
            this._popup = $find('mdlPopupJS');
        this._popup.hide();
        this._popup = null;
        myCallback(false);
    }
</script>