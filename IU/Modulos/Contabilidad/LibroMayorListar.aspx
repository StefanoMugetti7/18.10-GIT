<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="True" CodeBehind="LibroMayorListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.LibroMayorListar" %>

<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="buscarCuentasContables" TagPrefix="auge" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitLocalizacionSelect2);
            InitLocalizacionSelect2();

        });
        function SetTabIndexInput() {
            $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }


        function InitLocalizacionSelect2() {
            var ddlEjercicioContable = $('select[id$="ddlEjercicioContable"] option:selected').val();
            var control = $("select[name$='ddlCuentaContable']");
            control.select2({
                placeholder: 'Ingrese el numero o nombre de la cuenta',
                selectOnClose: true,
                //theme: 'bootstrap4',
                minimumInputLength: 1,
                //width: '100%',
                language: 'es',
                //tags: true,
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Contabilidad/ContabilidadWS.asmx/ContabilidadSeleccionarAjaxComboCuentas', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: control.val(), // search term");
                            filtro: params.term, // search term");
                            idEjercicioContable: ddlEjercicioContable

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
                                    text: item.text,
                                    id: item.id,
                                    numeroCuenta: item.numeroCuenta,
                                    descripcion: item.text,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            control.on('select2:select', function (e) {
                $("input:text[id$='txtCuenta']").val(e.params.data.descripcion);
                $("input:hidden[id$='hdfIdCuentaContable']").val(e.params.data.id);
                $("input:hidden[id$='hdfCuentaContable']").val(e.params.data.descripcion);
                $("input:hidden[id$='hdfCuentaContableCompleta']").val(e.params.data.text);
            });
            control.on('select2:unselect', function (e) {
                control.val(null).trigger('change');
                $("input:text[id$='txtCuenta']").val("");
                $("input:hidden[id$='hdfIdCuentaContable']").val('0');
                $("input:hidden[id$='hdfCuentaContable']").val('');
                $("input:hidden[id$='hdfCuentaContableCompleta']").val('');
            });
        }
    </script>
    <div class="LibroMayor">
        <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" OnSelectedIndexChanged="ddlEjercicioContable_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" ValidationGroup="Buscar" />
                    </div>
                </div>
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
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuentaContable" runat="server" Text="Cuenta Contable"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCuentaContable" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hdfIdCuentaContable" runat="server" />
                        <asp:HiddenField ID="hdfCuentaContable" runat="server" />
                        <asp:HiddenField ID="hdfCuentaContableCompleta" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuenta" runat="server" Text="Cuenta"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCuenta" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial:" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" />
                        <%--<asp:DropDownList CssClass="form-control select2" ID="ddlFilial" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged" AutoPostBack="true" runat="server" />--%>
                    </div>
                </div>
                <%--<AUGE:buscarCuentasContables ID="buscarCuenta" runat="server" TextoBoton="Buscar cta." MostrarEliminar="false" MostrarEtiquetas="true" OnCuentasContablesBuscarSeleccionar="buscarCuenta_CuentasContablesBuscarSeleccionar" />--%>
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" OnPageIndexChanging="gvDatos_PageIndexChanging" AllowSorting="false"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="NumeroAsiento"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="Fecha" DataField="FechaAsiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaAsiento" />
                        <asp:BoundField HeaderText="Número Asiento" DataField="NumeroAsiento" ItemStyle-Wrap="false" SortExpression="NumeroAsiento" />
                        <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacion" SortExpression="TipoOperacion" />
                        <asp:BoundField HeaderText="Nro Operacion" DataField="IdRefTipoOperacion" SortExpression="NroReferencia" />
                        <%--<asp:BoundField  HeaderText="Detalle" DataField="Detalle" SortExpression="DetalleGeneral" />--%>
                        <asp:BoundField HeaderText="Debe" DataField="Debe" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" DataFormatString="{0:C2}" SortExpression="Debe" />
                        <asp:BoundField HeaderText="Haber" DataField="Haber" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" DataFormatString="{0:C2}" SortExpression="Haber" />
                        <asp:BoundField HeaderText="Saldo" DataField="Saldo" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" DataFormatString="{0:C2}" SortExpression="Haber" />
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("IdTipoOperacion") %>' />
                                <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                <asp:HiddenField ID="hdfIdAfiliado" runat="server" Value='<%#Eval("IdAfiliado") %>' />
                                <asp:HiddenField ID="hdfIdAsientoContable" runat="server" Value='<%#Eval("IdAsientoContable") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>