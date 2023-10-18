<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="InformesRecepcionesListar.aspx.cs" Inherits="IU.Modulos.Compras.InformesRecepcionesListar" %>

<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="popUpBuscarProveedor" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script lang="javascript" type="text/javascript">

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            InitApellidoSelect2();
            $("input[type=text][id$='txtFechaDesde']").focus();
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
                                    //Cuit: item.TipoDocumentoDescripcion,
                                    NumeroDocumento: item.NumeroDocumento,
                                    RazonSocial: item.RazonSocial,
                                    IdCondicionFiscal: item.IdCondicionFiscal,
                                    Estado: item.EstadoDescripcion,
                                    Beneficiario: item.Beneficiario,
                                    cuit: item.CUIT,
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
                $("input[id*='hdfIdProveedor']").val(e.params.data.id);//.trigger("change");
                $("input[id*='hdfRazonSocial']").val(e.params.data.RazonSocial);
                $("input[type=text][id$='txtCUIT']").val(e.params.data.cuit);
                $("input[id*='hdfCuit']").val(e.params.data.cuit);
                //AfiliadoSeleccionar();
            });
            control.on('select2:unselect', function (e) {
                control.val(null).trigger('change');
                $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
                $("input[id*='hdfRazonSocial']").val('');
                $("input[type=text][id$='txtCUIT']").val('');
                $("input[id*='hdfCuit']").val('');
            });
        }
<%--        function AfiliadoSeleccionar() {
            __doPostBack("<%=button.UniqueID %>", "");
        }--%>
    </script>

    <div class="InformesRecepcionesListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoInforme" runat="server" Text="Codigo Informe"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoInforme" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoOrden" runat="server" Text="Codigo Orden Compra"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoOrden" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <div class="col-sm-1">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarAbierta" runat="server" Text="Agregar"
                            OnClick="btnAgregarAbierta_Click" />
                    </div>
                    <div class="col-sm-1">
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar con Orden Compra"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
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
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroSocio">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroProveedor" runat="server" Text="Proveedor"></asp:Label>
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroProveedor" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvNumeroProveedor" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroProveedor" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                                <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                                <asp:HiddenField ID="hdfCuit" runat="server" />
                            </div>
                        </div>
                    </div>
                    <%--                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />--%>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCUIT" runat="server" Text="CUIT"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Código Informe" SortExpression="CodigoInforme">
                            <ItemTemplate>
                                <%# Eval("IdInformeRecepcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Remito" SortExpression="FechaRemito">
                            <ItemTemplate>
                                <%# Eval("FechaEmision", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Numero" SortExpression="NumeroRemito">
                            <ItemTemplate>
                                <%# Eval("NumeroRemitoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Código Orden" SortExpression="CodigoOrden">
                            <ItemTemplate>
                                <%# Eval("OrdenCompra.IdOrdenCompra")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Proveedor.RazonSocial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Recepcion" SortExpression="FechaRecepcion">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" Visible="false" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnular"
                                    AlternateText="Anular Informe Recepcion" ToolTip="Anular Informe Recepcion" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" Visible="false" ID="btnConsultarAbierta"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" Visible="false" ID="btnAnularAbierta"
                                    AlternateText="Anular Informe Recepcion" ToolTip="Anular Informe Recepcion" />
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
