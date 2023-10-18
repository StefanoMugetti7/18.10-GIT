<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeriodosContablesDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.PeriodosContablesDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="PeriodosContablesDatos">

    <asp:UpdatePanel ID="upPeriodo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEjerciciosContables_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEjercicio" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlEjercicioContable" ValidationGroup="Aceptar" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodo" runat="server" Text="Período AAAAMM" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPeriodo" Enabled="false" MaxLength="6" Prefix="" NumberOfDecimals="0" ThousandsSeparator="" runat="server" />

                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaCierre" runat="server" Text="Fecha Cierre" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaCierre" runat="server" Enabled="false" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

