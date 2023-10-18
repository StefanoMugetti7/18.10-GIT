<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExpedientesModificarDatos.ascx.cs" Inherits="IU.Modulos.Expedientes.Controles.ExpedientesModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<div class="ExpedientesModificarDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroExpediente" runat="server" Text="Nro. Expediente" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtNumeroExpediente" runat="server" />
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTitulo" runat="server" Text="Titulo" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtTitulo" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTitulo" runat="server" ControlToValidate="txtTitulo"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoExpediente" runat="server" Text="Tipo Expediente" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoExpediente" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoExpediente" runat="server" ControlToValidate="ddlTipoExpediente"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
        <div class="col-lg-7 col-md-7 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" TextMode="MultiLine" runat="server" />
        </div>
    </div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpExpedienteTracking" HeaderText="Derivaciones">
            <ContentTemplate>
                <asp:UpdatePanel ID="upExpedienteTracking" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlDerivaciones" GroupingText="Derivar Expediente" runat="server" Visible="false">
                            <div class="form-group row">
                                <div class="col-lg-3 col-md-3 col-sm-9">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" runat="server" ControlToValidate="ddlFilial"
                                        ErrorMessage="*" ValidationGroup="AgregarTracking" />
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-9">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlSector" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSector" runat="server" ControlToValidate="ddlSector"
                                        ErrorMessage="*" ValidationGroup="AgregarTracking" />
                                </div>
                                <asp:Button CssClass="botonesEvol" ID="btnAgregaTracking" runat="server" Text="Agregar Derivación" ValidationGroup="AgregarTracking"
                                    OnClick="btnAgregaTracking_Click" Visible="true" CausesValidation="true" />
                            </div>
                        </asp:Panel>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false"
                                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting"
                                AllowPaging="true" AllowSorting="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                        <ItemTemplate>
                                            <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Filial" SortExpression="Sector.Filial.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Sector.Filial.Filial")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sector" SortExpression="Sector.Sector">
                                        <ItemTemplate>
                                            <%# Eval("Sector.Sector")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
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
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    onclick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
