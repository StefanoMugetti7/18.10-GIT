<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotasPedidosDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.NotasPedidosDatos" %>

<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%--<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" TagName="popUpBuscarCliente" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Facturas/Controles/FacturasBuscarPopUp.ascx" TagName="popUpFacturas" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/PresupuestosBuscarPopUp.ascx" TagName="popUpBuscarPresupuesto" TagPrefix="auge" %>
<%--<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProductoPrecio" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosDomicilioPopUp.ascx" TagName="popUpDomicilio" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesDatosCabeceraAjax.ascx" TagName="BuscarClienteAjax" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/RemitosBuscarPopUp.ascx" TagName="popUpBuscarRemito" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>


<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        intiGridDetalle();
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        $("input[type=text][id$='txtNumeroSocio']").focus();
        //$("input[type=text][id$='txtPrefijoNumeroRemito']").blur(function() { $(this).addLeadingZeros(4); });
        $("input[type=text][id$='txtNumeroRemito']").blur(function () { $(this).addLeadingZeros(8); });
    });


    $.fn.addLeadingZeros = function (length) {
        for (var el of this) {
            _value = el.value.replace(/^0+/, '');
            length = length - _value.length;
            if (length > 0) {
                while (length--) _value = '0' + _value;
            }
            el.value = _value;
        }
    };
    var TotalSinIVA = 0.00;
    var totalIVA = 0.00;
    var totalConIVA = 0.00;
    var totalPercepciones = 0.00;

   function CalcularItem() {
        var importeIVA = 0.00;
        var subTotalConIVA = 0.00;
        var TotalSinIVA = 0.00;
        var subTotalItem = 0.00;
        var totalIVA = 0.00;
        var totalConIVA = 0.00;
        var descuento = 0.00;
        var cantidadCuotas = 1; // $('select[id$="ddlCantidadCuotas"] option:selected').val();


      $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {

                //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
                //             var incluir = $(this).find('input:checkbox[id$="chkIncluir"]').is(":checked");

                var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
                var precio = $(this).find("input[id*='hdfPrecio']").val();
                var margenPorcentaje = $(this).find("input[id*='hdfMargenPorcentaje']").val();
                var margenImporte = $(this).find("input[id*='hdfMargenImporte']").val();
                var financiacionPorcentaje = $(this).find("input[id*='hdfFinanciacionPorcentaje']").val();
                var cantidad = $(this).find('input:text[id*="txtCantidad"]').maskMoney('unmasked')[0];
                var data = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
           var hdfImporteIva = $(this).find("input:hidden[id*='hdfImporteIva']");
           var hdfSubtotal = $(this).find("input:hidden[id*='hdfSubtotal']");
           var hdfSubtotalConIva = $(this).find("input:hidden[id*='hdfSubtotalConIva']");
                var alicuotaIVA = data.split('|')[1];
                var descuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]').maskMoney('unmasked')[0];
                var idProducto = $(this).find('[id*="ddlProducto"] option:selected').val();
                //var idProducto = $(this).find('input:text[id*="txtCodigo"]').val();
                //Math.Round((this.Precio * (1 + this.ListaPrecio.MargenPorcentaje / 100) + this.ListaPrecio.MargenImporte) * (1 + this.ListaPrecio.FinanciacionPorcentaje * (pCantidadCuotas == 1 ? 0 : pCantidadCuotas) / 100) + this.ListaPrecio.MargenImporte, 2);



                if (descuentoPorcentaje > 100) {
                    descuentoPorcentaje = 100;
                    $(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatMoney(descuentoPorcentaje, "", gblCantidadDecimales, "."));
                }

          if (importe && cantidad && precio && idProducto && idProducto > 0) {
              //importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
              precio = precio.replace('.', '').replace(',', '.');
              margenPorcentaje = margenPorcentaje.replace('.', '').replace(',', '.');
              margenImporte = margenImporte.replace('.', '').replace(',', '.');
              financiacionPorcentaje = financiacionPorcentaje.replace('.', '').replace(',', '.');

              //VER DE AGREGAR UNA VARIABLE PARA IMPORTE FIJO EN CUOTAS!!!
              //importe = (parseFloat(importe) * (1 + parseFloat(margenPorcentaje) / 100) + parseFloat(margenImporte)) * (1 + parseFloat(financiacionPorcentaje) * (cantidadCuotas == 1 ? 0 : cantidadCuotas) / 100) + parseFloat(margenImporte);
              //importe = parseFloat(importe).toFixed(2);
              //descuentoPorcentaje = descuentoPorcentaje.replace('.', '').replace(',', '.');
              descuento = parseFloat(parseFloat(importe) * parseFloat(cantidad) * parseFloat(descuentoPorcentaje) / 100).toFixed(2);
              $(this).find('span[id*="lblDescuentoImporte"]').text(accounting.formatMoney(descuento, gblSimbolo, 2, "."));
              subTotalItem = parseFloat(importe) * parseFloat(cantidad) - parseFloat(descuento);
              $(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, 2, "."));

              alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');

              if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                  alicuotaIVA = 0.00;
                  importeIVA = 0.00;
              } else {
                  importeIVA = parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100;
              }
               subTotalItem = round(subTotalItem, 2);
              subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
              TotalSinIVA += parseFloat(subTotalItem);
              totalIVA += parseFloat(importeIVA);

              //                 totalConIVA += parseFloat(subTotalItem) + parseFloat(importeIVA);
              $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(importeIVA, gblSimbolo, 2, "."));
              hdfImporteIva.val(importeIVA);
              hdfSubtotal.val(subTotalItem);
              hdfSubtotalConIva.val(subTotalConIVA);

              $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, 2, "."));
          }
          else if(idProducto > 0){
               hdfImporteIva.val(accounting.formatMoney(0, "", 2, "."));
              hdfSubtotal.val(accounting.formatMoney(0, "", 2, "."));
              hdfSubtotalConIva.val(accounting.formatMoney(0, "", 2, "."));
                          $(this).find('span[id*="lblDescuentoImporte"]').text(accounting.formatMoney(0.00, gblSimbolo, 2, "."));
                        $(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(0.00, gblSimbolo, 2, "."));
                      $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(0.00, gblSimbolo, 2, "."));
              $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(0.00, gblSimbolo, 2, "."));
          }
            });
       totalConIVA = parseFloat(TotalSinIVA) + parseFloat(totalIVA);


        $("input[type=text][id$='txtTotalSinIva']").val(accounting.formatMoney(TotalSinIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalIva']").val(accounting.formatMoney(totalIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(totalConIVA, gblSimbolo, 2, "."));
    }

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

    function ValidarShowConfirm(ctrl, msg) {
        if (Page_ClientValidate("Aceptar")) {
            if ($("input[id$='chkGenerarRemito']").is(':checked')) {
                showConfirm(ctrl, msg);
            } else {
                __doPostBack(ctrl.name, '');
            }
        }
    }

    /******************************************************
       Grilla Detalle
   *******************************************************/
    function intiGridDetalle() {
        var rowindex = 0;
        var idAfiliado = $("input[id*='hdfIdAfiliado']").val();
        var idUsuarioEvento = $("input[id*='hdfIdUsuarioEvento']").val();
        var idFilialPredeterminada = $("select[id*='ddlFilialEntrega']").val();
        var idMoneda = $('select[id$="ddlMonedas"] option:selected').val();

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlProducto = $(this).find('[id*="ddlProducto"]');
            var lblProductoDescripcion = $(this).find('[id*="lblProductoDescripcion"]');
            var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
            var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtDescripcion = $(this).find("input:text[id*='txtDescripcion']");
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPreUnitario = $(this).find("input:hidden[id*='hdfPreUnitario']");
            var hdfModificaPrecio = $(this).find("input:hidden[id*='hdfModificaPrecio']");
            var hdfStockeable = $(this).find("input[id*='hdfStockeable']");
            var SubTotal = $(this).find('span[id*="lblSubtotal"]');
            var ImporteIva = $(this).find('span[id*="lblImporteIva"]');
            var SubtotalConIva = $(this).find('span[id*="lblSubtotalConIva"]');
             var hdfImporteIva = $(this).find("input:hidden[id*='hdfImporteIva']");
           var hdfSubtotal = $(this).find("input:hidden[id*='hdfSubtotal']");
           var hdfSubtotalConIva = $(this).find("input:hidden[id*='hdfSubtotalConIva']");
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
                    url: '<%=ResolveClientUrl("~")%>Modulos/Facturas/FacturasWS.asmx/FacturasSeleccionarAjaxComboProductos', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: ddlProducto.val(), // search term");
                            filtro: params.term, // search term");
                            cantidadCuotas: 1,
                            idAfiliado: idAfiliado,
                            idFilialPredeterminada: $("select[id*='ddlFilialEntrega']").val(),
                            idMoneda: idMoneda,
                            idUsuarioEvento: idUsuarioEvento,
                            idListaPrecio: 0,
                        });
                    },
                    beforeSend: function (xhr, opts) {
                        if (idMoneda == "") {
                            MostrarMensaje('Debe Ingresar una Moneda para poder continuar', 'red');
                            xhr.abort();
                        }
                        if ($("select[id*='ddlFilialEntrega']").val() == "") {
                            MostrarMensaje('Debe Ingresar una filial para poder continuar', 'red');
                            xhr.abort();
                        }
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
                                    text: item.ProductoDescripcionCombo,
                                    id: item.ProductoIdProducto,
                                    productoDescripcion: item.ProductoDescripcion,
                                    precio: item.Precio,
                                    precioUnitarioSinIva: item.PrecioUnitarioSinIva,
                                    precioEditable: item.PrecioEditable,
                                    stockeable: item.Stockeable,
                                    //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });

            ddlProducto.on('select2:select', function (e) {
                lblProductoDescripcion.text(e.params.data.productoDescripcion);
                hdfProductoDetalle.val(e.params.data.productoDescripcion);
                hdfIdProducto.val(e.params.data.id);
                txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                hdfPreUnitario.val(e.params.data.precioUnitarioSinIva);
                txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                hdfModificaPrecio.val(e.params.data.precioEditable);
                hdfStockeable.val(e.params.data.stockeable);
                //CalcularPrecio();
                CalcularItem();
                //txtDescripcion.focus();
            });
            ddlProducto.on('select2:unselect', function (e) {
                lblProductoDescripcion.text('');
                hdfProductoDetalle.val('');
                hdfIdProducto.val('');
                txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                hdfPreUnitario.val('');
                hdfModificaPrecio.val('');
                hdfStockeable.val('');
                SubtotalConIva.text('');
                SubTotal.text('');
    
                hdfImporteIva.val('');
                hdfSubtotal.val('');
                hdfSubtotalConIva.val('');

                ImporteIva.text('');
               

            });

            rowindex++;
        });
    }

