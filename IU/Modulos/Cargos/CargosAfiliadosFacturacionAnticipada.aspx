<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CargosAfiliadosFacturacionAnticipada.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosFacturacionAnticipada" %>

<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentHead" runat="server">
    <link href='<%=ResolveUrl("~/assets/global/plugins/select2/css/select2.min.css")%>' rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(IniciarCombos);
            IniciarCombos();
        });

        function IniciarCombos() {
            $('select[id$="ddlTiposCargos"]').select2({
                language: "es",
                //placeholder: "Seleccione una opción",
                //allowClear: true
                //selectOnClose: true
            });
            //$('select[id$="ddlTiposCargos"]').val(null).trigger('change');
        }

        function CalcularItem() {
            var importeTotal = 0.00;
            $('#<%=gvCuentaCorriente.ClientID%> tr').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find("input[id*='hdfImporte']").val();

            if (incluir && importe) {
                importe = importe.replace('.', '').replace(',', '.');
                importeTotal += parseFloat(importe);
            }
        });
        $("#<%=gvCuentaCorriente.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(importeTotal, "$ ", 2, "."));

        }

        function CheckRow(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i]
                    != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        //    function checkAllRow(objRef) {
        //        $(objRef).closest('tr').closest('table').closest('div').next('div').find('tr').not(':last').each(function () {
        //            $(this).find('[id*="chkIncluir"]').prop('checked', objRef.checked);
        //        });
        //    }
        function checkAllRow(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef
                    != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }

    </script>
    <asp:UpdatePanel ID="upFacturacion" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-sm-4">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="Label1" runat="server" Text="Fecha" />
                    <div class="col-sm-4">
                 <asp:TextBox CssClass="form-control datepicker" ID="txtFechaMovimiento"  runat="server" />
                <asp:RequiredFieldValidator ID="rfvFechaMovimiento" ValidationGroup="GenerarCargos" ControlToValidate="txtFechaMovimiento" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                </div>
                    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblPeriodoHasta" runat="server" Text="Periodo" ToolTip="Periodo Hasta (AAAAMM)" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPeriodoHasta"  MaxLength="6" Prefix="" ThousandsSeparator="" NumberOfDecimals="0" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPeriodoHasta" ValidationGroup="GenerarCargos" ControlToValidate="txtPeriodoHasta" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                </div>
                </div>
                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo de Cargo" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTiposCargos" runat="server" />
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnGenerarCargos" runat="server" Text="Visualizar Cargos a Generar"
                        OnClick="btnGenerarCargos_Click" ValidationGroup="GenerarCargos" />
                </div>
            </div>

          <%--  <div class="form-group row">

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaMovimiento" runat="server" Text="Fecha Movimiento" />
                <div class="col-sm-1">
                 <asp:TextBox CssClass="form-control datepicker" ID="txtFechaMovimiento"  runat="server" />
                <asp:RequiredFieldValidator ID="rfvFechaMovimiento" ValidationGroup="GenerarCargos" ControlToValidate="txtFechaMovimiento" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoHasta" runat="server" Text="Periodo Hasta (AAAAMM)" />
                <div class="col-sm-1">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPeriodoHasta" MaxLength="6" Prefix="" ThousandsSeparator="" NumberOfDecimals="0" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvPeriodoHasta" ValidationGroup="GenerarCargos" ControlToValidate="txtPeriodoHasta" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
              

            </div>
            <div class="form-group row">
                <div class="col-sm-9"></div>
                
            </div>--%>

            <asp:GridView ID="gvCuentaCorriente" DataKeyNames="Id"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false"
                ShowFooter="true">
                <Columns>
                    <asp:BoundField HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="Periodo" />
                    <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                    <asp:TemplateField HeaderText="Forma Cobro" SortExpression="FormaCobro">
                        <ItemTemplate>
                            <%# Eval("FormaCobro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# string.Concat(Eval("Moneda"), Eval("Importe", "{0:N2}"))%>
                        </ItemTemplate>

                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text="$ 0.00"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this); CalcularItem();" Text="Incluir" TextAlign="Left" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this); CalcularItem();" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
            <center>
                <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Generar Cargos" Visible="false"
                    onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
