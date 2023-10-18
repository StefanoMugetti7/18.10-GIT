<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasListar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosFacturasListar" %>

<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%--<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" TagName="AfiliadosDatos" TagPrefix="auge" %>--%>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
                                    cuit: item.Cuit,
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
                $("input[id*='hdfCuit']").val(e.params.data.cuit);
                $("input:text[id*='txtCuit']").val(e.params.data.cuit);
            });

            control.on('select2:unselect', function (e) {
                $("select[id$='ddlNumeroSocio']").val(null).trigger("change");
                $("input[id*='hdfIdAfiliado']").val('');
                $("input[id*='hdfRazonSocial']").val('');
                $("input[id*='hdfCuit']").val('');
                $("input:text[id*='txtCuit']").val('');
                control.val(null).trigger('change');
            });
        }
    </script>
    <div class="OrdenesPagos">
        <asp:UpdatePanel ID="upEntidades" runat="server" UpdateMode="Conditional">
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
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
                    </div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Cliente" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                        <asp:HiddenField ID="hdfCuit" runat="server" />
                    </div>
                    <%--                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />--%>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuit" runat="server" Text="CUIT"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCuit" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumero" runat="server" Text="Nro. Orden Cobro"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Nro. Comprobante"></asp:Label>
                    <div class="col-sm-1">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroFactura" runat="server" maxlength="4" />
                    </div>
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" maxlength="8" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-sm-12">

                        <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />

                        <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdOrdenCobro, EstadoIdEstado"
                            runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Número" SortExpression="IdOrdenCobro">
                                    <ItemTemplate>
                                        <%# Eval("IdOrdenCobro")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Razon Social" SortExpression="RazonSocial">
                                    <ItemTemplate>
                                        <%# Eval("RazonSocial")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cuil" SortExpression="Afiliado.CUIL">
                                    <ItemTemplate>
                                        <%# Eval("AfiliadoCUIL")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacionTipoOperacion">
                                    <ItemTemplate>
                                        <%# Eval("TipoOperacionTipoOperacion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Número Recibo" SortExpression="NumeroReciboCompleto">
                                    <ItemTemplate>
                                        <%# Eval("NumeroReciboCompleto")%>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Fecha Emision" SortExpression="FechaEmision">
                                    <ItemTemplate>
                                        <%# Eval("FechaEmision", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Confirmacion" SortExpression="FechaConfirmacion">
                                    <ItemTemplate>
                                        <%# Eval("FechaConfirmacion", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe Total" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="ImporteBruto">
                                    <ItemTemplate>
                                        <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteBruto", "{0:N2}"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("EstadoDescripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/sendmail.png" runat="server" CommandName="EnviarMail" ID="btnEnviarMail"
                                            AlternateText="Enviar Mail" ToolTip="Enviar Mail" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                            AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                            AlternateText="Anular Orden Cobro" ToolTip="Anular Orden Cobro" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Evol:EvolGridView>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