</script>

 <AUGE:BuscarClienteAjax ID="ctrBuscarCliente" runat="server"></AUGE:BuscarClienteAjax>
<asp:HiddenField ID="hdfIdFilialPredeterminada" runat="server" />
<asp:HiddenField ID="hdfIdUsuarioEvento" runat="server" />

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos de notas de pedido
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" ControlToValidate="txtDescripcion" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFecha" ControlToValidate="txtFecha" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialEntrega" runat="server" Text="Filial Entrega" ToolTip="Filial de la que se descontara el Stock!"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilialEntrega" runat="server" ToolTip="Filial de la que se descontara el Stock!" />
                        <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" Enabled="false" ID="rfvFilialEntrega" ControlToValidate="ddlFilialEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" AutoPostBack="true" OnSelectedIndexChanged="ddlMonedas_OnSelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvMonedas" ValidationGroup="Aceptar" ControlToValidate="ddlMonedas" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfMonedaCotizacion" Value="" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDomicilio" runat="server" Text="Domicilio de Entrega"></asp:Label>
                    <div class="col-sm-7">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlDomicilio" runat="server" Enabled="false" />
                    </div>
                    <div class="col-sm-4">
                        <AUGE:popUpDomicilio ID="ctrDomicilios" runat="server" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarDomicilio" runat="server" Text="Agregar domicilio" OnClick="btnAgregarDomicilio_Click" Visible="false" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<AUGE:CamposValores ID="ctrCamposValores" runat="server" />

<asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Detalle del Comprobante
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <AUGE:popUpBuscarPresupuesto ID="ctrBuscarPresupuestoPopUp" runat="server" />
                    <AUGE:popUpFacturas ID="ctrBuscarFacturasPopUp" runat="server" />
                    <asp:PlaceHolder ID="phAgregarItem" runat="server">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                        <div class="col-sm-1">
                            <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                        </div>
                        <div class="btn-group" role="group">
                            <button type="button" class="botonesEvol dropdown-toggle"
                                data-toggle="dropdown" aria-expanded="false">
                                Importar <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarPresupuesto" runat="server" Text="Importar Presupuesto" OnClick="btnImportarPresupuesto_Click" />
                                </li>
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarFactura"  runat="server" Text="Importar Factura" OnClick="btnImportarFactura_Click" />         
                                </li>                                
                            </ul>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvItems" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                        OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaResponsive"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Código - Producto / Descripcion" SortExpression="">
                                <ItemTemplate>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("ListaPrecioDetalle.Producto.IdProducto") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("DescripcionProducto") %>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("ListaPrecioDetalle.Producto.Familia.Stockeable") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProductoDescripcion" Visible="false" Text='<%#Bind("Descripcion")%>' runat="server"></asp:Label>
                                    <asp:TextBox CssClass="form-control" placeholder="Ingrese un detalle..." ID="txtDescripcion" Enabled="false" Text='<%#Bind("Descripcion") %>' runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("DescripcionProducto") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" Prefix="" NumberOfDecimals="2" Enabled="false" runat="server" Text='<%#Bind("Cantidad", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prec. Unitario" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" AllowNegative="true"  NumberOfDecimals="2" Enabled="false" Text='<%#Bind("PrecioUnitarioSinIva", "{0:N2}") %>' runat="server"></Evol:CurrencyTextBox>
                                    <asp:HiddenField ID="hdfPreUnitario" Value='<%#Bind("PrecioUnitarioSinIva") %>' runat="server" />
                                    <asp:HiddenField ID="hdfPrecio" Value='<%#Bind("ListaPrecioDetalle.Precio", "{0:N2}") %>' runat="server" />
                                    <asp:HiddenField ID="hdfMargenPorcentaje" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.MargenPorcentaje", "{0:N2}") %>' runat="server" />
                                    <asp:HiddenField ID="hdfMargenImporte" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.MargenImporte", "{0:N2}") %>' runat="server" />
                                    <asp:HiddenField ID="hdfFinanciacionPorcentaje" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.FinanciacionPorcentaje", "{0:N2}") %>' runat="server" />
                                    <asp:HiddenField ID="hdfModificaPrecio" Value='<%#Bind("ListaPrecioDetalle.PrecioEditable") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% Desc." ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtDescuentoPorcentual" Prefix="" Enabled="false" runat="server" Text='<%# Eval("DescuentoPorcentual")==null ? "0,00" : Eval("DescuentoPorcentual", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe Desc." HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblDescuentoImporte" runat="server" Text='<%#Bind("DescuentoImporte", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfDescuentoImporte" Value='<%#Eval("DescuentoImporte", "{0:N2}") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtotal" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfSubtotal" Value='<%#(Eval("SubTotal") == null ? string.Empty : Eval("SubTotal")).ToString().Replace(",",".")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Alícuota IVA" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe IVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfImporteIva" Value='<%#Bind("ImporteIVA")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtotal c/IVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfSubtotalConIva" Value='<%# (Eval("SubTotalConIva") == null ? string.Empty : Eval("SubTotalConIva")).ToString().Replace(",",".")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar" SortExpression="">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
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
<div class="form-group row">
    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalSinIva" runat="server" Text="Total sin iva" />
    <div class="col-sm-3">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalSinIva" Enabled="false" runat="server" />
    </div>
</div>

<div class="form-group row">
    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalIva" runat="server" Text="Total iva" />
    <div class="col-sm-3">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalIva" Enabled="false" runat="server" />
    </div>
</div>

<div class="form-group row">
    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotal" runat="server" Text="Total con iva" />
    <div class="col-sm-3">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server" />
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                                <asp:Button CssClass="botonesEvol" ID="btnCopiar" runat="server" Text="Copiar Presupuesto" OnClick="btnCopiar_Click" Visible="false" CausesValidation="false" />

            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

