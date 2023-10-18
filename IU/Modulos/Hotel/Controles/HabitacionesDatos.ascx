<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HabitacionesDatos.ascx.cs" Inherits="IU.Modulos.Hotel.Controles.HabitacionesDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>

<script type="text/javascript">
    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        SetTabIndexInput();
    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
</script>

<div class="HotelesHabitaciones">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblHotel" runat="server" Text="Hotel"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlHoteles" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvHoteles" ControlToValidate="ddlHoteles" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblProducto" runat="server" Text="Producto"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass=" form-control select2" ID="ddlProductos" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProductos" ControlToValidate="ddlProductos" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroHabitacion" runat="server" Text="Nro. Habitacion"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtNumeroHabitacion" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroHabitacion" ControlToValidate="txtNumeroHabitacion" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPiso" runat="server" Text="Piso"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtPiso" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad de Personas"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidad" runat="server" Text=""></AUGE:NumericTextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidad" ControlToValidate="txtCantidad" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
    </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
        SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpHabitacionesDetalles"
            HeaderText="Detalle de Habitacion">
            <ContentTemplate>
                <asp:UpdatePanel ID="upDHabitacionesDetalles" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Button CssClass="botonesEvol" ID="btnAgregaDetalle" OnClick="btnAgregaDetalle_Click" runat="server" Text="Agregar item"
                            CausesValidation="false" />
                        <asp:GridView ID="gvDatos" ShowFooter="true" OnRowCommand="gvDatos_RowCommand"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdHabitacionDetalle"
                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Mobiliario">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlMoviliarios" CssClass="select2" runat="server"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripcion">
                                    <ItemTemplate>
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" Text='<%#Eval("Descripcion") %>' runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                            AlternateText="Elminiar" ToolTip="Eliminar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>