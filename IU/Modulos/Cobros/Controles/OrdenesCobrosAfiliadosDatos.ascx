<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesCobrosAfiliadosDatos.ascx.cs" Inherits="IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" tagname="AsientoMostrar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" tagname="popUpGrillaGenerica" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosValores.ascx" TagName="OrdenesCobrosValores" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="OrdenesCobrosDatos">

    <script language="javascript" type="text/javascript">

        function CalcularItem() {
            var importeTotal = 0.00;
            $('#<%=gvCuentaCorriente.ClientID%> tr').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find('input:text[id*="txtACobrar"]').maskMoney('unmasked')[0];

            if (incluir && importe) {
                importeTotal += parseFloat(importe);
            }
        });
        $("#<%=gvCuentaCorriente.ClientID %> [id$=lblTotalACobrar]").text(accounting.formatMoney(importeTotal, gblSimbolo, 2, "."));

        }
         /*gridViewId va ser la grilla en la que deseo que suceda el selectallrows*/ 
    var gridViewId = '#<%= gvCuentaCorriente.ClientID %>';

    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
        updateSelectionLabel();
    }
    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
    }
    </script>

    <asp:UpdatePanel ID="upArmarCobros" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">

                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOrdenCobro" runat="server" Text="Numero"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtOrdenCobro" Enabled="false" runat="server"></asp:TextBox>
                </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
                </div>

                 <AUGE:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Cobro" runat="server" />

            </div>
            <div class="form-group row">

                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" Enabled="false" runat="server"></asp:TextBox>
                </div>
              
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" OnSelectedIndexChanged="ddlMoneda_OnSelectedIndexChanged" AutoPostBack="true" runat="server" />
                </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialCobro" runat="server" Text="Filial Cobro" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilialCobro" runat="server" />
                </div>
               
            </div>

           
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control" ID="txtDetalle" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="rfvDetalle" ValidationGroup="OrdenesCobrosDatos" ControlToValidate="txtDetalle" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
                <div class="col-sm-1">
  <asp:Button CssClass="botonesEvol" ID="btnAnticipoCargos" runat="server" Text="Anticipo de Cargos" onclick="btnAnticipos_Click" ValidationGroup="Aceptar" />

                </div>
            </div>
                <AUGE:CamposValores ID="ctrCampos" runat="server" />
            <asp:Panel ID="pnlCuentaCorriente" Visible="false" GroupingText="Cargos Pendientes" runat="server">
                <div class="table-responsive">
                <asp:GridView ID="gvCuentaCorriente" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false"
                    ShowFooter="true" OnRowDataBound="gvCuentaCorriente_RowDataBound" OnPageIndexChanging="gvCuentaCorriente_PageIndexChanging">
                    <Columns>
                        <asp:BoundField HeaderText="Fecha Movimiento" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                        <asp:BoundField HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                        <asp:TemplateField HeaderText="Tipo Cargo" SortExpression="TipoCargo.TipoCargo">
                            <ItemTemplate>
                                <%# Eval("TipoCargo.TipoCargo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                        <%--<asp:TemplateField HeaderText="Tipo Movimiento" SortExpression="TipoMovimiento.TipoMovimiento">
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion.TipoMovimiento.TipoMovimiento")%>
                                </ItemTemplate>
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                                <ItemTemplate>
                                    <%# Eval("TipoValor.TipoValor")%>
                                </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Forma Cobro" SortExpression="FormaCobro.FormaCobro">
                            <ItemTemplate>
                                <%# Eval("FormaCobro.FormaCobro")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteEnviar" SortExpression="Importe" />
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A Cobrar" SortExpression="ACobrar">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtACobrar" runat="server" Text='<%#Bind("ImporteEnviar", "{0:C2}") %>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblTotalACobrar" runat="server" Text="0.00"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkIncluirTodos" runat="server" onclick="checkAllRow(this); CalcularItem();" Visible="true" Text="Todo" TextAlign="Left" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir"  runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                </asp:GridView>
               </div>
         
                <%--<div class="Espacio"></div><div class="Espacio"></div>
                <asp:Button CssClass="botonesEvol" ID="btnIncluirCobro" runat="server" Text="Inlcuir en Cobro" Visible="false" 
                        onclick="btnIncluirCobro_Click" CausesValidation="false" />--%>
            </asp:Panel>
            <%--<asp:Panel ID="pnlDetalleCobros" GroupingText="Detalle Orden Cobro" runat="server">
            <asp:GridView ID="gvOrdenesCobrosDetalles" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                OnRowDataBound="gvOrdenesCobrosDetalles_RowDataBound" onpageindexchanging="gvOrdenesCobrosDetalles_PageIndexChanging"
                OnRowCommand="gvOrdenesCobrosDetalles_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Periodo" SortExpression="CuentaCorriente.Periodo">
                                <ItemTemplate>
                                    <%# Eval("CuentaCorriente.Periodo")%>
                                </ItemTemplate>
                        </asp:TemplateField>
                    <asp:BoundField  HeaderText="Detalle" DataField="Detalle" />
                    <%--<asp:TemplateField HeaderText="Concepto">
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
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <AUGE:OrdenesCobrosValores ID="ctrOrdenesCobrosValores" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
          <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <%--<auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                   <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />

                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                   <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" Visible="false"
                    OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
                
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="OrdenesCobrosDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
        </div></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
