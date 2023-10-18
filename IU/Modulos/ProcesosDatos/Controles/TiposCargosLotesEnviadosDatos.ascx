<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposCargosLotesEnviadosDatos.ascx.cs" Inherits="IU.Modulos.ProcesosDatos.Controles.TiposCargosLotesEnviadosDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>



<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        SetTabIndexInput();
    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function round(value, exp) {
        if (typeof exp === 'undefined' || +exp === 0)
            return Math.round(value);

        value = +value;
        exp = +exp;

        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0))
            return NaN;

        // Shift
        value = value.toString().split('e');
        value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp)));

        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp));
    }
    function ValidarTotal(control) {
        var acum = 0;
        var tope = $("input:text[id*='" + control + "']").closest('table').closest('td').find("input:hidden[id*='hdfImporteAplicar']").maskMoney('unmasked')[0];
        var lbl = $("input:text[id*='" + control + "']").closest('table').closest('tr').find('span[id*="lblImporteTotal"]');
        $("input:text[id*='" + control + "']").closest('table').find('tr').not(':first').not(':last').each(function () {
            acum += $(this).find("input:text[id*='txtImporte']").maskMoney('unmasked')[0]
            if (acum > tope) {
                $(this).find("input:text[id*='txtImporte']").val("$0,00");
                ValidarTotal(control);//SI SE PASA EJECUTA NUEVAMENTE LA FNC PARA QUE CAMBIE CORRECTAMENTE EL LABEL
            }
        });
        if (acum <= tope) {
            var redondeado = (tope - acum);
            lbl.text("Importe Pendiente: $" + round(redondeado,2));
        }
        if (acum == tope) {
            lbl.text("Importe Pendiente: $0.00");
        }
    }
    function InitFooterDetalle(control) {
        var acum = 0;
        var tope = $("span[id*='" + control + "']").closest('table').closest('td').find("input:hidden[id*='hdfImporteAplicar']").maskMoney('unmasked')[0];
        var lbl = $("span[id*='" + control + "']");
        $(lbl).closest('table').find('tr').not(':first').not(':last').each(function () {
            acum += parseFloat($(this).find("input:text[id*='txtImporte']").maskMoney('unmasked')[0]);
        });
        if (acum <= tope) {
            var redondeado = (tope - acum);
            lbl.text("Importe Pendiente: $" + round(redondeado, 2));
        }
        if (acum == tope) {
            lbl.text("Importe Pendiente: $0.00");
        }
    }
    function EsconderDiv() {
        $("[id*='divArchivo']").attr('Style', 'display: none;');
    }
</script>

