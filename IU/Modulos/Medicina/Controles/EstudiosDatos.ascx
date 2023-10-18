<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstudiosDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.EstudiosDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Medicina/Controles/TurnosBuscarPopUp.ascx" TagPrefix="AUGE" TagName="Turnos" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%--<script src="https://cdn.ckeditor.com/ckeditor5/31.0.0/decoupled-document/ckeditor.js"></script>--%>
<%--<script src="https://cdn.ckeditor.com/ckeditor5/31.0.0/decoupled-document/translations/es.js"></script>--%>

<%--<script src="../../../ckeditor5/ckeditor.js"></script>

<style>
    :root {
        --ck-sample-base-spacing: 2em;
        --ck-sample-color-white: #fff;
        --ck-sample-color-green: #279863;
        --ck-sample-color-blue: #1a9aef;
        --ck-sample-container-width: 1285px;
        --ck-sample-sidebar-width: 350px;
        --ck-sample-editor-min-height: 400px;
        --ck-sample-editor-z-index: 10;
    }

    .editor__editable,
    main .ck-editor[role='application'] .ck.ck-content,
    .ck.editor__editable[role='textbox'],
    .ck.ck-editor__editable[role='textbox'],
    .ck.editor[role='textbox'] {
        width: 100%;
        background: #fff;
        font-size: 1em;
        line-height: 1.6em;
        min-height: var(--ck-sample-editor-min-height);
        padding: 1.5em 2em;
    }

    main .ck-editor[role='application'] {
        overflow: auto;
    }

    .ck.ck-editor__editable {
        background: #fff;
        border: 1px solid hsl(0, 0%, 70%);
        width: 100%;
    }

    .ck.ck-editor__editable {
        position: relative;
        z-index: var(--ck-sample-editor-z-index);
    }

    .editor-container {
        display: flex;
        flex-direction: row;
        flex-wrap: nowrap;
        position: relative;
        width: 100%;
        justify-content: center;
    }

    body[data-editor='DecoupledDocumentEditor'] .document-editor__toolbar {
        width: 100%;
    }

    body[data-editor='DecoupledDocumentEditor'] .collaboration-demo__editable,
    body[data-editor='DecoupledDocumentEditor'] .row-editor .editor {
        width: calc(21cm + 2px);
        min-height: calc(29.7cm + 2px);
        height: fit-content;
        padding: 2cm 1.2cm;
        margin: 2.5rem;
        border: 1px hsl( 0, 0%, 82.7% ) solid;
        background-color: var(--ck-sample-color-white);
        box-shadow: 0 0 5px hsla( 0, 0%, 0%, .1 );
        box-sizing: border-box;
    }

    body[data-editor='DecoupledDocumentEditor'] .row-editor {
        display: flex;
        position: relative;
        justify-content: center;
        overflow-y: auto;
        background-color: #f2f2f2;
        border: 1px solid hsl(0, 0%, 77%);
        max-height: 700px;
    }

    body[data-editor='DecoupledDocumentEditor'] .sidebar {
        background: transparent;
        border: 0;
        box-shadow: none;
    }

    body[data-editor='InlineEditor'] .collaboration-demo__row {
        border: 0;
    }

    body[data-revision-history='true'] .ck.ck-pagination-view-line::after {
        transform: translateX(-100%) !important;
        left: -1px !important;
        right: unset !important;
    }

    body, html {
        padding: 0;
        margin: 0;
        font-family: sans-serif, Arial, Verdana, "Trebuchet MS", "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol";
        font-size: 16px;
        line-height: 1.5;
    }

    body {
        height: 100%;
        color: #2D3A4A;
    }

    body * {
        box-sizing: border-box;
    }

    a {
        color: #38A5EE;
    }

    header .centered {
        display: flex;
        flex-flow: row nowrap;
        justify-content: space-between;
        align-items: center;
        min-height: 8em;
    }

    header h1 a {
        font-size: 20px;
        display: flex;
        align-items: center;
        color: #2D3A4A;
        text-decoration: none;
    }

    header h1 img {
        display: block;
        height: 64px;
    }

    header nav ul {
        margin: 0;
        padding: 0;
        list-style-type: none;
    }

    header nav ul li {
        display: inline-block;
    }

    header nav ul li + li {
        margin-left: 1em;
    }

    header nav ul li a {
        font-weight: bold;
        text-decoration: none;
        color: #2D3A4A;
    }

    header nav ul li a:hover {
        text-decoration: underline;
    }

    .centered {
        overflow: hidden;
        max-width: var(--ck-sample-container-width);
        margin: 0 auto;
        padding: 0 var(--ck-sample-base-spacing);
    }

    footer {
        margin: calc(2*var(--ck-sample-base-spacing)) var(--ck-sample-base-spacing);
        font-size: .8em;
        text-align: center;
        color: rgba(0,0,0,.4);
    }
