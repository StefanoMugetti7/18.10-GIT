<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesPagosDatos.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.OrdenesPagosDatos" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="AfiliadosDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="ProveedoresDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/OrdenesPagosValores.ascx" TagName="OrdenesPagosValores" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/AnticiposTurismoPopUp.ascx" TagName="AnticiposTurismo" TagPrefix="auge" %>

<script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.4/clipboard.min.js"></script>
<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        //$("input[type=text][id$='txtCodigo']").focus();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindEvents);

        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularValores);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ExpandCollapse);
        SetTabIndexInput();
        InitApellidoSelect2();
        CalcularValores();
        ExpandCollapse();
    });

    function CopyClipboard() {
        var $temp = $("<input type='text'>");
        $("body").append($temp);
        var link = $("input[type=hidden][id$='hfLinkFirmarDocumento']").val();
        $temp.val(link);
        $temp.focus();
        $temp.select();
        try {
            var successful = document.execCommand('copy');
            MostrarMensaje("Se ha Copiado el Link");
            //$('#copyClipboard').data('tooltip').show();
        } catch (err) {
            console.error('Unable to copy');
        }
        $temp.remove();
    }

    function EnviarWhatsApp(url) {
        var win = window.open(url, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            MostrarMensaje('Por favor habilite los popups para este sitio', 'red');
        }
    }

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function bindEvents(sender, args) {
        $("input[type=text][id$='txtImporteAPagar']").blur(function () {
            CalcularItem();
        });
    }

    function CalcularItem() {
        var importePagar = $("input[type=text][id$='txtImporteAPagar']").maskMoney('unmasked')[0];
        var controlImportePagar = $("input[id$='hdfImporteAPagar']").val().replace('.', '').replace(',', '.');
        if (parseFloat(importePagar) > parseFloat(controlImportePagar)) {
            importePagar = controlImportePagar;
            $("input[type=text][id$='txtImporteAPagar']").val(accounting.formatMoney(importePagar, "$ ", 2, "."));
        }

        var subTotal = 0.00;
        var notascreditos = 0.00;

        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find('input:text[id*="txtImporteParcial"]').maskMoney('unmasked')[0];
            var importeParcialOriginal = $(this).find("input[id*='hdfImporteParcial']").val().replace('.', '').replace(',', '.');

            if (incluir && importeParcialOriginal < 0) {
                //if (incluir && parseFloat(importe) = 0) {
                //    importe = parseFloat(importeParcialOriginal);
                //}
                if (parseFloat(importe) == 0) {
                    importe = parseFloat(importeParcialOriginal);
                }
                if (parseFloat(importeParcialOriginal) < 0 && parseFloat(importe) < parseFloat(importeParcialOriginal)) {
                    importe = importeParcialOriginal;
                }
                //if (incluir) {
                //notascreditos += parseFloat(importe);
                //}
                notascreditos += parseFloat(importe);
            }
        });
        //var pendienteImputar = importePagar;
        var pendienteImputar = parseFloat(importePagar) + parseFloat((notascreditos * -1));
        var pendiente = parseFloat(importePagar);
        subTotal = notascreditos;

        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find('input:text[id*="txtImporteParcial"]').maskMoney('unmasked')[0];
            var importeParcialOriginal = $(this).find("input[id*='hdfImporteParcial']").val().replace('.', '').replace(',', '.');

         

            if (incluir && parseFloat(importe) == 0) {
                importe = parseFloat(importeParcialOriginal);
            }

            if (importeParcialOriginal > 0 && importe > importeParcialOriginal) {
                importe = importeParcialOriginal;
            }
            if (importeParcialOriginal < 0 && importe < importeParcialOriginal) {
                importe = importeParcialOriginal;
            }

            if (incluir && importe && parseFloat(importe) > 0) {
                if (parseFloat(pendienteImputar) >= parseFloat(importe)) {
                    $(this).find('input:text[id*="txtImporteParcial"]').val(accounting.formatMoney(importe, "$ ", 2, "."));
                    subTotal += parseFloat(importe);
                    pendienteImputar -= parseFloat(importe);
                } else if (parseFloat(pendienteImputar) > 0) {
                    $(this).find('input:text[id*="txtImporteParcial"]').val(accounting.formatMoney(pendienteImputar, "$ ", 2, "."));
                    subTotal += parseFloat(pendienteImputar);
                    pendienteImputar -= parseFloat(pendienteImputar);
                }
                else {
                    pendienteImputar = 0;
                    $(this).find('input:text[id*="txtImporteParcial"]').val(accounting.formatMoney(0, "$ ", 2, "."));
                    //$(this).find('input:checkbox[id*="chkIncluir"]').prop('checked', false);
                }
            }
            else if (incluir && importe && parseFloat(importe) < 0) {
                $(this).find('input:text[id*="txtImporteParcial"]').val(accounting.formatMoney(importe, "$ ", 2, "."));
            }
            else {
                $(this).find('input:text[id*="txtImporteParcial"]').val(accounting.formatMoney(0, "$ ", 2, "."));
            }

        });
        $("#<%=gvDatos.ClientID %> [id$=lblImporteSubTotal]").text(accounting.formatMoney(subTotal, "$ ", 2, "."));
        $("input[type=text][id$='txtSubTotal']").val(accounting.formatMoney(subTotal, "$ ", 2, "."));
        $("input[type=text][id$='txtImporteAImputar']").val(accounting.formatMoney(pendiente - subTotal, "$ ", 2, "."));
        //$("input[type=text][id$='txtImporteAImputar']").val(accounting.formatMoney(pendiente - notascreditos, "$ ", 2, "."));
        if (($("input[type=text][id$='txtImporteAImputar']").val()) == "$ -0,00") {
            $("input[type=text][id$='txtImporteAImputar']").val(accounting.formatMoney(0, "$ ", 2, "."));
        }
         <%--   $("#<%=gvDatos.ClientID %> [id$=lblImporteSubTotal]").text(accounting.formatMoney(subTotal, "$ ", 2, "."));
            $("input[type=text][id$='txtSubTotal']").val(accounting.formatMoney(subTotal, "$ ", 2, "."));
            $("input[type=text][id$='txtImporteAImputar']").val(accounting.formatMoney(pendienteImputar - subTotal, "$ ", 2, "."));
        --%>
        CalcularTotal();

    }

    function CalcularAnticipos() {
        var subTotal = 0.00;
        $('#<%=gvAnticipo.ClientID%> tr').not(':first').not(':last').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluirAnticipo"]').is(":checked");
            var importe = $(this).find("input[id*='hdfImporteAnticipo']").val().replace('.', '').replace(',', '.');

            if (incluir && importe) {
                subTotal += parseFloat(importe);
            }
        });
        $("#<%=gvAnticipo.ClientID %> [id$=lblImporteSubTotalAnticipo]").text(accounting.formatMoney(subTotal, "$ ", 2, "."));
        $("input[type=text][id$='txtAnticipo']").val(accounting.formatMoney(subTotal, "$ ", 2, "."));

        CalcularTotal()
    }

    function CalcularTotal() {
        var subTotal = $("input[type=text][id$='txtSubTotal']").maskMoney('unmasked')[0];
        var hdfCalcularRetenciones = $("input[id$='hdfCalcularRetenciones']").val()

        if (hdfCalcularRetenciones == 1) {

            __doPostBack("<%=btnCalcularRetenciones.UniqueID %>", "");
        }
        var retenciones = $("input[type=text][id$='txtImporteRetenciones']").maskMoney('unmasked')[0];
        var anticipos = $("input[type=text][id$='txtAnticipo']").maskMoney('unmasked')[0];
        var total = subTotal - retenciones - anticipos;
        $("input[type=text][id$='txtTotalAPagar']").val(accounting.formatMoney(total, "$ ", 2, "."));
    }

    function SetearFechaPago() {
        $("input[type=text][id$='txtFechaCajaContabilizacion']").val($("input[type=text][id$='txtFechaAlta']").val());
    }

    //function CheckRow(objRef) {
    //    //Get the Row based on checkbox
    //    var row = objRef.parentNode.parentNode;
    //    //Get the reference of GridView
    //    var GridView = row.parentNode;
    //    //Get all input elements in Gridview
    //    var inputList = GridView.getElementsByTagName("input");
    //    for (var i = 0; i < inputList.length; i++) {
    //        //The First element is the Header Checkbox
    //        var headerCheckBox = inputList[0];
    //        //Based on all or none checkboxes
    //        //are checked check/uncheck Header Checkbox
    //        var checked = true;
    //        if (inputList[i].type == "checkbox" && inputList[i]
    //            != headerCheckBox) {
    //            if (!inputList[i].checked) {
    //                checked = false;
    //                break;
    //            }
    //        }
    //    }
    //    headerCheckBox.checked = checked;
    //}

    //function checkAllRow(objRef) {
    //    var GridView = objRef.parentNode.parentNode.parentNode;
    //    var inputList = GridView.getElementsByTagName("input");
    //    for (var i = 0; i < inputList.length; i++) {
    //        //Get the Cell To find out ColumnIndex
    //        var row = inputList[i].parentNode.parentNode;
    //        if (inputList[i].type == "checkbox" && objRef
    //            != inputList[i]) {
    //            if (objRef.checked) {
    //                inputList[i].checked = true;
    //            }
    //            else {
    //                inputList[i].checked = false;
    //            }
    //        }
    //    }
    //}

    function InitApellidoSelect2() {
        var ddlEntidad = $("select[name$='ddlEntidades']");
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
                url: '<%=ResolveClientUrl("~")%>/Modulos/CuentasPagar/CuentasPagarWS.asmx/BuscarPorEntidad', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        IdEntidad: ddlEntidad.val(), // search term");
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
                                id: item.id,
                                Cuit: item.CUIT,
                                //NumeroDocumento: item.NumeroDocumento,
                                RazonSocial: item.RazonSocial,
                                Beneficiario: item.Beneficiario,
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
            //CargarTipoPuntoVenta();
            AfiliadoSeleccionar();
        });

        control.on('select2:unselect', function (e) {
            $("input[id*='hdfIdAfiliado']").val('');

            control.val(null).trigger('change');
            //CargarTipoPuntoVenta();
              AfiliadoSeleccionar();

        });
    }
    function AfiliadoSeleccionar() {
        __doPostBack("<%=button.UniqueID %>", "");
    }
    function CalcularValores() {
        var importe = gvValoresCalcularImporteTotal();
        var neto = $("input[type=text][id$='txtTotalAPagar']").maskMoney('unmasked')[0];


        var total = neto - importe;
        $("input[type=text][id$='txtDiferencia']").val(accounting.formatMoney(total, "$ ", 2, "."));

    }
    function btnCalcularRetencionesAutomatico() {
        __doPostBack("<%=btnCalcularRetenciones.UniqueID %>", "");
    }


    function ExpandCollapse() {
        $("[src*=plus]").on('click', function () {
            var src = $(this).attr("src");
            if (src.indexOf("plus") >= 0) {
                $(this).attr("src", "../../Imagenes/minus.png");
                //$(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']").show();
                var panelDetalle = $(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']");
                panelDetalle.show();
            }
            else {
                $(this).attr("src", "../../Imagenes/plus.png");
                $(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']").hide();
            }
        });
    }

</script>
<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<asp:HiddenField ID="hdfCalcularRetenciones" runat="server" />
<asp:UpdatePanel ID="upEntidades" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEntidad" runat="server" Text="Entidad a Pagar"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEntidades" Enabled="false" runat="server" OnTextChanged="ddlEntidades_TextChanged" AutoPostBack="true"></asp:DropDownList>
            </div>
            <%--   <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
            <div class="col-sm-2">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server" OnTextChanged="txtCodigo_TextChanged" AutoPostBack="true"/>
            </div>
                <div class="col-sm-1">
                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarEntidad" Visible="false"
                AlternateText="Buscar" ToolTip="Buscar" onclick="btnBuscarEntidad_Click"  />
            <AUGE:AfiliadosDatos ID="ctrAfiliados" runat="server" />
            <AUGE:ProveedoresDatos ID="ctrProveedores" runat="server" />--%>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Proveedor" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" Enabled="false" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                <%--                <asp:HiddenField ID="hdfIdRefEntidad" runat="server" />--%>
            </div>
        </div>

        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuit" runat="server" Text="CUIT"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtCuit" Enabled="false" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBeneficiario" runat="server" Text="Beneficiario"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtBeneficiario" Enabled="false" runat="server"></asp:TextBox>
            </div>
            <div class="col-sm-3"></div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOrdenPagoNumero" runat="server" Text="Orden de Pago Nº "></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtOrdenPago" Enabled="false" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha Emisión "></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" Enabled="false" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaAlta" ControlToValidate="txtFechaAlta" ValidationGroup="ordendepago" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <AUGE:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Pago" runat="server" />
</div>
    <asp:UpdatePanel ID="upBotonesMenu" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialPago" runat="server" Text="Filial Pago "></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPago" runat="server"></asp:DropDownList>
    </div>
    <div class="col-sm-2">
        
        <div class="btn-group" role="group">
            <asp:PlaceHolder ID="phBotones" Visible="false" runat="server">
            <button type="button" class="botonesEvol dropdown-toggle"
                data-toggle="dropdown" aria-expanded="false">
                Comprobantes <span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu">
                <li>
                    <asp:Button ID="btnAnticipoTurismo" CssClass="dropdown-item" Visible="true" runat="server" Text="Generar Anticipo de Turismo" OnClick="btnAnticipoTurismo_Click" /></li>
                <li>
                    <asp:Button ID="btnAnticipoProveedor" CssClass="dropdown-item" Visible="true" runat="server" Text="Generar Anticipo a Proveedor" OnClick="btnAnticipoProveedor_Click" /></li>
                <li>
                    <asp:Button ID="btnComprobantes" CssClass="dropdown-item" Visible="true" runat="server" Text="Comprobantes de Compras" OnClick="btnComprobantesCompras_Click" /></li>
            </ul>
            </asp:PlaceHolder>
        </div>
    </div>
    <div class="col-sm-6">
        
    </div>
</div>
             </ContentTemplate>
                </asp:UpdatePanel>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />
<AUGE:AnticiposTurismo ID="ctrAnticiposTurismo" runat="server" />
<asp:Panel ID="pnlComprobantes" GroupingText="Detalle de Comprobantes" runat="server">
    <asp:UpdatePanel ID="upOrdenPagoDetalle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <script lang="javascript" type="text/javascript">

                //Esto esta puesto aca adentro porque sino no funciona
    /*gridViewId va ser la grilla en la que deseo que suceda el selectallrows*/
            /*gridViewId va ser la grilla en la que deseo que suceda el selectallrows*/ 
    var gridViewId = '#<%= gvDatos.ClientID %>';

    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
        updateSelectionLabel();
    }
    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
    }

                </script>

            <asp:Panel ID="pnlImportesAPagar" Visible="false" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblImporteAPagar" runat="server" Text="Importe a Pagar"></asp:Label>
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteAPagar" runat="server" Text="0.00" />
                        <asp:HiddenField ID="hdfImporteAPagar" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblImporteAImputar" runat="server" Text="Importe a Pagar Pendiente Imputar"></asp:Label>
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteAImputar" Enabled="false" runat="server" Text="0.00" />
                    </div>
                </div>
            </asp:Panel>
            <div class="table-responsive">
                <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Número" SortExpression="NumeroSolicitud">
                            <ItemTemplate>
                                <%# Eval("IdSolicitudPago")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Entidad.Nombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaSolicitud">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion.TipoOperacion">
                            <ItemTemplate>
                                <%# Eval("TipoOperacion.TipoOperacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# string.Concat(Eval("FechaFactura", "{0:dd/MM/yyyy}"), " ", Eval("TipoNumeroFacturaCompleto"), " ", Eval("TipoSolicitudPago.Descripcion"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Nro Factura" SortExpression="TipoNumeroFacturaCompleto" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("TipoNumeroFacturaCompleto")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Observacion" SortExpression="Observacion">
                            <ItemTemplate>
                                <%# Eval("Observacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" SortExpression="ImporteTotal" ItemStyle-Wrap="false" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("ImporteTotal", "{0:C2}")%>
                            </ItemTemplate>
                            <%--<FooterTemplate>
                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text="0.00"></asp:Label>
                    </FooterTemplate>  --%>    
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pagado" SortExpression="ImporteParcialPagado" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("ImporteParcialPagado", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A Pagar" ItemStyle-Wrap="false" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfImporteParcial" Value='<%#Bind("ImporteParcial") %>' runat="server" />
                                <asp:HiddenField ID="hdfImporteParcialModificado" runat="server" />
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteParcial" AllowNegative="true" runat="server" Enabled="false" Text='<%#Bind("ImporteParcial", "{0:C2}") %>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteSubTotal" runat="server" Text="0.00"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkIncluirTodos" runat="server" onclick="checkAllRow(this); CalcularItem();" Visible="false" Text="Todo" TextAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this); CalcularItem();" Visible="false" Checked='<%# Convert.ToBoolean (Eval("IncluirEnOP")) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:UpdatePanel ID="upRetenciones" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlRetenciones" GroupingText="Detalle de Retenciones" Visible="false" runat="server">
            <asp:Button CssClass="botonesEvol" ID="btnCalcularRetenciones" runat="server" Text="Calcular Retenciones" Visible="false" OnClick="btnCalcularRetencionesr_Click" />
            <asp:HiddenField ID="hdfImporteCalculoRetenciones" runat="server" />
            <div class="data-table">
                <asp:GridView ID="gvRetenciones" AllowPaging="false" AllowSorting="false"
                    OnRowDataBound="gvRetenciones_RowDataBound" OnRowCommand="gvRetenciones_RowCommand" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo Retencion" SortExpression="TipoRetencion.Descripcion">
                            <ItemTemplate>
                                <%# Eval("TipoRetencion.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Numero Certificado" SortExpression="NumeroCertificado">
                            <ItemTemplate>
                                <%# Eval("NumeroCertificado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Concepto" SortExpression="Concepto">
                            <ItemTemplate>
                                <%# Eval("Concepto")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("ImporteTotalRetencion", "{0:C2}")%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotalRetencion" runat="server" Text="0.00"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" SortExpression="">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                    AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />
                                <img alt="Mostrar / Ocultar" id="imgExpandCollapse" style="cursor: pointer; vertical-align: middle;" src="../../Imagenes/plus.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <!-- this row has the child grid-->
                                </td>
                                </tr>
                                <tr>
                                    <td colspan="100%" style="padding: 0px;">
                                        <%-- <asp:HiddenField ID="hdfIdTipoRetencion" Value='<%#Bind("TipoRetencion.IdTipoRetencion") %>' runat="server" />
                        <asp:HiddenField ID="hdfMostrarDetalle" Value="0" runat="server" />--%>
                                        <asp:Panel ID="pnlDatosDetalles" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDatosDetalles" runat="server" AutoGenerateColumns="false" DataKeyNames="IndiceColeccion" SkinID="GrillaBasicaFormal"
                                                ShowFooter="true">
                                                <Columns>
                                                    <asp:BoundField DataField="Alicuota" DataFormatString="{0:N2}" HeaderText="Alicuota" />
                                                    <asp:BoundField DataField="ImporteTotal" DataFormatString="{0:C2}" HeaderText="Importe Total Base" />
                                                    <asp:BoundField DataField="ImporteRetencion" DataFormatString="{0:C2}" HeaderText="Importe Total Retenciones" />
                                                    <asp:TemplateField HeaderText="Detalle">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="ltlDetalleCampos" Text="Concepto" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upAnticipos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlAnticipo" GroupingText="Anticipos" runat="server" Visible="false">

            <div class="data-table">
                <asp:GridView ID="gvAnticipo" AllowPaging="false" AllowSorting="false"
                    OnRowDataBound="gvAnticipo_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Número" SortExpression="NumeroSolicitud">
                            <ItemTemplate>
                                <%# Eval("IdSolicitudPago")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Entidad.Nombre")%>
                            </ItemTemplate>
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Fecha Anticipo" SortExpression="FechaAnticipo">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observacion" SortExpression="Observacion">
                            <ItemTemplate>
                                <%# Eval("Observacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("ImporteTotal", "{0:C2}")%>
                                <asp:HiddenField ID="hdfImporteAnticipo" Value='<%#Bind("ImporteTotal") %>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteSubTotalAnticipo" runat="server" Text="0.00"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkIncluirAnticipoTodos" runat="server" onclick="checkAllRow(this); CalcularAnticipos();" Visible="false" Text="Todo" TextAlign="Left" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIncluirAnticipo" onclick="CheckRow(this); CalcularAnticipos();" Visible="false" Checked='<%# Convert.ToBoolean (Eval("IncluirEnOP")) %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upTotales" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblSubTotal" runat="server" Text="Subtotal"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtSubTotal" runat="server" Enabled="false" Text="0.00" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblImporteRetenciones" runat="server" Text="Retenciones"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteRetenciones" runat="server" Enabled="false" Text="0.00" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblAnticipo" runat="server" Text="Anticipos"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtAnticipo" runat="server" Enabled="false" Text="0.00" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblNetoAPagar" runat="server" Text="Neto a Pagar"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtTotalAPagar" runat="server" Enabled="false" Text="0.00" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<AUGE:OrdenesPagosValores ID="ctrOrdenesPagosValores" runat="server" />
