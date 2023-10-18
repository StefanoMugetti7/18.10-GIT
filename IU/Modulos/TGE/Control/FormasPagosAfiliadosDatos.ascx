<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormasPagosAfiliadosDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.FormasPagosAfiliadosDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>

<div class="FormasPagoAfiliadosDatos">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiliales" runat="server" Text="Filial"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server"></asp:DropDownList>
                </div>
                <%--<br />
                <asp:Label CssClass="labelEvol" ID="lblPredeterminado" runat="server" Text="Predeterminado"></asp:Label>
                <asp:CheckBox ID="chkPredeterminado" runat="server" />--%>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaPago" runat="server" Text="Formas de Pago"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFormasPagos" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlFormasPagos_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblZG" runat="server" Text="Zona Grupo"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtZG" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
                <asp:TabPanel runat="server" ID="tpCamposValores" HeaderText="Datos adicionales">
                    <ContentTemplate>
                        <%--<asp:Panel ID="pnlCamposValores" GroupingText="Datos adicionales" runat="server">
                            </asp:Panel>--%>
                        <auge:CamposValores ID="ctrCamposValores" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
                    <ContentTemplate>
                        <auge:Comentarios ID="ctrComentarios" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
                    <ContentTemplate>
                        <auge:Archivos ID="ctrArchivos" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
                    <ContentTemplate>
                        <auge:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <br />
            <center>
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
