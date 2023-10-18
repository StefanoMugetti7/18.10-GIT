<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProveedorModificarDatosTelefonoPopUp.ascx.cs" Inherits="IU.Modulos.Proveedores.Controles.ProveedorModificarDatosTelefonoPopUp" %>

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarTurnos() {
        $("[id$='modalBuscarTurnos']").modal('show');
    }

    function HideModalBuscarTurnos() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarTurnos']").modal('hide');
    }
</script>


<div class="modal" id="modalBuscarTurnos" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">


        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-md" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Telefono</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">


                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblTipoTelefono" runat="server" Text="Tipo telefono"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoTelefono" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblPrefijo" runat="server" Text="Prefijo"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijo" runat="server"></AUGE:NumericTextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblNumero" runat="server" Text="Número"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" runat="server"></AUGE:NumericTextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumero" runat="server" ErrorMessage="Campo obligatorio"
                                    ControlToValidate="txtNumero" ValidationGroup="ProveedoresDatosTelefonos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblInterno" runat="server" Text="Interno"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtInterno" runat="server"></AUGE:NumericTextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                                    OnClick="btnAceptar_Click" ValidationGroup="ProveedoresDatosTelefonos" />
                                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                                    OnClick="btnCancelar_Click" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>

</div>
