<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CentrosCostosPrrorrateosDatosPopUp.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.CentrosCostosPrrorrateosDatosPopUp" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CentrosCostosProrrateosDatos.ascx" TagPrefix="AUGE" TagName="ControlDatos" %>

<div class="SolicitudPagoDetalleBuscarPopUp">
<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Escenario de Centros de Costos" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
    <AUGE:ControlDatos ID="ctrDatos" runat="server" />
</asp:Panel>
</div>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" >
</asp:ModalPopupExtender>