<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestadoresModificarDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.PrestadoresModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Medicina/Controles/DiasHorasModificarDatosPopUp.ascx" TagName="popUpDiasHoras" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Medicina/Controles/EspecializacionesModificarDatosPopUp.ascx" TagName="popUpEspecializaciones" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


<style>
    .wrapper {
        top: 5px;
        position: relative;
        width: 400px;
        height: 200px;
        -moz-user-select: none;
        -webkit-user-select: none;
        -ms-user-select: none;
        user-select: none;
    }

    .signature-pad {
        position: absolute;
        left: 0;
        top: 0;
        width: 400px;
        height: 200px;
        border: 1px solid;
    }
</style>

<script src="https://cdn.jsdelivr.net/npm/signature_pad@2.3.2/dist/signature_pad.min.js"></script>
<script type="text/javascript">






    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        InitApellidoSelect2();



        var canvas = document.querySelector("canvas");

        var signaturePad = new SignaturePad(canvas, {
            backgroundColor: 'transparent', //'#ffffff',
            penColor: 'rgb(0, 0, 0)'
        });

        function resizeCanvas() {
            var ratio = Math.max(window.devicePixelRatio || 1, 1);
            canvas.width = canvas.offsetWidth * ratio;
            canvas.height = canvas.offsetHeight * ratio;
            canvas.getContext("2d").scale(ratio, ratio);
            signaturePad.clear(); // otherwise isEmpty() might return incorrect value
        }

        window.addEventListener("resize", resizeCanvas);
        resizeCanvas();

        $("[id$='btnAgregarFirma']").click(function () {
            if (signaturePad.isEmpty()) {
                MostrarMensaje("Firme dentro del recuadro.", "red");
                return;
            }
            else {
                signaturePad.removeBlanks();
                var data = signaturePad.toDataURL('image/png');
                $("[id$='hiddenSigData']").val(data);
                //$("[id$='btnAceptar']").click();
            }
        });

        $("[id$='btnClean']").click(function () {
            signaturePad.clear();
        });

        var hdfFirma = $("[id$='hdfFirma']");


        if (hdfFirma.length > 0) {

            var Firma = hdfFirma.val();

            signaturePad.fromDataURL(Firma);

        }

    });




    SignaturePad.prototype.removeBlanks = function () {
        var imgWidth = this._ctx.canvas.width;
        var imgHeight = this._ctx.canvas.height;
        var imageData = this._ctx.getImageData(0, 0, imgWidth, imgHeight),
            data = imageData.data,
            getAlpha = function (x, y) {
                return data[(imgWidth * y + x) * 4 + 3]
            },
            scanY = function (fromTop) {
                var offset = fromTop ? 1 : -1;

                // loop through each row
                for (var y = fromTop ? 0 : imgHeight - 1; fromTop ? (y < imgHeight) : (y > -1); y += offset) {

                    // loop through each column
                    for (var x = 0; x < imgWidth; x++) {
                        if (getAlpha(x, y)) {
                            return y;
                        }
                    }
                }
                return null; // all image is white
            },
            scanX = function (fromLeft) {
                var offset = fromLeft ? 1 : -1;

                // loop through each column
                for (var x = fromLeft ? 0 : imgWidth - 1; fromLeft ? (x < imgWidth) : (x > -1); x += offset) {

                    // loop through each row
                    for (var y = 0; y < imgHeight; y++) {
                        if (getAlpha(x, y)) {
                            return x;
                        }
                    }
                }
                return null; // all image is white
            };

        var cropTop = scanY(true),
            cropBottom = scanY(false),
            cropLeft = scanX(true),
            cropRight = scanX(false);

        var relevantData = this._ctx.getImageData(cropLeft, cropTop, cropRight - cropLeft, cropBottom - cropTop);
        this._canvas.width = cropRight - cropLeft;
        this._canvas.height = cropBottom - cropTop;
        this._ctx.clearRect(0, 0, cropRight - cropLeft, cropBottom - cropTop);
        this._ctx.putImageData(relevantData, 0, 0);
    };




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
                                IdTipoDocumento: item.IdTipoDocumento,
                                NumeroDocumento: item.NumeroDocumento,
                                BuscarTurnosSocio: item.DescripcionCombo,
                                ObraSocial: item.IdObraSocial
                            }
                        })
                    };
                    cache: true;
                }
            }
        });

        control.on('select2:select', function (e) {

            var newOption = new Option(e.params.data.Apellido + ", " + e.params.data.Nombre, e.params.data.id, false, true);
            $("select[id$='ddlApellido']").append(newOption).trigger('change');
            $("input[type=text][id$='txtTipoDocumento']").val(e.params.data.IdTipoDocumento);
            $("input[type=text][id$='txtNumeroDocumento']").val(e.params.data.NumeroDocumento);
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);

        });
    }

    function UploadComplete()
    {
        hidePopupProgressBar();
        //__doPostBack("<%=button.UniqueID %>", "");
        document.getElementById('<%= button.ClientID %>').click();     
    }

</script>


