$(document).ready(function () {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControles);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ValidarShowConfirm);
    InitControles();
    ValidarShowConfirm();
});

function ValidarShowConfirm() {
    var msg = '¿Esta seguro que desea dar de baja la reserva? <BR /> La misma no se podrá volver a modificar.'
    var btn = $("input[type=submit][id$='btnAceptar']");
    btn.click(function (e) {
        var ddlEstados = $("select[name$=ddlEstados]");
        if (ddlEstados.val() == '0') {
            e.preventDefault();
            showConfirm(this, msg);
        }
    });
    var btnCont = $("input[type=submit][id$='btnAceptarContinuar']");
    btnCont.click(function (e) {
        var ddlEstados = $("select[name$=ddlEstados]");
        if (ddlEstados.val() == '0') {
            e.preventDefault();
            showConfirm(this, msg);
        }
    });
}

function CalcularTotalServicios(total) {

    var impuestoPais = $("input[type=text][id$='txtImpuestoPais']").maskMoney('unmasked')[0];
    impuestoPais = isNaN(impuestoPais) ? 0 : impuestoPais;
    var impuestoRG4815 = $("input:text[id$='txtPercepcionRG4815']").maskMoney('unmasked')[0];
    impuestoRG4815 = isNaN(impuestoRG4815) ? 0 : impuestoRG4815;
    var impuestoRG3819 = $("input:text[id$='txtPercepcionRG3819']").maskMoney('unmasked')[0];
    impuestoRG3819 = isNaN(impuestoRG3819) ? 0 : impuestoRG3819;
    var impuestoRG5272 = $("input:text[id$='txtPercepcionRG5272']").maskMoney('unmasked')[0];
    impuestoRG5272 = isNaN(impuestoRG5272) ? 0 : impuestoRG5272;
    var totalConImpuestos = parseFloat(total) + parseFloat(impuestoPais) + parseFloat(impuestoRG4815) + parseFloat(impuestoRG3819) + parseFloat(impuestoRG5272);

    $("input[type=text][id$='txtImporteCargo']").val(accounting.formatMoney(totalConImpuestos, gblSimbolo, 2, '.'));
    $("input[type=hidden][id$='hdfImporteCargo']").val(totalConImpuestos);
    $("input[type=hidden][id$='hdfImporteCuota']").val(totalConImpuestos);
    CalcularTotal();
}

function CalcularImporteCargoTurismo() {
    var totalServicios = $("input[type=hidden][id$='hdfTotalServicios']").val();
    totalServicios = isNaN(totalServicios) ? 0 : totalServicios;
    CalcularTotalServicios(totalServicios);
}

function SetMultitabProfileActive() {
    $('#myTab a[href="#profile"]').tab('show');
}
function InitControles() {

    var txtImporte = $("input[type=text][id$='txtImporteServicio']");
    var ddlServicios = $("select[name$=ddlServicio]");

    $("input[type=text][id$='txtImpuestoPais']").blur(function () { CalcularImporteCargoTurismo(); });
    $("input[type=text][id$='txtPercepcionRG4815']").blur(function () { CalcularImporteCargoTurismo(); });
    $("input[type=text][id$='txtPercepcionRG3819']").blur(function () { CalcularImporteCargoTurismo(); });
    $("input[type=text][id$='txtPercepcionRG5272']").blur(function () { CalcularImporteCargoTurismo(); });

    txtImporte.blur(function () {
        $.ajax({
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            url: ResolveUrl('~/Modulos/Turismo/TurismoWS.asmx/ObtenerPorcentajeGanancia'),
            data: JSON.stringify({ 'idTipoServicio': ddlServicios.val() }),
            beforeSend: function (xhr, opts) {
                if (txtImporte.val() == '' || ddlServicios.val() == '') {
                    xhr.abort();
                }
            },
            success: function (res) {
                var porcentaje = res.d.text;
                if (porcentaje == null)
                    porcentaje = 0;
                $("input[type=hidden][id$='hdfPorcentajeGananciaServicio']").val(porcentaje);
                var Importe = parseFloat(txtImporte.maskMoney('unmasked')[0]);

                var importeServicio = (Importe * parseFloat(porcentaje)) / 100;
                var total = Importe - importeServicio;
                $("input[type=text][id$='txtCostoServicio']").val(accounting.formatMoney(total, gblSimbolo, 2, '.'));
            }
        });
    });

    var control = $("select[name$='ddlEntidad']");
    var entidad = 187 //PROV
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
            url: ResolveUrl("~/Modulos/CuentasPagar/CuentasPagarWS.asmx/BuscarPorEntidad"), //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
            delay: 500,
            data: function (params) {
                return JSON.stringify({
                    IdEntidad: entidad, // search term");
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
                            text: item.text,
                            id: item.id
                            //Cuit: item.CUIT,
                        }
                    })
                };
                cache: true
            }
        }
    });

    control.on('select2:select', function (e) {
        $("input[id$='hdfIdProveedor']").val(e.params.data.id);
        $("input[id$='hdfProveedor']").val(e.params.data.text);
    });

    control.on('select2:unselect', function (e) {
        $("input[id$='hdfIdProveedor']").val('');
        $("input[id$='hdfProveedor']").val('');
        control.val(null).trigger('change');
    });
}

$("input[type=submit][id$='btnAceptar']").click(function (e) {
    var txtFechaIda = $("input[id$='txtFechaSalida']").val();
    var txtFechaVuelta = $("input[id$='txtFechaRegreso']").val();
    if (txtFechaIda != '' && txtFechaVuelta != '') {
        var fechaIda = new Date(toDate(txtFechaIda));
        var fechaVuelta = new Date(toDate(txtFechaVuelta));
        if (fechaIda > fechaVuelta) {
            e.preventDefault();
            MostrarMensaje('La Fecha de regreso debe ser mayor a la fecha de salida.', 'red');
        }
    }
});

$("input[type=submit][id$='btnAceptarContinuar']").click(function (e) {
    var txtFechaIda = $("input[id$='txtFechaSalida']").val();
    var txtFechaVuelta = $("input[id$='txtFechaRegreso']").val();
    if (txtFechaIda != '' && txtFechaVuelta != '') {
        var fechaIda = new Date(toDate(txtFechaIda));
        var fechaVuelta = new Date(toDate(txtFechaVuelta));
        if (fechaIda > fechaVuelta) {
            e.preventDefault();
            MostrarMensaje('La Fecha de regreso debe ser mayor a la fecha de salida.', 'red');
        }
    }
});
