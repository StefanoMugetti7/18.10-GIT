<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProveedorModificarDatosDomicilioPopUp.ascx.cs" Inherits="IU.Modulos.Proveedores.Controles.ProveedorModificarDatosDomicilioPopUp" %>
<script type="text/javascript" lang="javascript">
    function ShowModalBuscarTurnos(){
        $("[id$='modalBuscarTurnos']").modal('show');
    }

    function HideModalBuscarTurnos() {
         $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarTurnos']").modal('hide');
    }
</script>

    <div class="modal" id="modalBuscarTurnos" tabindex="-1" role="dialog" >
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate><div class="modal-dialog modal-dialog-scrollable modal-md" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Domicilio</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">

                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblTipoDomicilio" runat="server" Text="Tipo domicilio"></asp:Label>
                        <div class="col-lg-9 col-md-8 col-sm-8">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDomicilio" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblPredeterminado" runat="server" Text="Predeterminado"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <asp:CheckBox ID="chkPredeterminado" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblCalle" runat="server" Text="Calle"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <asp:TextBox CssClass="form-control " ID="txtCalle" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCalle" ControlToValidate="txtCalle" ValidationGroup="ProveedoresDatosDomicilios" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <AUGE:NumericTextBox CssClass="form-control " ID="txtNumero" runat="server"></AUGE:NumericTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumero" ControlToValidate="txtNumero" ValidationGroup="ProveedoresDatosDomicilios" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblPiso" runat="server" Text="Piso"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <AUGE:NumericTextBox CssClass="form-control " ID="txtPiso" runat="server"></AUGE:NumericTextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblDepartamento" runat="server" Text="Departamento"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <asp:TextBox CssClass="form-control " ID="txtDepartamento" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblProvincia" runat="server" Text="Provincia"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlProvincia" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblLocalidad" runat="server" Text="Localidad"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlLocalidad" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblCodigoPostal" runat="server" Text="Codigo Postal"></asp:Label>
                    <div class="col-lg-9 col-md-8 col-sm-8">
                            <AUGE:NumericTextBox CssClass="form-control " ID="txtCodigoPostal"
                                ValidationGroup="ProveedoresDatosDomicilios" runat="server"></AUGE:NumericTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoPostal" ControlToValidate="txtCodigoPostal" ValidationGroup="ProveedoresDatosDomicilios" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
         <div class="modal-footer">
               <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="ProveedoresDatosDomicilios" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
                    onclick="btnCancelar_Click" />
            </div></div></div>
            </div></div>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>


<%--<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />--%>
<%--<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground">
</asp:ModalPopupExtender>--%>
