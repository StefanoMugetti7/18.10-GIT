<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormasCobrosDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.FormasCobrosDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="FormasCobrosDatos">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="from-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Formas de Cobro"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtFormaCobro" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFormasCobros" CssClass="Validador" ControlToValidate="txtFormaCobro" runat="server" ValidationGroup="Aceptar" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoFormaCobro" runat="server" Text="Codigo "></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCodigoFormaCobro" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                    </asp:DropDownList>
                </div>

            </div>
            <div class="from-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProceso" runat="server" Text="Proceso"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlProceso" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <asp:Panel ID="pnlCamposValores" runat="server">
                <auge:CamposValores ID="ctrCamposValores" runat="server" />
            </asp:Panel>
            <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
