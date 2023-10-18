<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadoModificarDatosDomicilioPopUp.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadoModificarDatosDomicilioPopUp" %>

<script type="text/javascript" lang="javascript">

    function ShowModalDomicilios()
    {
        $("[id$='modalDomicilios']").modal('show');
    }

    function HideModalDomicilios() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalDomicilios']").modal('hide');
    }

    function ValidarDatos() {
        if (Page_ClientValidate("AfiliadosDatosDomicilios")) {
            return true;
        }
        return false;
    }
</script>

<div class="modal" id="modalDomicilios" tabindex="-1" role="dialog" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div class="modal-dialog modal-dialog-scrollable modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Domicilio</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    
            <div class="modal-body">
              <div class="form-group row">
                <asp:Label ID="lblTipoDomicilio" CssClass="col-sm-4 col-form-label" runat="server" Text="Tipo domicilio"></asp:Label>
                  <div class="col-sm-7">
                      <asp:DropDownList ID="ddlTipoDomicilio" CssClass="form-control select2" runat="server">
                    </asp:DropDownList>
                    </div>
                  <div class="col-sm-1"></div>
                </div>

                <div class="form-group row">
                <asp:Label ID="lblPredeterminado" CssClass="col-sm-4 col-form-label"  runat="server" Text="Predeterminado"></asp:Label>
                <div class="col-sm-7">
                <asp:CheckBox ID="chkPredeterminado" CssClass="form-control" runat="server" />
                </div>
                    <div class="col-sm-1"></div>
                </div>

                 <div class="form-group row">
                <asp:Label ID="lblCalle" CssClass="col-sm-4 col-form-label"  runat="server" Text="Calle"></asp:Label>
                   <div class="col-sm-7">
                <asp:TextBox ID="txtCalle" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
                     <div class="col-sm-1"></div>
                </div>

                <div class="form-group row">
                <asp:Label ID="lblNumero" CssClass="col-sm-4 col-form-label"  runat="server" Text="Numero"></asp:Label>
                    <div class="col-sm-7">
                <AUGE:NumericTextBox ID="txtNumero" CssClass="form-control" runat="server"></AUGE:NumericTextBox>
                </div>
                    <div class="col-sm-1">
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumero" ControlToValidate="txtNumero" ValidationGroup="AfiliadosDatosDomicilios" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="form-group row">
                <asp:Label ID="lblPiso" CssClass="col-sm-4 col-form-label"  runat="server" Text="Piso"></asp:Label>
                  <div class="col-sm-7">
                <AUGE:NumericTextBox ID="txtPiso" CssClass="form-control" runat="server"></AUGE:NumericTextBox>
                </div>
                    <div class="col-sm-1"></div>
                 </div>

               <div class="form-group row">
                <asp:Label ID="lblDepartamento" CssClass="col-sm-4 col-form-label"  runat="server" Text="Departamento"></asp:Label>
                <div class="col-sm-7">
                <asp:TextBox ID="txtDepartamento" CssClass="form-control" runat="server"></asp:TextBox>
                 </div>
                     <div class="col-sm-1"></div>
                  </div>

                <div class="form-group row">
                <asp:Label ID="lblProvincia" CssClass="col-sm-4 col-form-label"  runat="server" Text="Provincia"></asp:Label>
                 <div class="col-sm-7">
                    <asp:DropDownList ID="ddlProvincia" CssClass="form-control select2" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="ddlProvincia_SelectedIndexChanged">
                </asp:DropDownList>
                      <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProvincia" ControlToValidate="ddlProvincia" ValidationGroup="AfiliadosDatosDomicilios" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                  
                </div>
                      <div class="col-sm-1"></div>
                 </div>
                
                <div class="form-group row">
                <asp:Label ID="lblLocalidad" CssClass="col-sm-4 col-form-label"  runat="server" Text="Localidad"></asp:Label>
                  <div class="col-sm-7">
                <asp:DropDownList ID="ddlLocalidad" CssClass="form-control select2" runat="server">
                </asp:DropDownList>
                 </div>
                      <div class="col-sm-1"></div>
                 </div>

               <div class="form-group row">
                <asp:Label ID="lblCodigoPostal" CssClass="col-sm-4 col-form-label"  runat="server" Text="Codigo Postal"></asp:Label>
                 <div class="col-sm-7">
                    <asp:TextBox ID="txtCodigoPostal" CssClass="form-control" ValidationGroup="AfiliadosDatosDomicilios"  runat="server"></asp:TextBox>
                
                 </div>
                     <div class="col-sm-1">
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoPostal" ControlToValidate="txtCodigoPostal" ValidationGroup="AfiliadosDatosDomicilios" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                     </div>
                 </div>

                <div class="form-group row">
                    <asp:Label ID="lblEstado" CssClass="col-sm-4 col-form-label" runat="server" Text="Estado" />
                    <div class="col-sm-7">   
                    <asp:DropDownList ID="ddlEstado" CssClass="form-control" runat="server" />
                     </div>
                      <div class="col-sm-1"></div>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="AfiliadosDatosDomicilios" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
            </div>
    </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        </div>
