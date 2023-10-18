<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OrdenesPagosListar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.OrdenesPagosListar" %>

<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="AfiliadosDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="ProveedoresDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script lang="javascript" type="text/javascript">

        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            SetTabIndexInput();
            InitApellidoSelect2();
          $("input[type=text][id$='txtFechaDesde']").focus();
        });

        function SetTabIndexInput() {
            $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }

        function CheckRow(objRef) {
            var checked = true;
            $(objRef).closest('table').find('tr').not(':last').each(function () {
                if (!$(this).find('[id*="chkIncluir"]').is(':checked')) {
                    checked = false;
                    return false;
                }
            });
            $(objRef).closest('tr').closest('table').closest('div').prev('div').find('[id*="chkIncluirTodos"]').prop('checked', checked);
        }

        function checkAllRow(objRef) {
            $(objRef).closest('tr').closest('table').closest('div').next('div').find('tr').not(':last').each(function () {
                $(this).find('[id*="chkIncluir"]').prop('checked', objRef.checked);
            });
        }

        function InitApellidoSelect2() {
            var ddlEntidad = $("select[name$='ddlEntidades']");
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
                    url: '<%=ResolveClientUrl("~")%>/Modulos/CuentasPagar/CuentasPagarWS.asmx/BuscarPorEntidad', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            IdEntidad: ddlEntidad.val(), // search term");
                            filtro: params.term // search term");

                        });
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.text,
                                    id: item.id,
                                    //Cuit: item.TipoDocumentoDescripcion,
                                    //NumeroDocumento: item.NumeroDocumento,
                                    RazonSocial: item.RazonSocial,
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
                //CargarTipoPuntoVenta();
                //AfiliadoSeleccionar();
            });

            control.on('select2:unselect', function (e) {
                $("input[id*='hdfIdAfiliado']").val('');
      
                control.val(null).trigger('change');
                //CargarTipoPuntoVenta();
            
            });

        }
       
    </script>
    <div class="OrdenesPagos">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="Aceptar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEntidad" runat="server" Text="Entidad a Pagar"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEntidades" AutoPostBack="true" OnSelectedIndexChanged="ddlEntidades_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEntidad" ValidationGroup="Aceptar" ControlToValidate="ddlEntidades" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>

                        </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Proveedor" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                        <%--  <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />--%>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />

                    </div>



                    <%--  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>--%>
                    <%--  <asp:HiddenField ID="hdfNumeroProveedor" runat="server" />--%>
                    <AUGE:AfiliadosDatos ID="ctrAfiliados" runat="server" />
                    <AUGE:ProveedoresDatos ID="ctrProveedores" runat="server" />


                </div>
            
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumero" runat="server" Text="Número Orden Pago"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Número Factura"></asp:Label>
                    <div class="col-sm-1">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroFactura" runat="server" maxlength="4" />
                    </div>
                    <div class="col-sm-2">

                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" maxlength="8" />
                    </div>

                </div>

                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial de Pago"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-8"></div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                    </div>

                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                        <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdOrdenPago, EstadoIdEstado"
                            runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Número" SortExpression="IdOrdenPago">
                                    <ItemTemplate>
                                        <%# Eval("IdOrdenPago")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Beneficiario" SortExpression="EntidadNombreGrilla">
                                    <ItemTemplate>
                                        <%# Eval("EntidadNombre")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cuit" SortExpression="EntidadCuitGrilla">
                                    <ItemTemplate>
                                        <%# Eval("EntidadCuit")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                                    <ItemTemplate>
                                        <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Pago" SortExpression="FechaPago">
                                    <ItemTemplate>
                                        <%# Eval("FechaPago", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                        <ItemTemplate>
                            <%# Eval("TipoValor.TipoValor")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Importe Total" SortExpression="ImporteTotal">
                                    <ItemTemplate>
                                        <%# Eval("ImporteTotal", "{0:C2}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                    <ItemTemplate>
                                        <%# Eval("EstadoDescripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnModificar"
                                            AlternateText="Modificar Solicitud" ToolTip="Modificar Solicitud" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" ID="btnAutorizar"
                                            AlternateText="Autorizar" ToolTip="Autorizar" Visible="false" />
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                            AlternateText="Anular Orden Pago" ToolTip="Anular Orden Pago" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Evol:EvolGridView>
                    </div>
                </div>
            </ContentTemplate>
              <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
