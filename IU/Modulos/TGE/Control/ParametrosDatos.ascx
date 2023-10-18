<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParametrosDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.ParametrosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/TGE/Control/ParametrosDatosPopUp.ascx" TagName="ParametrosPopUp" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarProducto() {
        $("[id$='modalParametrosDatos']").modal('show');
    }

    function HideModalBuscarProducto() {
        $("[id$='modalParametrosDatos']").modal('hide');
    }
</script>

<div class="ParametrosDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblParametro" runat="server" Text="Nombre Parametro"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtParametro" Enabled="false" runat="server"></asp:TextBox>
        </div>
    </div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpParametrosValores" HeaderText="Valores">
            <ContentTemplate>
                <asp:UpdatePanel ID="upParametrosValores" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <AUGE:ParametrosPopUp ID="ctrParametrosPopUp" runat="server" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" OnClick="btnAgregar_Click" CausesValidation="false" runat="server" Text="Agregar Valor" />
                        <br />
                        <br />
                        <asp:GridView ID="gvParametrosValores" OnRowCommand="gvParametrosValores_RowCommand"
                            OnRowDataBound="gvParametrosValores_RowDataBound" DataKeyNames="IndiceColeccion" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="ParametroValor" HeaderText="Parametro Valor" SortExpression="ParametroValor" />
                                <asp:BoundField DataField="FechaDesde" HeaderText="Fecha Desde" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                                <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                                <asp:TemplateField HeaderText="Usuario Alta" SortExpression="UsuarioAlta.ApellidoNombre">
                                    <ItemTemplate><%# Eval("UsuarioAlta.ApellidoNombre")%></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                                            AlternateText="Eliminar Registro" ToolTip="Eliminar Registro" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
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
