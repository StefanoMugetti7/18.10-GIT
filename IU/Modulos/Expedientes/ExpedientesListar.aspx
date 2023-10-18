<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ExpedientesListar.aspx.cs" Inherits="IU.Modulos.Expedientes.ExpedientesListar" %>

<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />--%>
    <div class="AfiliadosListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblIdExpediente" runat="server" Text="Número"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtIdExpediente" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblExpedienteTipo" runat="server" Text="Tipo expediente"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlExpedientesTipos" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTitulo" runat="server" Text="Titulo"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtTitulo" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoAccion" runat="server" Text="Tipo de Acción"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstadosTracking" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <div class="table-responsive">
                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <%# Eval("FechaExpediente", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Numero" SortExpression="IdExpediente">
                                <ItemTemplate>
                                    <%# Eval("IdExpediente")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo" SortExpression="ExpedienteTipo.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("ExpedienteTipo.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Titulo" DataField="Titulo" SortExpression="Titulo" />
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Estado.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Filial -> Sector" SortExpression="ExpedienteTrackingSectorFilialFilial">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("ExpedienteTracking.Sector.Filial.Filial"), " -> ", Eval("ExpedienteTracking.Sector.Sector"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Derivado a" SortExpression="ExpedienteTrackingSectorSector">
                                <ItemTemplate>
                                    <%# Convert.ToInt32( Eval("ExpedienteDerivado.Sector.IdSector")) == 0 ? string.Empty : string.Concat(Eval("ExpedienteDerivado.Sector.Filial.Filial"), " -> ", Eval("ExpedienteDerivado.Sector.Sector"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                        AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Aceptar" ID="btnAceptarDerivacion"
                                        AlternateText="Aceptar Derivación" ToolTip="Aceptar Derivación" Visible="false" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Rechazar" ID="btnRechazarDerivacion"
                                        AlternateText="Rechazar Derivación" ToolTip="Rechazar Derivación" Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
