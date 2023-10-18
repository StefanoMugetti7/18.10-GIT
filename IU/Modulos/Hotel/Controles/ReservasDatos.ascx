<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReservasDatos.ascx.cs" Inherits="IU.Modulos.Hotel.Controles.ReservasDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Hotel/Controles/ReservasDetallesOpciones.ascx" TagName="ReservasDetallesOpciones" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<%--<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>--%>
<%--<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js"></script>--%>
<%--<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/bootstrap-datetimepicker/js/locales/bootstrap-datetimepicker.es.js"></script>--%>
<script type="text/javascript">
    var txtReservaFechaIngreso, txtReservaFechaEgreso;

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridOcupantes);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitSelect2CargaMasiva);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DeshabilitarEnter);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridOcupantesHabitaciones);
        SetTabIndexInput();
        IniciarAfiliadosWS();
        InitControls();
        InitSelect2();
        InitApellidoSelect2();
        intiGridDetalle();
        intiGridOcupantes();
        InitSelect2CargaMasiva();
        //DeshabilitarEnter();
        //intiGridOcupantesHabitaciones();
    });

    //function DeshabilitarEnter() {
    //    $(document).on('keypress', function (e) {
    //        var keyCode = e.keyCode || e.which;
    //        alert(keyCode);
    //        if (keyCode === 13) {
    //            e.preventDefault();
    //            return false;
    //        }
    //    });
    //}

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function IniciarAfiliadosWS() {
        $.ajax({
            url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/IniciarWS',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (response) {
                //do whatever your thingy..
            }
        });
    }

    function InitControls() {
        txtReservaFechaIngreso = $('input:text[id$=txtReservaFechaIngreso]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
        });
        txtReservaFechaIngreso.change(OnSelectedReservaFechaIngreso);
        txtReservaFechaIngreso.change(CargarListasPrecio);
        txtReservaFechaEgreso = $('input:text[id$=txtReservaFechaEgreso]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
            change: OnSelectedReservaFechaEgreso,
        });
        //txtReservaFechaEgreso.change(OnSelectedReservaFechaEgreso);
        var txtCantidadDias = $("input:text[id$='txtCantidadDias']")
        txtCantidadDias.blur(CalcularFechaEgreso);
        var ddlHoteles = $("select[name$='ddlHoteles']");
        ddlHoteles.on('select2:select', CargarListasPrecio);
    }

    function InitReservaFechaEgreso(minDate, val) {
        //alert(val);

        txtReservaFechaEgreso.destroy();
        txtReservaFechaEgreso = $('input:text[id$=txtReservaFechaEgreso]').datepicker({
            showOnFocus: true,
            uiLibrary: 'bootstrap4',
            locale: 'es-es',
            format: 'dd/mm/yyyy',
            minDate: minDate,
            value: val,
        });
        txtReservaFechaEgreso.change(OnSelectedReservaFechaEgreso);
        intiGridDetalle();
    }

    function CalcularFechaEgreso() {
        //if (isNaN(txtReservaFechaEgreso.value()))
        //    return;
        var backReservaFechaEgreso = txtReservaFechaEgreso.value();
        var txtCantidadDias = $("input:text[id$='txtCantidadDias']")
        var cantidadDias = txtCantidadDias.val();

        var egreso = new Date(toDate(txtReservaFechaIngreso.value()));
        egreso.setDate(egreso.getDate() + parseInt(cantidadDias));
        InitReservaFechaEgreso(egreso, toStrDate(egreso));

        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlTipoProducto = $(this).find('[id*="ddlTiposProductosHoteles"]');
            var txtfechaIngreso = $(this).find("input:text[id*='txtFechaIngreso']").datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
                minDate: toDate(txtReservaFechaIngreso.value()),
                showRightIcon: false
            });

            var txtfechaEgreso = $(this).find("input:text[id*='txtFechaEgreso']").datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
                minDate: toDate(txtReservaFechaIngreso.value()),
                maxDate: egreso,
                showRightIcon: false
            });

            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");

            if ((ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH01']").val()
                || ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH02']").val()
                || ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH03']").val()
                || ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH05']").val()
            )
                && txtfechaIngreso.val() != '' && txtfechaEgreso.val() != '') {
                if (txtReservaFechaIngreso.value() == txtfechaIngreso.value()
                    && backReservaFechaEgreso == txtfechaEgreso.value()) {
                    // txtCantidad.val(parseInt(cantidadDias));
                    txtCantidad.val(accounting.formatMoney(Math.round(cantidadDias), "", 2, "."));

                    //detBIDceFechaEgreso.set_endDate(ceFechaEgreso.get_selectedDate())
                    //txtfechaEgreso.val(txtReservaFechaEgreso.val());
                    txtfechaEgreso.destroy();
                    txtfechaEgreso = $(this).find("input:text[id*='txtFechaEgreso']").datepicker({
                        showOnFocus: true,
                        uiLibrary: 'bootstrap4',
                        locale: 'es-es',
                        format: 'dd/mm/yyyy',
                        minDate: egreso,
                        value: toStrDate(egreso),
                        showRightIcon: false
                    });
                }
            }
        });
        var ddlTiposDescuentos = $('#<%=gvDescuentos.ClientID%> tr').find('[id*="ddlTiposDescuentos"]');
        if (ddlTiposDescuentos.val() > 0) {
            $('#<%=gvDescuentos.ClientID%> tr').not(':first').not(':last').each(function () {
                var gvd_hdfCantidad = $(this).find("input[id*='hdf_gvd_Cantidad']");
                gvd_hdfCantidad.val(txtCantidadDias.val());
            });
        }
        CalcularPrecio();
    }
    function OnSelectedReservaFechaIngreso() {
        var egreso = new Date(toDate(txtReservaFechaIngreso.value()));
        var txtCantidadDias = $("input:text[id$='txtCantidadDias']");
        var cantidadDias = txtCantidadDias.val() == '' ? 1 : txtCantidadDias.val();
        $("input:text[id$='txtCantidadDias']").val(cantidadDias);
        CalcularFechaEgreso();
        //egreso.setDate(egreso.getDate() + parseInt(cantidadDias));
        //InitReservaFechaEgreso(egreso, toStrDate(egreso));
        //txtCantidadDias.focus().select();
    }

    function OnSelectedReservaFechaEgreso(sender, args) {
        if (isNaN(txtReservaFechaEgreso.value())) {
            return;
        }
        var ingreso = new Date(toDate(txtReservaFechaIngreso.value()));
        var egreso = new Date(toDate(txtReservaFechaEgreso.value()));
        var diff = new Date(egreso - ingreso);
        var days = diff / 1000 / 60 / 60 / 24;
        $("input:text[id$='txtCantidadDias']").val(Math.round(days));
        intiGridDetalle();
    }

    function InitSelect2() {
        var ddlTipoDocumento = $("select[name$='ddlReservaTipoDocumento']");
        ddlTipoDocumento.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
        var ddlHoraIngreso = $("select[name$='ddlHoraIngreso']");
        ddlHoraIngreso.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
        var ddlHoraEgreso = $("select[name$='ddlHoraEgreso']");
        ddlHoraEgreso.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
        var ddlListasPrecios = $("select[name$='ddlListasPrecios']");
        ddlListasPrecios.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
    }

    function InitSelect2CargaMasiva() {
        var ddlCargaMasivaProductos = $("select[name$='ddlCargaMasivaProductos']");
        ddlCargaMasivaProductos.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
    }

    function CargarListasPrecio() {
        var ddlHoteles = $("select[name$='ddlHoteles']");
        var txtReservaFechaIngreso = $("input:text[id$='txtReservaFechaIngreso']")
        $.ajax({
            type: "POST",
            url: '<%=ResolveClientUrl("~")%>/Modulos/Hotel/HotelWS.asmx/ListasPreciosHotelesCombo',
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ 'idHotel': ddlHoteles.val(), 'fecha': txtReservaFechaIngreso.val() }),
            beforeSend: function (xhr, opts) {
                if (txtReservaFechaIngreso.val() == '' || ddlHoteles.val() == '') {
                    $("select[name$='ddlListasPrecios']").empty().append($("<option></option>").val("").html("Seleccione una opción"));
                    xhr.abort();
                }
            },
            success: function (res) {
                $.each(res.d, function (data, value) {
                    $("select[name$='ddlListasPrecios']").empty().append($("<option></option>").val(value.id).html(value.text));
                })
            }
        });
    }

    function HotelesHorarios() {
        var ddlHoteles = $("select[name$='ddlHoteles']")
        $.ajax({
            type: "POST",
            url: '<%=ResolveClientUrl("~")%>/Modulos/Hotel/HotelWS.asmx/FechasHotelesReservas',
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ 'idHotel': ddlHoteles.val() }),
            beforeSend: function (xhr, opts) {

            },
            success: function (res) {
                //  $("input:text[id$='ddlHoraIngreso']").val(res.d.HorarioIngreso);
                if (res.d.HoraIngresoStr != null) {
                    $("select[name$='ddlHoraIngreso']").val(res.d.HoraIngresoStr).trigger('change');
                    $("select[name$='ddlHoraEgreso']").val(res.d.HoraEgresoStr).trigger('change');
                }
                else {
                    $("select[name$='ddlHoraIngreso']").val('10:00').trigger('change');
                    $("select[name$='ddlHoraEgreso']").val('15:00').trigger('change');
                }
            }
        });
    }

    function InitApellidoSelect2() {
        var control = $("select[name$='ddlReservaApellido']");
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
                $("select[id$='ddlReservaApellido']").append(newOption).trigger('change');
                $("select[id$='ddlReservaTipoDocumento']").val(e.params.data.IdTipoDocumento).trigger('change');
                $("input[type=text][id$='txtReservaNumeroDocumento']").val(e.params.data.NumeroDocumento);
                $("input[type=text][id$='txtReservaNombre']").val(e.params.data.Nombre);
                $("input[id*='hdfReservaIdAfiliado']").val(e.params.data.id);
                $("input[id*='hdfReservaIdAfiliadoTipo']").val(e.params.data.IdAfiliadoTipo);
                $("input[id*='hdfReservaApellido']").val(e.params.data.Apellido);
                $("select[id$='ddlCondicionFiscal']").val(e.params.data.IdCondicionFiscal).trigger('change');
                $("select[id$='ddlReservaTipoDocumento']").prop("disabled", true);
                $("input[type=text][id$='txtReservaNumeroDocumento']").prop("disabled", true);
                $("input[type=text][id$='txtReservaNombre']").prop("disabled", true);
                $("input[type=text][id$='txtEstadoSocio']").val(e.params.data.estadoDescripcion);
                $("input[type=text][id$='txtCategoria']").val(e.params.data.categoriaDescripcion);
                $("input[type=text][id$='txtReservaCorreoElectronico']").val(e.params.data.Correo);
                $("input[type=text][id$='txtReservaCorreoElectronico']").prop("disabled", true);
            }
            else {
                $("input[id*='hdfReservaApellido']").val(e.params.data.text);
                $("select[id$='ddlReservaTipoDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtReservaNumeroDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtReservaNombre']").prop("disabled", false);
            }
        });

        control.on('select2:unselect', function (e) {
            if ($.isNumeric(e.params.data.id)) {
                $("select[id$='ddlReservaTipoDocumento'] option:selected").val('');
                $("input[type=text][id$='txtReservaNumeroDocumento']").val('');
                $("input[type=text][id$='txtReservaNombre']").val('');
                $("input[id*='hdfReservaIdAfiliado']").val('');
                $("input[id*='hdfReservaIdAfiliadoTipo']").val('');
                $("input[type=text][id$='txtEstadoSocio']").val('');
                $("input[type=text][id$='txtCategoria']").val('');
                $("select[id$='ddlReservaTipoDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtReservaNumeroDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtReservaNombre']").prop("disabled", false);
                $("input[type=text][id$='txtReservaCorreoElectronico']").val('');
                $("input[type=text][id$='txtReservaCorreoElectronico']").prop("disabled", false);
            }
            control.val(null).trigger('change');
        });

        control.on('select2:clear', function (e) {
            $("select[id$='ddlReservaTipoDocumento']").val(e.params.data.IdTipoDocumento).trigger('change');
        });
    }


    /******************************************************
        Grilla Detalle Reserva
    *******************************************************/
    function intiGridDetalle() {
        //var txtReservaFechaIngreso = $("input[id$='txtReservaFechaIngreso']");
        //var txtReservaFechaEgreso = $("input[id$='txtReservaFechaEgreso']");
        //var ceFechaIngreso = $find("BIDceReservaFechaIngreso");
        //var ceFechaEgreso = $find("BIDceReservaFechaEgreso");
        var ddlHoteles = $("select[id$='ddlHoteles']");
        var ddlListasPrecios = $("select[name$='ddlListasPrecios']");
        var hdfReservaIdAfiliado = $("input[id$='hdfReservaIdAfiliado']");

        var rowindex = 0;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlTipoProducto = $(this).find('[id*="ddlTiposProductosHoteles"]');
            var txtfechaIngreso = $(this).find("input:text[id*='txtFechaIngreso']");
            var txtfechaEgreso = $(this).find("input:text[id*='txtFechaEgreso']");
            var ddlDetalleGastos = $(this).find('[id*="ddlDetalleGastos"]');
            var hdfDetalleGastos = $(this).find("input[id*='hdfDetalleGastos']");
            var hdfIdHabitacion = $(this).find("input[id*='hdfIdHabitacion']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtPrecio = $(this).find("input:text[id*='txtPrecio']");//.maskMoney('unmasked')[0];
            var hdfPrecioLista = $(this).find("input[id*='hdfPrecioLista']");
            var hdfPrecioHabitacionCompartida = $(this).find("input[id*='hdfPrecioHabitacionCompartida']");
            var descuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]')
            //var detBIDceFechaIngreso = $find("DetBIDceFechaIngreso_" + rowindex);
            //var detBIDceFechaEgreso = $find("DetBIDceFechaEgreso_" + rowindex);
            var hdfIdListaPrecioDetalle = $(this).find("input[id*='hdfIdListaPrecioDetalle']");
            var hdfPrecioEditable = $(this).find("input[id*='hdfPrecioEditable']");
            var chkCompartida = $(this).find("input:checkbox[id*='chkCompartida']");
            var ddlMoviliario = $(this).find('[id*="ddlMoviliario"]');
            var txtDescuentoPorcentual = $(this).find('[id*="txtDescuentoPorcentual"]');
            var lblDescuentoImporte = $(this).find('[id*="lblDescuentoImporte"]');
            var hdfDescuentoImporte = $(this).find('[id*="hdfDescuentoImporte"]');
            var hdfCantidadPersonas = $(this).find('[id*="hdfCantidadPersonas"]');
            var hdfCantidadPersonasOpciones = $(this).find('[id*="hdfCantidadPersonasOpciones"]');
            var hdfIdHabitacionDetalle = $(this).find('[id*="hdfIdHabitacionDetalle"]');
            var hdfLateCheckOut = $(this).find('[id*="hdfLateCheckOut"]');
            var lblDetalleOpciones = $(this).find('[id*="lblDetalleOpciones"]');



            txtfechaIngreso.datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
                //minDate:toDate(txtReservaFechaIngreso.value()),

                showRightIcon: false
            });

            txtfechaEgreso.change(function () {
                var ingreso = new Date(toDate(txtfechaIngreso.val()));
                var egreso = new Date(toDate(txtfechaEgreso.val()));
                var days = (egreso - ingreso) / (1000 * 60 * 60 * 24);
                //var row = $(this).closest("tr");
                //var txtCantidad = row.find("input:text[id*='txtCantidad']");
                var cantidad = txtCantidad.maskMoney('unmasked')[0];
                if (parseInt(days) != parseInt(cantidad)) {

                    txtCantidad.val(accounting.formatMoney(Math.round(days), "", 2, "."));
                    CalcularPrecio();
                }
            });
            txtfechaEgreso.datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
                //minDate: toDate(txtReservaFechaIngreso.value()),
                //maxDate: toDate(txtReservaFechaEgreso.value()),
                showRightIcon: false
            });

            if (new Date(toDate(txtfechaIngreso.value())) < new Date(toDate(txtReservaFechaIngreso.value())))
                txtfechaIngreso.val(txtReservaFechaIngreso.value());
            if (new Date(toDate(txtfechaIngreso.value())) > new Date(toDate(txtReservaFechaEgreso.value())))
                txtfechaIngreso.val(txtReservaFechaIngreso.value());

            if (new Date(toDate(txtfechaEgreso.val())) > new Date(toDate(txtReservaFechaEgreso.value())))
                txtfechaEgreso.val(txtReservaFechaEgreso.value());
            if (new Date(toDate(txtfechaEgreso.val())) < new Date(toDate(txtReservaFechaIngreso.value())))
                txtfechaEgreso.val(txtReservaFechaEgreso.value());

            //detBIDceFechaIngreso.set_startDate(ceFechaIngreso.get_selectedDate())
            //detBIDceFechaIngreso.set_endDate(ceFechaEgreso.get_selectedDate())
            //detBIDceFechaEgreso.set_startDate(ceFechaIngreso.get_selectedDate())
            //detBIDceFechaEgreso.set_endDate(ceFechaEgreso.get_selectedDate())

            txtCantidad.blur(CalcularPrecio);
            txtPrecio.blur(CalcularPrecio);
            descuentoPorcentaje.blur(CalcularPrecio);
            ddlTipoProducto.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
            ddlTipoProducto.change(function () {
                if ($(this).val() == $("input[id$='hdfCodigoTPH01']").val()
                    || $(this).val() == $("input[id$='hdfCodigoTPH02']").val()
                    || $(this).val() == $("input[id$='hdfCodigoTPH03']").val()
                    || $(this).val() == $("input[id$='hdfCodigoTPH05']").val()
                ) {

                    var ingreso = new Date(toDate(txtfechaIngreso.val()));
                    var egreso = new Date(toDate(txtfechaEgreso.val()));
                    var days = (egreso - ingreso) / (1000 * 60 * 60 * 24);

                    if (txtfechaIngreso.val() == '') txtfechaIngreso.val(txtReservaFechaIngreso.val());
                    if (txtfechaEgreso.val() == '') txtfechaEgreso.val(txtReservaFechaEgreso.val());
                    txtCantidad.val(accounting.formatMoney(Math.round(days), "", 2, "."));
                    txtfechaIngreso.prop("disabled", false);
                    txtfechaEgreso.prop("disabled", false);

                }
                else {
                    var d = new Date();
                    txtfechaIngreso.val(toStrDate(d));
                    txtfechaEgreso.val('');
                    txtfechaIngreso.prop("disabled", true);
                    txtfechaEgreso.prop("disabled", true);
                }
                ddlDetalleGastos.focus();
            });

            ddlDetalleGastos.select2({
                placeholder: 'Ingrese la Habitacion o gasto',
                selectOnClose: true,
                theme: 'bootstrap4',
                width: '100%',
                //theme: 'bootstrap',
                minimumInputLength: 4,
                language: 'es',
                //tags: true,
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Hotel/HotelWS.asmx/ReservasSeleccionarAjaxComboDetalleGastos', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        var filtro = {};
                        filtro.IdTipoProductoHotel = ddlTipoProducto.val();
                        filtro.Filtro = params.term;
                        filtro.FechaIngreso = new Date(toDate(txtfechaIngreso.val()));
                        if (txtfechaEgreso.val() != '') {
                            filtro.FechaEgreso = new Date(toDate(txtfechaEgreso.val()));
                        }
                        filtro.IdHotel = ddlHoteles.val();
                        filtro.IdListaPrecio = ddlListasPrecios.val();
                        filtro.Compartida = chkCompartida.prop('checked');
                        //filtro.IdAfiliado = hdfReservaIdAfiliado.val();
                        var gastosSeleccionados = ObtenerHabitacionesSeleccionadas();
                        //var camposValores = ObtenerCamposValores();

                        return "{filtro:" + JSON.stringify(filtro) + ", gastosSeleccionados:" + JSON.stringify(gastosSeleccionados) + "}"; //", camposValores:" + JSON.stringify(camposValores) + "}";
                    },
                    beforeSend: function (xhr, opts) {
                        if (txtReservaFechaIngreso.val() == '' || txtReservaFechaEgreso.val() == '') {
                            MostrarMensaje('Debe Ingresar una Fecha de Ingreso y una Fecha de Egreso de la Reserva', 'red');
                            xhr.abort();
                        }
                        if ((ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH01']").val()
                            || ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH02']").val()
                            || ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH03']").val()
                            || ddlTipoProducto.val() == $("input[id$='hdfCodigoTPH05']").val())
                            && (txtfechaIngreso.val() == '' || txtfechaEgreso.val() == '')
                        ) {
                            MostrarMensaje('Debe Ingresar una Fecha de Ingreso y una Fecha de Egreso', 'red');
                            xhr.abort();
                        }
                        if (ddlHoteles.val() == '') {
                            MostrarMensaje('Debe seleccionar un Hotel para la Reserva', 'red');
                            xhr.abort();
                        }
                        if (ddlListasPrecios.val() == '') {
                            MostrarMensaje('Debe seleccionar una Tarifa para la Reserva', 'red');
                            xhr.abort();
                        }
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.Detalle,
                                    id: item.IdProducto,
                                    idHabitacion: item.IdHabitacion,
                                    precio: item.Precio,
                                    idListaPrecioDetalle: item.IdListaPrecioDetalle,
                                    precioEditable: item.PrecioEditable,
                                    compartida: item.Compartida,
                                    precioHabitacionCompartida: item.PrecioHabitacionCompartida,
                                    cantidadPersonas: item.CantidadPersonas
                                }
                            })
                        };
                        cache: true
                    }
                }
            });

            ddlDetalleGastos.on('select2:select', function (e) {
                if (e.params.data.idListaPrecioDetalle > 0) {
                    hdfDetalleGastos.val(e.params.data.text);
                    hdfIdHabitacion.val(e.params.data.idHabitacion);
                    hdfPrecioLista.val(e.params.data.precio);
                    hdfPrecioHabitacionCompartida.val(e.params.data.precioHabitacionCompartida);
                    txtPrecio.val(accounting.formatMoney(e.params.data.precio, "$ ", 2, "."));
                    hdfIdListaPrecioDetalle.val(e.params.data.idListaPrecioDetalle);
                    hdfPrecioEditable.val(e.params.data.precioEditable);
                    hdfCantidadPersonas.val(e.params.data.cantidadPersonas);

                    if (e.params.data.compartida) {
                        chkCompartida.prop('checked', true);
                        chkCompartida.trigger("change");
                        chkCompartida.prop("disabled", true);
                        txtPrecio.focus();
                    }
                    else {
                        chkCompartida.prop('checked', false);
                        chkCompartida.prop("disabled", false);
                        chkCompartida.focus();
                    }
                    ddlHoteles.prop("disabled", false);
                } else {
                    MostrarMensaje(e.params.data.text + ' no se encuentra asociado a la Lista de Precios. Agregue el Producto a la Lista de Precios para poder seleccionarlo.', 'red');
                    ddlDetalleGastos.val(null).trigger('change');
                }

                CalcularPrecio();
            });
            ddlDetalleGastos.on('select2:unselect', function (e) {
                hdfDetalleGastos.val('');
                hdfIdHabitacion.val('');
                hdfIdListaPrecioDetalle.val('');
                hdfPrecioEditable.val('');
                lblDescuentoImporte.text(accounting.formatMoney(0, "$ ", 2, "."));
                hdfDescuentoImporte.val(0);
                hdfPrecioLista.val(0);
                hdfPrecioHabitacionCompartida.val(0);
                txtPrecio.val(accounting.formatMoney(0, "$ ", 2, "."));

                chkCompartida.disabled = true;
                hdfCantidadPersonas.val(0);
                hdfCantidadPersonasOpciones.val(0);
                hdfIdHabitacionDetalle.val('');
                hdfLateCheckOut.val('');
                lblDetalleOpciones.text('');
                CalcularPrecio();
            });

            <%--ddlMoviliario.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es', placeholder: 'Seleccione una cama' });
            chkCompartida.change(function () {
                ddlMoviliario.empty();
                txtPrecio.val(accounting.formatMoney(hdfPrecioLista.val().replace(',', '.'), "$ ", 2, "."));
                if (chkCompartida.prop('checked') == true) {
                    txtPrecio.val(accounting.formatMoney(hdfPrecioHabitacionCompartida.val().replace(',', '.'), "$ ", 2, "."));
                    $.ajax({
                        type: "POST",
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Hotel/HotelWS.asmx/HabitacionesMoviliarioDisponible',
                        dataType: "json",
                        contentType: "application/json",
                        data: JSON.stringify({ 'idHabitacion': hdfIdHabitacion.val(), 'fechaIngreso': txtfechaIngreso.val(), 'fechaEgreso': txtfechaEgreso.val() }),
                        success: function (res) {
                            ddlMoviliario.empty();
                            $.each(res.d, function (data, value) {
                                ddlMoviliario.append($("<option></option>").val(value.id).html(value.text));
                            })
                        }
                    });
                }
                CalcularPrecio();
            });--%>

            rowindex++;
        });
    }

    function ObtenerHabitacionesSeleccionadas() {
        var items = [];
        var gasto;
        var habitacionDetalle;

        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlTipoProducto = $(this).find('[id*="ddlTiposProductosHoteles"]');
            var txtfechaIngreso = $(this).find("input:text[id*='txtFechaIngreso']");
            var txtfechaEgreso = $(this).find("input:text[id*='txtFechaEgreso']");
            var ddlDetalleGastos = $(this).find('[id*="ddlDetalleGastos"]');
            var hdfDetalleGastos = $(this).find("input[id*='hdfDetalleGastos']");
            var hdfIdHabitacion = $(this).find("input[id*='hdfIdHabitacion']");
            var chkCompartida = $(this).find("input:checkbox[id*='chkCompartida']");
            var ddlMoviliario = $(this).find('[id*="ddlMoviliario"]');

            gasto = {};
            if (hdfIdHabitacion.val() != '') {
                gasto.IdHabitacion = hdfIdHabitacion.val();
                gasto.FechaIngreso = new Date(toDate(txtfechaEgreso.val()));
                gasto.FechaEgreso = new Date(toDate(txtfechaEgreso.val()));
                gasto.Compartida = chkCompartida.prop('checked');
                gasto.Detalle = hdfDetalleGastos.val();
                habitacionDetalle = {};
                habitacionDetalle.IdHabitacionDetalle = ddlMoviliario.val();
                gasto.HabitacionDetalle = habitacionDetalle;
                items.push(gasto);
            }
        });
        return items;
    }

    function ObtenerFechaIngreso(IdHabitacion) {
        var resultado = new Date();
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var txtfechaIngreso = $(this).find("input:text[id*='txtFechaIngreso']");
            var hdfIdHabitacion = $(this).find("input[id*='hdfIdHabitacion']");

            if (hdfIdHabitacion.val() == IdHabitacion) {
                resultado = new Date(toDate(txtfechaIngreso.val()));
                return false;
            }
        });
        return resultado;
    }


    //function ObtenerCamposValores() {
    //    var camposValores = [];
    //    var campoValor;
    //    var campo;

    //    var chkDescuentoPorDistancia = $("input[id$='|DescuentoPorDistancia']");
    //    campoValor = {};
    //    campo = {};
    //    campo.Nombre = "DescuentoPorDistancia";
    //    campoValor.Campo = campo;
    //    campoValor.Valor = chkDescuentoPorDistancia.length > 0 ? chkDescuentoPorDistancia.prop('checked') : "";
    //    camposValores.push(campoValor);

    //    return camposValores;
    //}

    function round(value, exp) {
        if (typeof exp === 'undefined' || +exp === 0)
            return Math.round(value);

        value = +value;
        exp = +exp;

        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0))
            return NaN;

        // Shift
        value = value.toString().split('e');
        value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp)));

        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp));
    }

    function CalcularPrecio() {
        var subTotal = 0.00;
        var total = 0.00;
        var descuento = 0.00;
        var descuentoGrilla = 0.00;
        var cantidadPersonas = 0;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlTipoProducto = $(this).find('[id*="ddlTiposProductosHoteles"]');
            var ddlDetalleGastos = $(this).find('[id*="ddlDetalleGastos"]');
            var txtfechaIngreso = $(this).find("input:text[id*='txtFechaIngreso']");
            var txtfechaEgreso = $(this).find("input:text[id*='txtFechaEgreso']");
            var chkCompartida = $(this).find("input:checkbox[id*='chkCompartida']");
            var ddlMoviliario = $(this).find('[id*="ddlMoviliario"]');
            var hdfIdHabitacion = $(this).find("input[id*='hdfIdHabitacion']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtPrecio = $(this).find("input:text[id*='txtPrecio']");
            var lblSubTotal = $(this).find("span[id*='lblSubTotal']");
            var hdfSubTotal = $(this).find("input[id*='hdfSubTotal']");
            var hdfCantidadPersonas = $(this).find('[id*="hdfCantidadPersonas"]');
            var habilitarDescuento = true;
            subTotal = 0.00;
            var cantidad = txtCantidad.maskMoney('unmasked')[0];
            //var cantidad = txtCantidad.val();


            if ($(this).find('[id*="ddlTiposProductosHoteles"]').val() == $("input[id$='hdfCodigoTPH01']").val()
                || $(this).find('[id*="ddlTiposProductosHoteles"]').val() == $("input[id$='hdfCodigoTPH02']").val()
                || $(this).find('[id*="ddlTiposProductosHoteles"]').val() == $("input[id$='hdfCodigoTPH03']").val()
                || $(this).find('[id*="ddlTiposProductosHoteles"]').val() == $("input[id$='hdfCodigoTPH05']").val()
            ) {
                var txtfechaIngreso = $(this).find("input:text[id*='txtFechaIngreso']");
                var txtfechaEgreso = $(this).find("input:text[id*='txtFechaEgreso']");
                var ingreso = new Date(toDate(txtfechaIngreso.val()));
                var egreso = new Date(toDate(txtfechaEgreso.val()));
                var days = (egreso - ingreso) / (1000 * 60 * 60 * 24);

                // if (parseInt(cantidad) == 0) {
                if (cantidad == 0) {

                    txtCantidad.val(accounting.formatMoney(Math.round(days), "", 2, "."));
                }
                // if (parseInt(cantidad) > days) {
                if (cantidad > days) {
                    txtCantidad.val(accounting.formatMoney(Math.round(cantidad), "", 2, "."));
                    if (ingreso.getDate() + cantidad > egreso.getDate()) {
                        egreso.setDate(ingreso.getDate() + cantidad);
                        txtfechaEgreso.val(toStrDate(egreso));
                        /*Seteo Fecha Egreso Reserva y Cantidad Reserva*/
                        var txtfechaEgresoReserva = $("input:text[id*='txtReservaFechaEgreso']");
                        var egresoReserva = new Date(toDate(txtfechaEgresoReserva.val()))
                        if (egreso > egresoReserva) {
                            var txtfechaIngresoReserva = $(this).find("input:text[id*='txtFechaIngreso']");
                            var ingresoReserva = new Date(toDate(txtfechaIngresoReserva.val()));
                            var daysDiff = Math.round((egreso.getTime() - egresoReserva.getTime()) / (1000 * 60 * 60 * 24));
                            var txtCantidadReserva = $("input:text[id$='txtCantidadDias']");
                            txtCantidadReserva.val(parseInt(txtCantidadReserva.val()) + parseInt(daysDiff));
                            CalcularFechaEgreso();
                        }
                    }
                }
            }

            var precio = txtPrecio.maskMoney('unmasked')[0];
            var txtDescuentoPorcentual = $(this).find('[id*="txtDescuentoPorcentual"]');
            var descuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]').maskMoney('unmasked')[0];
            if (descuentoPorcentaje > 100) {
                descuentoPorcentaje = 100;
                txtDescuentoPorcentual.val(accounting.formatMoney(descuentoPorcentaje, "", 2, "."));
            }

            if (precio && cantidad) {
                descuentoGrilla = 0.00;
                $('#<%=gvDescuentos.ClientID%> tr').not(':first').not(':last').each(function () {
                    var gvd_hdlHabitaciones = $(this).find('select[id*="ddlHabitaciones"]');
                    var gvd_hdfPorcentajeImporte = $(this).find('input[id*="hdf_gvd_PorcentajeImporte"]');
                    var gvd_hdfDescuentoImporte = $(this).find('input[id*="hdf_gvd_DescuentoImporte"]');
                    var gvd_hdfPrecioBase = $(this).find('input[id*="hdf_gvd_PrecioBase"]');
                    var hdf_gvd_Cantidad = $(this).find('input[id*="hdf_gvd_Cantidad"]');
                    var hdf_gvd_CantidadDisponible = $(this).find('input[id*="hdf_gvd_CantidadDisponible"]');
                    var gvd_lblCantidad = $(this).find('[id*="gvd_lblCantidad"]');
                    var gvd_hdfBaseCalculoImporte = $(this).find('input[id*="hdf_gvd_BaseCalculoImporte"]');
                    var gvd_lblBaseCalculoImporte = $(this).find('[id*="gvd_lblBaseCalculoImporte"]');
                    var gvd_hdflblSubTotal = $(this).find('input[id*="hdf_gvd_lblSubTotal"]');
                    var gvd_lblSubTotal = $(this).find('[id*="gvd_lblSubTotal"]');
                    var calcularDescuento = 0.00;
                    var precioCalculo = 0.00;
                    var precioBase = gvd_hdfPrecioBase.length > 0 ? parseFloat(gvd_hdfPrecioBase.val()) : 0.00;
                    var subTotalItem = 0;
                    var baseCalculoImporte = 0.00;
                    var cantidadDescuento = 0;
                    if (parseFloat(hdf_gvd_CantidadDisponible.val()) > parseFloat(cantidad)) {
                        cantidadDescuento = parseFloat(cantidad);
                    } else {
                        cantidadDescuento = parseFloat(hdf_gvd_CantidadDisponible.val());
                    }

                    if (hdfIdHabitacion.val() == gvd_hdlHabitaciones.val()) {
                        txtDescuentoPorcentual.val(accounting.formatMoney(0, "", 2, "."));
                        txtDescuentoPorcentual.prop("disabled", true);
                        habilitarDescuento = false;
                        precioCalculo = parseFloat(precioBase) > 0 ? parseFloat(precioBase) : parseFloat(precio);
                        baseCalculoImporte = parseFloat(precioCalculo);
                        calcularDescuento = parseFloat(round(parseFloat(parseFloat(baseCalculoImporte) * parseFloat(gvd_hdfPorcentajeImporte.val()) / 100), 2) * parseFloat(cantidadDescuento));
                        /* hasta aca llegue */
                        subTotalItem = parseFloat(calcularDescuento);
                        subTotalItem += parseFloat(parseFloat(gvd_hdfDescuentoImporte.val()) * parseFloat(cantidadDescuento));
                        descuentoGrilla += parseFloat(subTotalItem);
                        gvd_hdfBaseCalculoImporte.val(baseCalculoImporte);

                        gvd_lblCantidad.text(accounting.formatMoney(cantidadDescuento, "", 2, "."))
                        gvd_lblBaseCalculoImporte.text(accounting.formatMoney(precioCalculo, gblSimbolo, 2, "."));
                        gvd_hdflblSubTotal.val(subTotalItem);
                        gvd_lblSubTotal.text(accounting.formatMoney(subTotalItem, gblSimbolo, 2, "."));
                        hdf_gvd_Cantidad.val(cantidadDescuento);
                        ddlTipoProducto.prop("disabled", true);
                        ddlDetalleGastos.prop("disabled", true);
                        txtfechaIngreso.prop("disabled", true);
                        txtfechaEgreso.prop("disabled", true);
                        chkCompartida.prop("disabled", true);
                        ddlMoviliario.prop("disabled", true);
                        //txtCantidad.prop("disabled", true);
                        txtPrecio.prop("disabled", true);
                    }
                });
                if (habilitarDescuento) {
                    descuento = parseFloat(parseFloat(precio) * parseFloat(cantidad) * parseFloat(descuentoPorcentaje) / 100);
                    descuento = round(descuento, 2); // Number(Math.round(descuento + 'e2') + 'e-2');
                } else {
                    descuento = parseFloat(descuentoGrilla);
                }

                $(this).find('span[id*="lblDescuentoImporte"]').text(accounting.formatMoney(descuento, gblSimbolo, 2, "."));
                $(this).find('input:hidden[id*="hdfDescuentoImporte"]').val(descuento);
                subTotal = parseFloat(precio) * parseFloat(cantidad) - parseFloat(descuento);
                total += parseFloat(subTotal);
                hdfSubTotal.val(subTotal);

            }
            cantidadPersonas += parseFloat(hdfCantidadPersonas.val());
            lblSubTotal.text(accounting.formatMoney(subTotal, gblSimbolo, 2, "."));
        });
        $("#<%=gvDatos.ClientID %> [id$=lblTotal]").text(accounting.formatMoney(total, gblSimbolo, 2, "."));
        $("#<%=gvDatos.ClientID %> [id$=hdfTotal]").val(total);
        $("input:text[id$='txtCantidadPersonasNumero']").val(cantidadPersonas);
    }


    /******************************************************
    Grilla Detalle Ocupantes
    *******************************************************/
    function intiGridOcupantes() {
        $('#<%=gvOcupante.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlAfiliados = $(this).find('select[id*="ddlApellido"]');
            var ddlTipoDocumento = $(this).find('[id*="ddlTipoDocumento"]');
            var hdfApellido = $(this).find("input[id*='hdfApellido']");
            var hdfIdAfiliado = $(this).find("input[id*='hdfIdAfiliado']");
            var txtNumeroDocumento = $(this).find("input:text[id*='txtNumeroDocumento']");
            var txtNombre = $(this).find("input:text[id*='txtNombre']");

            var txtEdad = $(this).find("input:text[id*='txtEdad']");

            var ddlHabitacionesSeleccionadas = $(this).find('select[id*="ddlHabitacionesSeleccionadas"]');
            var hdfIdHabitacionSeleccionada = $(this).find('[id*="hdfIdHabitacionSeleccionada"]');
            var hdfDetalleHabitacion = $(this).find('[id*="hdfDetalleHabitacion"]');


            ddlTipoDocumento.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });

            if (hdfIdHabitacionSeleccionada.val() > 0 && hdfIdAfiliado.val() > 0) {
                ddlHabitacionesSeleccionadas.select2({
                    placeholder: 'Seleccione la Habitacion',
                    selectOnClose: false,
                    theme: 'bootstrap4',
                    width: '100%',
                    language: 'es',
                    allowClear: true,
                    data: CargarComboHabitaciones($(this)),
                })
            }

            ddlAfiliados.select2({
                placeholder: 'Ingrese el Apellido o Nombre',
                selectOnClose: true,
                theme: 'bootstrap4',
                width: '100%',
                minimumInputLength: 4,
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
                            value: ddlAfiliados.val(), // search term");
                            filtro: params.term // search term");
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

                                }
                            })
                        };
                        cache: true
                    }
                }
            });

            ddlAfiliados.on('select2:select', function (e) {
                if (e.params.data.id > 0) {
                    var newOption = new Option(e.params.data.Apellido, e.params.data.id, false, true);
                    ddlAfiliados.append(newOption).trigger('change');
                    ddlTipoDocumento.val(e.params.data.IdTipoDocumento).trigger('change');
                    hdfApellido.val(e.params.data.Apellido);
                    hdfIdAfiliado.val(e.params.data.id);
                    txtNumeroDocumento.val(e.params.data.NumeroDocumento);
                    txtNombre.val(e.params.data.Nombre);

                    ObtenerEdad(txtEdad, e.params.data.id);


                }
                else {
                    hdfApellido.val(e.params.data.text);
                }

                var newOption = new Option('Seleccione la Habitacion', '-1', false, true);
                ddlHabitacionesSeleccionadas.append(newOption);

                ddlHabitacionesSeleccionadas.select2({
                    placeholder: 'Seleccione la Habitacion',
                    selectOnClose: false,
                    theme: 'bootstrap4',
                    width: '100%',
                    language: 'es',
                    allowClear: true,
                    data: CargarComboHabitaciones($(this)),
                });

                ddlHabitacionesSeleccionadas.on('select2:select', function (e) {
                    hdfIdHabitacionSeleccionada.val(e.params.data.id);
                    hdfDetalleHabitacion.val(e.params.data.text);
                });

                //ddlHabitacionesSeleccionadas.on('select2:unselect', function (e) {
                //    ddlHabitacionesSeleccionadas.val(null).trigger('change');
                //    hdfIdHabitacionSeleccionada.val('');
                //    hdfDetalleHabitacion.val('');
                //});
                //CargarComboHabitaciones($(this));

            });

            ddlAfiliados.on('select2:unselect', function (e) {
                if ($.isNumeric(e.params.data.id)) {
                    hdfIdAfiliado.val('');
                    hdfApellido.val('');
                    txtNumeroDocumento.val('');
                    txtNombre.val('');
                    txtEdad.val('');
                    hdfIdHabitacionSeleccionada.val('');
                    hdfDetalleHabitacion.val('');

                }
                ddlAfiliados.val(null).trigger('change');
                ddlTipoDocumento.val(null).trigger('change');
                ddlHabitacionesSeleccionadas.val(null).trigger('change');
            });

            ddlHabitacionesSeleccionadas.on('select2:select', function (e) {
                if (hdfIdHabitacionSeleccionada.val() > 0) {
                    hdfIdHabitacionSeleccionada.val(e.params.data.id);
                    hdfDetalleHabitacion.val(e.params.data.text);
                }
            });
            ddlHabitacionesSeleccionadas.on('select2:unselect', function (e) {
                ddlHabitacionesSeleccionadas.val(null).trigger('change');
                hdfIdHabitacionSeleccionada.val('');
                hdfDetalleHabitacion.val('');
            });

        });
    }

    function CargarComboHabitaciones(row) {
        //var ddlHabitacionesSeleccionadas = row.find('select[id*="ddlHabitacionesSeleccionadas"]');
        //var hdfIdHabitacionSeleccionada = row.find('[id*="hdfIdHabitacionSeleccionada"]');
        //var hdfDetalleHabitacion = row.find('[id*="hdfDetalleHabitacion"]');

        var habitaciones = $.map(ObtenerHabitacionesSeleccionadas(), function (obj) {
            obj.id = obj.id || obj.IdHabitacion; // replace pk with your identifier
            obj.text = obj.text || obj.Detalle; // replace name with the property used for the text

            return obj;
        });


        //if (hdfIdHabitacionSeleccionada.val() > 0) {
        //    var newOption = new Option(hdfDetalleHabitacion.val(), hdfIdHabitacionSeleccionada.val(), false, true);
        //    ddlHabitacionesSeleccionadas.append(newOption).trigger('change');
        //}
        //else {
        //    var newOption = new Option('Seleccione la Habitacion 99', '-1', false, true);
        //    ddlHabitacionesSeleccionadas.append(newOption);
        //}

        //$.each(habitaciones, function (data, value) {
        //    var newOption = new Option(value.text, value.id);
        //   
        //        ddlHabitacionesSeleccionadas.append(newOption).trigger('change');
        //});
        //ddlHabitacionesSeleccionadas.select2({
        //    placeholder: 'Seleccione la Habitacion',
        //                selectOnClose: false,
        //                theme: 'bootstrap4',
        //                width: '100%',
        //                //theme: 'bootstrap',
        //                //minimumInputLength: 4,
        //                language: 'es',
        //                //tags: true,
        //                allowClear: true,
        //                // data: habitaciones,
        //                data: habitaciones,
        //});
        return habitaciones;
    }

    function ObtenerEdad(txtEdad, idAfiliado) {
        //var txtFechaEgreso = $("input:text[id$='txtFechaEgreso']");
        var txtReservaFechaEgreso = $("input:text[id$='txtReservaFechaEgreso']");
        /* Parche por fecha egreso NaN/NaN/NaN */
        if (txtReservaFechaEgreso.val() == 'NaN/NaN/NaN') {
            var egreso = new Date();
            egreso.getDate();
            txtReservaFechaEgreso.val(toStrDate(egreso));
        }

        $.ajax({
            url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/ObtenerEdad',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            data: JSON.stringify({ 'idAfiliado': idAfiliado, 'fechaEgreso': txtReservaFechaEgreso.val() }),
            success: function (response) {
                txtEdad.val(response.d.Edad)
            }
        });
    }


    /******************************************************
    Grilla Detalle Descuentos
    *******************************************************/
    function InitGrillaDescuento() {
        $('#<%=gvDescuentos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlHabitaciones = $(this).find('select[id*="ddlHabitaciones"]');
            var ddlTiposDescuentos = $(this).find('[id*="ddlTiposDescuentos"]');
            var hdf_gvd_IdTipoDescuento = $(this).find("input[id*='hdf_gvd_IdTipoDescuento']");
            var gvd_hdfTipoDescuentoDescripcion = $(this).find("input[id*='hdf_gvd_TipoDescuentoDescripcion']");
            var ddlHoteles = $("select[id$='ddlHoteles']");

            ddlTiposDescuentos.prop("disabled", true);
            ddlHabitaciones.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
            ddlTiposDescuentos.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });

            ddlHabitaciones.change(function () {
                if ($(this).val() != "")
                    $(this).closest("tr").find('[id*="ddlTiposDescuentos"]').prop("disabled", false);
                else
                    $(this).closest("tr").find('[id*="ddlTiposDescuentos"]').prop("disabled", true);
            });

            ddlTiposDescuentos.change(function () {
                if ($(this).val() > 0) {
                    gvd_hdfTipoDescuentoDescripcion.val($('option:selected', this).text());
                    var descuento = {};
                    descuento.IdDescuento = $(this).val();
                    descuento.IdReserva = $("input[id$='txtNumeroReserva']").val() == '' ? 0 : $("input[id$='txtNumeroReserva']").val();
                    descuento.IdHotel = ddlHoteles.val();
                    descuento.IdAfiliado = $("input[id$='hdfReservaIdAfiliado']").val() == '' ? 0 : $("input[id$='hdfReservaIdAfiliado']").val();
                    descuento.IdAfiliadoTipo = $("input[id$='hdfReservaIdAfiliadoTipo']").val() == '' ? 0 : $("input[id$='hdfReservaIdAfiliadoTipo']").val();
                    descuento.FechaIngreso = ObtenerFechaIngreso(ddlHabitaciones.val());
                    var row = $(this).closest("tr");
                    $.ajax({
                        type: "POST",
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Hotel/HotelWS.asmx/DescuentosObtenerDatos',
                        dataType: "json",
                        contentType: "application/json",
                        data: JSON.stringify({ 'descuento': descuento }),
                        success: function (data) {
                            row.find("input[id*='hdf_gvd_IdDescuento']").val(data.d.IdDescuento);
                            row.find("input[id*='hdf_gvd_IdTipoDescuento']").val(data.d.IdTipoDescuento);
                            row.find("input[id*='hdf_gvd_PorcentajeImporte']").val(data.d.DescuentoPorcentaje);
                            row.find("input[id*='hdf_gvd_DescuentoImporte']").val(data.d.DescuentoImporte);
                            row.find("input[id*='hdf_gvd_PrecioBase']").val(data.d.PrecioBase);
                            row.find("input[id*='hdf_gvd_Cantidad']").val(data.d.Cantidad);
                            row.find("span[id*='gvd_lblPorcentajeDescuento']").text(accounting.formatMoney(data.d.DescuentoPorcentaje, "", 4, "."));
                            row.find("span[id*='gvd_lblDescuentoImporte']").text(accounting.formatMoney(data.d.DescuentoImporte, gblSimbolo, 2, "."));

                            CalcularPrecio();
                        }
                    });
                }
            });
        });
    }
