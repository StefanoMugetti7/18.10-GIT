<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudPagoDetalleBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.SolicitudPagoDetalleBuscarPopUp" %>

<div class="SolicitudPagoDetalleBuscarPopUp">
<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Comprobantes de Compras Detalles" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social" />
      <div class="col-sm-3">  <asp:TextBox CssClass="form-control" ID="txtRazonSocial" runat="server" />
   </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="NumeroFactura" ></asp:Label>
     <div class="col-sm-1">   <AUGE:NumericTextBox CssClass="form-control" ID="txtPreNumeroFactura" runat="server"  MaxLength="4" ></AUGE:NumericTextBox>
  </div>   <div class="col-sm-2">   <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server"  MaxLength="8" ></AUGE:NumericTextBox>
</div>
     <div class="col-sm-3">   <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
   </div></div>
     <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Factura - Fecha Desde"></asp:Label>
           <div class="col-sm-3">     <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
           </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
           <div class="col-sm-3">    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
        </div>
  </div> <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción producto" />
     <div class="col-sm-3">   <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
  </div> </div> <div class="table-responsive">
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="false" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdSolicitudPagoDetalle"
    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
        <Columns>
            <asp:BoundField  HeaderText="Razon Social" DataField="RazonSocial" />
            <asp:BoundField  HeaderText="Fecha Factura" DataField="FechaFactura" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField  HeaderText="Comprobante" DataField="NumeroFacturaCompleto" />
            <asp:BoundField  HeaderText="Descripción" DataField="Descripcion" />
            <asp:BoundField  HeaderText="Cantidad" DataField="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"  />
            <asp:BoundField  HeaderText="Importe" DataField="Importe" HeaderStyle-CssClass="text-right"  ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}"/>
            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>       
        </Columns>
    </asp:GridView></div>
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" />
    </center>
</asp:Panel>
</div>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnVolver" >
</asp:ModalPopupExtender>
