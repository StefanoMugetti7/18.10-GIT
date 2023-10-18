<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadosVisitasDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadosVisitasDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<script type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        IniciarAfiliadosWS();
        var txtReservaFechaIngreso, txtReservaFechaEgreso;
        InitApellidoSelect2();
    });

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function IniciarAfiliadosWS() {
        $.ajax({
            url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/IniciarWS',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
            success: function (response) {
                //do whatever your thingy..
            }
        });
    }

    function InitApellidoSelect2() {
        var control = $("select[name$='ddlNumeroDocumento']");
        control.select2({
            placeholder: 'Ingrese el Numero de Documento',
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
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosVisitasCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
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
                                id: item.NumeroDocumento,
                                IdAfiliado: item.IdAfiliado,
                                Apellido: item.Apellido,
                                Nombre: item.Nombre,
                                IdTipoDocumento: item.IdTipoDocumento,
                                NumeroDocumento: item.NumeroDocumento,
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
            if (e.params.data.IdAfiliado > 0) {
                var newOption = new Option(e.params.data.id, e.params.data.NumeroDocumento, true, false);
            //if (e.params.data.id > 0) {
            //    var newOption = new Option(e.params.data.NumeroDocumento, e.params.data.id, false, true);
                $("select[id$='ddlNumeroDocumento']").append(newOption).trigger('change');
                $("select[id$='ddlTipoDocumento']").val(e.params.data.IdTipoDocumento).trigger('change');
                $("input[type=text][id$='txtApellido']").val(e.params.data.Apellido);
                $("input[type=text][id$='txtNombre']").val(e.params.data.Nombre);
                //$("input[type=text][id$='txtNumeroDocumento']").val(e.params.data.NumeroDocumento);
                $("input[id*='hdfIdAfiliado']").val(e.params.data.IdAfiliado);
                //$("input[id*='hdfApellido']").val(e.params.data.Apellido);
                $("input[id*='hdfNumeroDocumento']").val(e.params.data.id);
                //$("select[id$='ddlCondicionFiscal']").val(e.params.data.IdCondicionFiscal).trigger('change');
                $("select[id$='ddlTipoDocumento']").prop("disabled", true);
                $("input[type=text][id$='txtApellido']").prop("disabled", true);
                $("input[type=text][id$='txtNombre']").prop("disabled", true);
            }
            else {
                //$("input[id*='hdfApellido']").val(e.params.data.text);
                $("input[id*='hdfNumeroDocumento']").val(e.params.data.id);
                //$("input[type=text][id$='txtNumeroDocumento']").prop("disabled", false);
                $("select[id$='ddlTipoDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtApellido']").prop("disabled", false);
                $("input[type=text][id$='txtNombre']").prop("disabled", false);
            }
        });

        control.on('select2:unselect', function (e) {
            if ($.isNumeric(e.params.data.id)) {
                $("select[id$='ddlTipoDocumento'] option:selected").val('');
                //$("input[type=text][id$='txtNumeroDocumento']").val('');
                $("input[type=text][id$='txtApellido']").val('');
                $("input[type=text][id$='txtNombre']").val('');
                $("input[id*='hdfIdAfiliado']").val('');
                $("select[id$='ddlTipoDocumento']").prop("disabled", false);
                //$("input[type=text][id$='txtNumeroDocumento']").prop("disabled", false);
                $("input[type=text][id$='txtApellido']").prop("disabled", false);
                $("input[type=text][id$='txtNombre']").prop("disabled", false);
            }
            control.val(null).trigger('change');
        });
    }

</script>

<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Numero Documento"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroDocumento" runat="server"></asp:DropDownList>
<%--        <asp:HiddenField ID="hdfApellido" runat="server" />--%>
        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
        <asp:HiddenField ID="hdfNumeroDocumento" runat="server" />
<asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroDocumento" ControlToValidate="ddlNumeroDocumento" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo Documento"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoDocumento" ControlToValidate="ddlTipoDocumento" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

    </div>
    <div class="col-sm-3"></div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" ControlToValidate="txtNombre" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvApellido" ControlToValidate="txtApellido" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <div class="col-sm-3"> 
                        
    </div>
</div>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />
<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />

    <div class="row justify-content-md-center">
            <div class="col-md-auto">
<asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
</div>


