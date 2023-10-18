<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListasValoresDetallesDatosPopUp.ascx.cs" Inherits="IU.Modulos.TGE.Control.ListasValoresDetallesDatosPopUp" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoValor" runat="server" Text="Codigo"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtCodigoValor" runat="server"></asp:TextBox>
    </div>

    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvDescripcion" CssClass="Validador" ValidationGroup="ListasValoresDetallesDatosPopUp" ControlToValidate="txtDescripcion" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
      <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
    </div>
      <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaDepende" runat="server" Text="Lista Valores Dependientes"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlListaDepende" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvListaDepende" Visible="false" runat="server" ValidationGroup="ListasValoresDetallesDatosPopUp" ControlToValidate="ddlListaDepende" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConsultaDinamicaCombos" Visible="false" runat="server" Text="Consulta Dinamica para Combos"></asp:Label>
    <div class="col-sm-7">
        <asp:TextBox CssClass="form-control" ID="txtConsultaDinamicaCombos" Visible="false" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
</div>

<asp:Panel ID="pnlCamposValores" Visible="true" GroupingText="Datos adicionales" runat="server">
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
</asp:Panel>

<div class="row justify-content-md-center">
    <div class="col-md-auto">
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" ValidationGroup="ListasValoresDetallesDatosPopUp" runat="server" Text="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" Text="Volver" CausesValidation="false" runat="server" />
    </div>
</div>
<%--    </div>--%>
<%--</asp:Panel>--%>
<%--<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender 
    ID="mpePopUp" runat="server" 
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" 
    BehaviorID="bhidMpePopUpCamposValores"
    CancelControlID="btnCancelar"
    >
</asp:ModalPopupExtender>--%>