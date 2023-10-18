<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConveniosDatos.ascx.cs" Inherits="IU.Modulos.Turismo.Controles.ConveniosDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        SetTabIndexInput();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularTotales);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(AtacharEventos);
        CalcularTotales();
        AtacharEventos();
    });
    function AtacharEventos() {
        $("input[type=submit][id$='btnAceptar']").click(function (e) {
            var txtFechaIda = $("input[id$='txtFechaInicioTemporadaAlta']").val();
            var txtFechaVuelta = $("input[id$='txtFechaFinalTemporadaAlta']").val();
            if (txtFechaIda != '' && txtFechaVuelta != '')
            {
                var fechaIda = new Date(toDate(txtFechaIda));
                var fechaVuelta = new Date(toDate(txtFechaVuelta));
                if (fechaIda > fechaVuelta)
                {
                    e.preventDefault();
                    MostrarMensaje('La fecha final de la temporada alta debe ser mayor a la fecha de inicio.', 'red');
                }
            }
        });
        $("input[type=submit][id$='btnAceptar']").click(function (e) {
            var txtFechaIda = $("input[id$='txtFechaInicioConvenio']").val();
            var txtFechaVuelta = $("input[id$='txtFechaFinalConvenio']").val();
            if (txtFechaIda != '' && txtFechaVuelta != '') { }
            if (txtFechaIda != '' && txtFechaVuelta != '') { }
            if (txtFechaIda != '' && txtFechaVuelta != '') { }
            if (txtFechaIda != '' && txtFechaVuelta != '') {
                var fechaIda = new Date(toDate(txtFechaIda));
                var fechaVuelta = new Date(toDate(txtFechaVuelta));
                if (fechaIda > fechaVuelta) {
                    e.preventDefault();
                    MostrarMensaje('La fecha final del convenio debe ser mayor a la fecha de inicio.', 'red');
                }
            }
        });
    }
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function CalcularTotales() {
        CalcularPlazas();
        CalcularPlazasDias();
    }
    function CalcularPlazas() {
        var cantidad = 0;
        $('#<%=gvDetalles.ClientID%> tr').not(':first').each(function () {
            var cantidadPlazas = $(this).find("input:text[id*='txtCantidad']").val();
            if (cantidadPlazas > 0) {
                cantidad += parseInt(cantidadPlazas);
            }
        });
        $("input[type=text][id$='txtCantidadPlazas']").val(cantidad);
    }
    function CalcularPlazasDias() {
        var cantidad = 0;
        $('#<%=gvExcepciones.ClientID%> tr').not(':first').each(function () {
            var cantidadPlazasExcepcion = $(this).find("input:text[id*='txtCantidadPlazasExcepcion']").val();
            if (cantidadPlazasExcepcion > 0) {
                cantidad += parseInt(cantidadPlazasExcepcion);
            }
        });
        $("input[type=text][id$='txtCantidadDiasPlazas']").val(cantidad);
    }
</script>
<asp:UpdatePanel ID="pnlPrinc" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="ConveniosDatos">
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblHotel" runat="server" Text="Hotel"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlHotel" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvHotel" ControlToValidate="ddlHotel" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaConvenio" runat="server" Text="Fecha Convenio"></asp:Label>
                <div class="col-sm-3">
                    <div class="form-group row">
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaInicioConvenio" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFechaInicioConvenio" ControlToValidate="txtFechaInicioConvenio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaFinalConvenio" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFechaFinalConvenio" ControlToValidate="txtFechaFinalConvenio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaTempAlta" runat="server" Text="Fecha Temp. Alta"></asp:Label>
                <div class="col-sm-3">
                    <div class="form-group row">
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaInicioTemporadaAlta" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFechaInicioTemporadaAlta" ControlToValidate="txtFechaInicioTemporadaAlta" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaFinalTemporadaAlta" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvFechaFinalTemporadaAlta" ControlToValidate="txtFechaFinalTemporadaAlta" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidadPlazas" runat="server" Text="Cant. Plazas"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control select2" ID="txtCantidadPlazas" Enabled="false" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCantidadDias" ControlToValidate="txtCantidadPlazas" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidadPlazasDias" runat="server" Text="Cant. Plazas Dias"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control select2" ID="txtCantidadDiasPlazas" Enabled="false" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCantidadDiasPlazas" ControlToValidate="txtCantidadDiasPlazas" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpDetalles" HeaderText="Detalles">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDetalles" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregarDetalles" runat="server" Text="Cantidad: "></asp:Label>
                                <Evol:CurrencyTextBox CssClass="form-control col-sm-2" ID="txtCantidadAgregarDetalles" Prefix="" NumberOfDecimals="0" ThousandsSeparator="" runat="server"></Evol:CurrencyTextBox>
                                <div class="col-sm-1">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarItemDetalles" runat="server" Text="Agregar item" OnClick="btnAgregarItemDetalles_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvDetalles" OnRowCommand="gvDetalles_RowCommand"
                                    OnRowDataBound="gvDetalles_RowDataBound" ShowFooter="true" DataKeyNames="IdConvenioDetalle"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Tipo Habitacion">
                                            <ItemTemplate>
                                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoHabitacion" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cantidad" SortExpression="">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control col-sm-6" Prefix="" ID="txtCantidad" NumberOfDecimals="0" ThousandsSeparator="" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                                    AlternateText="Eliminar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpExcepciones" HeaderText="Excepciones">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upExcepciones" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregarExcepciones" runat="server" Text="Cantidad: "></asp:Label>
                                <Evol:CurrencyTextBox CssClass="form-control col-sm-2" ID="txtCantidadAgregarExcepciones" Prefix="" NumberOfDecimals="0" ThousandsSeparator="" runat="server"></Evol:CurrencyTextBox>
                                <div class="col-sm-1">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarItemExcepciones" runat="server" Text="Agregar item" OnClick="btnAgregarItemExcepciones_Click" />
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvExcepciones" OnRowCommand="gvExcepciones_RowCommand"
                                    OnRowDataBound="gvExcepciones_RowDataBound" ShowFooter="true" DataKeyNames="IdConvenioExcepcion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fecha&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox CssClass="form-control datepicker col-sm-9" ID="txtFecha" Text='<%#Eval("FechaExcepcion", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cantidad Plazas" SortExpression="">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control col-sm-6" Prefix="" ID="txtCantidadPlazasExcepcion" NumberOfDecimals="0" ThousandsSeparator="" runat="server" Text='<%#Bind("CantidadPlazasExcepcion") %>'></Evol:CurrencyTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                                    AlternateText="Eliminar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
        <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                <center>
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
