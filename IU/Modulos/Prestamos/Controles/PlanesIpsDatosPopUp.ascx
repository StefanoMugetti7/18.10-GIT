<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesIpsDatosPopUp.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PlanesIpsDatosPopUp" %>

<script lang="javascript" type="text/javascript">

 function ShowModalPopUp() {
        $("[id$='modalPopUp']").modal('show');
    }

    function HideModalPopUp() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPopUp']").modal('hide');
    }




    </script>

<div class="modal" id="modalPopUp" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-md" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Sistema de gestión para mutuales</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">


                        <div class="form-group row">
        <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta"></asp:Label>
                        <div class="col-lg-9 col-md-8 col-sm-8">  <asp:TextBox CssClass="form-control" ID="txtFechaAlta" Enabled="false" runat="server"></asp:TextBox>
   </div></div>
     
                        <div class="form-group row">   <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
       <div class="col-lg-9 col-md-8 col-sm-8">  <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
   
 </div></div>
       
                        <div class="form-group row"> <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblImporteTotal" runat="server" Text="Importe Total"></asp:Label>
       <div class="col-lg-9 col-md-8 col-sm-8">  <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteTotal" runat="server"></Evol:CurrencyTextBox>
        <asp:RequiredFieldValidator ID="rfvImporteTotal" ValidationGroup="PlanesIpsDatosPopUp"  ControlToValidate="txtImporteTotal" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
 </div></div>
      
                        <div class="form-group row">  <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cantidad Cuotas"></asp:Label>
      <div class="col-lg-9 col-md-8 col-sm-8">   <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCuotas" NumberOfDecimals="0" Prefix="" runat="server"></Evol:CurrencyTextBox>
        <asp:RequiredFieldValidator ID="rfvCantidadCuotas" ValidationGroup="PlanesTasasDatosPopUp"  ControlToValidate="txtCantidadCuotas" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
</div></div>
       
                        <div class="form-group row"> <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblImporteCuota" runat="server" Text="Importe Cuota"></asp:Label>
        <div class="col-lg-9 col-md-8 col-sm-8"><Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCuota" runat="server"></Evol:CurrencyTextBox>
        <asp:RequiredFieldValidator ID="rfvImporteCuota" ValidationGroup="PlanesIpsDatosPopUp"  ControlToValidate="txtImporteCuota" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
</div></div>
       
                        <div class="form-group row"> <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
      <div class="col-lg-9 col-md-8 col-sm-8">   <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" Enabled="false" runat="server">
        </asp:DropDownList>
      </div> </div></div>

                        <div class="modal-footer">

    <div class="row justify-content-md-center">
            <div class="col-md-auto">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" ValidationGroup="PlanesIpsDatosPopUp" runat="server" Text="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" Text="Volver" runat="server" />
    </div></div>
    </div>


                   
                    </div>
             </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>