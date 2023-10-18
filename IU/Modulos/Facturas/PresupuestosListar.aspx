<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PresupuestosListar.aspx.cs" Inherits="IU.Modulos.Facturas.PresupuestosListar" %>

<%--<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" TagName="popUpBuscarCliente" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

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
                                    //razonSocial: item.RazonSocial,
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
                $("select[id$='ddlNumeroSocio']").val(null).trigger("change");
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
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Número"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroFactura" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3"></div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Cliente" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdPresupuesto"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="# Cliente" SortExpression="IdAfiliado">
                            <ItemTemplate>
                                <%# Eval("IdAfiliado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Razon Social" SortExpression="RazonSocial">
                            <ItemTemplate>
                                <%# Eval("RazonSocial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="# Presupuesto" SortExpression="IdPresupuesto">
                            <ItemTemplate>
                                <%# Eval("IdPresupuesto")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                            <ItemTemplate>
                                <%# string.Concat(Eval("Moneda"), Eval("ImporteSinIva", "{0:N2}"))%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Iva" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="IvaTotal">
                            <ItemTemplate>
                                 <%# string.Concat(Eval("Moneda"), Eval("IvaTotal", "{0:N2}"))%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Total" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                            <ItemTemplate>
                                <%# string.Concat(Eval("Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar" 
                                AlternateText="Autorizar Comprobante" ToolTip="Autorizar Comprobante" />--%>
                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
