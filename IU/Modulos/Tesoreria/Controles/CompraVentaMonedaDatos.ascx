<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompraVentaMonedaDatos.ascx.cs" Inherits="IU.Modulos.Tesoreria.Controles.CompraVentaMonedaDatos" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<script type="text/javascript" lang="javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        InitApellidoSelect2();
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

                            }
                        })
                    };
                    cache: true
                }
            }
        });

        control.on('select2:select', function (e) {

            var newOption = new Option(e.params.data.Apellido + ", " + e.params.data.Nombre, e.params.data.id, false, true);
            $("select[id$='ddlApellido']").append(newOption).trigger('change');
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
            //$("select[id$='ddlObraSocial']").val.(e.params.data.IdObraSocial);


        });

        control.on('select2:unselect', function (e) {
            $("input[id*='hdfIdAfiliado']").val('');
            //$("select[id$='ddlObraSocial']").val.('');
            control.val(null).trigger('change');

        });
    }

    function CalcularTotal() {
        var importe = $("input[type=text][id$='txtImporte']").maskMoney('unmasked')[0];
        var hdfCotizacion = $("input[id$='hdfCotizacion']").val();

        //importe = importe.replace('.', '').replace(',', '.');
        hdfCotizacion = hdfCotizacion.replace('.', '').replace(',', '.');

        var total = parseFloat(importe) * parseFloat(hdfCotizacion);
        //total = parseFloat(total);
        $("input[type=text][id$='txtImporteTotal']").val(accounting.formatMoney(total, "$ ", 2, "."));
    }

</script>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<div class="TesoreriasMovimientosDatos">
    <asp:UpdatePanel ID="upTipoOperacionConceptos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblNumero" runat="server" Text="Numero"></asp:Label>
                <div class="col-9 col-md-6 col-lg-3">
                    <asp:TextBox CssClass="form-control" ID="txtNumero" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <AUGE:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Movimiento" runat="server" />

                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblApellido" runat="server" Text="Socio"></asp:Label>
                <div class="col-9 col-md-6 col-lg-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" runat="server">
                    </asp:DropDownList>
                    <%-- <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvApellido" ControlToValidate="ddlApellido" ValidationGroup="Aceptar" runat="server" ></asp:RequiredFieldValidator>
                    --%>
                    <asp:HiddenField ID="hdfIdAfiliado" runat="server" />

                </div>


            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                <div class="col-9 col-md-6 col-lg-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoOperacion_SelectedIndexChanged" Enabled="false" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacion"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-9 col-md-6 col-lg-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" AutoPostBack="true" runat="server" Enabled="false" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMoneda" runat="server" ControlToValidate="ddlMoneda"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                <div class="col-9 col-md-6 col-lg-3">
                    <EVOL:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ErrorMessage="*"
                        ControlToValidate="txtImporte" ValidationGroup="Aceptar" />
                    
                </div>
                <asp:Label CssClass="col-3 col-md-2 col-lg-1 col-form-label" ID="lblImporteTotal" runat="server" Text="Importe Total" />
                <div class="col-9 col-md-6 col-lg-3">
                    <AUGE:CurrencyTextBox CssClass="form-control" ID="txtImporteTotal" runat="server" />
                    <asp:HiddenField ID="hdfCotizacion" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-3 col-md-3 col-lg-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                <div class="col-9 col-md-9 col-lg-7">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                        OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" Visible="false" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
