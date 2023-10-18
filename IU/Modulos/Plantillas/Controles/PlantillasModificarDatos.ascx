<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlantillasModificarDatos.ascx.cs" Inherits="IU.Modulos.Plantillas.Controles.PlantillasModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<script type="text/javascript">

    function UpdPanelUpdate() {

        __doPostBack("<%= button.ClientID %>", "");

    }
</script>

<style>
    .columns > .editor {
        float: left;
        width: 80%;
        position: relative;
        z-index: 1;
    }

    .columns > .campos {
        float: right;
        width: 20%;
        box-sizing: border-box;
        padding: 0 0 0 20px;
    }
</style>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblcodigo" runat="server" Text="Código" />

            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtCodigo" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlantilla" runat="server" Text="Plantilla" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtPlantilla" Enabled="false" runat="server" />

                <asp:RequiredFieldValidator ID="rfvPlantilla" ControlToValidate="txtPlantilla" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
            </div>

        </div>
        <div class="form-group row">

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblStore" runat="server" Text="Nombre SP" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtStore" Enabled="false" runat="server" />
            </div>


            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHojaAlto" runat="server" Text="Hoja Alto" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtHojaAlto" Prefix="" Enabled="false" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHojaAncho" runat="server" Text="Hoja Ancho" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtHojaAncho" Prefix="" Enabled="false" runat="server" />
            </div>



        </div>
        <div class="form-group row">

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMargenIzquierdo" runat="server" Text="Margen Izquierdo" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtMargenIzquierdo" Prefix="" Enabled="false" runat="server" />
            </div>


            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMargenDerecho" runat="server" Text="Margen Derecho" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtMargenDerecho" Prefix="" Enabled="false" runat="server" />
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMargenSuperior" runat="server" Text="Margen Superior" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtMargenSuperior" Prefix="" Enabled="false" runat="server" />
            </div>

        </div>
        <div class="form-group row">

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMargenInferior" runat="server" Text="Margen Inferior" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtMargenInferior" Prefix="" Enabled="false" runat="server" />
            </div>


            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumerarPaginas" runat="server" Text="Numerar Páginas" />

            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlNumerarPaginas" runat="server">
                    <asp:ListItem Text="Nada" Value="0" Selected="true" />
                    <asp:ListItem Text="Pagina N" Value="1" />
                    <asp:ListItem Text="Pagina N de N" Value="2" />

                </asp:DropDownList>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAjustarUnaHoja" runat="server" Text="Ajustar una hoja" />
            <div class="col-sm-3">
                <asp:CheckBox ID="chkAjustarUnaHoja" CssClass="form-control" runat="server" />
            </div>

        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsarHojaPorDefecto" runat="server" Text="No Usar Hoja Por Defecto" />
            <div class="col-sm-3">
                <asp:CheckBox ID="chkUsarHojaPorDefecto" CssClass="form-control" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoPlanilla" runat="server" Text="Tipo de Plantilla" />
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlTipoPlanilla" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoPlantilla_OnSelectedIndexChanged" runat="server" />
                <asp:RequiredFieldValidator ID="rfvTipoPlanilla" ControlToValidate="ddlTipoPlanilla" ValidationGroup="Aceptar" CssClass="Validador" runat="server" Enabled="false" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="Label3" runat="server" Text="Tipo de proceso" />
            <div class="col-sm-3">
                <asp:DropDownList ID="ddlTipoProceso" Enabled="false" CssClass="form-control select2" runat="server" />
            </div>


        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCopias" runat="server" Text="Cantidad Copias" />

            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCopias" Prefix="" NumberOfDecimals="0" Enabled="false" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRellenarFilasVacias" runat="server" Text="Rellenar filas" />
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtRellenarFilasVacias" Prefix="" NumberOfDecimals="0" Enabled="false" runat="server" />
            </div>
            
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblKeysPDFCorte" runat="server" Text="Keys Corte" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtKeysPDFCorte" Enabled="true" runat="server" />
            </div>

        </div>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpCuentaCorriente" TabIndex="0"
                HeaderText="Detalle de Plantilla">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-4">
                            <asp:AsyncFileUpload ID="afuLogo" Visible="true" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" runat="server"
                                UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
                            <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
                            <asp:UpdatePanel ID="upImagen" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>

                                    <asp:Label CssClass="col-sm-1" ID="lblImagen" runat="server" Visible="true" Text="Imagen de Fondo:"></asp:Label>

                                    <asp:Image ID="imgLogo" runat="server" Width="200px" Visible="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:Button CssClass="botonesEvol" ID="button" runat="server" OnClick="button_Click" Style="display: none;" />
                        </div>
                        <div class="col-sm-3">
                            <asp:Button CssClass="botonesEvol" ID="btnBorrarImagen" Text="Borrar Imagen" runat="server" Visible="false" OnClick="btnBorrarImagen_Click" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-2">
                            <asp:Button CssClass="botonesEvol" ID="btnPlantilla" runat="server" Text="Agregar Subplantilla" OnClick="btnAgregarPlantilla_Click" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                            <Columns>
                                <asp:BoundField HeaderText="SubCodigo" DataField="Codigo" SortExpression="Codigo" />
                                <asp:BoundField HeaderText="SubPlantilla" DataField="NombrePlantilla" SortExpression="NombrePlantilla" />
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                            AlternateText="Modificar" ToolTip="Modificar" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="columns">
                        <div class="editor row justify-content-md-center">
                            <CKEditor:CKEditorControl ID="CKEditor1" BasePath="~/ckeditor/"
                                ImageRemoveLinkByEmptyURL="true" Toolbar="Full" Width="750" Height="500" EnterMode="BR" AutoGrowOnStartup="true" AutoGrowMinHeight="500"
                                runat="server"></CKEditor:CKEditorControl>
                        </div>
                        <div class="campos">
                            <asp:Label CssClass="labelEvol" Width="100%" ID="Label2" runat="server" Text="Lista de Campos" />
                            <br />
                            <asp:ListBox CssClass="listbox" Width="100%" ID="lstCampos" Rows="15" runat="server"></asp:ListBox>
                            <br />
                            <asp:Label CssClass="labelEvol" Width="100%" ID="Label1" runat="server" Text="Ingrese en el formulario los campos que desea de la lista de arriba de la siguiente forma {NombreCampo}" />
                        </div>s
                    </div>
                    <div style="clear: both"></div>

                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="TabPanel1" TabIndex="0"
                HeaderText="Configuracion de Plantilla">
                <ContentTemplate>
                    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>

        </asp:TabContainer>
        <div class="row justify-content-md-center">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" Enabled="true" CausesValidation="false" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
