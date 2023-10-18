<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoriasClinicasModificarDatos.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.HistoriasClinicasModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="HistoriasClinicasModificarDatos">
    <%--<asp:Label CssClass="labelEvol" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
                <div class="Espacio"></div>
                <asp:Label CssClass="labelEvol" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <asp:DropDownList CssClass="selectEvol" ID="ddlEstados" runat="server" >
                </asp:DropDownList>
                <br />--%>
    <%--<asp:Label CssClass="labelEvol" ID="lblPrestador" runat="server" Text="Prestador"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtPrestador" Enabled="false" runat="server"></asp:TextBox>
                <div class="Espacio"></div>
                <asp:Label CssClass="labelEvol" ID="lblEspecializacion" runat="server" Text="Especialización"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtEspecializacion" Enabled="false" runat="server"></asp:TextBox>
                <br />
                <asp:Label CssClass="labelEvol" ID="lblObraSocial" runat="server" Text="Obra Social"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtObraSocial" Enabled="false" runat="server"></asp:TextBox>
                <div class="Espacio"></div>
                <asp:Label CssClass="labelEvol" ID="lblNomenclador" runat="server" Text="Nomenclador"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtNomenclador" Enabled="false" runat="server"></asp:TextBox>--%>
    <%--                <br />
                <asp:Label CssClass="labelEvol" ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtObservaciones" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>--%>
    <br />
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <br />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
        SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpComentarios"
            HeaderText="Evoluciones">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
                <div class="table-responsive">
                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                <ItemTemplate>
                                    <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prestador" HeaderStyle-Width="20%" SortExpression="Prestador.ApellidoNombre">
                                <ItemTemplate>
                                    <%# Eval("Prestador.ApellidoNombre")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Evoluciones" HeaderStyle-Width="40%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" SortExpression="Evoluciones">
                                <ItemTemplate>
                                    <%# Eval("Observaciones")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Estado.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                        AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpPrestaciones"
            HeaderText="Prestaciones" Enabled="false">
            <ContentTemplate>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpLaboratorios"
            HeaderText="Laboratorios" Enabled="false">
            <ContentTemplate>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpRecetas"
            HeaderText="Recetas" Enabled="false">
            <ContentTemplate>
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos"
            HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <%--<asp:TabPanel runat="server" ID="tpHistorial"
                            HeaderText="Auditoria" >
                            <ContentTemplate>
                                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                            </ContentTemplate>
                        </asp:TabPanel>--%>
    </asp:TabContainer>
    <br />
    <asp:UpdatePanel ID="upBotones" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    onclick="btnAceptar_Click" ValidationGroup="PrestacionesModificarDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver"
                    onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
