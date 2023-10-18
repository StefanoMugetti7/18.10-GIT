$(function () {
    $("input:text[id$='txtBuscar']").keypress(function (event) {
        var key = (event.keyCode ? event.keyCode : event.which); //e.which;
        if (key == 13) // the enter key code
        {
            event.preventDefault();
            $('#btnBuscarCuentas').click();
            return false;
        }
    });

});

function HideModalBuscarCuentaContable() {
    $("[id$='modalBuscarCuentaContable']").modal('hide');
}


function BuscarCuentasContables() {
    //showPopupProgressBar();
    var tree = $("[id$='tvCuentasContables']").tree({
        uiLibrary: 'bootstrap4',
        dataSource: ResolveUrl('~/Modulos/Contabilidad/ContabilidadWS.asmx/CuentasContablesArmarArbol'),
        primaryKey: 'id',
        textField: 'text',
        imageUrlField: 'flagUrl',
        autoLoad: false,
        select: function (e, node, id) {
            var record = tree.getDataById(id);
            if (record.imputable) {
                //popupBuscarCuentaContable(id);
                $("[id$='modalBuscarCuentaContable']").modal('hide');
                var btn = $("[id$='btnSeleccionar']");
                var hdf = $("input[id$='hdfPopUpCtaCbleBuscarIdCuentacontable']");
                hdf.val(id);
                btn.click();
            }
            else
                tree.expand(node);
        }
    });
    tree.reload({ idEjercicio: $("select[id$='ddlEjercicioContable']").val(), descripcion: $("input:text[id$='txtBuscar']").val() });
    //hidePopupProgressBar();
}