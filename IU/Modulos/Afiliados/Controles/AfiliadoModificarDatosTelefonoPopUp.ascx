<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadoModificarDatosTelefonoPopUp.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadoModificarDatosTelefonoPopUp" %>

<script type="text/javascript" lang="javascript">

    function ShowModalTelefonos()
    {
        $("[id$='modalTelefonos']").modal('show')
    }

    function HideModaTelefonos() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalTelefonos']").modal('hide');
    }

    function ValidarDatos() {
        if (Page_ClientValidate("AfiliadosDatosTelefonos")) {
            return true;
        }
        return false;
    }
</script>

<div class="modal" id="modalTelefonos" tabindex="-1" role="dialog" >
    <form class="needs-validation" novalidate>
    <div class="modal-dialog modal-dialog-scrollable " role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Telefono</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblTipoTelefono" runat="server" Text="Tipo telefono"></asp:Label>
                 <div class="col-sm-7">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoTelefono" OnSelectedIndexChanged="ddlTipoTelefono_SelectedIndexChanged"  AutoPostBack="true" runat="server" />
                </div>
                <div class="col-sm-1"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblEmpresaTelefonica" runat="server" Text="Empresa Telefonica"></asp:Label>
                 <div class="col-sm-7">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEmpresaTelefonica" runat="server" />
                  </div>
                <div class="col-sm-1"></div>
            </div>
            <%--<div class="form-group row">
                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblPrefijo" runat="server" Text="Prefijo"></asp:Label>
                 <div class="col-sm-7">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijo" runat="server"></AUGE:NumericTextBox>
                  </div>
                <div class="col-sm-1"></div>
            </div>--%>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblNumero" runat="server" Text="Número"></asp:Label>
                 <div class="col-sm-7">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" runat="server"></AUGE:NumericTextBox>
                </div>
                 <div class="col-sm-1">
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumero" runat="server" ErrorMessage="*" 
                    ControlToValidate="txtNumero" ValidationGroup="AfiliadosDatosTelefonos"/>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-4 col-form-label" ID="lblInterno" runat="server" Text="Interno"></asp:Label>
                 <div class="col-sm-7">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtInterno" runat="server"></AUGE:NumericTextBox>
                </div>
                 <div class="col-sm-1"></div>
            </div>
            <div class="form-group row">
                 <asp:Label CssClass="col-sm-4 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                 <div class="col-sm-7">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                     </div>
                 <div class="col-sm-1"></div>
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="AfiliadosDatosTelefonos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
        </div>
    </div>
        </div>
        </form>
</div>