<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BienesUsosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.BienesUsosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/SolicitudPagoDetalleBuscarPopUp.ascx" TagName="popUpSolicitudPagoDetalleBuscar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="CuentasContables" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<script language="javascript" type="text/javascript">
    $(function () {
        $('#<%=txtVidaUtil.ClientID%>').blur(function () {
            var vidaUtil = $("#<%=txtVidaUtil.ClientID%>").val();
            $('#<%=txtVidaRestante.ClientID%>').val(vidaUtil);
        });
    });
</script>

<div class="BienesUsosDatos">
    <asp:UpdatePanel ID="upBinesUso" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción" />
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" MaxLength="500" TextMode="MultiLine" /></div>
                <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" ID="btnBuscarSolicitudPagoDetalle" Visible="false"
                        AlternateText="Buscar Item/Producto" ToolTip="Buscar" OnClick="btnBuscarSolicitudPagoDetalle_Click" />
                </div>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" runat="server" ErrorMessage="*"
                    ControlToValidate="txtDescripcion" ValidationGroup="Aceptar" />
                <AUGE:popUpSolicitudPagoDetalleBuscar ID="puSolicitudPagoDetalle" runat="server" />
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaActivacion" runat="server" Text="Fecha de Activación" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaActivacion" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblClasificador" runat="server" Text="Clasificador" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlClasificador" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvClasificador" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlClasificador" ValidationGroup="Aceptar" />
                </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" /></div>
            </div>
            <div class="form-group row">

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidad" runat="server"></AUGE:NumericTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidad" runat="server" ErrorMessage="*"
                        ControlToValidate="txtCantidad" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Valor de Compra" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ErrorMessage="*"
                        ControlToValidate="txtImporte" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAmortAcumulada" runat="server" Text="Amortización Acumulada" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtAmortAcumulada" runat="server" Enabled="false" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVidaUtil" runat="server" Text="Vida Util (años)" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtVidaUtil" runat="server"></AUGE:NumericTextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvVidaUtil" runat="server" ErrorMessage="*"
                ControlToValidate="txtVidaUtil" ValidationGroup="Aceptar" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVidaTranscurrida" runat="server" Text="Vida Transcurrida" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtVidaTranscurrida" runat="server" Enabled="false"></AUGE:NumericTextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVidaRestante" runat="server" Text="Vida Restante" />
        <div class="col-sm-3">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtVidaRestante" runat="server" Enabled="false"></AUGE:NumericTextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="upCuentasContables" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:CuentasContables ID="ctrCuentasContables" MostrarEtiquetas="true" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Fecha Asiento" SortExpression="" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("AsientoContable.FechaAsiento", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Número Asiento" SortExpression="AsientoContable.NumeroAsiento">
                        <ItemTemplate>
                            <%# Eval("AsientoContable.NumeroAsiento")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Descripción" SortExpression="AsientoContable.DetalleGeneral">
                        <ItemTemplate>
                            <%# Eval("AsientoContable.DetalleGeneral")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Amortizado" ItemStyle-Wrap="false" FooterStyle-Wrap="false" SortExpression="" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("ImporteAmortizado", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblTotalAmortizacion" runat="server" Style="text-align: right"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>