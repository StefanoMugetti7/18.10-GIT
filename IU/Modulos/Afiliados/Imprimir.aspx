<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="Imprimir.aspx.cs" Inherits="IU.Modulos.Afiliados.Imprimir" Title="" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
    <script lang="javascript" type="text/javascript">
        function EnviarWhatsApp(url) {
            var win = window.open(url, '_blank');
            if (win) {
                //Browser has allowed it to be opened
                win.focus();
            } else {
                //Browser has blocked it
                MostrarMensaje('Por favor habilite los popups para este sitio', 'red');
            }
        }
        function CopyClipboard() {
            var $temp = $("<input type='text'>");
            $("body").append($temp);
            var link = $("input[type=hidden][id$='hfLinkFirmarDocumento']").val();
            $temp.val(link);
            $temp.focus();
            $temp.select();
            try {
                var successful = document.execCommand('copy');
                MostrarMensaje("Se ha Copiado el Link");
                //$('#copyClipboard').data('tooltip').show();
            } catch (err) {
                console.error('Unable to copy');
            }
            $temp.remove();
        }
    </script>
    <div class="Imprimir">
        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="form-group row ">
                    <div class="col-sm-4"></div>
                    <asp:Label CssClass="col-sm-1" ID="lblSocio" runat="server" Text="Socio"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlSocio" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSocio" ValidationGroup="Imprimir" ControlToValidate="ddlSocio" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row ">
                    <div class="col-sm-4"></div>
                    <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                    <asp:Label CssClass="col-sm-1" ID="lblSeleccionar" runat="server" Text="Comprobantes"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlComprobantes" AutoPostBack="true" OnSelectedIndexChanged="ddlComprobantes_Click" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvComprobantes" ValidationGroup="Imprimir" ControlToValidate="ddlComprobantes" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                </div>
                <div class="form-group row " runat="server" id="FechaDesde" visible="false">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-4">
                        <div class="form-group row ">
                            <asp:Label CssClass="col-form-label col-sm-2" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" placeholder="Desde" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="RequiredFieldValidator1" ValidationGroup="Imprimir" ControlToValidate="ddlComprobantes" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <%--<asp:Label CssClass="col-form-label col-sm-2" ID="lblFechaHasta" runat="server" Text="Hasta"></asp:Label>--%>
                            <div class="col-sm-4">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" placeholder="Hasta" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="RequiredFieldValidator2" ValidationGroup="Imprimir" ControlToValidate="ddlComprobantes" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row justify-content-md-center">
                    <div class="col-md-auto ">
                        <asp:HiddenField ID="hfLinkFirmarDocumento" runat="server" />
                        <button runat="server" visible="false" type="button" id="copyClipboard" data-tooltip="Se ha copiado el link" class="botonesEvol" onclick="CopyClipboard()">Copiar link</button>

                        <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/whatsup26x26.jpg" runat="server" ID="btnWhatsAppFirmarDocumento" Visible="false"
                            OnClick="btnWhatsAppFirmarDocumento_Click" AlternateText="Enviar Whatsapp para Firmar" ToolTip="Enviar Whatsapp para Firmar" />
                        <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnFirmarDocumento" Visible="false"
                            OnClick="btnFirmarDocumento_Click" AlternateText="Enviar Mail para Firmar" ToolTip="Enviar Mail para Firmar" />
                        <asp:Button CssClass="botonesEvol" ID="btnFirmarDocumentoBaja" Visible="false" runat="server" Text="Eliminar Firma" OnClick="btnFirmarDocumentoBaja_Click" />
                        <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir"
                            OnClick="btnImprimir_Click" ValidationGroup="Imprimir" Visible="false" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
