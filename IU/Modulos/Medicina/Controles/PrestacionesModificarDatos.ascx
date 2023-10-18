<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestacionesModificarDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.PrestacionesModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Medicina/Controles/TurnosBuscarPopUp.ascx" TagPrefix="AUGE" TagName="Turnos" %>

<script type="text/javascript">
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

<asp:UpdatePanel ID="UpAfiliado" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label1" runat="server" Text="Tipo doc."></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" Enabled="false" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoDocumento" Enabled="true" ControlToValidate="ddlTipoDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" Enabled="false" runat="server"></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroDocumento" Enabled="true" ControlToValidate="txtNumeroDocumento" ValidationGroup="AfiliadosModificarDatosAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3  col-form-label" ID="lblEstadoPaciente" runat="server" Text="Estado Paciente"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtEstadoPaciente" Enabled="false" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" runat="server"></asp:DropDownList>
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
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaEstudio" runat="server" Text="Fecha Prestacion"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvFecha" ControlToValidate="txtFecha" ValidationGroup="EstudiosDatos" runat="server"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPrestador" runat="server" Text="Prestador"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlPrestadores" runat="server" OnSelectedIndexChanged="ddlPrestadores_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvPrestadores" ControlToValidate="ddlPrestadores" ValidationGroup="EstudiosDatos" runat="server"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstadoPrestacion" runat="server" Text="Estado Prestacion"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEspecializacion" runat="server" Text="Especializacion"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEspecializaciones" OnSelectedIndexChanged="ddlEspecializaciones_SelectedIndexChanged" AutoPostBack="true" runat="server" />
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTurno" runat="server" Text="Turno"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTurnos" runat="server" />
            </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblObraSocial" runat="server" Text="Obra Social"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlObraSocial" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNomenclador" runat="server" Text="Nomenclador"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlNomenclador" runat="server"></asp:DropDownList>
            </div>
        </div>
        <AUGE:CamposValores ID="CamposValores1" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
    <div class="col-sm-7">
        <asp:TextBox CssClass="form-control" ID="txtObservaciones" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
</div>
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
    SkinID="MyTab">
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
<asp:UpdatePanel ID="upBotones" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="PrestacionesModificarDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                    OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
