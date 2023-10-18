<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesComprasListar.aspx.cs" Inherits="IU.Modulos.Compras.OrdenesComprasListar" %>

<%--<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="popUpBuscarProveedor" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            InitApellidoSelect2();
        });

        function InitApellidoSelect2() {
            var control = $("select[name$='ddlNumeroProveedor']");
            //var lblCUIT = $(this).find("input:text[id*='lblCUIT']");
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
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Proveedores/ProveedoresWS.asmx/ProveedoresCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: control.val(), // search term");
                            filtro: params.term // search term");
                        });
                    },
                    beforeSend: function (xhr, opts) {
                        var algo = JSON.parse(this.data); // this.data.split('"');
                        //console.log(algo.filtro);
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

                                    id: item.IdProveedor,
                                    text: item.CodigoProveedor,
                                    RazonSocial: item.RazonSocial,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            control.on('select2:select', function (e) {
                $("input[id*='hdfIdProveedor']").val(e.params.data.id);//.trigger("change");
                $("input[id*='hdfRazonSocial']").val(e.params.data.RazonSocial);
                //AfiliadoSeleccionar();
            });
            control.on('select2:unselect', function (e) {
                control.val(null).trigger('change');
                $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
                $("input[id*='hdfRazonSocial']").val('');
            });
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <contenttemplate>
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
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoOrden" runat="server" Text="Codigo Orden Compra"></asp:Label>
                <div class="col-sm-3">
                    <auge:numerictextbox cssclass="form-control" id="txtCodigoOrdenCompra" runat="server"></auge:numerictextbox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <%--          <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion de pago" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionPago" runat="server" />
                </div>--%>
                <div class="col-sm-8"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroProveedor" runat="server"></asp:DropDownList>
                    <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                    <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                </div>
                <div class="col-sm-4"></div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                        OnClick="btnBuscar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                        OnClick="btnAgregar_Click" />
                </div>
                <%--   <AUGE:NumericTextBox CssClass="txtCodigoBuscador" ID="txtCodigo" Enabled="false" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarProveedor"
                    AlternateText="Buscar proveedor" ToolTip="Buscar" OnClick="btnBuscarProveedor_Click" />
                <asp:ImageButton ImageUrl="~/Imagenes/Baja.png" runat="server" ID="btnLimpiar" Visible="false"
                    AlternateText="Limpiar" ToolTip="Limpiar" OnClick="btnLimpiar_Click" />
                <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />--%>
                <%--                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>--%>
                <div class="col-sm-8"></div>
            </div>
            <%--<asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                                runat="server" onclick="btnExportarExcel_Click" Visible="false" />--%>
            <auge:popupcomprobantes id="ctrPopUpComprobantes" runat="server" />
            <div class="table-responsive">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <columns>
                        <asp:TemplateField HeaderText="Código Orden" SortExpression="CodigoOrden">
                            <itemtemplate>
                                <%# Eval("IdOrdenCompra")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <itemtemplate>
                                <%# Eval("Proveedor.RazonSocial")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Orden Compra" SortExpression="FechaOrden">
                            <itemtemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <itemtemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </itemtemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <itemtemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" Visible="false" ID="btnAutorizar"
                                    AlternateText="Autorizar Orden Compra" ToolTip="Autorizar Orden Compra" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnular"
                                    AlternateText="Anular Orden Compra" ToolTip="Anular Orden Compra" />
                            </itemtemplate>
                        </asp:TemplateField>
                    </columns>
                </asp:GridView>
            </div>
        </contenttemplate>
        <%--<Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
