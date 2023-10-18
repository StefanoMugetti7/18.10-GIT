<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FondoSuplementarioDatos.ascx.cs" Inherits="IU.Modulos.Haberes.Controles.FondoSuplementarioDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>

<script>
    function UpdPanelUpdate() {
        __doPostBack("<%=button.UniqueID %>", "");
        //document.getElementById('<%= button.ClientID %>').click();        
    }

    function CalcularMeses() {
        var cantidadMesesNuevo = 0;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
        var cantidadMeses = $(this).find("input:text[id*='txtCantidadMeses']").val();

            var cantidadMesesParseado = parseInt(cantidadMeses);
            if (cantidadMesesParseado > 0)
                cantidadMesesNuevo += cantidadMesesParseado;
        });
        
        $("#<%=gvDatos.ClientID %> [id$=lblTotal]").text("Cant. Total Meses: " + cantidadMesesNuevo);
    }

    function ValidarCalcular() {
        var hidden = $("input[type='hidden'][id$='hdfValidarCalcular']")
        hidden.val("1");
    }

</script>


<asp:UpdatePanel ID="upFondoSuplementario" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoInicioJubilatorio" runat="server" Text="Periodo Inicio Jubilatorio" />
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtPeriodoInicioJubilatorio" MaxLength="6" runat="server" AutoPostBack="true" OnTextChanged="txtPeriodoInicioJubilatorio_TextChanged"></AUGE:NumericTextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Inicio de Jubilación" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaJubilacion" runat="server"></asp:TextBox>
            </div>
               </div>
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpLiquidacionTotal" HeaderText="Liquidación Total">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upLiquidacionTotal" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <asp:GridView ID="gvLiquidacion" DataKeyNames="" AllowPaging="false" AllowSorting="false"
                                    OnRowCommand="gvLiquidacion_RowCommand" runat="server" SkinID="GrillaResponsive"
                                    AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvLiquidacion_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Aporte Inicial %" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                            <%# Eval("AporteInicialPorcentual")%>
                                                </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aporte Inicial $" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                             <%# Eval("AporteInicialPesos", "{0:C4}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aporte Total %" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                                 <%# Eval("AporteTotalPorcentual")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Aporte Total $" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                                 <%# Eval("AporteTotalPesos", "{0:C4}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sueldo Promedio $" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                                <%# Eval("SueldoPromedio", "{0:C4}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sueldo Bruto $" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                                <%# Eval("SueldoBruto", "{0:C4}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sueldo Suplementario $" ItemStyle-Wrap="false" SortExpression="">
                                            <ItemTemplate>
                                                <%# Eval("SueldoSuplementario", "{0:C4}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>

            <asp:TabPanel runat="server" ID="tpGrilla" HeaderText="Aportes Jubilatorios">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upGrilla" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:PlaceHolder ID="phAgregarItem" runat="server">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                                    <div class="col-sm-1">
                                        <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                                    </div>
                                </asp:PlaceHolder>
                            </div>
                            
                            <div class="table-responsive">
                                <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                                    OnRowCommand="gvDatos_RowCommand" runat="server" SkinID="GrillaResponsive"
                                    AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvDatos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Cantidad de Meses" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control select2" ID="txtCantidadMeses" Text='<%#Bind("CantidadMeses") %>' runat="server" Prefix="" NumberOfDecimals="0"></Evol:CurrencyTextBox>
                                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidadMeses" ControlToValidate="txtCantidadMeses" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label CssClass="labelFooterEvol" ID="lblTotal" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Categoria" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:DropDownList CssClass="form-control select2" ID="ddlCategorias" runat="server" />
                                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCategorias" ControlToValidate="ddlCategorias" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Coeficiente" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control select2" ID="txtCoeficiente" Text='<%#Bind("Coeficiente") %>' runat="server" Prefix="" DecimalSeparator="," NumberOfDecimals="4"></Evol:CurrencyTextBox>
                                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCoeficiente" ControlToValidate="txtCoeficiente" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Eliminar" SortExpression="">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>

            <asp:TabPanel Visible ="false" runat="server" ID="tpArchivo" HeaderText="Importar Archivo">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                    <asp:Button CssClass="botonesEvol" ID="btnDescargarPlantilla" runat="server" Text="Descargar Plantilla" OnClick="btnDescargarPlantilla_Click" CausesValidation="false" />
                            </div>
                    </div>
                    <asp:UpdatePanel ID="upArchivos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblArchivo" runat="server" Text="Adjuntar archivo"></asp:Label>
                                <asp:AsyncFileUpload ID="afuArchivo" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server"
                                    UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
                                <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:HiddenField id="hdfValidarCalcular" runat="server"/>
                <asp:Button CssClass="botonesEvol" ID="btnCalcularAporte" runat="server" Text="Calcular"  OnClick="btnCalcularAporte_Click" ValidationGroup="Calcular" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" Visible="false" />
                <asp:Button CssClass="botonesEvol" ID="btnModificar" runat="server" Text="Modificar" OnClick="btnModificar_Click" Visible="false" />
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
