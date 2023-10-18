<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacturacionesHabitualesDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturacionesHabitualesDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>
<%@ Register src="~/Modulos/Afiliados/Controles/ClientesDatosCabecera.ascx" tagname="ClientesDatosCabecera" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="FacturacionesHabitualesDatos">
    <AUGE:ClientesDatosCabecera ID="ctrClienteDatosCabecera" runat="server" />
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtFechaAlta" Enabled="false" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"/>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
        </div>
    </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFacturaHabitual" runat="server" Text="Tipo" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFacturaHabitual" runat="server" />
    <asp:RequiredFieldValidator ID="rfvTipoFacturaHabitual" ValidationGroup="Aceptar" ControlToValidate="ddlTipoFacturaHabitual" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIncrementoPorcentaje" runat="server" Text="Incremento Porcentaje"></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtIncrementoPorcentaje" Prefix="" NumberOfDecimals="0" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIncrementoPeriodoMeses" runat="server" Text="Incremento Periodo Meses"></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtIncrementoPeriodoMeses" Prefix="" NumberOfDecimals="0" MaxLength="2" runat="server" />
    </div>
    </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoInicio" runat="server" Text="Periodo Inicio"></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPeriodoIncio" Prefix="" ThousandsSeparator="" NumberOfDecimals="0" MaxLength="6" runat="server" />
    <asp:RequiredFieldValidator ID="rfvPeriodoIncio" ValidationGroup="Aceptar" ControlToValidate="txtPeriodoIncio" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoFin" runat="server" Text="Periodo Fin"></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPeriodoFin" Prefix="" ThousandsSeparator="" NumberOfDecimals="0" MaxLength="6" runat="server" />
        </div>
    </div>
    <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo Factura" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" runat="server" />
    <asp:RequiredFieldValidator ID="rfvTipoFactura" ValidationGroup="Aceptar" ControlToValidate="ddlTipoFactura" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFacturaDiaVencimiento" runat="server" Text="Factura dia Vto."></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtFacturaDiaVencimiento" Prefix="" NumberOfDecimals="0" MaxLength="2" runat="server" />
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Correo Electronico"/>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server" />
        </div>
    </div>    
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:UpdatePanel ID="upItems" UpdateMode="Conditional" runat="server" >
                <ContentTemplate>     
                <AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />
                <div class="form-group row">
                    <div class="col-sm-12">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" onclick="btnAgregarItem_Click" />
                        </div>
                </div>
                <asp:GridView ID="gvItems" AllowPaging="true" DataKeyNames="IndiceColeccion"
                OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaBasicaFormal" 
                AutoGenerateColumns="false" ShowFooter="true" onrowdatabound="gvItems_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText ="Código" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCodigo" Prefix="" ThousandsSeparator="" NumberOfDecimals="0" Enabled="false" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' onTextChanged="txtCodigo_TextChanged" ></Evol:CurrencyTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText ="" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" AutoPostBack="true" CommandName="BuscarProducto" CommandArgument='<%# Container.DisplayIndex%>' ID="btnBuscarProducto" Visible="false"
                                    AlternateText="Buscar producto" ToolTip="Buscar"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText ="Producto" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <%# Eval("Producto.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText ="Cantidad" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox ID="txtCantidad" Enabled="false" class="form-control" Prefix="" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText ="Importe" ItemStyle-Wrap="false" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox ID="txtImporte" Enabled="false" class="form-control" runat="server"  Text='<%# Eval("Importe", "{0:C2}") %>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Centro de Costo" SortExpression="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostos" runat="server" />
                            </ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText ="Alícuota IVA" ItemStyle-Wrap="false" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText ="Estado" ItemStyle-Width="" SortExpression ="">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" Enabled="false" />
                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                AlternateText="Elminiar" ToolTip="Eliminar" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>

    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
                <div class="row justify-content-md-center">
            <div class="col-md-auto">
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </div>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel> 
</div>