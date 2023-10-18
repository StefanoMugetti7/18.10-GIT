<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesTasasDatosPopUp.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PlanesTasasDatosPopUp" %>

<script lang="javascript" type="text/javascript">

    function ShowModalPlanesTasasPopUp() {
        $("[id$='modalPlanesTasasPopUp']").modal('show');
    }

    function HideModalPlanesTasasPopUp() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPlanesTasasPopUp']").modal('hide');
    }

    function CalcularTasaEfectivaMensual() {
        //Hago los calculos desde la TEM
        var TEM = 0.00;
        var TEA = 0.00;
        var TNA = 0.00;
        TEM = $("input[type=text][id$='txtTasaEfectivaMensual']").maskMoney('unmasked')[0];
        if (TEM > 0) {
            //TEA = [(1+TEM) ^ 12 -1] * 100 
            TEA = (1 + Math.pow(TEM, 12) - 1) * 100;
            TNA = 12 * (Math.pow(1 + parseFloat(TEA) / 100, 30 / 360) - 1) * 100;
            $("input[type=text][id$='txtTasaEfectivaAnual']").val(accounting.formatMoney(TEA, '', 4, "."));
            $("input[type=text][id$='txtTasaNominalAnual']").val(accounting.formatMoney(TNA, '', 4, "."));
        }
    }

    function CalcularTasaEfectivaAnual() {
        //Hago los calculos desde la TEA
        var TEM = 0.00;
        var TEA = 0.00;
        var TNA = 0.00;
        TEA = $("input[type=text][id$='txtTasaEfectivaAnual']").maskMoney('unmasked')[0];
        if (TEA > 0) {
            // = ((1+ (D2/100)^(1/12) -1))
            TEM = 1 + Math.pow(TEA / 100, 1 / 12) - 1;
            //= D3 * ( (1+D2/100)^(D6/D5) -1) * 100
            TNA = 12 * (Math.pow(1 + parseFloat(TEA) / 100, 30 / 360) - 1) * 100;
            $("input[type=text][id$='txtTasaEfectivaMensual']").val(accounting.formatMoney(TEM, '', 4, "."));
            $("input[type=text][id$='txtTasaNominalAnual']").val(accounting.formatMoney(TNA, '', 4, "."));
        }
    }

    function CalcularTasaNominalAnual() {
        //Hago los calculos desde la TNA
        var TEM = 0.00;
        var TEA = 0.00;
        var TNA = 0.00;
        var TNA = $("input[type=text][id$='txtTasaNominalAnual']").maskMoney('unmasked')[0];
        if (TNA > 0) {
            //( (1+TNA/12/100)^12) -1
            TEA = (Math.pow(1 + TNA / 12 / 100, 12) - 1) * 100;
            TEM = 1 + Math.pow(TEA / 100, 1 / 12) - 1;
            $("input[type=text][id$='txtTasaEfectivaAnual']").val(accounting.formatMoney(TEA, '', 4, "."));
            $("input[type=text][id$='txtTasaEfectivaMensual']").val(accounting.formatMoney(TEM, '', 4, "."));
        }
    }

</script>
<div class="modal" id="modalPlanesTasasPopUp" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-md" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Sistema de gestión</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control" ID="txtFechaAlta" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>

                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblTasaNominalAnual" runat="server" Text="Tasa Nominal Anual"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaNominalAnual" NumberOfDecimals="4" Prefix="" runat="server"></Evol:CurrencyTextBox>
                                <asp:RequiredFieldValidator ID="rfvTasaNominalAnual" ValidationGroup="PlanesTasasDatosPopUp2" ControlToValidate="txtTasaNominalAnual" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblTasaEfectivaMensual" runat="server" Text="Tasa Efectiva Mensual"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaEfectivaMensual" NumberOfDecimals="4" Prefix="" Enabled="false" runat="server"></Evol:CurrencyTextBox>
                                <asp:RequiredFieldValidator ID="rfvTasaEfectivaMensual" ValidationGroup="PlanesTasasDatosPopUp2" ControlToValidate="txtTasaEfectivaMensual" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblTasaEfectivaAnual" runat="server" Text="Tasa Efectiva Anual"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaEfectivaAnual" NumberOfDecimals="4" Prefix="" Enabled="false" runat="server"></Evol:CurrencyTextBox>
                                <asp:RequiredFieldValidator ID="rfvTasaEfectivaAnual" ValidationGroup="PlanesTasasDatosPopUp2" ControlToValidate="txtTasaEfectivaAnual" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cantidad Cuotas Desde"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtCantindadCuotas" runat="server"></AUGE:NumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvCantindadCuotas" ValidationGroup="PlanesTasasDatosPopUp2" ControlToValidate="txtCantindadCuotas" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblCantidadCuotasHasta" runat="server" Text="Cantidad Cuotas Hasta"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtCanidadCuotasHasta" runat="server"></AUGE:NumericTextBox>
                                <asp:RequiredFieldValidator ID="rfvCantidadCuoasHasta" ValidationGroup="PlanesTasasDatosPopUp2" ControlToValidate="txtCanidadCuotasHasta" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblImporteDesde" runat="server" Text="Importe desde"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteDesde" runat="server"></Evol:CurrencyTextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblImporeHasta" runat="server" Text="Importe hasta"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteHasta" runat="server"></Evol:CurrencyTextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                                 <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" Enabled="false" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <div class="row justify-content-md-center">
                                <div class="col-md-auto">


                                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" ValidationGroup="PlanesTasasDatosPopUp2" runat="server" Text="Aceptar" />
                                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" Text="Volver" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


