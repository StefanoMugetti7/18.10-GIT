<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProveedoresCabecerasDatos.ascx.cs" Inherits="IU.Modulos.Proveedores.Controles.ProveedoresCabecerasDatos" %>
<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        InitApellidoSelect2();
    });


    function InitApellidoSelect2() {
        var control = $("select[name$='ddlNumeroProveedor']");
        //var lblCUIT = $(this).find("input:text[id*='lblCUIT']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            //width: '100%',
            language: 'es',
            //tags: true,
            //allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Proveedores/ProveedoresWS.asmx/ProveedoresCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(), // search term");
                        filtro: params.term // search term");
                    });
                },
                beforeSend: function (xhr, opts) {
                    var algo = JSON.parse(this.data); // this.data.split('"');
                    //console.log(algo.filtro);
                    if (isNaN(algo.filtro)) {
                        if (algo.filtro.length < 4) {
                            xhr.abort();
                        }
                    }
                    else {
                    }
                },
                processResults: function (data, params) {
                    //return { results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            return {

                                id: item.IdProveedor,
                                text: item.CodigoProveedor,
                                Cuit: item.TipoDocumentoDescripcion,
                                NumeroDocumento: item.NumeroDocumento,
                                RazonSocial: item.RazonSocial,
                                IdCondicionFiscal: item.IdCondicionFiscal,
                                Estado: item.EstadoDescripcion,
                                Beneficiario: item.Beneficiario


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
            $("input[id*='hdfIdProveedor']").val(e.params.data.id);//.trigger("change");
            $("input[id*='hdfNumeroProveedor']").val(e.params.data.text);
            AfiliadoSeleccionar();
        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
            $("input[id*='hdfNumeroProveedor']").val('');
        });

    }

    function AfiliadoSeleccionar() {
        __doPostBack("<%=button.UniqueID %>", "");
    }
</script>

<div class="card">
    <div class="card-header">
        Datos del Proveedor
    </div>
    <div class="card-body">
        <div class="form-group row">
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroSocio">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroProveedor" runat="server" Text="Proveedor"></asp:Label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroProveedor" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvNumeroProveedor" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroProveedor" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                        <asp:HiddenField ID="hdfNumeroProveedor" runat="server" />

                    </div>
                </div>
            </div>
            <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCUIT">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCUIT" runat="server" Text="CUIT"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvEstado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvBeneficiario">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblBeneficiario" runat="server" Text="Beneficiario"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtBeneficiario" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCondicionFiscal">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Cond. Fiscal"></asp:Label>
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCondicionFiscal" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
