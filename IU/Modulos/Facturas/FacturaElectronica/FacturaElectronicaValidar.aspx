<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FacturaElectronicaValidar.aspx.cs" Inherits="IU.Modulos.Facturas.FacturaElectronica.FacturaElectronicaValidar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CopmletarCerosComprobantes);
        CopmletarCerosComprobantes();
        $.fn.addLeadingZeros = function (length) {
            for(var el of this) {
                _value = el.value.replace(/^0+/, '');
                length = length - _value.length;
                if (length > 0) {
                    while (length--) _value = '0' + _value;
                }
                el.value = _value;
            }
        };
    });

    function CopmletarCerosComprobantes() {
        $("input[type=text][id$='txtNumeroComprobante']").blur(function () { $(this).addLeadingZeros(8); });
    }
</script>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
    <br />
    <asp:Label CssClass="labelEvol" ID="lblResultado" runat="server" Text="Servicio Factura Electronica"></asp:Label>
    <asp:Button CssClass="botonesEvol" ID="btnValidarServicio" CausesValidation="false" runat="server" Text="ValidarServicio" onclick="btnValidarServicio_Click" />
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblResultado2" runat="server" Text="" Width="50%"></asp:Label>
    <br />
    <asp:Label CssClass="labelEvol" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlFiliales" runat="server" OnSelectedIndexChanged="ddlFiliales_SelectedIndexChanged" 
    AutoPostBack="true"/>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Aceptar" ControlToValidate="ddlFilialPuntoVenta" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblTipoEmision" runat="server" Text="Tipo de Emisión"></asp:Label>
    <%--<asp:TextBox CssClass="textboxEvol" ID="txtFilialDescripcion" runat="server" Enabled="false" ></asp:TextBox>--%>
    <asp:DropDownList CssClass="selectEvol" ID="ddlFilialPuntoVenta" runat="server" OnSelectedIndexChanged="ddlFilialPuntoVenta_SelectedIndexChanged" 
    AutoPostBack="true"/>
    <asp:RequiredFieldValidator ID="rfvFilialPuntoVenta" ValidationGroup="Aceptar" ControlToValidate="ddlFilialPuntoVenta" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    <br />

    <asp:Label CssClass="labelEvol" ID="lblTipoFactura" runat="server" Text="Tipo Comprobante" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlTipoFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoFactura_SelectedIndexChanged" runat="server" />
    <asp:RequiredFieldValidator ID="rfvTipoFactura" ValidationGroup="Aceptar" ControlToValidate="ddlTipoFactura" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    <div class="EspacioValidador"></div>
    <asp:Label CssClass="labelEvol" ID="lblNumeroFactura" runat="server" Text="Punto de Venta"></asp:Label>
    <%--<AUGE:NumericTextBox CssClass="txtPrefijoNumeroComprobante" ID="txtPrefijoNumeroFactura" Enabled="false" runat="server" maxlength="4"/>--%>
    <asp:DropDownList CssClass="ddlPrefijoNumeroComprobante" ID="ddlPrefijoNumeroFactura" runat="server" />
            
    <br />
    <asp:Label CssClass="labelEvol" ID="Label1" runat="server" Text=" " />
    <asp:Button CssClass="botonesEvol" ID="btnValidarProximoNuero" ValidationGroup="Aceptar" Enabled="false" runat="server" Text="Obtener Proxima Factura" onclick="btnValidarProximoNuero_Click" />
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblProximaFactura2" runat="server" Text="" Width="50%"></asp:Label>
    <br />
            <br />
            <asp:Label CssClass="labelEvol" ID="lblNumeroComprobante" runat="server" Text="Nro. Comprobante" />
            <asp:TextBox ID="txtNumeroComprobante" runat="server" ValidationGroup="ConsultarComprobante"></asp:TextBox>
            <asp:Button CssClass="botonesEvol" ID="Button1" ValidationGroup="ConsultarComprobante" Enabled="true" runat="server" Text="Consultar Comprobante" onclick="btnConsultarComprobante_Click" />
            <br />
            <asp:GridView ID="gvDatos" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Nro. Doc." ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <%# Eval("Afiliado.NumeroDocumento")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Comprobante" ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <%# Eval("TipoFactura.Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Número Comprobante" ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <%# Eval("NumeroFacturaCompleto")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Factura" ItemStyle-Wrap="false" >
                            <ItemTemplate>
                                <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("ImporteSinIVA", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Iva" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("IvaTotal", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Total" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <%# Eval("ImporteTotal", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CAE" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <%# Eval("CAE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
            </asp:GridView>
    <%--<div class="Espacio"></div><asp:Label CssClass="labelEvol" ID="lblErrores" runat="server" Text=""></asp:Label>--%>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
