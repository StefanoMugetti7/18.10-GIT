<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformesRecepcionesDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.InformesRecepcionesDatos" %>

<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>  
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Compras/Controles/OrdenesComprasBuscarPopUp.ascx" tagname="popUpBuscarOrden" tagprefix="auge" %>

<div class="InformesRecepcionesDatos">

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-header">
                    Orden Compra
                </div>
                <div class="card-body">
                    <div class="form-group row">
                        <AUGE:popUpBuscarOrden ID="ctrBuscarOrdenCompraPopUp" runat="server" />
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOrdenCompra" runat="server" Text="Orden Compra" ></asp:Label>
                        <div class="col-sm-2">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtOrdenCompra" AutoPostBack="true" Enabled="false" OnTextChanged="txtCodigo_TextChanged" runat="server" />
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvOrdenCompra" ControlToValidate="txtOrdenCompra" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-1">
                            <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarOrden" ID="btnBuscarOrden" Visible="false"
                                AlternateText="Buscar Orden" ToolTip="Buscar" OnClick="btnBuscarOrden_Click" />
                        </div>
                        <div class="col-sm-8"></div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    Informe Recepcion
                </div>
                <div class="card-body">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombreProveedor" runat="server" Text="Nombre Proveedor" />
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtNombreProveedor" runat="server" Enabled="false" />
                        </div>
                        <%-- <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Numero Remito" />
    <asp:TextBox CssClass="textboxEvol" ID="txtNumeroRemito" runat="server" Enabled="false" />
    <div class="EspacioValidador"></div>--%>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEmision" runat="server" Text="Fecha Emision"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEmision" Enabled="true" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaEmision" ControlToValidate="txtFechaEmision" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion:" />
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtObservacion" runat="server" Enabled="false" TextMode="MultiLine" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMotivoCancelado" runat="server" Text="Motivo Cancelado:" />
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtMotivoCancelado" runat="server" Enabled="false" TextMode="MultiLine" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">
                    Detalles de la Orden
                </div>
                <div class="card-body">
                    <div class="form-group row">
                        <asp:UpdatePanel ID="upInformesRecepcionesDetalle" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <Evol:EvolGridView ID="gvDatos" AllowPaging="false" AllowSorting="false"
                                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Producto" SortExpression="CodigoProducto">
                                            <ItemTemplate>
                                                <%# Eval("IdProducto")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cant. Pedida" SortExpression="Cant. Pedida">
                                            <ItemTemplate>
                                                <%# Eval("CantidadPedida")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderText="Cant. Pagada" SortExpression="CantidadPagada">
                        <ItemTemplate>
                             <%# Eval("CantidadPagada")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Cant. Recibida" SortExpression="CantidadRecibida">
                                            <ItemTemplate>
                                                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCantidadRecibida" Enabled="false" runat="server" Text='<%#Bind("CantidadRecibida") %>'></AUGE:NumericTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cant. Devuelta" SortExpression="Cant. Devuelta">
                                            <ItemTemplate>
                                                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCantidadDevuelta" Enabled="false" runat="server" Text='<%#Bind("CantidadDevuelta") %>'></AUGE:NumericTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Cant. Cambio" SortExpression="Cant. Cambio">
                                            <ItemTemplate>
                                                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCantidadCambio" Enabled="false" runat="server" Text='<%#Bind("CantidadCambio") %>'></AUGE:NumericTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>



                                    </Columns>
                                </Evol:EvolGridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            <br />
        </center>
        </ContentTemplate>
    </asp:UpdatePanel>


</div>
