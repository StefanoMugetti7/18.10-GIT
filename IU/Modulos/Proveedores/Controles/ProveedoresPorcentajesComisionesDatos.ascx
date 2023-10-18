<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProveedoresPorcentajesComisionesDatos.ascx.cs" Inherits="IU.Modulos.Proveedores.Controles.ProveedoresPorcentajesComisionesDatos" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        InitApellidoSelect2();  
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitVendedorSelect2);
        InitVendedorSelect2();
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

        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
            $("input[id*='hdfNumeroProveedor']").val('');
        });

    }
    function InitVendedorSelect2() {
        var control = $("select[name$='ddlVendedor']");
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
            $("input[id*='hdfIdVendedor']").val(e.params.data.id);//.trigger("change");
            $("input[id*='hdfVendedor']").val(e.params.data.text);

        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdVendedor']").val('');//.trigger("change")
            $("input[id*='hdfVendedor']").val('');
        });

    }

</script>
    <div class="ProveedoresPorcentajesComisionesDatos">
        <div class="form-group row">
  
               <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroProveedor" runat="server" Text="Proveedor"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroProveedor" runat="server"></asp:DropDownList>
                         <asp:RequiredFieldValidator ID="rfvProveedores" CssClass="Validador" ValidationGroup="AceptarDatos"  ControlToValidate="ddlNumeroProveedor" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                        <asp:HiddenField ID="hdfNumeroProveedor" runat="server" />

                    </div>

    <asp:Label CssClass="col-form-label col-sm-1" ID="lblTipoOperacion"  runat="server" Text="Tipo Operacion"></asp:Label>
  <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlTiposOperaciones"  runat="server">
    </asp:DropDownList>
    <asp:RequiredFieldValidator ID="rfvTiposOperaciones" CssClass="Validador" ValidationGroup="AceptarDatos"  ControlToValidate="ddlTiposOperaciones" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
</div>
    <asp:Label CssClass="col-form-label col-sm-1" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
   <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" Enabled="false" runat="server">
    </asp:DropDownList></div>
</div>
           <div class="form-group row">
                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma de Cobro" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server"  />
         
            </div>

    <asp:Label CssClass="col-form-label col-sm-1" ID="lblFechaInicio" runat="server" Text="Fecha Inicio" />
  <div class="col-sm-3">  <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInicio" runat="server" />

    
        <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvFechaInicio" ControlToValidate="txtFechaInicio" ValidationGroup="AceptarDatos" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-form-label col-sm-1" ID="lblPorcentaje" runat="server" Text="% Comision"></asp:Label>
<div class="col-sm-3">    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentaje" NumberOfDecimals="4" Prefix="" runat="server"></Evol:CurrencyTextBox>
    <asp:RequiredFieldValidator ID="rfvPorcentaje" ValidationGroup="AceptarDatos" CssClass="Validador" ControlToValidate="txtPorcentaje" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
</div></div>
        <div class="form-group row">
               <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Vendedor"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlVendedor" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hdfIdVendedor" runat="server" />
                        <asp:HiddenField ID="hdfVendedor" runat="server" />

                    </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
     <div class="row justify-content-md-center">
            <div class="col-md-auto">
         
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" ValidationGroup="AceptarDatos" runat="server" Text="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" Text="Volver" />
    
      </div></div>
    </div>
                </ContentTemplate>
</asp:UpdatePanel>
        </div>