<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrePrestamosLotesDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrePrestamosLotesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script type="text/javascript">

    var gridViewId = '#<%= gvItems.ClientID %>';

    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
        updateSelectionLabel();
    }
    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Lote
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroLote" runat="server" Text="Nro. Lote"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtNumeroLote" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblInversor" runat="server" Text="Inversor"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlInversor" runat="server" OnSelectedIndexChanged="ddlInversor_SelectedIndexChanged"
                            AutoPostBack="true" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTasaInversor" runat="server" Text="Tasa Inversor" />
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaInversor" Prefix="" Enabled="false" runat="server"></Evol:CurrencyTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" Rows="2" ID="txtDetalle" runat="server" Placeholder="Detalle" TextMode="MultiLine" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>
                </div>
      <%--          <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidad" runat="server" Text="Cantidad"></asp:Label>
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" Prefix="" runat="server"></Evol:CurrencyTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTotal" runat="server" Text="Importe Total"></asp:Label>
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteTotal" runat="server"></Evol:CurrencyTextBox>
                    </div>
                    <div class="col-sm-3"></div>
                </div>--%>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upTabControl" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpPrestamosSeleccionados" TabIndex="0" HeaderText="Prestamos Seleccionados">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upPrestamosSeleccionados" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <asp:GridView ID="gvDatos" AllowPaging="true" AllowSorting="false"
                                    OnRowCommand="gvDatos_RowCommand" DataKeyNames="IdPrestamo" runat="server"
                                    SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                    OnRowDataBound="gvDatos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nro. Socio">
                                            <ItemTemplate>
                                                <%# Eval("AfiliadoNumeroSocio")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nro. Documento">
                                            <ItemTemplate>
                                                <%# Eval("AfiliadoNumeroDocumento")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Apellido Nombre">
                                            <ItemTemplate>
                                                <%# Eval("AfiliadoApellidoNombre")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nro. Prestamo">
                                            <ItemTemplate>
                                                <%# Eval("PrestamoNroDeIdentificacion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Confirmacion">
                                            <ItemTemplate>
                                                <%# Eval("PrestamoFechaConfirmacion", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("PrestamoImporteAutorizado", "{0:N2}"))%>
                                                <asp:HiddenField ID="hdfImporte" Value='<%#Bind("PrestamoImporteAutorizado") %>' runat="server" />
                                            </ItemTemplate>
                                                <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe Cuota">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("PrestamoImporteCuota", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cuotas">
                                            <ItemTemplate>
                                                <%# Eval("PrestamoCantidadCuotas")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scoring">
                                            <ItemTemplate>
                                                <%# Eval("Scoring")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="5%" SortExpression="">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" Visible="false" runat="server" CommandName="Borrar" ID="btnEliminar"
                                                    AlternateText="Eliminar" ToolTip="Eliminar Item" />
                                            </ItemTemplate>
                                        <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-auto">
                                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" />
                                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpBusquedaPrestamos" TabIndex="1" HeaderText="Busqueda de Prestamos">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upBusquedaPrestamos" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:Button CssClass="botonesEvol" ID="Button1" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteDesde" runat="server" Text="Importe Desde"></asp:Label>
                                <div class="col-sm-3">
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteDesde" runat="server"></Evol:CurrencyTextBox>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteHasta" runat="server" Text="Importe Hasta"></asp:Label>
                                <div class="col-sm-3">
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteHasta" runat="server"></Evol:CurrencyTextBox>
                                </div>
                                <div class="col-sm-3"></div>
                            </div>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblInversorGrilla" runat="server" Text="Inversor"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlInversorGrilla" runat="server"></asp:DropDownList>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cantidad de Cuotas"></asp:Label>
                                <div class="col-sm-3">
                                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadCuotas" runat="server"></AUGE:NumericTextBox>
                                </div>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="gvItems" AllowPaging="true" AllowSorting="false"
                                    OnRowCommand="gvItems_RowCommand" DataKeyNames="IdPrestamo,IdPrestamoLoteDetalle" OnPageIndexChanging="gvItems_PageIndexChanging"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                    OnRowDataBound="gvItems_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Nro. Socio">
                                            <ItemTemplate>
                                                <%# Eval("AfiliadoNumeroSocio")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nro. Documento">
                                            <ItemTemplate>
                                                <%# Eval("AfiliadoNumeroDocumento")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Apellido Nombre">
                                            <ItemTemplate>
                                                <%# Eval("AfiliadoApellidoNombre")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nro. Prestamo">
                                            <ItemTemplate>
                                                <%# Eval("PrestamoNroDeIdentificacion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Confirmacion">
                                            <ItemTemplate>
                                                <%# Eval("PrestamoFechaConfirmacion", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("PrestamoImporteAutorizado", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe Cuota">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("PrestamoImporteCuota", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cuotas">
                                            <ItemTemplate>
                                                <%# Eval("PrestamoCantidadCuotas")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scoring">
                                            <ItemTemplate>
                                                <%# Eval("Scoring")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Inversor">
                                            <ItemTemplate>
                                                <%# Eval("ProveedorRazonSocial")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" Checked='<%# Eval("Incluir")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-auto">
                                    <asp:Button CssClass="botonesEvol" ID="btnAceptarIncluir" runat="server" Text="Incluir" OnClick="btnAceptarIncluir_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </ContentTemplate>
</asp:UpdatePanel>
