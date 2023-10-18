<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AfiliadoCompensacionesDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.AfiliadoCompensaciones" %>

<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="popUpAfiliadosBuscar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        InitApellidoSelect2();
    });
    function CalcularItem() {
        var compensacion = $("input:text[id$='txtCompensacion']").maskMoney('unmasked')[0];
        var gastoRep = $("input:text[id$='txtGastosRepresentacion']").maskMoney('unmasked')[0];
        var fondoRep = $("input:text[id$='txtFondoRepresentacion']").maskMoney('unmasked')[0];
        var total = 0.00;

        total = parseFloat(compensacion) + parseFloat(gastoRep) - parseFloat(fondoRep);
        $("input[type=text][id$='txtTotalAAcreditar']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));
    }

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function InitApellidoSelect2() {
        var control = $("select[name$='ddlNumeroSocio']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            //width: '100%',
            language: 'es',
            //tags: true,
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
                    //return {results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.DescripcionCombo,
                                id: item.IdAfiliado,
                                descripcionAfiliado: item.DescripcionCombo,
                                //Cuit: item.TipoDocumentoDescripcion,
                                //NumeroDocumento: item.NumeroDocumento,
                                //RazonSocial: item.RazonSocial,
                                //IdCondicionFiscal: item.IdCondicionFiscal,
                                //Estado: item.EstadoDescripcion,
                                //Detalle: item.Detalle,
                                //Apellido: item.Apellido,
                                //Nombre: item.Nombre,
                                //IdTipoDocumento: item.IdTipoDocumento,
                                //TipoDocumento: item.DescripcionTipoDocumento,
                                //NumeroDocumento: item.NumeroDocumento,
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
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
            $("input[id*='hdfRazonSocial']").val(e.params.data.descripcionAfiliado);
            AfiliadoSeleccionar();
        });
        control.on('select2:unselect', function (e) {
            $("input[id*='hdfIdAfiliado']").val('');
            $("input[id*='hdfRazonSocial']").val('');
            control.val(null).trigger('change');
            //CargarTipoPuntoVenta();
        });

        function AfiliadoSeleccionar() {
            __doPostBack("<%=button.UniqueID %>", "");
        }
    }
</script>

<div class="AfiliadosCompensacionesDatos">
    <asp:UpdatePanel ID="upAfiliado" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Socio" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                    <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-form-label col-sm-1" ID="lblCompensacion" runat="server" Text="Compensacion"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtCompensacion" runat="server"></Evol:CurrencyTextBox>
                </div>
                <asp:Label CssClass="col-form-label col-sm-1" ID="lblGastosRepresentacion" runat="server" Text="Gastos Representacion"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtGastosRepresentacion" runat="server"></Evol:CurrencyTextBox>
                </div>
                <asp:Label CssClass="col-form-label col-sm-1" ID="lblFondoRepresentacion" runat="server" Text="Fondo Representacion"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtFondoRepresentacion" runat="server"></Evol:CurrencyTextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-form-label col-sm-1" ID="lblTotalAAcreditar" runat="server" Text="Total A Acreditar"></asp:Label>
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox ID="txtTotalAAcreditar" CssClass="form-control" runat="server" Enabled="false"></Evol:CurrencyTextBox>
                </div>
                <asp:Label CssClass="col-form-label col-sm-1" ID="lblCuenta" runat="server" Text="Cuenta"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCuenta" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuenta" ControlToValidate="ddlCuenta" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-form-label col-sm-1" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        </asp:TabPanel>
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
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>