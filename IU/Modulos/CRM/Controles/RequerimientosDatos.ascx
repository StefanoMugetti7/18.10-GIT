<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequerimientosDatos.ascx.cs" Inherits="IU.Modulos.CRM.Controles.RequerimientosDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        SetTabIndexInput();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitOrigenSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitDestinoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        InitOrigenSelect2();
        InitDestinoSelect2();
        InitApellidoSelect2();
    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function InitOrigenSelect2() {
        var control = $("select[name$='ddlElegirEntidadOrigen']");
        control.select2({
            placeholder: ' Ingrese Entidad',
            selectOnClose: true,
            minimumInputLength: 1,
            language: 'es',
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/CRM/RequerimientosWS.asmx/EntidadesOrigenCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: 1, // search term");
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
                                text: item.Tabla,
                                id: item.IdRefTabla,
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        control.on('select2:select', function (e) {
            $("input[type=hidden][id$='hdfElegirOrigen']").val(e.params.data.id);
        });
        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[type=hidden][id$='hdfElegirOrigen']").val("");
        });
    }
    function InitDestinoSelect2() {
        var control = $("select[name$='ddlElegirEntidadDestino']");
        control.select2({
            placeholder: ' Ingrese Destino',
            selectOnClose: true,
            minimumInputLength: 1,
            language: 'es',
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/CRM/RequerimientosWS.asmx/EntidadesDestinoCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica")); 
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: 1, // search term");
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
                                text: item.TablaAccion,
                                id: item.IdRefTablaAccion,
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        control.on('select2:select', function (e) {
            $("input[type=hidden][id$='hdfElegirDestino']").val(e.params.data.id);
        });
        control.on('select2:unselect', function (e) {
            $("input[type=hidden][id$='hdfElegirDestino']").val("");
        });
    }
    function InitApellidoSelect2() {
        var control = $("select[name$='ddlNumeroSocio']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            language: 'es',
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosClienteCombo',
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(),
                        filtro: params.term
                    });
                },
                beforeSend: function (xhr, opts) {
                    var algo = JSON.parse(this.data);
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
                                text: item.DescripcionCombo,
                                id: item.IdAfiliado,
                                Cuit: item.TipoDocumentoDescripcion,
                                NumeroDocumento: item.NumeroDocumento,
                                RazonSocial: item.RazonSocial,
                                IdCondicionFiscal: item.IdCondicionFiscal,
                                Estado: item.EstadoDescripcion,
                                Detalle: item.Detalle,
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        control.on('select2:select', function (e) {
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);//.trigger("change");
            AfiliadoSeleccionar();
        });
        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdAfiliado']").val('');//.trigger("change")
        });
    }

    function AfiliadoSeleccionar() {
        __doPostBack("<%=button.UniqueID %>", "");
    }
</script>

<div class="form-group row">
    <div class="col-lg-2"></div>
    <div class="col-lg-10" id="Botonera" visible="true" runat="server">
        <div class="btn-group" role="group">
            <button type="button" class="botonesEvol dropdown-toggle"
                data-toggle="dropdown" aria-expanded="false">
                Agregar <span class="caret"></span>
            </button>
            <ul class="dropdown-menu" role="menu">
                <li>
                    <asp:Button CssClass="dropdown-item" Style="margin-right: 5%;" ID="btnSeguimiento" Visible="true" OnClick="btnSeguimiento_Click" runat="server" Text="Seguimiento" />
                <li>
                    <asp:Button CssClass="dropdown-item" ID="btnDocumento" OnClick="btnDocumento_Click" Visible="true" runat="server" Text="Documento" />
                <li>
                    <asp:Button CssClass="dropdown-item" ID="btnSolucion" runat="server" OnClick="btnSolucion_Click" Visible="true" Text="Solucion" />
            </ul>
        </div>
    </div>
