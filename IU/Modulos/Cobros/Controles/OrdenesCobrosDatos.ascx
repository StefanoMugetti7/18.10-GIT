<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesCobrosDatos.ascx.cs" Inherits="IU.Modulos.Cobros.Controles.OrdenesCobrosDatos" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" tagname="AsientoMostrar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" tagname="popUpGrillaGenerica" tagprefix="auge" %>

<script language="javascript" type="text/javascript">
// fires on blur and keyup events for #myID
    $(document).on('blur', '#txtPrefijoNumeroRecibo', function() {
        $(this).addLeadingZeros(4);
    });

    $(document).on('blur', '#txtNumeroRecibo', function() {
        $(this).addLeadingZeros(4);
    });

    $.fn.addLeadingZeros = function(length) {
      for(var el of this){
        _value = el.value.replace(/^0+/,'');
        length = length - _value.length;
        if(length > 0){
          while (length--) _value = '0' + _value;
        }
        el.value = _value;
      }
    };
</script>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="OrdenesCobrosDatos">

    <asp:UpdatePanel ID="upArmarCobros" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            
                        <div class="form-group row">

            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblOrdenCobro" runat="server" Text="Numero"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtOrdenCobro" Enabled="false" runat="server"></asp:TextBox>
       </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
          <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
     </div>   </div>
                        <div class="form-group row">

            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
           <div class="col-lg-7 col-md-7 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtDetalle" TextMode="MultiLine" runat="server"></asp:TextBox>
   </div>    </div>
                        <div class="form-group row">

            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilialCobro" runat="server" Text="Filial Cobro" />
        <div class="col-lg-3 col-md-3 col-sm-9">     <asp:DropDownList CssClass="form-control select2" ID="ddlFilialCobro" runat="server" />
   </div>   </div>
                       

            <asp:Panel ID="pnlCobrosManuales" GroupingText="Cobros Manuales" runat="server">
             
                        <div class="form-group row">

                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblConceptos" runat="server" Text="Concepto"></asp:Label>
              <div class="col-lg-3 col-md-3 col-sm-9">   <asp:DropDownList CssClass="form-control select2" ID="ddlConceptosContables" runat="server">
                </asp:DropDownList>
              </div>
               <div class="col-lg-3 col-md-3 col-sm-9">  <AUGE:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte" 
                    ErrorMessage="*" ValidationGroup="IngresarConcepto"/>
            </div> <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:Button CssClass="botonesEvol" ID="btnIngresarConcepto" runat="server" Text="Ingresar Cobro" 
                        onclick="btnIngresarConcepto_Click" ValidationGroup="IngresarConcepto" /></div>
           </div> </asp:Panel>
      
            <asp:Panel ID="pnlDetalleCobros" GroupingText="Detalle Orden Cobro" runat="server">
            <asp:GridView ID="gvOrdenesCobrosDetalles" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnRowDataBound="gvOrdenesCobrosDetalles_RowDataBound" onpageindexchanging="gvOrdenesCobrosDetalles_PageIndexChanging"
                OnRowCommand="gvOrdenesCobrosDetalles_RowCommand">
                <Columns>
                    <asp:BoundField  HeaderText="Detalle" DataField="Detalle" />
                    <asp:TemplateField HeaderText="Concepto">
                            <ItemTemplate>
                                <%# Eval("ConceptoContable.ConceptoContable")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField  HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                            AlternateText="Elminiar" ToolTip="Eliminar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
            </ContentTemplate>
    </asp:UpdatePanel>
            <br />
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
                <auge:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
               <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <%--<auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="OrdenesCobrosDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
               </div></div>
            </ContentTemplate>
        </asp:UpdatePanel> 
</div>
