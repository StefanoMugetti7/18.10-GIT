<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubsidiosDatos.ascx.cs" Inherits="IU.Modulos.Subsidios.Controles.SubsidiosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Subsidios/Controles/SubEscalasDatosPopUp.ascx" TagName="popUpEscalas" TagPrefix="auge" %>


<div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Nombre"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion"
            ErrorMessage="*" ValidationGroup="Aceptar" />

    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoSubsidio" runat="server" Text="Tipo Subsidio"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoSubsidio" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoSubsidio" runat="server" ControlToValidate="ddlTipoSubsidio"
            ErrorMessage="*" ValidationGroup="Aceptar" />
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />


    <div class="col-lg-3 col-md-3 col-sm-9">
        <asp:DropDownList ID="ddlEstado" CssClass="form-control select2" runat="server"></asp:DropDownList></div>
</div>
<div class="form-group row">
 <%--   <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMesesCarencia" runat="server" Text="Meses Carencia" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtMesesCarencia" runat="server"></AUGE:NumericTextBox>
    </div>--%>
     <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMesesValidezEvento" runat="server" Text="Meses Validez Evento" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtMesesValidezEvento" runat="server"></AUGE:NumericTextBox>
    </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label1" runat="server" Text="Cantidad Maxima" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadMaxima" runat="server"></AUGE:NumericTextBox>
    </div>
     <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="Label2" runat="server" Text="Frecuencia Mensual" />
    <div class="col-lg-3 col-md-3 col-sm-9">
        <AUGE:NumericTextBox CssClass="form-control" ID="txtFrecuenciaAnual" runat="server"></AUGE:NumericTextBox>
    </div>
    </div>
  
<div class="form-group row">
 

     <asp:Label CssClass="col-sm-1 col-form-label" ID="lblModificaImporte" runat="server" Text="Modifica Importe"></asp:Label>
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkModificaImporte" CssClass="form-control" runat="server" />
                    </div>
    </div>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />



<asp:UpdatePanel ID="upEscalas" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpEscalas ID="ctrEscalas" runat="server" />
        <asp:Button CssClass="botonesEvol" ID="btnAgregarEscalas" runat="server" Text="Agregar Escala"
            OnClick="btnAgregarEscalas_Click" CausesValidation="false" />

        <div class="data-table">
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Ingreso Desde" SortExpression="FechaIngresoDesde">
                        <ItemTemplate>
                            <%# Eval("FechaIngresoDesde", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ingreso Hasta" SortExpression="FechaSolicitud">
                        <ItemTemplate>
                            <%# Eval("FechaIngresoHasta", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edad Desde" SortExpression="EdadDesde">
                        <ItemTemplate>
                            <%# Eval("EdadDesde")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edad Hasta" SortExpression="EdadHasta">
                        <ItemTemplate>
                            <%# Eval("EdadHasta")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Antiguedad Desde" SortExpression="EdadDesde">
                        <ItemTemplate>
                            <%# Eval("AntiguedadDesde")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Antiguedad Hasta" SortExpression="AntiguedadHasta">
                        <ItemTemplate>
                            <%# Eval("AntiguedadHasta")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Beneficio" SortExpression="ImporteBeneficio">
                        <ItemTemplate>
                            <%# Eval("ImporteBeneficio", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inicio Vigencia" SortExpression="FechaInicioVigencia">
                        <ItemTemplate>
                            <%# Eval("FechaInicioVigencia", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fin Vigencia" SortExpression="FechaFinVigencia">
                        <ItemTemplate>
                            <%# Eval("FechaFinVigencia", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--                    <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                                <ItemTemplate>
                                    <%# Eval("Filial.Filial")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
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
                                AlternateText="Modificar" ToolTip="Modificar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                AlternateText="Elminiar" ToolTip="Eliminar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
    </ContentTemplate>
</asp:UpdatePanel>