</script>
<div id="deshabilitarControles">
    <asp:UpdatePanel ID="upReservaDatos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-header">
                    Datos de la Reserva
                </div>
                <div class="card-body">
                    <asp:HiddenField ID="hdfOpcionAbierta" runat="server" />
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroReserva" runat="server" Text="Nro Reserva"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtNumeroReserva" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoteles" runat="server" Text="Hotel" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlHoteles" runat="server" />
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvHoteles" ControlToValidate="ddlHoteles" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlReservaApellido" runat="server">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hdfReservaIdAfiliado" runat="server" />
                            <asp:HiddenField ID="hdfReservaIdAfiliadoTipo" runat="server" />
                            <asp:HiddenField ID="hdfReservaApellido" runat="server" />
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvApellido" ControlToValidate="ddlReservaApellido" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtReservaNombre" runat="server" />
                            <%--<asp:RequiredFieldValidator CssClass="Validador2" ID="rfvNombre" ControlToValidate="txtReservaNombre" ValidationGroup="Aceptar" runat="server" ></asp:RequiredFieldValidator>--%>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionesFiscales" runat="server" Text="Condicion Fiscal"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" TabIndex="18" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlReservaTipoDocumento" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvTipoDocumento" ControlToValidate="ddlReservaTipoDocumento" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtReservaNumeroDocumento" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvNumeroDocumento" ControlToValidate="txtReservaNumeroDocumento" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstadoSocio" runat="server" Text="Estado Socio"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtEstadoSocio" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtCategoria" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblReservaCorreoElectronico" runat="server" Text="Mail"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtReservaCorreoElectronico" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaIngreso" runat="server" Text="Fecha Ingreso"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtReservaFechaIngreso" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvFechaIngreso" ControlToValidate="txtReservaFechaIngreso" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad días"></asp:Label>
                        <div class="col-sm-3">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadDias" runat="server" Text=""></AUGE:NumericTextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEgreso" runat="server" Text="Fecha Egreso"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtReservaFechaEgreso" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvFechaEgreso" ControlToValidate="txtReservaFechaEgreso" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraIngreso" runat="server" Text="Hora Ingreso"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlHoraIngreso" runat="server">
                            </asp:DropDownList>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraEgreso" runat="server" Text="Hora Egreso"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlHoraEgreso" runat="server">
                            </asp:DropDownList>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaPrecio" runat="server" Text="Tarifas"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlListasPrecios" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvListasPrecios" ControlToValidate="ddlListasPrecios" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-5 col-form-label" ID="lblCantidadPersonas" runat="server" Text="Cantidad de Personas que se pueden alojar"></asp:Label>
                        <div class="col-sm-3">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadPersonasNumero" Enabled="false" runat="server"></AUGE:NumericTextBox>
                        </div>
                    </div>
                    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="card">
        <div class="card-header">
            Detalle de la Reserva
        </div>
        <div class="card-body">
            <AUGE:ReservasDetallesOpciones ID="ctrReservasDetallesOpciones" runat="server" />
            <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
                SkinID="MyTab">
                <asp:TabPanel runat="server" ID="tpDetalleGastos"
                    HeaderText="Detalle de Reserva">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upDetalleGastos" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" OnClick="btnAgregarItem_Click" runat="server" Text="Agregar item"
                                            CausesValidation="false" />

                                        <asp:Button CssClass="botonesEvol" ID="btnCargaMasivaMostrar" OnClick="btnCargaMasivaMostrar_Click" runat="server" Text="Carga masiva"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                                <asp:UpdatePanel ID="upCargaMasiva" Visible="false" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group row">
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCargaMasivaProductos" runat="server" Text="Tipo de Habitacion"></asp:Label>
                                            <div class="col-sm-3">
                                                <asp:DropDownList ID="ddlCargaMasivaProductos" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvCargaMasivaProductos" ControlToValidate="ddlCargaMasivaProductos" ValidationGroup="CargaMasivaAgregar" runat="server"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCargaMasivaCantidad" runat="server" Text="Cantidad"></asp:Label>
                                            <div class="col-sm-3">
                                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCargaMasivaCantidad" Prefix="" NumberOfDecimals="0" runat="server" Text=""></Evol:CurrencyTextBox>
                                                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvCargaMasivaCantidad" ControlToValidate="txtCargaMasivaCantidad" ValidationGroup="CargaMasivaAgregar" runat="server"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-sm-4">
                                                <asp:Button CssClass="botonesEvol" ID="btnCargaMasivaAgregar" OnClick="btnCargaMasivaAgregar_Click" ValidationGroup="CargaMasivaAgregar" runat="server" Text="Agregar Carga" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:HiddenField ID="hdfCodigoTPH01" runat="server" />
                                <asp:HiddenField ID="hdfCodigoTPH02" runat="server" />
                                <asp:HiddenField ID="hdfCodigoTPH03" runat="server" />
                                <asp:HiddenField ID="hdfCodigoTPH05" runat="server" />
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <div style="overflow-x: scroll">
                                            <div class="table-responsive">
                                                <asp:GridView ID="gvDatos" ShowFooter="true" OnRowCommand="gvDatos_RowCommand"
                                                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdReservaDetalle"
                                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Tipo Producto">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlTiposProductosHoteles" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Desde" ItemStyle-Wrap="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox CssClass="form-control form-control-sm" Width="115px" ID="txtFechaIngreso" Text='<%#Eval("FechaIngreso", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hasta" ItemStyle-Wrap="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox CssClass="form-control" ID="txtFechaEgreso" Width="115px" Text='<%#Eval("FechaEgreso", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Detalle">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlDetalleGastos" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                                <asp:HiddenField ID="hdfIdHabitacion" Value='<%#Bind("IdHabitacion") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfDetalleGastos" Value='<%#Bind("Detalle") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfCantidadPersonas" Value='<%#Bind("CantidadPersonas") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText=" " ItemStyle-Wrap="false">
                                                            <ItemTemplate>
                                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="true"
                                                                    AlternateText="Modificar" ToolTip="Agregar Opcion" />
                                                                <asp:HiddenField ID="hdfCantidadPersonasOpciones" Value='<%#Bind("CantidadPersonasOpciones") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfMoviliario" Value='<%#Bind("HabitacionDetalle.IdHabitacionDetalle") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfLateCheckOut" Value='<%#Bind("LateCheckOut") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Opciones" ItemStyle-Wrap="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDetalleOpciones" Visible="true" runat="server" Text='<%#Bind("DetalleOpciones") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cantidad" ItemStyle-Wrap="false">
                                                            <ItemTemplate>
                                                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" Prefix="" NumberOfDecimals="2" runat="server" Text='<%#Bind("Cantidad", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Prec. Unitario" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecio" NumberOfDecimals="2" Text='<%#Bind("Precio", "{0:C2}") %>' runat="server"></Evol:CurrencyTextBox>
                                                                <asp:HiddenField ID="hdfIdListaPrecioDetalle" Value='<%#Bind("IdListaPrecioDetalle") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfPrecioEditable" Value='<%#Bind("PrecioEditable") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfPrecioLista" Value='<%#Bind("Precio") %>' runat="server" />
                                                                <asp:HiddenField ID="hdfPrecioHabitacionCompartida" Value='<%#Bind("PrecioHabitacionCompartida") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="% Desc." ItemStyle-Wrap="false" SortExpression="">
                                                            <ItemTemplate>
                                                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtDescuentoPorcentual" Prefix="" runat="server" Text='<%# Eval("DescuentoPorcentaje")==null ? "0,00" : Eval("DescuentoPorcentaje", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                                                <asp:HiddenField ID="hdfDescuentoHabilitado" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Importe Desc." HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                                            <ItemTemplate>
                                                                <asp:Label CssClass="col-form-label" ID="lblDescuentoImporte" runat="server" Text='<%#Bind("DescuentoImporte", "{0:C2}") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfDescuentoImporte" Value='<%#Bind("DescuentoImporte")%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SubTotal" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label CssClass="col-form-label" ID="lblSubTotal" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdfSubTotal" Value='<%#Eval("SubTotal") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label CssClass="col-form-label labelFooterEvol" ID="lblTotal" runat="server"></asp:Label>
                                                                <asp:HiddenField ID="hdfTotal" runat="server" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                                            <ItemTemplate>
                                                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                         AlternateText="Consultar" ToolTip="Consultar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                         AlternateText="Modificar" ToolTip="Modificar" />--%>
                                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<asp:GridView ID="gvDatosTotales" ShowFooter="true" 
                            runat="server" SkinID="GrillaBasicaFormalMedia" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Habitacion" >
                                    <ItemTemplate>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText ="Descuento" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" >
                                    <ItemTemplate>

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upDescuentos" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarDescuento" OnClick="btnAgregarDescuento_Click" runat="server" Text="Agregar descuento"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvDescuentos" ShowFooter="true" OnRowCommand="gvDescuentos_RowCommand"
                                        OnRowDataBound="gvDescuentos_RowDataBound" DataKeyNames="IdReservaDetalleDescuento"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Habitacion" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlHabitaciones" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo de Descuento" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlTiposDescuentos" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                    <asp:HiddenField ID="hdf_gvd_IdDescuento" runat="server" />
                                                    <asp:HiddenField ID="hdf_gvd_IdTipoDescuento" runat="server" />
                                                    <asp:HiddenField ID="hdf_gvd_TipoDescuentoDescripcion" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% Desc." HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-Width="10%" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label CssClass="gvLabelMoneda" ID="gvd_lblPorcentajeDescuento" runat="server" Text='<%#Bind("DescuentoPorcentaje", "{0:N4}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdf_gvd_PorcentajeImporte" Value='<%#Bind("DescuentoPorcentaje")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Importe Desc." HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-Width="10%" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label CssClass="gvLabelMoneda" ID="gvd_lblDescuentoImporte" runat="server" Text='<%#Bind("DescuentoImporte", "{0:C2}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdf_gvd_DescuentoImporte" Value='<%#Bind("DescuentoImporte")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Base Calculo" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-Width="10%" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label CssClass="gvLabelMoneda" ID="gvd_lblBaseCalculoImporte" runat="server" Text='<%#Bind("BaseCalculoImporte", "{0:C2}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdf_gvd_PrecioBase" Value='<%#Bind("PrecioBase")%>' runat="server" />
                                                    <asp:HiddenField ID="hdf_gvd_BaseCalculoImporte" Value='<%#Bind("BaseCalculoImporte")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-Width="5%" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label CssClass="gvLabelMoneda" ID="gvd_lblCantidad" runat="server" Text='<%#Bind("Cantidad", "{0:N2}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdf_gvd_Cantidad" Value='<%#Bind("Cantidad")%>' runat="server" />
                                                    <asp:HiddenField ID="hdf_gvd_CantidadDisponible" Value='<%#Bind("Cantidad")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descuento" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-Width="10%" SortExpression="">
                                                <ItemTemplate>
                                                    <asp:Label CssClass="gvLabelMoneda" ID="gvd_lblSubTotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdf_gvd_lblSubTotal" Value='<%#Bind("SubTotal")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpOcumpantes" HeaderText="Ocupantes">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upOcupantes" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarOcupante" OnClick="btnAgregarOcupante_Click" runat="server" Text="Agregar ocupante"
                                            CausesValidation="false" />
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvOcupante" ShowFooter="true" OnRowCommand="gvOcupante_RowCommand"
                                        OnRowDataBound="gvOcupante_RowDataBound" DataKeyNames="IndiceColeccion"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Apellido" ItemStyle-Width="35%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlApellido" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                    <asp:HiddenField ID="hdfIdAfiliado" Value='<%#Bind("IdAfiliado") %>' runat="server" />
                                                    <asp:HiddenField ID="hdfApellido" Value='<%#Bind("Apellido") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nombre" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:TextBox CssClass="form-control" Text='<%#Bind("Nombre") %>' ID="txtNombre" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo Documento" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Numero Documento" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox CssClass="form-control" ID="txtNumeroDocumento" Text='<%#Bind("NumeroDocumento") %>' runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edad" ItemStyle-Wrap="false" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtEdad" Prefix="" NumberOfDecimals="0" runat="server" Text='<%#Bind("EdadFechaSalida", "{0:N0}") %>'></Evol:CurrencyTextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Habitacion" ItemStyle-Width="35%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlHabitacionesSeleccionadas" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                    <asp:HiddenField ID="hdfIdHabitacionSeleccionada" Value='<%#Bind("IdHabitacion") %>' runat="server" />
                                                    <asp:HiddenField ID="hdfDetalleHabitacion" Value='<%#Bind("DetalleHabitacion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                         AlternateText="Consultar" ToolTip="Consultar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                         AlternateText="Modificar" ToolTip="Modificar" />--%>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpFacturacionCobro" HeaderText="Facturacion y Cobros">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upCtaCte" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarComprobante" Visible="false" runat="server" Text="Agregar Comprobante" OnClick="btnAgregarComprobante_Click" />
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarOC" Visible="false" runat="server" Text="Agregar Cobro" OnClick="btnAgregarOC_Click" />
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gvCuentaCorriente" DataKeyNames="Orden" OnRowDataBound="gvCuentaCorriente_RowDataBound" OnRowCommand="gvCuentaCorriente_RowCommand"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                        <Columns>
                                            <asp:BoundField HeaderText="Fecha" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                            <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                                            <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacionTipoOperacion" SortExpression="TipoOp" />
                                            <asp:BoundField HeaderText="Debito" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteDebito" SortExpression="ImporteDebito" />
                                            <asp:BoundField HeaderText="Credito" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="ImporteCredito" SortExpression="ImporteCredito" />
                                            <asp:BoundField HeaderText="Saldo Actual" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="SaldoActual" SortExpression="SaldoActual" />
                                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                                    <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("TipoOperacionIdTipoOperacion") %>' />
                                                    <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
                    <ContentTemplate>
                        <AUGE:Comentarios ID="ctrComentarios" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
                    <ContentTemplate>
                        <AUGE:Archivos ID="ctrArchivos" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
                    <ContentTemplate>
                        <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </div>
    </div>
    <br />
</div>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" Visible="false"
                    OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptarContinuar" runat="server" Text="Aplicar" OnClick="btnAceptarContinuar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Guardar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>