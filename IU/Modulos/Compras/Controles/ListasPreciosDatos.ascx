<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListasPreciosDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.ListasPreciosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Compras/Controles/ListasPreciosBuscarPopUp.ascx" TagName="popUpBuscarListaPrecio" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" TagName="popUpBuscarCliente" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="ListasPreciosDatos">
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
    <script type="text/javascript">

        function CheckRow(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAllRow(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        if (inputList[i].disabled == false) {
                            inputList[i].checked = true;
                        }
                        else {
                            inputList[i].checked = false;
                        }
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function UpdPanelUpdate() {
            __doPostBack("<%=button.UniqueID %>", "");
        }

    </script>
    <asp:Panel ID="pnlListaPrecio" runat="server">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMargenPorcentual" runat="server" Text="Margen %"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtMargenPorcentual" Enabled="false" runat="server" NumberOfDecimals="4"></Evol:CurrencyTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMargenPorcentual" ControlToValidate="txtMargenPorcentual" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMargenImporte" runat="server" Text="Margen $"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtMargenImporte" Enabled="false" runat="server" NumberOfDecimals="4"></Evol:CurrencyTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMargenImporte" ControlToValidate="txtMargenImporte" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFinanciacion" runat="server" Text="Financiacion %"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtFinanciacion" Enabled="false" runat="server" NumberOfDecimals="4"></Evol:CurrencyTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFinanciacion" ControlToValidate="txtFinanciacion" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblInicioVigencia" runat="server" Text="Inicio de Vigencia"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtInicioVigencia" Enabled="false" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvInicioVigencia" ControlToValidate="txtInicioVigencia" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFinVigencia" runat="server" Text="Fin de Vigencia"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFinVigencia" Enabled="false" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="RequiredFieldValidator4" ControlToValidate="txtFinVigencia" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-3"></div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>
            <div class="col-sm-8"></div>
        </div>
    </asp:Panel>
    <AUGE:popUpBuscarListaPrecio ID="ctrBuscarListaPrecio" runat="server"></AUGE:popUpBuscarListaPrecio>
    <%--<auge:popUpImportarArchivo ID="ctrImportarArchivo" Visible="false" runat="server" />--%>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:UpdatePanel ID="upTabControl" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
                <asp:TabPanel runat="server" ID="tpPrecioDetalles" HeaderText="Lista Precios Detalles">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Visible="false" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                                        <asp:Button CssClass="botonesEvol" ID="btnImportarLista" runat="server" Visible="false" Text="Importar Lista" OnClick="btnImportarLista_Click" />
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcionFiltro" runat="server" Text="Producto"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionFiltro" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNuevos" runat="server" Text="Solo Nuevos"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:CheckBox ID="chkNuevos" CssClass="form-control" runat="server" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Button CssClass="botonesEvol" ID="btnFiltrar" runat="server" Visible="true" Text="Filtrar" OnClick="btnFiltrar_Click" />
                                        <asp:Button CssClass="botonesEvol" ID="btnLimpiar" runat="server" Visible="true" Text="Limpiar" OnClick="btnLimpiar_Click" />
                                        <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                                    </div>
                                </div>

                                <AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />
                                <div class="form-group row">
                                    <div class="col-sm-12">
                                        <asp:GridView ID="gvItems" AllowPaging="true" PageSize="50" AllowSorting="false"
                                            OnRowCommand="gvItems_RowCommand" DataKeyNames="IdListaPrecioDetalle" OnPageIndexChanging="gvItems_PageIndexChanging"
                                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                            OnRowDataBound="gvItems_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Código">
                                                    <ItemTemplate>
                                                        <asp:TextBox CssClass="form-control" ID="txtCodigoProducto" runat="server" Enabled="false" Text='<%#Bind("IdProducto") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" AutoPostBack="true" CommandName="BuscarProducto" CommandArgument='<%# Container.DisplayIndex%>' ID="btnBuscarProducto" Visible="false"
                                                            AlternateText="Buscar producto" ToolTip="Buscar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Producto">
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox CssClass="form-control" ID="txtProducto" runat="server" Enabled="false" Text='<%#Bind("Descripcion") %>'></asp:TextBox>--%>
                                                        <%# Eval("Descripcion")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Costo">
                                                    <ItemTemplate>
                                                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecio" Enabled="false" runat="server" Text='<%# Eval("Precio", "{0:C4}") %>' NumberOfDecimals="4"></Evol:CurrencyTextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Margen %">
                                                    <ItemTemplate>
                                                        <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtMargenPorcentaje" Enabled="false" runat="server" Text='<%#Bind("MargenPorcentaje", "{0:N4}") %>' NumberOfDecimals="4"></Evol:CurrencyTextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Precio">
                                                    <ItemTemplate>
                                                        <asp:Label CssClass="col-form-label" ID="lblPrecioTotal" runat="server" Text='<%#Bind("PrecioConMargen", "{0:C4}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Moneda">
                                                    <ItemTemplate>
                                                        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" Enabled="false" runat="server"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estado">
                                                    <ItemTemplate>
                                                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstadosDetalles" Enabled="false" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Precio Editable" TextAlign="Left" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" Checked='<%# Eval("PrecioEditable")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="5%" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                                            AlternateText="Eliminar" ToolTip="Eliminar Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpFiliales" HeaderText="Filiales">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="chkFiliales" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpAfiliados" HeaderText="Clientes">
                    <ContentTemplate>
                        <asp:UpdatePanel ID="upAfiliados" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlDatosDelSocio" Visible="false" GroupingText="Datos del Cliente" runat="server">
                                    <div class="form-group row">
                                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Numero" />
                                        <div class="col-sm-2">
                                            <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSocio" AutoPostBack="true" OnTextChanged="txtNumeroSocio_TextChanged" runat="server" />
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCliente" ID="btnBuscarSocio" Visible="true"
                                                AlternateText="Buscar socio" ToolTip="Buscar" OnClick="btnBuscarCliente_Click" />
                                            <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="IngresarCliente" ControlToValidate="txtNumeroSocio" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-sm-8"></div>
                                    </div>
                                </asp:Panel>
                                <AUGE:popUpBuscarCliente ID="ctrBuscarClientePopUp" runat="server" />
                                <asp:GridView ID="gvDatosAfiliados" OnRowCommand="gvDatosAfiliados_RowCommand" AllowPaging="true" AllowSorting="true"
                                    OnRowDataBound="gvDatosAfiliados_RowDataBound" DataKeyNames="IdAfiliado"
                                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                    OnPageIndexChanging="gvDatosAfiliados_PageIndexChanging" OnSorting="gvDatosAfiliados_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Codigo" SortExpression="IdAfiliado">
                                            <ItemTemplate>
                                                <%# Eval("IdAfiliado")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento.TipoDocumento">
                                            <ItemTemplate>
                                                <%# Eval("TipoDocumento.TipoDocumento")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
                                        <asp:BoundField HeaderText="Razon Social" DataField="RazonSocial" SortExpression="RazonSocial" />
                                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Estado.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                                    AlternateText="Borrar Cliente de la lista de precio" ToolTip="Borrar Cliente de la lista de precio" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpImportarArchivo" Visible="true" HeaderText="Importar Archivo">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblListaPrecio" runat="server" Text="Lista de Precio"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlListasPrecios" Enabled="false" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvListasPrecios" ControlToValidate="ddlListasPrecios" ValidationGroup="DescargarPlantilla" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnDescargarArchivo" runat="server" Text="Descargar Plantilla" OnClick="btnDescargarPlantilla_Click" ValidationGroup="DescargarPlantilla" />
                            </div>
                            <div class="col-sm-3"></div>
                        </div>
                        <div class="form-group row">
                        <asp:Label CssClass="col-sm-12 col-form-label" ID="lblColumnas" runat="server" Width="100%" Text="Nombre de Columnas Obligatorias"></asp:Label>
                        </div>
                        <div class="form-group row">
                        <asp:Label CssClass="col-sm-12 col-form-label" ID="lblColumnasDetalles" runat="server" Width="100%" Text="IdProducto, Descripcion, Precio, MargenPorcentaje, PrecioEditable, Moneda, IdMoneda, Familia, IdFamilia, Incluir"></asp:Label>
                        </div>
                            <asp:PlaceHolder ID="phSubirArchivo" Visible="false" runat="server">
                            <div class="form-group row">
                            <asp:Label CssClass="col-sm-12 col-form-label" ID="lblArchivo" runat="server" Text="Adjuntar archivo"></asp:Label>
                            <div class="col-sm-8">
                                <asp:AsyncFileUpload ID="afuArchivo" OnUploadedFileError="afuArchivo_UploadedFileError" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server"
                                UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
                            </div>
                                <div class="col-sm-3">
                                <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
                        </div>
                                </div>
                                </asp:PlaceHolder>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
                    <ContentTemplate>
                        <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="tcDatos$tpImportarArchivo$btnDescargarArchivo" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" Visible="false"
                    onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
