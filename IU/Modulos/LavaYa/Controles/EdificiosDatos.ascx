<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EdificiosDatos.ascx.cs" Inherits="IU.Modulos.LavaYa.Controles.EdificiosDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitLocalizacionSelect2);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(MarcarMapa);
        SetTabIndexInput();
        InitLocalizacionSelect2();
        $("select[name$='ddlServicios']").select2({
            allowClear: true,
            selectOnClose: false
        });

    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function InitLocalizacionSelect2() {

        var control = $("select[name$='ddlLocalizacion']");
        control.select2({
            placeholder: ' (EJ:Mitre 678 Carmen de Areco)',
            selectOnClose: true,
            //theme: 'bootstrap4',
            minimumInputLength: 1,
            //width: '100%',
            language: 'es',
            //tags: true,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Comunes/OpenStreetWS.asmx/ListarLocalizaciones', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        filtro: params.term,// search term");

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
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.text,
                                id: item.PlaceID,
                                PlaceID: item.PlaceID
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        control.on('select2:select', function (e) {
            $("input:hidden[id$='hdfLocalizacionCompleta']").val(e.params.data.text);
            $.ajax({
                type: "POST",
                url: '<%=ResolveClientUrl("~")%>/Modulos/Comunes/OpenStreetWS.asmx/ObtenerDatosCompletos',
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify({ 'PlaceID': e.params.data.PlaceID }),
                success: function (res) {
                    $("input:hidden[id$='hdfLongitud']").val(accounting.formatMoney(res.d.Longitude, "", 14, "."));//14 decimales son los q uso en el store
                    $("input:hidden[id$='hdfLatitud']").val(accounting.formatMoney(res.d.Latitude, "", 14, "."));
                    $("input:text[id$='txtProvincia']").val(res.d.State);
                    $("input:text[id$='txtLocalidad']").val(res.d.Town);
                    $("input:hidden[id$='hdfProvincia']").val(res.d.State);
                    $("input:hidden[id$='hdfLocalidad']").val(res.d.Town);
                    $("input:hidden[id$='hdfNumeroCasa']").val(res.d.HouseNumber);
                    $("input:hidden[id$='hdfCalleCasa']").val(res.d.Road);
                    $("input:hidden[id$='hdfCodigoPostal']").val(res.d.PostCode);
                    //var mapa = document.getElementById("MapaCoordenadas");
                    //mapa.setAttribute('src', 'https://' + 'www.google.com/maps?q=' + res.d.Latitude + ',' + res.d.Longitude + "&output=embed");
                }

            });
        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input:text[id$='txtCodigoPostal']").val("");
            $("input:text[id$='txtDireccion']").val("");
            $("input:text[id$='txtNumero']").val("");
            $("input:text[id$='txtPartido']").val("");
            $("input:text[id$='txtProvincia']").val("");
            $("input:text[id$='txtLocalidad']").val("");
            $("input:text[id$='txtLongitud']").val("");
            $("input:text[id$='txtLatitud']").val("");
            $("input:text[id$='txtLocalizacionCompleta']").val("");
            $("input:hidden[id$='hdfProvincia']").val("");
            $("input:hidden[id$='hdfLocalidad']").val("");
            $("input:hidden[id$='hdfNumeroCasa']").val("");
            $("input:hidden[id$='hdfCalleCasa']").val("");
            $("input:hidden[id$='hdfCodigoPostal']").val("");
        });
    }

    //function MarcarMapa() {
    //    var lat = $("input[type=hidden][id$='hdfLatitud']").val();
    //    var lon = $("input[type=hidden][id$='hdfLongitud']").val();
    //    var latFormat = lat.replace(",", ".");
    //    var lonFormat = lon.replace(",", ".");
    //    var mapa = document.getElementById("MapaCoordenadas");
    //    mapa.setAttribute('src', 'https://' + 'www.google.com/maps?q=' + latFormat + ',' + lonFormat + "&output=embed");
    //}

    function SumarMaquinas(tipo) {
        if (tipo == "lavadora") {
            var control = $("input:text[id$='txtCantidadMaquinasLavado']");
            $("input:hidden[id$='hdfCantidadMaquinasLavado']").val((parseInt(control.val()) + 1));
            control.val((parseInt(control.val()) + 1));
        }
        if (tipo == "secadora") {
            var control = $("input:text[id$='txtCantidadMaquinasSecado']");
            $("input:hidden[id$='hdfCantidadMaquinasSecado']").val((parseInt(control.val()) + 1));
            control.val((parseInt(control.val()) + 1));
        }
    }

    function RestarMaquinas(tipo) {
        if (tipo == "lavadora") {
            var control = $("input:text[id$='txtCantidadMaquinasLavado']");
            $("input:hidden[id$='hdfCantidadMaquinasLavado']").val((parseInt(control.val()) - 1));
            control.val((parseInt(control.val()) - 1));
        }
        if (tipo == "secadora") {
            var control = $("input:text[id$='txtCantidadMaquinasSecado']");
            $("input:hidden[id$='hdfCantidadMaquinasSecado']").val((parseInt(control.val()) - 1));
            control.val((parseInt(control.val()) - 1));
        }
    }

</script>

<div class="PuntosVentas">
    <div class="form-group row">
        <asp:HiddenField ID="hdfLatitud" runat="server" />
        <asp:HiddenField ID="hdfLongitud" runat="server" />
        <asp:HiddenField ID="hdfNumeroCasa" runat="server" />
        <asp:HiddenField ID="hdfCalleCasa" runat="server" />
        <asp:HiddenField ID="hdfCodigoPostal" runat="server" />
        <div class="col-sm-12">
            <div class="form-group row">
                <div class="col-sm-4" runat="server" id="dvCodigo" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Codigo" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvDescripcion" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Condominio"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvEstado" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-12" runat="server" id="dvLocalizacion" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLocalizacion" runat="server" Text="Dirección"></asp:Label>
                        <div class="col-sm-7">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlLocalizacion" runat="server"></asp:DropDownList>
                            <asp:HiddenField ID="hdfLocalizacionCompleta" runat="server" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button CssClass="botonesEvol" ID="btnVerMapa" runat="server" Text="Ver Ubicacion"
                                OnClick="btnVerMapa_Click" Visible="true" CausesValidation="false" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvLocalidad" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblLocalidad" runat="server" Text="Localidad"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtLocalidad" Enabled="false" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="hdfLocalidad" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvProvincia" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblProvincia" runat="server" Text="Provincia"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtProvincia" Enabled="false" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="hdfProvincia" runat="server" />
                        </div>
                    </div>
                </div>


                <div class="col-sm-4" runat="server" id="dvContacto" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblContacto" runat="server" Text="Contacto"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtContacto" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvPartido" visible="false">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblPartido" Visible="false" runat="server" Text="Partido"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtPartido" Visible="false" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvDireccion" visible="false">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblDireccion" runat="server" Visible="false" Text="Direccion"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass=" form-control" ID="txtDireccion" Enabled="false" Visible="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvNumero" visible="false">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblNumero" runat="server" Visible="false" Text="Numero"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" Enabled="false" Visible="false" runat="server"></AUGE:NumericTextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvCodigoPostal" visible="false">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblCodigoPostal" runat="server" Visible="false" Text="Codigo Postal"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoPostal" Enabled="false" Visible="false" runat="server" Text=""></AUGE:NumericTextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dbLongitud" visible="false">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblLongitud" runat="server" Text="Longitud"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <AUGE:CurrencyTextBox Prefix="" NumberOfDecimal="14" CssClass="form-control" ID="txtLongitud" Enabled="false" runat="server" Text=""></AUGE:CurrencyTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvLongitud" ControlToValidate="txtLongitud" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvLatitud" visible="false">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblLatitud" runat="server" Text="Latitud"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <AUGE:CurrencyTextBox Prefix="" NumberOfDecimal="14" CssClass="form-control" ID="txtLatitud" Enabled="false" runat="server" Text=""></AUGE:CurrencyTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvLatitud" ControlToValidate="txtLatitud" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvFechaAlta" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha PEM" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvContrato" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblContrato" runat="server" Text="Contrato"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlContrato" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvHorario" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblHorario" runat="server" Text="Horario" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlHorario" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvUnidadesFuncionales" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblUnidadesFuncionales" runat="server" Text="UF"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <AUGE:CurrencyTextBox CssClass="form-control" ID="txtUnidadesFuncionales" Enabled="true" ThousandsSeparator="" NumberOfDecimals="0" MaxLength="3" runat="server" Text=""></AUGE:CurrencyTextBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvServicios" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblServicios" runat="server" Text="Servicios"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:ListBox CssClass="form-control select2" ID="ddlServicios" SelectionMode="multiple" runat="server"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvSistemaPago" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblSistemaPago" runat="server" Text="Sistema de Pago"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlSistemaPago" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvCantidadMaquinasLavado" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblCantidadMaquinasLavado" runat="server" Text="Maquinas de Lavado" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtCantidadMaquinasLavado" Enabled="false" runat="server" />
                            <asp:HiddenField ID="hdfCantidadMaquinasLavado" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvCantidadMaquinasSecado" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblCantidadMaquinasSecado" runat="server" Text="Maquinas de Secado" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtCantidadMaquinasSecado" Enabled="false" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="hdfCantidadMaquinasSecado" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="divRelleno" visible="true">
                </div>
                <div class="col-sm-4" runat="server" id="dvFrecuenciaRecaudacion" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblFrecuenciaRecaudacion" runat="server" Text="Frecuencia Recaudacion" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtFrecuenciaRecaudacion" ThousandsSeparator="" NumberOfDecimals="0" Prefix="" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4" runat="server" id="dvFrecuenciaAspiracion" visible="true">
                    <div class="row">
                        <asp:Label CssClass="col-lg-3 col-md-3 col-sm-3 col-form-label" ID="lblFrecuenciaAspiracion" runat="server" Text="Frecuencia Aspiracion" />
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtFrecuenciaAspiracion" ThousandsSeparator="" NumberOfDecimals="0" Prefix="" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--        <asp:UpdatePanel ID="UpMapa" runat="server">
            <ContentTemplate>
                <div class="col-sm-4" id="divMapa">
                    <iframe class="iframe" id="MapaCoordenadas" src="https://www.google.com/maps?q=-34.6037346,-58.3817651&output=embed" height="300" widht="400"></iframe>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
        <div class="col-sm-8">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblImagen" runat="server" Visible="true" Text="Codigo QR:"></asp:Label>
                <div class="col-sm-4">
                    <asp:Image ID="imgLogo" runat="server" Width="128px" Visible="true" />
                </div>
            </div>
        </div>
    </div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpMaquinas" HeaderText="Maquinas">
            <ContentTemplate>
                <asp:UpdatePanel ID="upMaquinas" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-1 col-md-1 col-sm-1 col-form-label" ID="lblMaquinas" runat="server" Text="Maquinas"></asp:Label>
                            <div class="col-sm-5">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlMaquinas" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMaquinas" ControlToValidate="ddlMaquinas" ValidationGroup="CargarMaquina" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-1">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtNumeroMaquina" placeholder="Numero" ThousandsSeparator="" NumberOfDecimals="0" Prefix="" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroMaquina" ControlToValidate="txtNumeroMaquina" ValidationGroup="CargarMaquina" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button CssClass="botonesEvol" ID="btnCargarMaquina" runat="server" Text="Cargar Maquina"
                                    OnClick="btnCargarMaquina_Click" ValidationGroup="CargarMaquina" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarMaquina" runat="server" Text="Nueva Maquina"
                                    OnClick="btnAgregarMaquina_Click" Visible="true" CausesValidation="false" />
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvMaquinas" OnRowCommand="gvMaquinas_RowCommand"
                                OnRowDataBound="gvMaquinas_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo" SortExpression="TipoMaquina">
                                        <ItemTemplate>
                                            <%# Eval("TipoMaquina")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Numero" SortExpression="Numero">
                                        <ItemTemplate>
                                            <%# Eval("Numero")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Marca" SortExpression="Marca">
                                        <ItemTemplate>
                                            <%# Eval("Marca")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Modelo" SortExpression="Modelo">
                                        <ItemTemplate>
                                            <%# Eval("Modelo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnEliminar" Visible="true"
                                                AlternateText="Eliminar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpPuntosVentas" HeaderText="Puntos de Venta">
            <ContentTemplate>
                <asp:UpdatePanel ID="upPuntosVentas" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:GridView ID="gvPuntosVentas" OnRowCommand="gvPuntosVentas_RowCommand"
                                OnRowDataBound="gvPuntosVentas_RowDataBound" DataKeyNames="IdPuntoVenta"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Barrio" SortExpression="Barrio">
                                        <ItemTemplate>
                                            <%# Eval("Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Direccion" SortExpression="Direccion">
                                        <ItemTemplate>
                                            <%# Eval("Localizacion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpRequerimientos" HeaderText="Requerimientos">
            <ContentTemplate>
                <asp:UpdatePanel ID="upRequerimientos" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="table-responsive">
                            <asp:GridView ID="gvRequerimientos" OnRowCommand="gvRequerimientos_RowCommand"
                                OnRowDataBound="gvRequerimientos_RowDataBound" DataKeyNames="IdRequerimiento"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Id" SortExpression="IdRequerimiento">
                                        <ItemTemplate>
                                            <%# Eval("IdRequerimiento")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Titulo" SortExpression="Titulo">
                                        <ItemTemplate>
                                            <%# Eval("Nombre")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha Apertura" SortExpression="FechaRequerimiento">
                                        <ItemTemplate>
                                            <%# Eval("FechaRequerimiento")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                        <ItemTemplate>
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Consultar" ToolTip="Consultar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos"
            HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Nuevo" OnClick="btnAgregar_Click" visible="false" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>




