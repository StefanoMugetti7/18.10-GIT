<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CargoAfiliadoModificarDatosFormaCobro.ascx.cs" Inherits="IU.Modulos.Cargos.Controles.CargoAfiliadoModificarDatosFormaCobro" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
 <%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" tagname="popUpGrillaGenerica" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

 <auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        InitApellidoSelect2();
    });

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function CalcularItem() {
        var importeTotal = 0.00;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var importe = $(this).find('input:text[id*="txtImporteRevertir"]').maskMoney('unmasked')[0];
            var controlImporteCobrado = $(this).find("input[id*='hdfImporteCobrado']").val().replace('.', '').replace(',', '.');
            var controlImporteRevertido = $(this).find("input[id*='ImporteRevertido']").val().replace('.', '').replace(',', '.');
            if (importe) {
                if (parseFloat(importe) > (parseFloat(controlImporteCobrado) - parseFloat(controlImporteRevertido))) {
                    importe = parseFloat(controlImporteCobrado) - parseFloat(controlImporteRevertido);
                    $(this).find('input:text[id*="txtImporteRevertir"]').val(accounting.formatMoney(importe, "$ ", 2, "."));
                }
                importeTotal += parseFloat(importe);
            }
        });
        $("#<%=gvDatos.ClientID %> [id$=lblTotalARevertir]").text(accounting.formatMoney(importeTotal, "$ ", 2, "."));

    }

    function InitApellidoSelect2() {
        var formaCobro = $("select[name$='ddlFormaCobro']");
        var control = $("select[name$='ddlNumeroSocio']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            //width: '100%',
            language: 'es',
            //tags: true,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosFormasCobroCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            idformacobro: formaCobro.val(),
                            value: control.val(), // search term");
                            filtro: params.term // search term");
                        });
                    },
                    beforeSend: function (xhr, opts) {
                        var algo = JSON.parse(this.data); // this.data.split('"');
                        if (isNaN(algo.filtro)) {
                            if (algo.filtro.length < 4) {
                                xhr.abort();
                            }
                        }
                        else {
                        }
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.DescripcionCombo,
                                    id: item.IdAfiliado
                                }
                            })
                        };
                        cache: true
                    }
                }
            });

        control.on('select2:select', function (e) {
                $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
                $("input[id*='hdfRazonSocial']").val(e.params.data.text);
            });

            control.on('select2:unselect', function (e) {
                $("select[id$='ddlNumeroSocio']").val(null).trigger("change");
                $("input[id*='hdfIdAfiliado']").val('');
                $("input[id*='hdfRazonSocial']").val('');
                control.val(null).trigger('change');
            });
        }
</script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaMovimiento" runat="server" Text="Fecha Movimiento"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaMoviminto" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodo" runat="server" Text="Periodo"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtPeriodo" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo Cargo"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtTipoCargo" Enabled="false" runat="server"></asp:TextBox>
                    </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo Valor"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtTipoValor" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma Cobro"></asp:Label>
                <div class="col-sm-2">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server"  OnSelectedIndexChanged="ddlFormaCobro_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                </div>
                <div class="col-sm-1">
                    <button class="botonesEvol" type="button" id="btnDetalleFormaCobro" data-toggle="collapse" data-target="#collapseExampleDetalleCobros" aria-expanded="false" aria-controls="collapseExample">
                        Detalle
                    </button>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe"></asp:Label>
                <div class="col-sm-3">
                <AUGE:CurrencyTextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server"></AUGE:CurrencyTextBox>
                    </div>
            </div>    
            <div class="collapse" id="collapseExampleDetalleCobros">
                    <div class="card card-body row">
                        <div class="col">
                            <div class="col-sm-6">
                                <asp:GridView ID="gvDetalleFormaCobro" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="true">
                                </asp:GridView>
                            </div>
                            <div class="col-sm-6">
                                <asp:GridView ID="gvFormasCobrosCodigosConceptos" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                     <Columns>
                                        <asp:BoundField  HeaderText="Codigo Concepto" DataField="CodigoConcepto" />
                                         <asp:BoundField  HeaderText="Codigo Concepto Plan" DataField="CodigoConceptoPrestamoPlan" />
                                         <asp:BoundField  HeaderText="Sub Concepto" DataField="CodigoSubConcepto" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Concepto"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtConcepto" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Los Cargos de Este Socio los Paga" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" /> 
                        <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                        <asp:HiddenField ID="hdfCuit" runat="server" />
                    </div>
                <asp:PlaceHolder ID="pnlCuentasAhorros" Visible="false" runat="server">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentasAhorros" runat="server" Text="Cuentas de Ahorro"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCuentasAhorros" runat="server">
                        </asp:DropDownList>
                    </div>
                </asp:PlaceHolder>
            </div>
                    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
            <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
            <asp:TabPanel runat="server" ID="tpDetalleCobros" HeaderText="Detalle de Cobros" >
                <ContentTemplate>
                    <asp:GridView ID="gvDatos" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" 
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdCuentaCorrienteCobro" >
        <Columns>
            <asp:BoundField  HeaderText="Fecha de Cobro" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
            <asp:TemplateField HeaderText="Concepto" >
                    <ItemTemplate>
                        <%# Eval("Concepto")%>
                    </ItemTemplate>
            </asp:TemplateField>
<%--            <asp:TemplateField HeaderText="Tipo Valor" >
                    <ItemTemplate>
                        <%# Eval("TipoValor")%>
                    </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="Forma de Cobro" >
                    <ItemTemplate>
                        <%# Eval("FormaCobro")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Importe Cobrado" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteCobrado" />
            <asp:BoundField  HeaderText="Saldo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="SaldoActual" />
            <asp:BoundField  HeaderText="Importe Revertido" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteRevertido" />
            <asp:TemplateField HeaderText="A Revertir" SortExpression="" Visible="false">
                <ItemTemplate>
                    <Evol:CurrencyTextBox CssClass="gvTextBox" ID="txtImporteRevertir" runat="server" ></Evol:CurrencyTextBox>
                    <asp:HiddenField ID="hdfImporteCobrado" Value='<%#Bind("ImporteCobrado") %>' runat="server" />
                    <asp:HiddenField ID="hdfImporteRevertido" Value='<%#Bind("ImporteRevertido") %>' runat="server" />
                </ItemTemplate>
                <FooterTemplate>
                <asp:Label CssClass="labelFooterEvol" ID="lblTotalARevertir" runat="server" Text="0.00" ></asp:Label>
            </FooterTemplate> 
            </asp:TemplateField>
            </Columns>
    </asp:GridView>
                </ContentTemplate>
            </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
                <ContentTemplate>
                    <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
            <center>
                    <br />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
                    onclick="btnAceptar_Click" ValidationGroup="AfiliadosDatosTelefonos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
                    onclick="btnCancelar_Click" />
                </center>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
