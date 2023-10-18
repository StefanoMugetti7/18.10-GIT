<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EspecializacionesModificarDatosPopUp.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.EspecializacionesModificarDatosPopUp" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<script type="text/javascript" lang="javascript">
    function ShowModalBuscarEspecializacion(){
        $("[id$='modalEspecializaciones']").modal('show');
    }

    function HideModalBuscarEspecializacion() {
         $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalEspecializaciones']").modal('hide');
    }
</script>

<div class="modal" id="modalEspecializaciones" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Especializaciones</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">


                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblEspecialidad" runat="server" Text="Especialidad"></asp:Label>
                            <div class="col-sm-7">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEspecialidad" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEspecialidad" ControlToValidate="ddlEspecialidad" ValidationGroup="EspecializacionesModificarDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                               <div class="col-sm-1"></div>
                            </div>
                                         
                        <div class="form-group row">

                             <asp:Label CssClass="col-sm-4 col-form-label" ID="lblPredeterminado" runat="server" Text="Predeterminado"></asp:Label>
                            <div class="col-sm-7">
                                <asp:CheckBox ID="chkPredeterminado" runat="server" CssClass="form-control" />
                            </div>
                               <div class="col-sm-1"></div>
                        </div>
                         <div class="form-group row">
                            <asp:Label CssClass="col-sm-4 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                            <div class="col-sm-7">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                                </asp:DropDownList>
                            </div>
                                <div class="col-sm-1"></div>
                        </div>
                        <AUGE:CamposValores ID="ctrCamposValores" cssLabel="col-sm-4 col-form-label" cssCol="col-sm-7 col-12" ccsContainer="col-12 col-md-12 col-lg-12" runat="server" />
                   </div>
                      <div class="modal-footer">
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                                OnClick="btnAceptar_Click" ValidationGroup="EspecializacionesModificarDatos" />
                       
                     
                            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                                OnClick="btnCancelar_Click" />
                          </div>
                      
                   </div>
                </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