<div class="form-group row">
    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblDiferencia" runat="server" Text="Diferencia"></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtDiferencia" runat="server" Enabled="false" Text="0.00" />
    </div>
</div>
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
        <ContentTemplate>
            <AUGE:Comentarios ID="ctrComentarios" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>

<asp:UpdatePanel ID="upAcciones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
        <center>
                    <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                    <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                    <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
            <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
            <asp:HiddenField ID="hfLinkFirmarDocumento" runat="server" />
                <button runat="server" visible="false" type="button" id="copyClipboard" data-tooltip="Se ha copiado el link" class="botonesEvol" onclick="CopyClipboard()">Copiar link</button>
                <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/whatsup26x26.jpg" runat="server" ID="btnWhatsAppFirmarDocumento" Visible="false"
                     OnClick="btnWhatsAppFirmarDocumento_Click" AlternateText="Enviar Whatsapp para Firmar" ToolTip="Enviar Whatsapp para Firmar" />
                <asp:Button CssClass="botonesEvol" ID="btnFirmaDigital" Visible="false" runat="server" Text="Firma Digital" OnClick="btnFirmaDigital_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnFirmaDigitalBaja" Visible="false" runat="server" Text="Eliminar Firma" OnClick="btnFirmaDigitalBaja_Click" />       
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="ordendepago" onclick="btngrabar_Click"/>
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                    </center>
    </ContentTemplate>
</asp:UpdatePanel>

