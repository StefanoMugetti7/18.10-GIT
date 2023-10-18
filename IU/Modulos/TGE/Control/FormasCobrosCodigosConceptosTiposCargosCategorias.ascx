<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormasCobrosCodigosConceptosTiposCargosCategorias.ascx.cs" Inherits="IU.Modulos.TGE.Control.FormasCobrosCodigosConceptosTiposCargosCategorias" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma de Cobro" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFormaCobro" runat="server" ControlToValidate="ddlFormaCobro"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo de Cargo" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCargo" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoCargo" runat="server" ControlToValidate="ddlTipoCargo"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCategoria" runat="server" Text="Categoria" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlCategoria" runat="server" />
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPrestamoPlan" runat="server" Text="Prestamo Plan" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlPrestamoPlan" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTopeEnvioCargo" runat="server" Text="Importe Tope Envio de Cargo" />
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteTopeEnvioCargo" runat="server"></Evol:CurrencyTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporteTopeEnvioCargo" runat="server" ControlToValidate="txtImporteTopeEnvioCargo"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoConceptoPrestamoPlan" runat="server" Text="Codigo Concepto Prestamo Plan" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtCodigoConceptoPrestamoPlan" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoConcepto" runat="server" Text="Codigo Concepto" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtCodigoConcepto" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoSubConcepto" runat="server" Text="Codigo SubConcepto" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtCodigoSubConcepto" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSeEnviaComoPrestamo" runat="server" Text="Se envia como Prestamo" />
            <div class="col-sm-3">
                <asp:CheckBox ID="chkSeEnviaComoPrestamo" CssClass="form-control" runat="server" />
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSeEnviaTXT" runat="server" Text="Se envia en TXT" />
            <div class="col-sm-3">
                <asp:CheckBox ID="chkSeEnviaTXT" CssClass="form-control" runat="server" />
            </div>
        </div>
        <center>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
        </center>
    </ContentTemplate>
</asp:UpdatePanel>