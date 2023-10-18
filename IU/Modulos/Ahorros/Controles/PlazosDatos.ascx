<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlazosDatos.ascx.cs" Inherits="IU.Modulos.Ahorros.Controles.PlazosDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/ParametrosValores.ascx" tagname="ParametrosValores" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


    <div class="form-group row">
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaAlta">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaDesde">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvParametroValor" CssClass="ValidadorCalendario" ValidationGroup="Aceptar" ControlToValidate="txtFechaDesde" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvEstado">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvMoneda">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvMoneda" CssClass="Validador" runat="server" ControlToValidate="ddlMoneda"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPlazoDias">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPlazoDias" runat="server" Text="Plazo Días" />
                <%--<AUGE:ParametrosValores ID="ctrParametrosValores" runat="server" />--%>
                <div class="col-sm-9">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtPlazoDias" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPlazoDias" runat="server" InitialValue="" ControlToValidate="txtPlazoDias"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTasaInteres">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTasaInteres" runat="server" Text="Tasa Interes" />
                <div class="col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtTasaInteres" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTasaInteres" runat="server" InitialValue="" ControlToValidate="txtTasaInteres"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                    <asp:RangeValidator ID="rvTasaInteres" runat="server" ErrorMessage="*" ControlToValidate="txtTasaInteres"
                        MinimumValue="0" MaximumValue="100" Type="Currency" ValidationGroup="Aceptar" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteDesde">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteDesde" runat="server" Text="Importe Desde" />
                <div class="col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteDesde" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteHasta">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteHasta" runat="server" Text="Importe Hasta" />
                <div class="col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteHasta" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPlazoAnterior">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPlazoAnterior" runat="server" Text="Plazo Anterior"></asp:Label>
                <div class="col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlPlazoAnterior" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />

    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
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

