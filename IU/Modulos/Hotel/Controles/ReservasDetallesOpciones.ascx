<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservasDetallesOpciones.ascx.cs" Inherits="IU.Modulos.Hotel.Controles.ReservasDetallesOpciones" %>
<script lang="javascript" type="text/javascript">
   function ShowModalPopUpReservasDetallesOpciones() {
        $("[id$='modalPopUpReservasDetallesOpciones']").modal('show');
    }

    function HideModalPopUpReservasDetallesOpciones() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPopUpReservasDetallesOpciones']").modal('hide');
    }

     function CalcularPersonas() {
        var CantidadPersonasHab = $("input[id*='hdfCantidadPersonas']").val();       
        var Cantidad = $("input[type=text][id$='txtCantidad']").val();

            if (CantidadPersonasHab < Cantidad) {              
            
                $("input[type=text][id$='txtCantidad']").val(CantidadPersonasHab);
                //$("input[type=text][id$='txtCantidad']").focus();
                var mensaje = "La Cantidad De Personas No Puede Ser Mayor a " + CantidadPersonasHab; 
                MostrarMensaje(mensaje, false);
                //alert("El importe del Plazo Fijo no puede ser mayo al Saldo de la cuenta.")
            }
        
    }


</script>

<div class="modal" id="modalPopUpReservasDetallesOpciones" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-centered modal-md" role="document">
                <div class="modal-content">
                             <div class="modal-header">
                                <h5 class="modal-title">Reservas Detalles Opciones</h5>
                                   <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                      <span aria-hidden="true">&times;</span>
                                   </button>
                             </div>
                         <div class="modal-body">
                       
                             <div class="form-group row">
                                <asp:Label CssClass="col-lg-5 col-md-6 col-sm-4 col-form-label" ID="lblCompartida" runat="server" Text="Compartida"></asp:Label>
                              <div class="col-lg-7 col-md-6 col-sm-8">

                               <asp:DropDownList ID="ddlMoviliario" CssClass="form-control select2" runat="server"></asp:DropDownList></div></div>
                            
                             <div class="form-group row">
                                   <asp:Label CssClass="col-lg-5 col-md-6 col-sm-6 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad de Personas"></asp:Label>
                                  <div class="col-lg-3 col-md-3 col-sm-3">
                              
                            <evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadMaxima" Enabled="false" NumberOfDecimals="0" Prefix="" runat="server" Text=""></evol:CurrencyTextBox>
                        </div>
                       <div class="col-lg-4 col-md-3 col-sm-5">
                              <asp:HiddenField ID="hdfCantidadPersonas" runat="server" />
                            <evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" NumberOfDecimals="0" Prefix="" runat="server" Text=""></evol:CurrencyTextBox>
                        </div></div>
                                <div class="form-group row">
                                                                       <asp:Label CssClass="col-lg-5 col-md-6 col-sm-6 col-form-label" ID="lblCheckOut" runat="server" Text="Late CheckOut"></asp:Label>

                                      <div class="col-lg-7 col-md-6 col-sm-8">
                                <asp:CheckBox ID="chkChekOut" CssClass="form-control" runat="server" />
                                          </div>
                                    </div>
                                   </div>

                    <div class="modal-footer">
<div class="row justify-content-md-center">
            <div class="col-md-auto">
              
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="TurnosModificarDatosPopUp" />
                        
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
                    onclick="btnCancelar_Click" />
                </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>