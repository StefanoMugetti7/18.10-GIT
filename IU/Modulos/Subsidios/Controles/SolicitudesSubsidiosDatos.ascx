<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesSubsidiosDatos.ascx.cs" Inherits="IU.Modulos.Subsidios.Controles.SolicitudesSubsidiosDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>

 <script language="javascript" type="text/javascript">

     function CalcularItem() {
         var importe = 0.00;
         var cantidad = 0;
         var Total = 0.00;
         var cantidad = 1;
         var importe = $("input:text[id$='txtImporte']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
         var txtCantidad = $('input:text[id$="CantidadSolicitada"]');
         if ($('input:text[id$="CantidadSolicitada"]').val() != undefined) {
             cantidad = $('input:text[id$="CantidadSolicitada"]').val();
         }
         Total = parseFloat(importe) * parseFloat(cantidad);
         $("input[type=text][id$='txtImporteTotal']").val(accounting.formatMoney(Total, "$ ", 2, "."));
     }

</script>
<div class="SolicitudesPagosDatos">
   <asp:UpdatePanel ID="upGeneral" UpdateMode="Conditional" runat="server" >
                <ContentTemplate> 
                     <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha" ></asp:Label>
            <div class="col-sm-3">
                         <asp:TextBox CssClass="form-control" ID="txtFecha" Enabled="false" runat="server"/>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialPago" runat="server" Text="Filial Pago:" ></asp:Label>
                         <div class="col-sm-3">
                         <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPago" Enabled="false" runat="server"> </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvFilialPago" Enabled="true" ControlToValidate="ddlFilialPago" ValidationGroup="ValidadorControlesDinamicos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                             </div>
                         <div class="col-sm-3"></div>
                         </div>
                     <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion"  />
            <div class="col-sm-7">
                <%--lo cambie a 310, estaba  en 500, por si quieren volver a dejarlo como antes.--%>
                         <asp:TextBox CssClass="form-control" ID="txtObservacion" Width="700px" runat="server" MaxLength="600" TextMode="MultiLine" />
            </div>
                         </div>
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
                <ContentTemplate>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubsidio" runat="server" Text="Subsidio" ></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlSubsidio" AutoPostBack="true" runat="server" 
                        onselectedindexchanged="ddlSubsidio_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSubsidio" runat="server" ControlToValidate="ddlSubsidio" 
                        ErrorMessage="*" ValidationGroup="ValidadorControlesDinamicos"/>
                </div>
                        <div class="col-sm-8"></div>
                        </div>
                        </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="upCamposDinamicos" UpdateMode="Conditional" runat="server" >
                <ContentTemplate>
                    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel ID="pnlImporte" runat="server">
                <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe Beneficio" />
                <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server" />
             </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTotal" runat="server" Text="Importe Total" />
           <div class="col-sm-3">     <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteTotal" Enabled="false" runat="server" /></div></div>
            </asp:Panel>
            </ContentTemplate>
            </asp:UpdatePanel>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
    SkinID="MyTab">

    <asp:TabPanel runat="server" ID="tpArchivos"
        HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>

</asp:TabContainer>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                 <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                          onclick="btnImprimir_Click" ValidationGroup="Imprimir" Visible="false"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="ValidadorControlesDinamicos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
          </div></div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>