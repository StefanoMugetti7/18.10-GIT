<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PuntosVentasDatos.ascx.cs" Inherits="IU.Modulos.LavaYa.Controles.PuntosVentasDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/LavaYa/Controles/DiasHorasModificarDatosPopUp.ascx" TagName="popUpDiasHoras" TagPrefix="auge" %>


<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitLocalizacionSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(MarcarMapa);
        SetTabIndexInput();
        InitLocalizacionSelect2();

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
                                //PostCode: item.PostCode,
                                //District: item.District,
                                //State: item.State,
                                //HouseNumber: item.HouseNumber,
                                //Road: item.Road,
                                //Town: item.Town,
                                ////Latitude: item.Latitude,
                                //Longitude: item.Longitude,
                                PlaceID: item.PlaceID
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        control.on('select2:select', function (e) {
            ///* $("input:text[id$='txtCodigoPostal']").val(e.params.data.PostCode);*/
            // $("input:text[id$='txtDireccion']").val(e.params.data.Road);
            // $("input:text[id$='txtNumero']").val(e.params.data.HouseNumber);
            ///* $("input:text[id$='txtPartido']").val(e.params.data.District);*/
            // $("input:text[id$='txtProvincia']").val(e.params.data.State);
            $("input:hidden[id$='hdfLocalizacionCompleta']").val(e.params.data.text);
            $.ajax({
                type: "POST",
                url: '<%=ResolveClientUrl("~")%>/Modulos/Comunes/OpenStreetWS.asmx/ObtenerDatosCompletos',
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify({ 'PlaceID': e.params.data.PlaceID }),
                //beforeSend: function (xhr, opts) {
                //    if (txtReservaFechaIngreso.val() == '' || ddlHoteles.val() == '') {

                //        xhr.abort();
                //    }
                //},
                success: function (res) {
                    $("input:hidden[id$='hdfLongitud']").val(accounting.formatMoney(res.d.Longitude, "", 14, "."));//14 decimales son los q uso en el store
                    $("input:hidden[id$='hdfLatitud']").val(accounting.formatMoney(res.d.Latitude, "", 14, "."));
                    $("input:text[id$='txtProvincia']").val(res.d.State);
                    $("input:text[id$='txtLocalidad']").val(res.d.Town);
                    var mapa = document.getElementById("MapaCoordenadas");
                    mapa.setAttribute('src', 'https://' + 'www.google.com/maps?q=' + res.d.Latitude + ',' + res.d.Longitude + "&output=embed");
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
        });
    }

    function MarcarMapa() {
        var lat = $("input[type=hidden][id$='hdfLatitud']").val();
        var lon = $("input[type=hidden][id$='hdfLongitud']").val();
        var latFormat = lat.replace(",", ".");
        var lonFormat = lon.replace(",", "."); 
        var mapa = document.getElementById("MapaCoordenadas");
        mapa.setAttribute('src', 'https://' + 'www.google.com/maps?q=' + latFormat + ',' + lonFormat + "&output=embed");
    }

    function CargarDetalle(id) {

        $.ajax({
            type: "POST",
            url: '<%=ResolveClientUrl("~")%>/Modulos/Comunes/OpenStreetWS.asmx/BuscarLocalizacionPorId',
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify({ 'id': id }),
            success: function (res) {
                var txtCodigoPostal = $("input:text[id$='txtCodigoPostal']");
                var txtDireccion = $("input:text[id$='txtDireccion']");
                var txtNumero = $("input:text[id$='txtNumero']");
                var txtProvincia = $("input:text[id$='txtProvincia']");
                var txtPartido = $("input:text[id$='txtPartido']");
                var txtLocalidad = $("input:text[id$='txtLocalidad']");

                txtCodigoPostal.val(res.d.DisplayName);
                txtDireccion.val(e.params.data.Address.Road);
                txtNumero.val(e.params.data.Address.HouseNumber);
                txtProvincia.val(e.params.data.Address.City);
                txtPartido.val(e.params.data.Address.District);
                txtLocalidad.val(e.params.data.Address.State);
            }
        });
    }

</script>

<div class="PuntosVentas">
    <div class="form-group row">
        <asp:HiddenField ID="hdfLatitud" runat="server" />
        <asp:HiddenField ID="hdfLongitud" runat="server" />
        <div class="col-sm-8">
            <div class="form-group row">
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Barrio"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblLocalizacion" runat="server" Text="Localizacion"></asp:Label>
                <div class="col-sm-10">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlLocalizacion" runat="server"></asp:DropDownList>
                    <asp:HiddenField ID="hdfLocalizacionCompleta" runat="server" />
                </div>

            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblProvincia" runat="server" Text="Provincia"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtProvincia" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblLocalidad" runat="server" Text="Localidad"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtLocalidad" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblContacto" runat="server" Text="Contacto"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtContacto" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" Visible="false" ID="lblPartido" runat="server" Text="Partido"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtPartido" Enabled="false" Visible="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblDireccion" Visible="false" runat="server" Text="Direccion"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <asp:TextBox CssClass=" form-control" ID="txtDireccion" Enabled="false" Visible="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblNumero" Visible="false" runat="server" Text="Numero"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" Enabled="false" Visible="false" runat="server"></AUGE:NumericTextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblLatitud" runat="server" Text="Latitud" Visible="false"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <AUGE:CurrencyTextBox Prefix="" NumberOfDecimal="14" CssClass="form-control" ID="txtLatitud" Enabled="false" Visible="false" runat="server" Text=""></AUGE:CurrencyTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvLatitud" ControlToValidate="txtLatitud" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblLongitud" runat="server" Visible="false" Text="Longitud"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <AUGE:CurrencyTextBox Prefix="" NumberOfDecimal="14" CssClass="form-control" ID="txtLongitud" Enabled="false" Visible="false" runat="server" Text=""></AUGE:CurrencyTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvLongitud" ControlToValidate="txtLongitud" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-2 col-md-2 col-sm-3 col-form-label" ID="lblCodigoPostal" Visible="false" runat="server" Text="Codigo Postal"></asp:Label>
                <div class="col-lg-4 col-md-4 col-sm-9">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoPostal" Enabled="false" Visible="false" runat="server" Text=""></AUGE:NumericTextBox>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpMapa" runat="server">
            <ContentTemplate>
                <div class="col-sm-4" id="divMapa">
                    <iframe class="iframe" id="MapaCoordenadas" src="https://www.google.com/maps?q=-34.6037346,-58.3817651&output=embed" height="300" widht="400"></iframe>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />

    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
        SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpDiasHoras"
                HeaderText="Horarios">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDiasHoras" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarDiasHoras" runat="server" Text="Agregar Horario"
                                OnClick="btnAgregarDiasHoras_Click" CausesValidation="false" />
                            <br />
                            <AUGE:popUpDiasHoras ID="ctrDiasHoras" runat="server" />
                            <asp:GridView ID="gvDiasHoras" OnRowCommand="gvDiasHoras_RowCommand"
                                OnRowDataBound="gvDiasHoras_RowDataBound" DataKeyNames="IdPuntoVenta"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Dia" SortExpression="Dia">
                                        <ItemTemplate>
                                            <%# Eval("Dia")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Hora desde" DataField="HoraDesde" SortExpression="HoraDesde" />
                                    <asp:BoundField HeaderText="Hora hasta" DataField="HoraHasta" SortExpression="HoraHasta" />
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                AlternateText="Consultar" ToolTip="Consultar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
    </asp:TabContainer>
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




