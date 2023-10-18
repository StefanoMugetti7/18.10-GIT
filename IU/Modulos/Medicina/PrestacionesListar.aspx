<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PrestacionesListar.aspx.cs" Inherits="IU.Modulos.Medicina.PrestacionesListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            InitApellidoSelect2();
        });

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
                                    tipoDocumentoDescripcion: item.TipoDocumentoDescripcion,
                                    ObraSocial: item.IdObraSocial,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            control.on('select2:select', function (e) {

                //var newOption = new Option(e.params.data.Apellido + ", " + e.params.data.Nombre, e.params.data.id, false, true);
                //$("select[id$='ddlApellido']").append(newOption).trigger('change');
                $("input[type=text][id$='txtTipoDocumento']").val(e.params.data.tipoDocumentoDescripcion);
                $("input[type=text][id$='txtNumeroDocumento']").val(e.params.data.NumeroDocumento);
                $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
                $("input[id*='hdfAfiliado']").val(e.params.data.text);
            });
            control.on('select2:unselect', function (e) {

                $("select[id$='ddlApellido']").val(null).trigger("change");
                $("input[type=text][id$='txtTipoDocumento']").val('');
                $("input[type=text][id$='txtNumeroDocumento']").val('');
                $("input[id*='hdfIdAfiliado']").val('');
                $("input[id*='hdfAfiliado']").val('');
                control.val(null).trigger('change');
            });
        }
    </script>
    <div class="PrestacionesListar">
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
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPrestador" runat="server" Text="Prestador"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlPrestador" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPaciente" runat="server" Text="Paciente"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlApellido" runat="server">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfAfiliado" runat="server" />
                        <%--          <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvApellido" ControlToValidate="ddlApellido" ValidationGroup="PrestacionesModificarDatos" runat="server"></asp:RequiredFieldValidator>
                        --%>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" runat="server"
                            OnClick="btnExportarExcel_Click" />
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                <ItemTemplate>
                                    <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prestador" SortExpression="Prestador.ApellidoNombre">
                                <ItemTemplate>
                                    <%# Eval("Prestador.ApellidoNombre")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Paciente" SortExpression="Afiliado.ApellidoNombre">
                                <ItemTemplate>
                                    <%# Eval("Afiliado.ApellidoNombre")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Turno" SortExpression="Turno.FechaHoraDesde">
                                <ItemTemplate>
                                    <%# Eval("Turno.FechaHoraDesde", "{0:dd/MM/yyyy HH:mm}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Estado.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nomenclador" SortExpression="Nomenclador.Prestacion">
                                <ItemTemplate>
                                    <%# Eval("Nomenclador.Prestacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Obra Social" SortExpression="ObraSocial.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("ObraSocial.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                        AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
