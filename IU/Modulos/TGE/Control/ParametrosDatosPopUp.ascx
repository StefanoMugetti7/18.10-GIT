<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParametrosDatosPopUp.ascx.cs" Inherits="IU.Modulos.TGE.Control.ParametrosDatosPopUp" %>
<script type="text/javascript" lang="javascript">
 function ShowModalBuscarTurnos(){
        $("[id$='modalParametrosDatos']").modal('show');
    }

    function HideModalBuscarTurnos() {
         $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalParametrosDatos']").modal('hide');
    }



</script>
<%--<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Sistema de Gestion para Mutuales" Style='display:none' CssClass="modalPopUpDomicilios" >--%>
 <div class="modal" id="modalParametrosDatos" tabindex="-1" role="dialog" >
      <ContentTemplate>
          <div class="modal-dialog modal-dialog-scrollable modal-xl modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Agregar un parámetro</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">

         <div class="form-group row">
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta"></asp:Label>
          <div class="col-sm-4">
        <asp:TextBox CssClass="form-control" ID="txtFechaAlta" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
             <div class="col-sm-4">
               
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
       </div>
               </div>
                  <div class="form-group row">
        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblParametroValor" runat="server" Text="Valor"></asp:Label>
             <div class="col-sm-4">
        <asp:TextBox CssClass="form-control" ID="txtParametroValor" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvParametroValor" CssClass="Validador" ValidationGroup="ParametrosDatosPopUp"  ControlToValidate="txtParametroValor" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
       </div>
          <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
         <div class="col-sm-4">
          <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" Enabled="false" runat="server">
        </asp:DropDownList>
                 </div>
          </div>
 <div class="row justify-content-md-center">
            <div class="col-md-auto">
                 <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" ValidationGroup="ParametrosDatosPopUp" runat="server" Text="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" Text="Volver" runat="server" />
            </div>
  </div>

           
      </div>
        </div></div>
          </ContentTemplate>
    </div>
<%--</asp:Panel>--%>
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<%--<asp:ModalPopupExtender 
    ID="mpePopUp" runat="server" 
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" 
    BehaviorID="bhidMpePopUpCamposValores"
    CancelControlID="btnCancelar"--%>
<%--    >
</asp:ModalPopupExtender>--%>