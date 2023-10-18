<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListaEsperaDatos.ascx.cs" Inherits="IU.Modulos.Hotel.Controles.ListaEsperaDatos" %>

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

<div id="deshabilitarControles">
    <asp:UpdatePanel ID="upDatos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-header">
                    Datos de la Reserva
                </div>
                <div class="card-body">
                    <asp:HiddenField ID="hdfOpcionAbierta" runat="server" />
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Cliente"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" runat="server"></asp:DropDownList>
                            <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                            <asp:HiddenField ID="hdfIdAfiliadoTipo" runat="server" />
                            <asp:HiddenField ID="hdfApellido" runat="server" />
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvApellido" ControlToValidate="ddlApellido" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvNumeroDocumento" ControlToValidate="txtNumeroDocumento" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Producto"></asp:Label>
                        <div class="col-sm-3">
                            <itemtemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="True"></asp:DropDownList>
                                <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvProducto" ControlToValidate="ddlProducto" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                            </itemtemplate>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad de personas"></asp:Label>
                        <div class="col-sm-3">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" runat="server" Text="" Prefix="" NumberOfDecimals="2" AllowNegative="false"></Evol:CurrencyTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvCantidad" ControlToValidate="txtCantidad" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <center>
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" CausesValidation="false" />
        </center>
    </ContentTemplate>
</asp:UpdatePanel>
