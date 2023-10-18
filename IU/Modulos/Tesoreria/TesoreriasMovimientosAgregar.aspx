<%@ Page Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpTesorerias.master" AutoEventWireup="true" CodeBehind="TesoreriasMovimientosAgregar.aspx.cs" Inherits="IU.Modulos.Tesoreria.TesoreriasMovimientosAgregar" Title="" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <div class="TesoreriasMovimientosAgregar">
        <asp:UpdatePanel ID="upMovimientos" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
                <br />
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server" OnSelectedIndexChanged="ddlTipoOperacion_OnSelectedIndexChanged"
                            AutoPostBack="true" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacion"
                            ErrorMessage="*" ValidationGroup="Aceptar" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" OnSelectedIndexChanged="ddlMoneda_OnSelectedIndexChanged" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rfvMoneda" CssClass="Validador" ValidationGroup="Aceptar" ControlToValidate="ddlMoneda" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Tesoreria" />
                    <div class="col-sm-3">
                        <AUGE:CurrencyTextBox CssClass="form-control" enabled="false" ID="txtSaldoActual" runat="server" />
                    </div>
                </div>
                <asp:Panel ID="pnlCajas" Visible="false" runat="server">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCajero" runat="server" Text="Cajero" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlCajero" runat="server" OnSelectedIndexChanged="ddlCajero_OnSelectedIndexChanged"
                                AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="rfvCajero" CssClass="Validador" ValidationGroup="Aceptar" ControlToValidate="ddlCajero" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCaja" runat="server" Text="Número de Caja" />
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtNumeroCaja" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoCaja" runat="server" Text="Saldo Caja" Visible="false" />
                        <div class="col-sm-3">
                            <AUGE:CurrencyTextBox CssClass="form-control" ID="txtSaldoCaja" runat="server" Visible="false" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlCuentaInternaDestino" Visible="false">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoDestino" runat="server" Text="Banco destino"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" OnSelectedIndexChanged="ddlBancos_OnSelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancos" Enabled="false" runat="server" ControlToValidate="ddlBancos"
                                ErrorMessage="*" ValidationGroup="Aceptar" />
                        </div>

                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Cuenta destino"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentas" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentas" Enabled="false" runat="server" ControlToValidate="ddlBancosCuentas"
                                ErrorMessage="*" ValidationGroup="Aceptar" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlFilialesDestino" Visible="false">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDestino" runat="server" Text="Filial destino"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFiliales" Enabled="false" runat="server" ControlToValidate="ddlFiliales"
                                ErrorMessage="*" ValidationGroup="Aceptar" />
                        </div>
                    </div>
                </asp:Panel>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte"
                            ErrorMessage="*" ValidationGroup="Aceptar" />
                    </div>
                    <AUGE:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Movimiento" runat="server" />
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" TextMode="MultiLine" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:TabContainer ID="tbArchivos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="TabPanel1"
                HeaderText="Archivos">
                <ContentTemplate>
                    <AUGE:Archivos ID="ctrArchivos" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
        <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="row justify-content-md-center">
                    <div class="col-md-auto">
                         <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