</style>--%>

<script>
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        InitApellidoSelect2();

        //data-editor="DecoupledDocumentEditor" data-collaboration="false" data-revision-history="false"
        //$('body').attr('data-editor', 'DecoupledDocumentEditor');
        //$("[id$='btnAceptar']").click(function () {
        //    $("[id$='hdfEditor']").val(editor.getData().toString());
        //})
    });

    function EditorSetData() {
        //const editor = document.querySelector('.editor');
        //console.log($("[id$='hdfEditor']").val());
        editor.setData($("[id$='hdfEditor']").val());
    }

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
                        filtro: params.term, // search term");

                    });
                },
                processResults: function (data, params) {
                    //return { results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            console.log(item)
                            return {
                                text: item.DescripcionCombo,
                                id: item.IdAfiliado,
                                Apellido: item.Apellido,
                                Nombre: item.Nombre,
                                IdTipoDocumento: item.IdTipoDocumento,
                                NumeroDocumento: item.NumeroDocumento,
                                IdAfiliadoTipo: item.IdAfiliadoTipo,
                                IdCondicionFiscal: item.IdCondicionFiscal,
                                CondicionFiscalDescripcion: item.CondicionFiscalDescripcion,
                                estadoDescripcion: item.EstadoDescripcion,
                                categoriaDescripcion: item.CategoriaDescripcion,
                                Correo: item.CorreoElectronico,
                                FechaNacimiento: item.FechaNacimientoTexto,
                            }
                        })
                    };
                    cache: true
                }
            }
        });

        control.on('select2:select', function (e) {
            if (e.params.data.id > 0) {
                console.log(e.params.data)
                var newOption = new Option(e.params.data.Apellido, e.params.data.id, false, true);
                $("select[id$='ddlApellido']").append(newOption).trigger('change');
                $("select[id$='ddlTipoDocumento']").val(e.params.data.IdTipoDocumento).trigger('change');
                $("input[type=text][id$='txtNumeroDocumento']").val(e.params.data.NumeroDocumento);
                $("input[type=text][id$='txtNombre']").val(e.params.data.Nombre);
                $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
                $("input[id*='hdfIdAfiliadoTipo']").val(e.params.data.IdAfiliadoTipo);
                $("input[id*='hdfApellido']").val(e.params.data.Apellido);
                $("input[type=text][id$='txtEstadoPaciente']").val(e.params.data.estadoDescripcion);
                $("input[type=text][id$='txtFechaNacimiento']").val(e.params.data.FechaNacimiento);
            }
            else {
                $("input[id*='hdfApellido']").val(e.params.data.text);
                $("select[id$='ddlTipoDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtNumeroDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtNombre']").prop("disabled", false);
            }
        });
    }
