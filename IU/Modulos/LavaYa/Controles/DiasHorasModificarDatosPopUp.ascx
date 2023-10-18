<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiasHorasModificarDatosPopUp.ascx.cs" Inherits="IU.Modulos.LavaYa.Controles.DiasHorasModificarDatosPopUp" %>
<script type="text/javascript" lang="javascript">
    function ShowModalDiasHoras() {
        $("[id$='modalDiasHoras']").modal('show');
    }

    function HideModalDiasHoras() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalDiasHoras']").modal('hide');
    }
</script>
<%--EspecializacionesModificarDatos--%>

<div class="modal" id="modalDiasHoras" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Dias Horas</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">

                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDia" runat="server" Text="Dia"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlDia" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDia" ControlToValidate="ddlDia" ValidationGroup="DiasHorasModificarDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraDesde" runat="server" Text="Hora Desde"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control hourpicker" ID="txtHoraDesde" runat="server"></asp:TextBox>
                                <asp:MaskedEditExtender ID="meeStartTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                    Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtHoraDesde"
                                    InputDirection="LeftToRight" AcceptNegative="Left"></asp:MaskedEditExtender>
                                <asp:MaskedEditValidator ID="mevStartTime" runat="server" ControlExtender="meeStartTime"
                                    ControlToValidate="txtHoraDesde" IsValidEmpty="false" EmptyValueMessage="*"
                                    InvalidValueMessage="Time is invalid" Display="Dynamic" EmptyValueBlurredText="*"
                                    InvalidValueBlurredMessage="*" ValidationGroup="DiasHorasModificarDatos" CssClass="Validador" />
                            </div>

                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraHasta" runat="server" Text="Hora hasta"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control " ID="txtHoraHasta" runat="server"></asp:TextBox>
                                <asp:MaskedEditExtender ID="meeEndTime" runat="server" AcceptAMPM="true" MaskType="Time"
                                    Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                    ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtHoraHasta"
                                    InputDirection="LeftToRight" AcceptNegative="Left"></asp:MaskedEditExtender>
                                <asp:MaskedEditValidator ID="mevEndTime" runat="server" ControlExtender="meeEndTime"
                                    ControlToValidate="txtHoraHasta" IsValidEmpty="false" EmptyValueMessage="*"
                                    InvalidValueMessage="*" Display="Dynamic" EmptyValueBlurredText="*"
                                    InvalidValueBlurredMessage="*" ValidationGroup="DiasHorasModificarDatos" CssClass="Validador" />
                            </div>

                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                            OnClick="btnAceptar_Click" ValidationGroup="DiasHorasModificarDatos" />
                        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                            OnClick="btnCancelar_Click" />
                    </div>


                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

