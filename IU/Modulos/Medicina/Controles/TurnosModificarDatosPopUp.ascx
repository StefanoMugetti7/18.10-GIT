<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnosModificarDatosPopUp.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.TurnosModificarDatosPopUp" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagPrefix="AUGE" TagName="Afiliados" %>
<script type="text/javascript" lang="javascript">
    function ShowModalBuscarTurnos() {
        $("[id$='modalBuscarTurnos']").modal('show');
    }

    function HideModalBuscarTurnos() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalBuscarTurnos']").modal('hide');
    }

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
                                IdTipoDocumento: item.IdTipoDocumento,
                                NumeroDocumento: item.NumeroDocumento,
                                BuscarTurnosSocio: item.DescripcionCombo,
                                ObraSocial: item.IdObraSocial
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
            $("input[type=text][id$='txtTipoDocumento']").val(e.params.data.IdTipoDocumento);
            $("input[type=text][id$='txtNumeroDocumento']").val(e.params.data.NumeroDocumento);
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
            //$("select[id$='ddlObraSocial']").val.(e.params.data.IdObraSocial);
        });

        control.on('select2:unselect', function (e) {
            $("input[type=text][id$='txtTipoDocumento']").val('');
            $("input[type=text][id$='txtNumeroDocumento']").val('');
            $("input[id*='hdfIdAfiliado']").val('');
            $("select[id$='ddlPrestadores']").val('');
            //$("select[id$='ddlObraSocial']").val.('');
            control.val(null).trigger('change');
        });
    }
</script>
<AUGE:Afiliados ID="ctrAfiliados" runat="server" />

<div class="modal" id="modalBuscarTurnos" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Buscar Turnos</h5>
                    </div>
                    <div class="modal-body">
                        <div>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
                                </div>
                                <%--<div class="Calendario">
                    <asp:Image ID="imgFecha" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                    <asp:CalendarExtender ID="ceFecha" runat="server" Enabled="true" 
                        TargetControlID="txtFecha" PopupButtonID="imgFecha" Format="dd/MM/yyyy"></asp:CalendarExtender>
                </div>--%>
                                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblApellido" runat="server" Text="Socio"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" runat="server">
                                    </asp:DropDownList>
                                    <%--  <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvApellido" ControlToValidate="ddlApellido" ValidationGroup="Aceptar" runat="server" ></asp:RequiredFieldValidator>
                                    --%>
                                    <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo Documento"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control select2" ID="txtTipoDocumento" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Numero Documento"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control select2" ID="txtNumeroDocumento" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblPrestador" runat="server" Text="Prestador"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control select2" ID="txtPrestador" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEspecializacion" runat="server" Text="Especialización"></asp:Label>
                            <div class="col-sm-4">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEspecializacion" runat="server">
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvEspecializacion" ControlToValidate="ddlEspecializacion" ValidationGroup="Aceptar" runat="server" ></asp:RequiredFieldValidator>
                                --%>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblObraSocial" runat="server" Text="Obra Social"></asp:Label>
                            <div class="col-sm-4">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlObraSocial" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control select2" ID="txtObservaciones" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                            <div class="col-sm-4">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-1">
                                <asp:Button CssClass="botonesEvol" ID="btnAtender" runat="server" Text="Atender"
                                    OnClick="btnAtender_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-5"></div>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                                OnClick="btnAceptar_Click" ValidationGroup="TurnosModificarDatosPopUp" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                                OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>