</script>
<asp:UpdatePanel ID="upEstudioMedico" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo doc."></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" Enabled="false" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoDocumento" Enabled="true" ControlToValidate="ddlTipoDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" Enabled="false" runat="server"></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroDocumento" Enabled="true" ControlToValidate="txTNumeroDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3  col-form-label" ID="Label1" runat="server" Text="Estado Paciente"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtEstadoPaciente" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" Enabled="false" runat="server"></asp:DropDownList>
                <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                <asp:HiddenField ID="hdfIdAfiliadoReferrer" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador2" ID="RequiredFieldValidator1" ControlToValidate="ddlApellido" ValidationGroup="EstudiosDatos" runat="server"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtNombre" Enabled="false" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" Enabled="true" ControlToValidate="txtNombre" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaNacimiento" runat="server" Text="Fecha de Nacimiento"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaNacimiento" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaEstudio" runat="server" Text="Fecha del Estudio"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEstudio" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvFecha" ControlToValidate="txtFechaEstudio" ValidationGroup="EstudiosDatos" runat="server"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPrestador" runat="server" Text="Prestador"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlPrestadores" AutoPostBack="true" OnSelectedIndexChanged="ddlPrestadores_SelectedIndexChanged" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvPrestadores" ControlToValidate="ddlPrestadores" ValidationGroup="EstudiosDatos" runat="server"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstadoEstudio" runat="server" Text="Estado del Estudio"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoEstudio" runat="server" Text="Tipo de Estudio"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoEstudio" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoEstudio_SelectedIndexChanged" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvTipoEstudio" ControlToValidate="ddlTipoEstudio" ValidationGroup="EstudiosDatos" runat="server"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row justify-content-md-center">
                <CKEditor:CKEditorControl ID="CKEditor1" BasePath="~/ckeditor/"
                    ImageRemoveLinkByEmptyURL="true" Toolbar="Source
                    Bold|Italic|Underline|Strike|-|Subscript|Superscript
                    NumberedList|BulletedList|-|Outdent|Indent
                    /
                    Styles|Format|Font|FontSize|TextColor|BGColor|-|About" 
                    Width="750" Height="500"  EnterMode="BR" AutoGrowOnStartup="true" AutoGrowMinHeight="500"
                    runat="server"></CKEditor:CKEditorControl>
            </div>
        <%--<asp:HiddenField ID="hdfEditor" runat="server" />--%>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<div class="form-group row">
    <div class="col-12 col-md-12 col-lg-12 row" runat="server">
        <asp:Label CssClass="col-sm-12 col-form-label" ID="lblInformeMedico" runat="server" Text="Informe Medico"></asp:Label>
    </div>
</div>
<%--<asp:TextBox CssClass="col-sm-12 col-md-12 col-lg-12" ID="__editor" TextMode="MultiLine" runat="server"></asp:TextBox>--%>
<%--<div class="centered">
    <div class="row">
        <div class="document-editor__toolbar"></div>
    </div>
    <div class="row row-editor">
        <div class="editor-container">
            <div class="editor">
                <p>This is the initial editor content.</p>
            </div>
        </div>
    </div>
</div>--%>




<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>

<asp:UpdatePanel ID="upBotones" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="EstudiosDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script>
    
    //DecoupledEditor
    //    .create(document.querySelector('.editor'), {
    //        language: 'es',
    //        licenseKey: '',
    //        initialData: $("[id$='hdfEditor']").val(),
    //    })
    //    .then(editor => {
    //        window.editor = editor;
    //        // Set a custom container for the toolbar.
    //        document.querySelector('.document-editor__toolbar').appendChild(editor.ui.view.toolbar.element);
    //        document.querySelector('.ck-toolbar').classList.add('ck-reset_all');
    //        console.log( Array.from( editor.ui.componentFactory.names() ) );
    //    })
    //    .catch(error => {
    //        console.error('Oops, something went wrong!');
    //        console.error('Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:');
    //        console.warn('Build id: bzhj04jn01i4-u9490jx48w7r');
    //        console.error(error);
    //    });

    //.create( document.querySelector( '.editor' ), {
    //    licenseKey: '',
    //    language: 'es',
				//} )
				//.then( editor => {
				//	window.editor = editor;
			
				//	// Set a custom container for the toolbar.
				//	document.querySelector( '.document-editor__toolbar' ).appendChild( editor.ui.view.toolbar.element );
				//	document.querySelector( '.ck-toolbar' ).classList.add( 'ck-reset_all' );
				//} )
				//.catch( error => {
				//	console.error( 'Oops, something went wrong!' );
				//	console.error( 'Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:' );
				//	console.warn( 'Build id: rrgqlsq4ads8-64werf9w6ou' );
				//	console.error( error );
				//} );
</script>
