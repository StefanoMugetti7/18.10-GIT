<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadoCompensacionesListar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadoCompensacionesListar" %>

<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="popUpAfiliadosBuscar" TagPrefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script lang="javascript" type="text/javascript">

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            SetTabIndexInput();
            InitApellidoSelect2();
        });

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
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosClienteCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
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
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.DescripcionCombo,
                                    id: item.IdAfiliado,
                                    descripcionAfiliado: item.DescripcionAfiliado,
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
            });
            control.on('select2:unselect', function (e) {
                $("input[id*='hdfIdAfiliado']").val('');
                $("input[id*='hdfRazonSocial']").val('');
                control.val(null).trigger('change');
            });
        }
    </script>
    <div class="CompensacionesListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Socio" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CausesValidation="false" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="True" AllowSorting="True"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="False" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Numero Socio" SortExpression="NumeroSocio">
                            <ItemTemplate>
                                <%# Eval("NumeroSocio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellido y Nombre" SortExpression="ApeNom">
                            <ItemTemplate>
                                <%# Eval("ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nro. Cuenta" SortExpression="NroCuenta">
                            <ItemTemplate>
                                <%# Eval("NumeroCuenta")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Compensacion" SortExpression="Compensacion" HeaderStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("Compensacion", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gastos Representacion" SortExpression="GastosRepresentacion" HeaderStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("GastosRepresentacion", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fondo Representacion" SortExpression="FondoRepresentacion" HeaderStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("FondoRepresentacion", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" SortExpression="total" HeaderStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# Eval("TotalAAcreditar", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