</div>
<div class="form-group row">
    <div class="col-lg-2 col-md-2 col-sm-12">
        <div class="list-group" id="Submenu" runat="server">
            <asp:Button CssClass="botonesEvol" ID="btnIntervencion" OnClick="btnIntervencion_Click" runat="server" Text="Intervenciones" />
            <asp:Button CssClass="botonesEvol" ID="btnIncidente" OnClick="btnIncidente_Click" runat="server" Text="Requerimiento" />
            <asp:Button CssClass="botonesEvol" ID="btnTareasProyecto" runat="server" Visible="false" Text="Tareas de Proyecto" />
            <asp:Button CssClass="botonesEvol" ID="btnEstadisticas" runat="server" Visible="false" Text="Estadisticas" />
            <asp:Button CssClass="botonesEvol" ID="btnHistorico" runat="server" Visible="false" Text="Historico" />
            <asp:Button CssClass="botonesEvol" ID="btnDocumentoListar" OnClick="btnDocumentoListar_Click" runat="server" Text="Documentos" />
        </div>
    </div>
    <div class="col-lg-10 col-md-10 col-sm-10">
        <div class="TabIncidentes" id="tpIncidentes" runat="server">
            <div class="DatosCliente" runat="server" id="dvDatosCliente" visible="true">

                <div class="card-header">
                    Datos del Cliente
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblNumeroSocio" runat="server" Text="Cliente"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblCUIT" runat="server" Text="CUIT"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblEstadoAfiliado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtEstadoAfiliado" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtDetalle" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Cond. Fiscal"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCondicionFiscal" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="card-header">
                Datos del Requerimiento
            </div>
            <div class="Requerimientos" id="dvDatosRequerimientos" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblFechaRequerimiento" runat="server" Text="Fecha Requerimiento"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRequerimiento" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblFechaResolucion" runat="server" Text="Fecha Resolucion"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaResolucion" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblFechaInternaResolucion" runat="server" Text="Fecha Interna Resolucion"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInternaResolucion" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblUsuarioAlta" Visible="false" runat="server" Text="Usuario Alta"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtUsuarioAlta" Enabled="false" Visible="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-10">
                    <hr widht="10%" />
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblOrigenSolicitud" runat="server" Text="Origen Solicitud"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlOrigenSolicitud" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblTecnico" runat="server" Text="Asignado a "></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTecnico" runat="server"></asp:DropDownList>
                    </div>

                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblPrioridad" runat="server" Text="Prioridad"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlPrioridad" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPrioridad" ControlToValidate="ddlPrioridad" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblEsPrivado" runat="server" Text="Privado" />
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:CheckBox ID="chkEsPrivado" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblTipoRequerimiento" runat="server" Text="Tipo Requerimiento"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoRequerimiento" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoOperacion_SelectedIndexChanged" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoRequerimiento" ControlToValidate="ddlTipoRequerimiento" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
                    <div class="col-lg-3 col-md-6 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCategoria" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-sm-10">
                    <hr widht="10%" />
                </div>
                <AUGE:CamposValores ID="ctrCamposValores" ccsContainer="col-12 col-md-6 col-lg-6" cssLabel="col-sm-4 col-form-label" cssCol="col-sm-8" runat="server" />
                <div class="col-sm-10">
                    <hr widht="10%" />
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Titulo"></asp:Label>
                    <div class="col-lg-8 col-md-9 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-2 col-md-6 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion:"></asp:Label>
                    <div class="col-lg-8 col-md-9 col-sm-9">
                        <CKEditor:CKEditorControl ID="CKEditor1" BasePath="~/ckeditor/" ImageRemoveLinkByEmptyURL="true" Toolbar="Full" runat="server"></CKEditor:CKEditorControl>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCKEditor" ControlToValidate="CKEditor1" EnterMode="BR" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>

        <div class="tpIntervenciones" id="tpIntervenciones" visible="false" runat="server">
            <div class="card-header" runat="server" visible="false" id="TituloAgregar">
                Agregar
            </div>
            <div class="col-lg-10 col-md-10 col-sm-10" id="DatosSeguimiento" visible="false" runat="server">
                <CKEditor:CKEditorControl ID="CKEditorIntervencion" BasePath="~/ckeditor/" Width="100%" ImageRemoveLinkByEmptyURL="true" Toolbar="Full" runat="server"></CKEditor:CKEditorControl>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCKEditorIntervencion" ControlToValidate="CKEditorIntervencion" EnterMode="BR" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
               <center>
                    <asp:Button CssClass="botonesEvol" ID="btnAceptarSeguimiento" runat="server" Text="Agregar" OnClick="btnAgregarSeguimiento_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelarSeguimiento" runat="server" Text="Cancelar" OnClick="btnCancelarSeguimiento_Click" ValidationGroup="AceptarSeguimiento" />
                  </center>
            </div>
            <div class="col-lg-10 col-md-10 col-sm-10" id="DatosSolucion" visible="false" runat="server">
                <CKEditor:CKEditorControl ID="CKEditorSolucion" BasePath="~/ckeditor/" Width="100%" ImageRemoveLinkByEmptyURL="true" Toolbar="Full" runat="server"></CKEditor:CKEditorControl>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCKEditorSolucion" ControlToValidate="CKEditorSolucion" EnterMode="BR" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                <center>

                    <asp:Button CssClass="botonesEvol" ID="btnAceptarSolucion" runat="server" Text="Agregar" OnClick="btnAgregarSolucion_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelarSolucion" runat="server" Text="Cancelar" OnClick="btnCancelarSolucion_Click" ValidationGroup="AceptarSolucion" />
                </center>
            </div>
            <asp:UpdatePanel ID="upCards" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <asp:Literal ID="ltrCards" runat="server"></asp:Literal>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="tpTareas" id="tpTareas" runat="server">
        </div>

        <div class="tpEstadisticas" id="tpEstadisticas" runat="server">
        </div>

        <div class="tpHistorico" id="tpHistorico" runat="server">
        </div>
        <div class="card-header" runat="server" visible="false" id="TituloAgregarDocumentos">
            AGREGAR DOCUMENTOS
        </div>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpArchivos" Visible="false" HeaderText="Documentos">
                <ContentTemplate>
                    <AUGE:Archivos ID="ctrArchivos" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
</div>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <center>
            <auge:popupmensajespostback id="popUpMensajes" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </center>
    </ContentTemplate>
</asp:UpdatePanel>
