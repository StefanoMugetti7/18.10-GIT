<%@ Page Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpCajas.master" AutoEventWireup="true" CodeBehind="CajasMovimientosAutomaticos.aspx.cs" EnableEventValidation="false" Inherits="IU.Modulos.Tesoreria.CajasMovimientosAutomaticos" Title="" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        InitApellidoSelect2();
    });


    function InitApellidoSelect2() {
        var control = $("select[name$='ddlNumeroSocio']");
        var idUsuarioEvento = $("input[id*='hdfIdUsuarioEvento']").val();
        control.select2({
            placeholder: 'Ingrese el codigo o Nombre',
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
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosClienteComboCaja', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(), // search term");
                        filtro: params.term, // search term");
                        idUsuarioEvento: idUsuarioEvento,
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
                                Cuit: item.TipoDocumentoDescripcion,
                                NumeroDocumento: item.NumeroDocumento,
                                RazonSocial: item.RazonSocial,
                                IdCondicionFiscal: item.IdCondicionFiscal,
                                Estado: item.EstadoDescripcion,
                                Detalle: item.Detalle,
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
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);//.trigger("change");

        });

        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdAfiliado']").val('');//.trigger("change")
        });

    }


    </script>
    <div cssclass="CajasMovimientosAutomaticos">
        <asp:HiddenField ID="hdfIdUsuarioEvento" runat="server" />
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operacion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposOperaciones" runat="server"
                            OnSelectedIndexChanged="ddlTiposOperaciones_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                          <div class="col-sm-1">
                                    <div class="btn-group" role="group">
                                        <asp:PlaceHolder ID="phBotones" runat="server">
                                        <button type="button" class="botonesEvol dropdown-toggle"
                                            data-toggle="dropdown" aria-expanded="false">
                                            Agregar <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <%--<asp:Literal ID="ltrMenu" runat="server"></asp:Literal>--%>
                                            <li>
                                                <asp:Button CssClass="dropdown-item" ID="btnAgregarMovimiento" runat="server" OnClick="btnAgregarMovimiento_Click" Text="Agregar Movimiento" />
                                            <li>
                                                <asp:Button ID="btnCompraVenta" CssClass="dropdown-item" Visible="true" runat="server" Text="Compra Venta de Moneda" OnClick="btnCompraVenta_Click" />
                                            </li>
                                                <li>
                                                <asp:Button ID="btnAgregarOrdenescobrosFacturas" CssClass="dropdown-item" Visible="true" runat="server" Text="Cobro de Facturas" OnClick="btnAgregarOrdenescobrosFacturas_Click" />
                                            </li>
                                        </ul>
                                        </asp:PlaceHolder>
                                    </div>
                                </div>
               <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Socio"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Ingresar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        </div>
                    <div class="col-sm-1">
                     <asp:Button CssClass="botonesEvol" ID="btnSocioIngresar" runat="server" OnClick="btnIngresarSocio_Click" Text="Ingresar" ValidationGroup="Ingresar" /></div>
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnSocioOperaciones" runat="server" OnClick="btnSocioOperaciones_Click" Text="Buscar" />
                    </div>
                </div>

<%--                <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound">--%>

                       <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdRefTipoOperacion, TipoOperacionIdTipoOperacion"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true" >
                    <Columns>

               
                        <asp:TemplateField HeaderText="Moneda" SortExpression="CajaMoneda.Moneda.Descripcion">
                            <ItemTemplate>
                                <%# Eval("CajaMonedaMonedaDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                        <%--<asp:BoundField  HeaderText="Descripcion" DataField="Descripcion" SortExpression="Descripcion" />--%>
                  <%--      <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion.TipoOperacion">
                            <ItemTemplate>
                                <%# Eval("TipoOperacionTipoOperacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="Tipo Operación" DataField="TipoOperacionTipoOperacion" ItemStyle-HorizontalAlign="Left" SortExpression="TipoOperacionTipoOperacion" />

                        <asp:BoundField HeaderText="Número Referencia" DataField="IdRefTipoOperacion" ItemStyle-HorizontalAlign="Left" SortExpression="IdRefTipoOperacion" />
                        <asp:BoundField HeaderText="TipoOperacionIdTipoOperacion" DataField="TipoOperacionIdTipoOperacion" Visible="false" ItemStyle-HorizontalAlign="Left" SortExpression="TipoOperacionIdTipoOperacion" />

                        
                 

                        <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                      
                                    <%# string.Concat(Eval("AfiliadoApellido"), (", "), Eval("AfiliadoNombre"))%>
                                </ItemTemplate>
                            </asp:TemplateField>   
                        <%--<asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                    <ItemTemplate>
                        <%# Eval("TipoValor.TipoValor")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                       <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("CajaMonedaMonedaMoneda"),  Eval("Importe", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Confirmar Movimiento" ToolTip="Confirmar Movimiento" />
                            </ItemTemplate>
                              <FooterTemplate>
                    <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text=""></asp:Label>
                </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