<div class="PuntosVentas">
    <asp:UpdatePanel ID="upProceso" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProcesos" runat="server" Text="Procesos"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlProceso" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProceso_SelectedIndexChanged"></asp:DropDownList>
                    <asp:Label CssClass="col-sm-4 col-form-label font-weight-bold" ID="lblNombreProceso" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body" runat="server" id="divParametros">
                <asp:Panel ID="tablaParametros" CssClass="form-group" runat="server">
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="divArchivo" id="divArchivo" runat="server">
        <asp:Panel ID="pnlArchivo" runat="server" Visible="true">
            <asp:AsyncFileUpload ID="AsyncFileUpload1" Width="211px" runat="server"
                OnUploadedComplete="FileUploadComplete" Height="21px" Font-Size="Larger" />
        </asp:Panel>
    </div>
    <asp:UpdatePanel ID="upDatosFiltrar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Filtro" Visible="false"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" Visible="false"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblImputacion" runat="server" Visible="false" Text="Imputacion" />
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlImputacion" Visible="false" runat="server" />
                        </div>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" Visible="false" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <Evol:EvolGridView ID="gvDatosLote" OnRowCommand="gvDatosLote_RowCommand" AllowPaging="true"
                OnRowDataBound="gvDatosLote_RowDataBound" DataKeyNames="IdTipoCargoLoteEnviadoDetalle"
                runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatosLote_PageIndexChanging" Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="Nro. Socio">
                        <ItemTemplate>
                            <%# Eval("NumeroSocio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nro. Documento">
                        <ItemTemplate>
                            <%# Eval("NumeroDocumento")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nro. Legajo">
                        <ItemTemplate>
                            <%# Eval("MatriculaIAF")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apellido y Nombre">
                        <ItemTemplate>
                            <%# Eval("ApellidoNombre")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Enviado">
                        <ItemTemplate>
                            <%# Eval("ImporteEnviado", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe a Imputar">
                        <ItemTemplate>
                            <%# Eval("ImporteAAplicar", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observacion">
                        <ItemTemplate>
                            <%# Eval("Observaciones")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/plus.png" runat="server" CommandName="Consultar" ID="btnConsultar" AlternateText="Mostrar / Ocultar" ToolTip="Mostrar / Ocultar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/mark_f2.png" runat="server" CommandName="Listar" ID="btnBuscarCargos" Visible="false" AlternateText="Buscar cargos pendientes de imputar" ToolTip="Buscar cargos pendientes de imputar" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <!-- this row has the child grid-->
                            </td>
                                </tr>
                                <tr>
                                    <td colspan="100%" style="padding: 0px;">
                                        <asp:HiddenField ID="hdfIdTipoCargoLoteEnviadoDetalle" Value='<%#Bind("IdTipoCargoLoteEnviadoDetalle") %>' runat="server" />
                                        <asp:HiddenField ID="hdfImporteAplicar" Value='<%#Bind("ImporteAAplicar") %>' runat="server" />
                                        <asp:HiddenField ID="hdfIdAfiliadoCabecera" Value='<%#Bind("IdAfiliado") %>' runat="server" />
                                        <asp:HiddenField ID="hdfMostrarDetalle" Value="1" runat="server" />
                                        <asp:Panel ID="pnlDatosDetalles" runat="server" Visible="false">
                                            <asp:GridView ID="gvDatosDetalles" runat="server" AutoGenerateColumns="false"
                                                SkinID="GrillaBasicaFormal" ShowFooter="true" OnRowDataBound="gvDatosDetalles_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="FechaMovimiento" HeaderText="Fecha Movimiento" />
                                                    <asp:BoundField DataField="Periodo" HeaderText="Periodo" />
                                                    <asp:BoundField DataField="Concepto" HeaderText="Concepto" />
                                                    <asp:BoundField DataField="ImporteCC" DataFormatString="{0:C2}" HeaderText="Importe" />
                                                    <asp:BoundField DataField="ImporteCobrado" DataFormatString="{0:C2}" HeaderText="Importe Cobrado" />
                                                    <asp:TemplateField HeaderText="Importe a Imputar">
                                                        <ItemTemplate>
                                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" Prefix="$" NumberOfDecimals="2" runat="server" Text='<%#Eval("ImporteAAplicarLEDCC","{0:C2}")%>' />
                                                            <asp:HiddenField ID="hdfIdCuentaCorriente" Value='<%#Bind("IdCuentaCorriente") %>' runat="server" />
                                                            <asp:HiddenField ID="hdfIdAfiliado" Value='<%#Bind("IdAfiliado") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DescripcionEstado" HeaderText="Estado" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upEstructura">
        <ContentTemplate>
            <asp:Accordion ID="Accordion1" runat="server"
                SelectedIndex="-1" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                ContentCssClass="accordionContent" AutoSize="None" FadeTransitions="true"
                TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
                SuppressHeaderPostbacks="true" Visible="false">
                <Panes>
                    <asp:AccordionPane ID="AccordionPane1" runat="server"
                        HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected"
                        ContentCssClass="accordionContent">
                        <Header>Detalle de estructura del Archivo</Header>
                        <Content>
                            <asp:GridView ID="gvArchivo" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="IndiceColeccion" SkinID="GrillaBasicaFormal">
                                <Columns>
                                    <asp:BoundField DataField="RowDelimitator" HeaderText="Limitador Fila" />
                                    <asp:BoundField DataField="FieldDelimitator" HeaderText="Limitador Campo" />
                                    <asp:BoundField DataField="CabLines" HeaderText="CabLines" Visible="false" />
                                    <asp:BoundField DataField="TrailLines" HeaderText="TrailLines" Visible="false" />
                                    <asp:BoundField DataField="NombreArchivo" HeaderText="Nombre Archivo" />
                                    <asp:BoundField DataField="Type" HeaderText="Tipo" />
                                    <asp:BoundField DataField="SheetName" HeaderText="Nombre Hoja (Excel)" />
                                </Columns>
                            </asp:GridView>
                            <br />
                            <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="IndiceColeccion" SkinID="GrillaBasicaFormal">
                                <Columns>
                                    <asp:BoundField DataField="TableField" HeaderText="Campo Tabla" />
                                    <asp:BoundField DataField="DefaultValue" HeaderText="Valor por defecto" Visible="false" />
                                    <asp:BoundField DataField="FileField" HeaderText="Campo Archivo" />
                                    <asp:BoundField DataField="FromChar" HeaderText="Posicion desde" />
                                    <asp:BoundField DataField="ToChar" HeaderText="Posicion hasta" />
                                    <asp:BoundField DataField="FieldsOrder" HeaderText="Campo orden" />
                                </Columns>
                            </asp:GridView>
                        </Content>
                    </asp:AccordionPane>
                </Panes>
            </asp:Accordion>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" Visible="false" />
                <asp:Button CssClass="botonesEvol" ID="btnContinuar" runat="server" Text="Continuar" OnClick="btnContinuar_Click" ValidationGroup="Continuar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
