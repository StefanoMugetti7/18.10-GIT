<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcopiosModificarDatos.ascx.cs" Inherits="IU.Modulos.Acopios.Controles.AcopiosModificarDatos" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);       
        intiGridDetalle();
    });

    /******************************************************
        Grilla Detalle
    *******************************************************/
    function intiGridDetalle() {
        var hdfTabla = $("input[id$='hdfTabla']");
        var txtCodigo = $("input[id$='txtCodigo']");
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            
            var ddlDetalleImportes = $(this).find('[id*="ddlDetalleImportes"]');
            var hdfIdRefTabla = $(this).find("input[id*='hdfIdRefTabla']");
            var hdfFecha = $(this).find("input[id*='hdfFecha']");
            var hdfImporte = $(this).find("input[id*='hdfImporte']");
            var lblFecha = $(this).find("span[id*='lblFecha']");
            var lblImporte = $(this).find("span[id*='lblImporte']");
           
            ddlDetalleImportes.select2({
                placeholder: 'Ingrese el Número de Comprobante',
                selectOnClose: true,
                theme: 'bootstrap4',
                width: '100%',
                minimumInputLength: 1,
                language: 'es',
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Acopios/AcopiosWS.asmx/AcopiosImportesSeleccionarAjaxComboComprobantes',
                    delay: 500,
                    data: function (params) {
                        var acopio = {};
                        acopio.Filtro = params.term;
                        acopio.IdRefTabla = txtCodigo.val();
                        acopio.Tabla = hdfTabla.val();
                        return "{acopio:" + JSON.stringify(acopio) + "}";
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.Detalle,
                                    id: item.IdRefTabla,
                                    importe: item.Importe,
                                    fecha: item.FechaDDMMAAAA
                                }
                            })
                        };
                        cache: true
                    }
                }
            });

            ddlDetalleImportes.on('select2:select', function (e) {
                hdfIdRefTabla.val(e.params.data.id);
                hdfFecha.val(toDate(e.params.data.fecha));
                hdfImporte.val(e.params.data.importe);
                lblFecha.text(e.params.data.fecha);
                lblImporte.text(accounting.formatMoney(e.params.data.importe, gblSimbolo, 2, "."));
                CalcularItem();
            });
            ddlDetalleImportes.on('select2:unselect', function (e) {
                hdfIdRefTabla.val('');
                hdfFecha.val('');
                hdfImporte.val('');
                lblFecha.text('');
                lblImporte.text('');
                CalcularItem();
            });
        });
    }

    function CalcularItem() {
        var total = 0.00;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var importe = $(this).find("input[id*='hdfImporte']").val();
            if (importe) {
                importe = importe.replace('.', '').replace(',', '.');
                total+= parseFloat(importe);
            }
        });
        $("input[type=text][id$='txtImporteTotal']").val(accounting.formatMoney(total, gblSimbolo, 2, "."));
    }

</script>
<div id="deshabilitarControles">
<div class="card">
    <div class="card-header">
        Datos del Acopio
    </div>
    <div class="card-body">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Codigo"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox ID="txtCodigo" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hdfTabla" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="Label2" runat="server" Text="Razon Social"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox ID="txtRazonSocial" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIdAcopio" runat="server" Text="Numero"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox ID="txtIdAcopio" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox ID="txtDescripcion" CssClass="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescripcion" ValidationGroup="Aceptar" ControlToValidate="txtDescripcion" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe Total"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox ID="txtImporteTotal" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
</div>
<div class="card">
    <div class="card-header">
        Detalles
    </div>
    <div class="card-body">
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
        SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpDetalleConsumos"
            HeaderText="Detalle de Consumos">
            <ContentTemplate>

            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpDetalleImportes"
            HeaderText="Detalle de Importes">
            <ContentTemplate>
        <div class="form-group row">
            <div class="col-sm-4">
                <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
            </div>
            <div class="col-sm-8"></div>
        </div>
        <asp:GridView ID="gvDatos" DataKeyNames="IdAcopioImporte" AllowPaging="false" AllowSorting="false"
            runat="server" SkinID="GrillaBasicaFormal"
            AutoGenerateColumns="false" ShowFooter="true" >
            <Columns>
                <asp:TemplateField HeaderText="Nro Comprobante">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlDetalleImportes" CssClass="form-control select2" runat="server"></asp:DropDownList>
                        <asp:HiddenField ID="hdfIdRefTabla" Value='<%#Bind("IdRefTabla") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha">
                    <ItemTemplate>
                        <asp:Label CssClass="col-form-label" ID="lblFecha" runat="server" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>'></asp:Label>
                        <asp:HiddenField ID="hdfFecha" Value='<%#Bind("Fecha") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Importe">
                    <ItemTemplate>
                        <asp:Label CssClass="col-form-label" ID="lblImporte" runat="server" Text='<%#Bind("Importe", "{0:C2}") %>'></asp:Label>
                        <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe")%>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>                        
            </Columns>
        </asp:GridView>
            </ContentTemplate>
        </asp:TabPanel>
        </asp:TabContainer>
    </div>
</div>
</div>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <%--<asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptarContinuar" runat="server" Text="Aplicar" OnClick="btnAceptarContinuar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Guardar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
