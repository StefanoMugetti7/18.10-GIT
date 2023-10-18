<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanesDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PlanesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PlanesTasasDatosPopUp.ascx" TagName="ParametrosPopUp" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PlanesIpsDatos.ascx" TagName="PlanesIpsDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PlanesGrillasDatos.ascx" TagName="PlanesGrillasDatos" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPlan" runat="server" Text="Nombre Plan"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:TextBox CssClass="form-control" ID="txtPlan" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvPlan" ControlToValidate="txtPlan" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" />
        <asp:RequiredFieldValidator ID="rfvMoneda" CssClass="Validador" runat="server" ControlToValidate="ddlMoneda"
            ErrorMessage="*" ValidationGroup="Aceptar" />
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server">
        </asp:DropDownList>

    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPorcentajeGasto" runat="server" Text="Porcentaje Gastos" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeGasto" runat="server" NumberOfDecimals="4" Prefix="" />
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblImporteGastos" runat="server" Text="Importe Gastos" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteGastos" runat="server" NumberOfDecimals="2" Prefix="" />
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPorcentajeCapitalSocial" runat="server" Text="Porcentaje Capital Social" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeCapitalSocial" runat="server" NumberOfDecimals="4" Prefix="" />
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPorcentajeSeguro" runat="server" Text="Porcentaje Seguro Cuotas" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeSeguro" runat="server" NumberOfDecimals="6" Prefix="" />
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoUnidad" runat="server" Text="Tipo Unidad"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoUnidad" runat="server">
        </asp:DropDownList>
    </div>
</div>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpParametrosValores" HeaderText="Valores">
        <ContentTemplate>
            <asp:UpdatePanel ID="upParametrosValores" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <AUGE:ParametrosPopUp ID="ctrParametrosPopUp" runat="server" />
                    <div class="form-group row">
                        <div class="col-lg-3 col-md-3 col-sm-9">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregar" OnClick="btnAgregar_Click" CausesValidation="false" runat="server" Text="Agregar Valor" />
                        </div>

                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvParametrosValores" OnRowCommand="gvParametrosValores_RowCommand"
                            OnRowDataBound="gvParametrosValores_RowDataBound" DataKeyNames="IndiceColeccion" AllowPaging="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="FechaInicioVigencia" HeaderText="Fecha Desde" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaInicioVigencia" />
                                <asp:BoundField DataField="Tasa" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" HeaderText="TNA" SortExpression="Tasa" />
                                <asp:BoundField DataField="TasaEfectivaMensual" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" HeaderText="TEM" SortExpression="TasaEfectivaMensual" />
                                <asp:BoundField DataField="TasaEfectivaAnual" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N4}" HeaderText="TEA" SortExpression="TasaEfectivaAnual" />
                                <asp:BoundField DataField="CantidadCuotas" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" HeaderText="Cantidad Cuotas Desde" SortExpression="CantidadCuotas" />
                                <asp:BoundField DataField="CantidadCuotasHasta" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" HeaderText="Cantidad Cuotas Hasta" SortExpression="CantidadCuotas" />
                                <asp:BoundField DataField="ImporteDesde" DataFormatString="{0:C2}" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" HeaderText="Importe desde" SortExpression="ImporteDesde" />
                                <asp:BoundField DataField="ImporteHasta" DataFormatString="{0:C2}" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" HeaderText="Importe Hasta" SortExpression="ImporteHasta" />
                                <%--<asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" Enabled="false" />--%>
                                <%--<asp:TemplateField HeaderText="Usuario Alta" SortExpression="UsuarioAlta.ApellidoNombre">
                            <ItemTemplate><%# Eval("UsuarioAlta.ApellidoNombre")%></ItemTemplate>
                        </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Eliminar" ID="btnEliminar"
                                        AlternateText="Eliminar Registro" ToolTip="Eliminar Registro" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpFormasCobros" HeaderText="Formas de Cobro">
        <ContentTemplate>
            <asp:CheckBoxList ID="chkFormasCobros" runat="server" RepeatDirection="Horizontal" RepeatColumns="3">
            </asp:CheckBoxList>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpTiposOperaciones" HeaderText="Tipos Operaciones">
        <ContentTemplate>
            <asp:CheckBoxList ID="chkTiposOperaciones" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
            </asp:CheckBoxList>
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpPlanesIps" HeaderText="Planes IPS">
        <ContentTemplate>
            <AUGE:PlanesIpsDatos ID="ctrPlanesIps" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpPlanesGrillas" HeaderText="Planes por Grillas">
        <ContentTemplate>
            <AUGE:PlanesGrillasDatos ID="ctrPlanesGrillas" runat="server" />
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

<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
