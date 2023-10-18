<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FacturasListar.aspx.cs" Inherits="IU.Modulos.Facturas.FacturasListar" %>

<%--<%@ Register src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" tagname="popUpBuscarCliente" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/EnviarMails.ascx" TagName="EnviarMails" TagPrefix="auge" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Content ID="headScript" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script lang="javascript" type="text/javascript">

        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            SetTabIndexInput();
            InitApellidoSelect2();
        });

        function SetTabIndexInput() {
            $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }

        //function CheckRow(objRef) {
        //    var row = objRef.parentNode.parentNode;
        //    var GridView = row.parentNode;
        //    var inputList = GridView.getElementsByTagName("input");
        //    for (var i = 0; i < inputList.length; i++) {
        //        var headerCheckBox = inputList[0];
        //        var checked = true;
        //        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
        //            if (!inputList[i].checked) {
        //                checked = false;
        //                break;
        //            }
        //        }
        //    }
        //    headerCheckBox.checked = checked;
        //}

        //function checkAllRow(objRef) {
        //    var GridView = objRef.parentNode.parentNode.parentNode;
        //    var inputList = GridView.getElementsByTagName("input");
        //    for (var i = 0; i < inputList.length; i++) {
        //        //Get the Cell To find out ColumnIndex
        //        var row = inputList[i].parentNode.parentNode;
        //        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
        //            if (objRef.checked) {
        //                if (inputList[i].disabled == false) {
        //                    inputList[i].checked = true;
        //                }
        //                else {
        //                    inputList[i].checked = false;
        //                }
        //            }
        //            else {
        //                inputList[i].checked = false;
        //            }
        //        }
        //    }
        //}
        //function CheckRow(objRef) {
        //    var checked = true;
        //    $(objRef).closest('table').find('tr').not(':last').each(function () {
        //        if (!$(this).find('[id*="chkIncluir"]').is(':checked')) {
        //            checked = false;
        //            return false;
        //        }
        //    });
        //    $(objRef).closest('tr').closest('table').closest('div').prev('div').find('[id*="chkIncluirTodos"]').prop('checked', checked);
        //}

        //function checkAllRow(objRef) {
        //    $(objRef).closest('tr').closest('table').closest('div').next('div').find('tr').not(':last').each(function () {
        //        $(this).find('[id*="chkIncluir"]').prop('checked', objRef.checked);
        //    });
        //}
        function InitApellidoSelect2() {
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
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosClienteCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
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
                                    id: item.IdAfiliado,
                                    descripcionAfiliado: item.DescripcionAfiliado,
                                    //Cuit: item.TipoDocumentoDescripcion,
                                    //NumeroDocumento: item.NumeroDocumento,
                                    //RazonSocial: item.RazonSocial,
                                    //IdCondicionFiscal: item.IdCondicionFiscal,
                                    //Estado: item.EstadoDescripcion,
                                    //Detalle: item.Detalle,
                                    //Apellido: item.Apellido,
                                    //Nombre: item.Nombre,
                                    //IdTipoDocumento: item.IdTipoDocumento,
                                    //TipoDocumento: item.DescripcionTipoDocumento,
                                    //NumeroDocumento: item.NumeroDocumento,
                                    //IdAfiliadoTipo: item.IdAfiliadoTipo,
                                    //IdCondicionFiscal: item.IdCondicionFiscal,
                                    //CondicionFiscalDescripcion: item.CondicionFiscalDescripcion
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            control.on('select2:select', function (e) {
                $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
                $("input[id*='hdfRazonSocial']").val(e.params.data.descripcionAfiliado);
                //CargarTipoPuntoVenta();
            });
            control.on('select2:unselect', function (e) {
                $("input[id*='hdfIdAfiliado']").val('');
                $("input[id*='hdfRazonSocial']").val('');
                control.val(null).trigger('change');
                //CargarTipoPuntoVenta();
            });
        }
    </script>
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

    <div class="FacturasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Número Factura"></asp:Label>
                    <div class="col-sm-1">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroFactura" runat="server" maxlength="4" />
                    </div>
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" maxlength="8" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo factura" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFacturaLote" runat="server" Text="Lote de Facturas"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFacturasLotes" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Cliente" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                    </div>
                    <div class="col-sm-4"></div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnValidarFacturasElectronicas" runat="server" Text="Validar Facturas Electronicas" Visible="false"
                            OnClick="btnValidarFacturasElectronicas_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                        <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                        <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdFactura"
                            runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="# Cliente" SortExpression="AfiliadoIdAfiliado">
                                    <ItemTemplate>
                                        <%# Eval("AfiliadoIdAfiliado")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Razon Social" SortExpression="AfiliadoRazonSocial">
                                    <ItemTemplate>
                                        <%# Eval("AfiliadoRazonSocial")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Comprobante" ItemStyle-Wrap="false" SortExpression="TipoFacturaDescripcion">
                                    <ItemTemplate>
                                        <%# Eval("TipoFacturaDescripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Número Comprobante" ItemStyle-Wrap="false" SortExpression="NumeroFactura">
                                    <ItemTemplate>
                                        <%# Eval("NumeroFacturaCompleto")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Factura" ItemStyle-Wrap="false" SortExpression="FechaFactura">
                                    <ItemTemplate>
                                        <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                                    <ItemTemplate>
                                        <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteSinIVA", "{0:N2}"))%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Iva" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="IvaTotal">
                                    <ItemTemplate>
                                        <%# string.Concat(Eval("MonedaMoneda"), Eval("IvaTotal", "{0:N2}"))%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe Total" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                    <ItemTemplate>
                                        <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Usuario Alta" SortExpression="UsuarioAltaApellidoNombre">
                                    <ItemTemplate>
                                        <%# Eval("UsuarioAltaApellidoNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                    <ItemTemplate>
                                        <%# Eval("EstadoDescripcion")%>
                                        <asp:Image ID="imgAlerta" runat="server" Visible="false" ToolTip="Factura Electronica Pendiente de Validar en AFIP" AlternateText="Factura Electronica Pendiente de Validar en AFIP" ImageUrl="~/Imagenes/alerta.png" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkIncluirTodos" runat="server" onclick="checkAllRow(this); " Text="Envios" TextAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("CantidadEnvios") %>
                                        <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this);" Checked='<%# Convert.ToBoolean (Eval("Incluir")) %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/sendmail.png" runat="server" CommandName="EnviarMail" ID="btnEnviarMail"
                                            AlternateText="Enviar Mail" ToolTip="Enviar Mail" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar"
                                            AlternateText="Autorizar Comprobante" ToolTip="Autorizar Comprobante" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                            AlternateText="Anular Factura" ToolTip="Anular Factura" Visible="false" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                            AlternateText="Modificar" ToolTip="Modificar" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Evol:EvolGridView>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
        <%--        <asp:UpdatePanel ID="upEnviarMails" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <AUGE:EnviarMails ID="ctrEnviarMails" Visible="false" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
