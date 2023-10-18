<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridViewCheckImporte.ascx.cs" Inherits="IU.Modulos.ProcesosDatos.Controles.GridViewCheckImporte" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ExpandCollapse);
        ExpandCollapse();
    });

    function ExpandCollapse() {
        $("[src*=plus]").on('click', function () {
            var src = $(this).attr("src");
            if (src.indexOf("plus") >= 0) {
                $(this).attr("src", "../../Imagenes/minus.png");
                var panelDetalle = $(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']");
                panelDetalle.show();
                var lbl = $(panelDetalle).find("[id *='lblImporteTotal']").attr('id');
                InitFooterDetalle(lbl);
            }
            else {
                $(this).attr("src", "../../Imagenes/plus.png");
                $(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']").hide();
            }
        });
    }

    var gridViewId = '#<%= gvDatosLote.ClientID %>';
    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
    }

    function ValidarTotal(control) {
        var acum = 0;
        var importeEnviar = $("input:text[id*='" + control + "']").maskMoney('unmasked')[0];
        var importe = $("input:text[id*='" + control + "']").closest('td').find("input:hidden[id*='hdfImporte']").maskMoney('unmasked')[0];
        if (importeEnviar > importe) {
            $("input:text[id*='" + control + "']").val(accounting.formatMoney(importe, gblSimbolo, gblCantidadDecimales, "."));
        }
        var lbl = $("input:text[id*='" + control + "']").closest('table').find('tr').last().find("span:first").attr('id');;
        InitFooterDetalle(lbl);
    }
    function InitFooterDetalle(control) {
        var acum = 0;
        var lbl = $("span[id*='" + control + "']");
        $(lbl).closest('table').find('tr').not(':first').not(':last').each(function () {
            acum += parseFloat($(this).find("input:text[id*='txtImporte']").maskMoney('unmasked')[0]);
        });
        lbl.text("Importe a Enviar: " + accounting.formatMoney(acum, gblSimbolo, gblCantidadDecimales, "."));
        $(lbl).closest('table').closest('tr').prev().find('span').text(accounting.formatMoney(acum, gblSimbolo, gblCantidadDecimales, "."));
    }
</script>

<asp:UpdatePanel ID="upDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <Evol:EvolGridView ID="gvDatosLote" AllowPaging="false"
                OnRowDataBound="gvDatosLote_RowDataBound" DataKeyNames="IdValor, DatoAdicional"
                runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatosLote_PageIndexChanging" Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="Nro. Socio">
                        <ItemTemplate>
                            <%# Eval("NumeroSocio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nro. Documento">
                        <ItemTemplate>
                            <%# Eval("NumeroDocumento")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nro. Legajo">
                        <ItemTemplate>
                            <%# Eval("MatriculaIAF")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apellido y Nombre">
                        <ItemTemplate>
                            <%# Eval("ApellidoNombre")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Dato Adicional">
                        <ItemTemplate>
                            <%# Eval("DatoAdicional")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe a Enviar">
                        <ItemTemplate>
                            <asp:Label ID="lblImporteEnviado" runat="server" Text='<%# Eval("ImporteEnviado", "{0:C2}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" Checked="true" runat="server" onclick="checkAllRow(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" Checked='<%# Eval("Incluir")%>' runat="server" />
                        </ItemTemplate>
                     </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <img alt = "Mostrar / Ocultar" id="imgExpandCollapse" style="cursor: pointer; vertical-align:middle;" src="../../Imagenes/plus.png" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <!-- this row has the child grid-->
                            </td>
                                </tr>
                                <tr>
                                    <td colspan="999" style="padding: 0px;">
                                        <asp:HiddenField ID="hdfMostrarDetalle" Value="1" runat="server" />
                                        <asp:Panel ID="pnlDatosDetalles" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDatosDetalles" runat="server" AutoGenerateColumns="false"
                                                SkinID="GrillaBasicaFormal" ShowFooter="true" OnRowDataBound="gvDatosDetalles_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="FechaMovimiento" HeaderText="Fecha Movimiento" />
                                                    <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                    <asp:BoundField DataField="Concepto" HeaderText="Concepto" />
                                                    <asp:BoundField DataField="Importe" DataFormatString="{0:C2}" HeaderText="Importe" />
                                                    <asp:TemplateField HeaderText="Importe a Enviar">
                                                        <ItemTemplate>
                                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" Text='<%#Eval("ImporteEnviar")%>' />
                                                            <asp:HiddenField ID="hdfIdValorDetalle" Value='<%#Bind("IdValorDetalle") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>