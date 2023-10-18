//function MaskMoney(ctrl) {
//    $(ctrl).maskMoney({ thousands: gblSeparadorMil, decimal: gblSeparadorDecimal, allowZero: true, allowNegative: false, prefix: gblSimbolo });
//    //$(ctrl).fn.decVal = function () { alert("Hola Mundo"); };
//}

var EvolControls = function () {
    return {
        //main function to initiate the module
        init: function () {
            //Control Select
            //$(".EvolMoney").maskMoney();
            $(".EvolMoney").maskMoney({
                thousands: gblSeparadorMil,
                decimal: gblSeparadorDecimal,
                allowZero: true,
                allowNegative: false,
                prefix: gblSimbolo
            });
//            $(".EvolMoney").fn.num = function () {
//                 return this.each(function () {
//                    return $(this).maskMoney('unmasked')[0];
//                });
//            };
        }
    };
} ();