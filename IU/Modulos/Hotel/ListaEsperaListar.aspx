<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListaEsperaListar.aspx.cs" Inherits="IU.Modulos.Hotel.ListaEsperaListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitProductoSelect2);
            SetTabIndexInput();

            InitApellidoSelect2();
            InitProductoSelect2();
        });

        function SetTabIndexInput() {
            $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }

        function InitApellidoSelect2() {
            var control = $("select[name$='ddlApellido']");

            control.select2({
                placeholder: 'Ingrese el Apellido o Nombre',
                selectOnClose: true,
                theme: 'bootstrap4',
                minimumInputLength: 4,
                width: '100%',
                language: 'es',
                tags: true,
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: control.val(), // search term");
                            filtro: params.term, // search term");

                        });
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.DescripcionCombo,
                                    id: item.IdAfiliado,
                                    Apellido: item.Apellido,
                                    Nombre: item.Nombre,
                                    IdTipoDocumento: item.IdTipoDocumento,
                                    NumeroDocumento: item.NumeroDocumento,
                                    IdAfiliadoTipo: item.IdAfiliadoTipo,
                                    IdCondicionFiscal: item.IdCondicionFiscal,
                                    CondicionFiscalDescripcion: item.CondicionFiscalDescripcion,
                                    estadoDescripcion: item.EstadoDescripcion,
                                    categoriaDescripcion: item.CategoriaDescripcion,
                                    Correo: item.CorreoElectronico,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });

            control.on('select2:select', function (e) {
                if (e.params.data.id > 0) {
                    var newOption = new Option(e.params.data.Apellido, e.params.data.id, false, true);
                    $("select[id$='ddlApellido']").append(newOption).trigger('change');
                    $("select[id$='ddlTipoDocumento']").val(e.params.data.IdTipoDocumento).trigger('change');
                    $("input[type=text][id$='txtNumeroDocumento']").val(e.params.data.NumeroDocumento);
                    $("input[type=text][id$='txtNombre']").val(e.params.data.Nombre);
                    $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
                    $("input[id*='hdfIdAfiliadoTipo']").val(e.params.data.IdAfiliadoTipo);
                    $("input[id*='hdfApellido']").val(e.params.data.Apellido);
                    $("select[id$='ddlTipoDocumento']").prop("disabled", true);
                    $("input[type=text][id$='txtNumeroDocumento']").prop("disabled", true);
                    $("input[type=text][id$='txtNombre']").prop("disabled", true);
                }
                else {
                    $("input[id*='hdfApellido']").val(e.params.data.text);
                    $("select[id$='ddlTipoDocumento']").prop("disabled", false);
                    $("input[type=text][id$='txtNumeroDocumento']").prop("disabled", false);
                    $("input[type=text][id$='txtNombre']").prop("disabled", false);
                }
            });

            control.on('select2:unselect', function (e) {
                if ($.isNumeric(e.params.data.id)) {
                    $("select[id$='ddlTipoDocumento'] option:selected").val('');
                    $("input[type=text][id$='txtNumeroDocumento']").val('');
                    $("input[type=text][id$='txtNombre']").val('');
                    $("input[id*='hdfIdAfiliado']").val('');
                    $("input[id*='hdfIdAfiliadoTipo']").val('');
                    $("select[id$='ddlTipoDocumento']").prop("disabled", false);
                    $("input[type=text][id$='txtNumeroDocumento']").prop("disabled", false);
                    $("input[type=text][id$='txtNombre']").prop("disabled", false);
                }
                control.val(null).trigger('change');
            });

            control.on('select2:clear', function (e) {
                $("select[id$='ddlTipoDocumento']").val(e.params.data.IdTipoDocumento).trigger('change');
            });
        }

        function InitProductoSelect2() {
            var ddlProducto = $("select[name$='ddlProducto']");
            var hdfProductoDetalle = $("input:hidden[id*='hdfProductoDetalle']");
            var hdfIdProducto = $("input[id*='hdfIdProducto']");

            if (hdfIdProducto.val() > 0) {
                var newOption = new Option(hdfProductoDetalle.val(), hdfIdProducto.val(), false, true);
                ddlProducto.append(newOption).trigger('change');
            }

            ddlProducto.select2({
                placeholder: 'Ingrese el codigo o producto',
                selectOnClose: true,
                theme: 'bootstrap4',
                width: '100%',
                //theme: 'bootstrap',
                minimumInputLength: 1,
                language: 'es',
                //tags: true,
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/CMPProductosSeleccionarFiltro', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: ddlProducto.val(), // search term");
                            filtro: params.term, // search term");
                            proveedor: 0,
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
                                    id: item.IdProducto,
                                    productoDescripcion: item.Descripcion,
                                    stockActual: item.StockActual,
                                    precio: item.Precio,
                                    //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            ddlProducto.on('select2:select', function (e) {
                hdfProductoDetalle.val(e.params.data.productoDescripcion);
                hdfIdProducto.val(e.params.data.id);
            });
            ddlProducto.on('select2:unselect', function (e) {
                hdfProductoDetalle.val('');
                hdfIdProducto.val('');
            });
        }
    </script>

    <div class="ReservasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfIdAfiliadoTipo" runat="server" />
                        <asp:HiddenField ID="hdfApellido" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvApellido" ControlToValidate="ddlApellido" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                    </div>

                    <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Producto"></asp:Label>
                    <div class="col-sm-3">
                        <itemtemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="True"></asp:DropDownList>
                            <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                            <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                            <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                        </itemtemplate>
                    </div>

                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>

                </div>

                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                    runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                <br />

                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdListaEspera"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="# Lista de espera" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("IdListaEspera")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cliente" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("ProductoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad de personas" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("Cantidad")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
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