<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<div class="PrestadoresModificarDatos">
    <div class="form-group row">




        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtApellido" TabIndex="2" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvApellido" ControlToValidate="txtApellido" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNombre" TabIndex="3" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" ControlToValidate="txtNombre" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMatricula" runat="server" Text="Matricula"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtMatricula" TabIndex="4" runat="server"></asp:TextBox>
        </div>


    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" TabIndex="5" runat="server">
            </asp:DropDownList>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número documento"></asp:Label>
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" TabIndex="6" runat="server"></AUGE:NumericTextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvtNumeroDocumento" ControlToValidate="txtNumeroDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>

        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCUIL" runat="server" Text="Número CUIL"></asp:Label>
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtCUIL" TabIndex="7" runat="server"></AUGE:NumericTextBox>
            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCUIL" ControlToValidate="txtCUIL" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
        </div>
    </div>

    <div class="form-group row">


        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSexo" runat="server" Text="Sexo"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlSexo" TabIndex="9" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSexo" ControlToValidate="ddlSexo" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>

        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
            </asp:DropDownList>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaNacimiento" runat="server" Text="Fecha Nacimiento"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaNacimiento" TabIndex="11" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaIngreso" runat="server" Text="Fecha Ingreso"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaIngreso" TabIndex="12" runat="server"></asp:TextBox>

        </div>
    </div>

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstadoCivil" runat="server" Text="Estado civil"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstadoCivil" TabIndex="13" runat="server">
            </asp:DropDownList>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Correo electronico"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" TabIndex="14" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaBaja" runat="server" Text="Fecha Baja"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaBaja" Enabled="false" runat="server"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaBaja" Enabled="false" ControlToValidate="txtFechaBaja" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>

    <asp:Label CssClass="labelEvol" ID="lblFoto" Visible="false" runat="server" Text="Foto"></asp:Label>
    <asp:Label CssClass="labelEvol" ID="lblFirma" Visible="false" runat="server" Text="Firma"></asp:Label>

    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <div>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpFirma"
                HeaderText="Firma">
                <ContentTemplate>

                    <div id="dvFirmaCrear" runat="server">
                        <div class="row justify-content-md-center" id="divSignature" runat="server">
                            <div class="col-md-auto">
                                <div class="wrapper">
                                    <canvas id="signature-pad" class="signature-pad" width="400" height="200" />
                                </div>
                                <br />
                                <input type="hidden" id="hiddenSigData" runat="server" name="hiddenSigData" />
                                <input type="hidden" id="hdfFirma" runat="server" name="hdfFirma" />
                            </div>
                        </div>
                        <div class="row justify-content-md-center">
                            <div>
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarFirma" runat="server" Text="Agregar Firma" CausesValidation="false"
                                    OnClick="btnAgregarFirma_Click" />
                                <button type="button" class="botonesEvol" id="btnClean" runat="server">Limpiar</button>
                                <asp:AsyncFileUpload ID="afuArchivo" OnClientUploadStarted="showPopupProgressBar" OnClientUploadComplete="UploadComplete" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server"
                                    UploadingBackColor="#CCFFFF" UploaderStyle="Traditional"  />
                                <asp:Button CssClass="botonesEvol" ID="button" OnClick="btnPostBack" runat="server" Style="display: none;" />
                            </div>
                        </div>
                    </div>
                    <div id="dvFirma" runat="server">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Image ID="imgLogo" runat="server" Visible="true" />
                            </div>
                        </div>

                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnEliminarFirma" runat="server" Text="Eliminar Firma" CausesValidation="false"
                                    OnClick="btnEliminarFirma_Click" />
                            </div>
                        </div>


                    </div>

                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpEspecialidades"
                HeaderText="Especialidades">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upEspecialidades" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarEspecialidad" runat="server" Text="Agregar Especialidad"
                                OnClick="btnAgregarEspecialidad_Click" CausesValidation="false" />

                            <AUGE:popUpEspecializaciones ID="ctrEspecialidades" runat="server" />
                            <asp:GridView ID="gvEspecialidades" OnRowCommand="gvEspecialidades_RowCommand"
                                OnRowDataBound="gvEspecialidades_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Especialidad" SortExpression="Especializacion.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Especializacion.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Predeterminado" DataField="EspecializacionPorDefecto" SortExpression="EspecializacionPorDefecto" />
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
            <asp:TabPanel runat="server" ID="tpDiasHoras"
                HeaderText="Horarios">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDiasHoras" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarDiasHoras" runat="server" Text="Agregar Horario"
                                OnClick="btnAgregarDiasHoras_Click" CausesValidation="false" />
                            <br />
                            <br />
                            <AUGE:popUpDiasHoras ID="ctrDiasHoras" runat="server" />
                            <asp:GridView ID="gvDiasHoras" OnRowCommand="gvDiasHoras_RowCommand"
                                OnRowDataBound="gvDiasHoras_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Dia" SortExpression="Dia.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Dia.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Hora desde" DataField="HoraDesde" SortExpression="HoraDesde" />
                                    <asp:BoundField HeaderText="Hora  hasta" DataField="HoraHasta" SortExpression="HoraHasta" />
                                    <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                                        <ItemTemplate>
                                            <%# Eval("Filial.Filial")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Especialidad" SortExpression="Especializacion.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("EspecializacionString")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
            <asp:TabPanel runat="server" ID="tpComentarios"
                HeaderText="Comentarios">
                <ContentTemplate>
                    <AUGE:Comentarios ID="ctrComentarios" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpArchivos"
                HeaderText="Archivos">
                <ContentTemplate>
                    <AUGE:Archivos ID="ctrArchivos" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpHistorial"
                HeaderText="Auditoria">
                <ContentTemplate>
                    <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>

        </asp:TabContainer>
    </div>


    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">

                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                        OnClick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
