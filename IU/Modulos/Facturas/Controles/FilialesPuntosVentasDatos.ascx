<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilialesPuntosVentasDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FilialesPuntosVentasDatos" %>

<script type="text/javascript" lang="javascript">

    function ShowModalFilialesPuntosVentas()
    {
        $("[id$='modalFilialesPuntosVentas']").modal('show')
    }

    function HideModalFilialesPuntosVentas() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalFilialesPuntosVentas']").modal('hide')
    }

    function ValidarDatos() {
        if (Page_ClientValidate("Aceptar")) {
            return true;
        }
        return false;
    }
</script>
<div class="modal" id="modalFilialesPuntosVentas" tabindex="-1" role="dialog" >
    <div class="modal-dialog modal-dialog-scrollable modal-dialog-centered" role="document">
    <div class="modal-content">
        <div class="modal-header">
        <h5 class="modal-title">Alta Baja de Punto de Venta</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    
            <div class="modal-body">

    <div class="form-group row"> 
        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblNumero" runat="server" Text="Numero"></asp:Label>
        <div class="col-sm-8">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" maxlength="8" />
        <asp:RequiredFieldValidator ID="rfvNumeroFactura" ValidationGroup="Aceptar" ControlToValidate="txtNumeroFactura" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblTipoPuntoVenta" runat="server" Text="Tipo de Punto de Venta"></asp:Label>
        <div class="col-sm-8">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposPuntosVentas" runat="server">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvTiposPuntosVentas" ValidationGroup="Aceptar" ControlToValidate="ddlTiposPuntosVentas" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblFilial" runat="server" Text="Filial" ToolTip="Filial a la que esta asociado el Punto de Venta"></asp:Label>
        <div class="col-sm-8">
        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" ToolTip="Filial a la que esta asociado el Punto de Venta"/>
        <asp:RequiredFieldValidator ID="rfvFilial" ValidationGroup="Aceptar" ControlToValidate="ddlFilial" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo de Comprobante"></asp:Label>
        <div class="col-sm-8">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposFacturas" runat="server"></asp:DropDownList>
            </div>
        </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblUltimoNumeroFactura" runat="server" Text="Ultimo numero Factura"></asp:Label>
        <div class="col-sm-8">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtUltimoNumeroFactura" runat="server" maxlength="8" />
            </div>
        </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-4 col-form-label" ID="lblEstado"  runat="server" Enabled="false" Text="Estado"></asp:Label>
        <div class="col-sm-8">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados"  runat="server">
            </asp:DropDownList>
            </div>
        </div>
    
        </div>
            <div class="modal-footer">
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click"  OnClientClick="HideModalFilialesPuntosVentas();" ValidationGroup="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
            </div>
        </div>
    </div>
</div>