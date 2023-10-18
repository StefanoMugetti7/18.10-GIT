<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubEscalasDatosPopUp.ascx.cs" Inherits="IU.Modulos.Subsidios.Controles.SubEscalasDatosPopUp" %>
<script lang="javascript" type="text/javascript">
    function ShowModalEscalasPopUp() {
        $("[id$='modalEscalasPopUp']").modal('show');
    }

    function HideModalEscalasPopUp() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalEscalasPopUp']").modal('hide');
    }
</script>
<div class="modal" id="modalEscalasPopUp" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-md" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Subsidios Escalas</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Ingreso Desde"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaDesde" runat="server" ControlToValidate="txtFechaDesde"
                                    ErrorMessage="*" ValidationGroup="DiasHorasModificarDatos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Ingreso Hasta"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblEdadDesde" runat="server" Text="Edad Desde"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtEdadDesde" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEdadDesde" runat="server" ControlToValidate="txtEdadDesde"
                                    ErrorMessage="*" ValidationGroup="DiasHorasModificarDatos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblEdadHasta" runat="server" Text="Edad Hasta"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtEdadHasta" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblAntiguedadDesde" runat="server" Text="Antiguedad Meses Desde"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtAntiguedadDesde" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvAntiguedadDesde" runat="server" ControlToValidate="txtAntiguedadDesde"
                                    ErrorMessage="*" ValidationGroup="DiasHorasModificarDatos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblAntiguedadHasta" runat="server" Text="Antiguedad Meses Hasta"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtAntiguedadHasta" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblImporteBeneficio" runat="server" Text="Importe Beneficio"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                   <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteBeneficio" runat="server" />
                                   
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaInicioVigencia" runat="server" Text="Fecha Inicio Vigencia"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInicioVigencia" runat="server"></asp:TextBox>

                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaInicioVigencia" runat="server" ControlToValidate="txtFechaInicioVigencia"
                                    ErrorMessage="*" ValidationGroup="DiasHorasModificarDatos" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblFechaFinVigencia" runat="server" Text="Fecha Fin Vigencia"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFinVigencia" runat="server"></asp:TextBox>

                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-3 col-md-4 col-sm-4 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                            <div class="col-lg-9 col-md-8 col-sm-8">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                                    OnClick="btnAceptar_Click" ValidationGroup="DiasHorasModificarDatos" />
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
