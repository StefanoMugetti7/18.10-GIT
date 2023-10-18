<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CondicionesFiscalesTiposFacturasDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.CondicionesFiscalesTiposFacturasDatos" %>
<div class="PlazosFijosDatos">
    <asp:UpdatePanel ID="upSaldoActual" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Condicion Fiscal" />
                <div class="col-sm-3">
                 <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" runat="server">
                        </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCondicionFiscal" runat="server" ControlToValidate="ddlCondicionFiscal" ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo de Factura" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoFactura" runat="server" ControlToValidate="ddlTipoFactura" ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>