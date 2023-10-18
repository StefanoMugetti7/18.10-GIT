<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CobranzasExternasConciliacionesDatos.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.CobranzasExternasConciliacionesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<div class="CobranzasExternasDatos">

    <script language="javascript" type="text/javascript">

        var importeTotal;
        var deduccionesTotal;

        function CalcularItem() {
            importeTotal = 0.00;

            var cobranzasTotal = 0.00;
            $('#<%=gvCobranzaDetalles.ClientID%> tr').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find('span[id*="lblImporte"]').text();;; //$("td:eq(4)", this).html();


            if (incluir) {
                importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                cobranzasTotal += parseFloat(importe);
                importeTotal += parseFloat(importe);

            }

        });
        $("#<%=gvCobranzaDetalles.ClientID %> [id$=lblTotalDetalles]").text(accounting.formatMoney(cobranzasTotal, "$ ", 2, "."));
            CalcularDeduccion();
            MostrarTotales();
        }



        function CalcularDeduccion() {
            deduccionesTotal = 0.00;

            $('#<%=gvDeducciones.ClientID%> tr').each(function () {

                var importe = $(this).find("input:text[id*='txtImporteDeduccion']").maskMoney('unmasked')[0];
                
                if (importe) {
                //importe = importe.val().replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                deduccionesTotal += parseFloat(importe);

                }
            });
            
            $("#<%=gvDeducciones.ClientID %> [id$=lblImporteDeducciones]").text(accounting.formatMoney(deduccionesTotal, "$ ", 2, "."));
            MostrarTotales();
        }


        function MostrarTotales() {
            //        alert(importeTotal);
            //        alert(deduccionesTotal);

            if (isNaN(importeTotal)) {
                importeTotal = 0.00;
            }
            if (isNaN(deduccionesTotal)) {
                deduccionesTotal = 0.00;
            }
            $("input[type=text][id$='txtTotal']").val(accounting.formatMoney(importeTotal - deduccionesTotal, "$ ", 2, "."));

        }
//    function CalcularTotal() {
//        var importeTotal = 0.00;

//        var importeDeducciones = $("#<%=gvDeducciones.ClientID %> [id$=lblImporteDeducciones]").val();
//        var importeDetalles = $("#<%=gvCobranzaDetalles.ClientID %> [id$=lblTotalDetalles]").val();
//        alert(importeDeducciones);
//        alert(importeDetalles);
//        if (importeDeducciones || importeDetalles) {
//            importeTotal = importeDeducciones + importeDetalles;
//        }
//        alert(importeTotal);

//        $("input[type=text][id$='txtTotal']").val(accounting.formatMoney(importeTotal, "$ ", 2, "."));
//    }


    </script>

    <asp:UpdatePanel ID="upRefFormaCobro" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="Filial" runat="server" Text="Filial" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlFiliales" ValidationGroup="Buscar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma Cobro" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server" OnSelectedIndexChanged="ddlFormaCobro_SelectedIndexChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFormaCobro" runat="server" ErrorMessage="*"
                        ControlToValidate="ddlFormaCobro" ValidationGroup="Buscar" />
                </div>

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRefFormaCobro" runat="server" Text="Ref Forma Cobro:"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlRefFormaCobro" Enabled="false" runat="server" OnSelectedIndexChanged="ddlRefFormaCobro_SelectedIndexChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvRefFormaCobro" runat="server" ControlToValidate="ddlRefFormaCobro"
                        ErrorMessage="*" ValidationGroup="Buscar" />
                </div>
            </div>
            <div class="form-group row">
              
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                  <div class="col-sm-3">
           <div class="form-group row">
                     <div class="col-sm-6">
                    <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>

                </div>
                <div class="col-sm-6">
                    <asp:TextBox CssClass="form-control datepicker" PlaceHolder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                </div></div>
                </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLote" runat="server" Text="Lote"></asp:Label>
            <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control"  ID="txtLote" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" Visible="false"
                        OnClick="btnBuscar_Click" ValidationGroup="Buscar" />
                </div>
                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upGrilla" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlCobranzaDetalles" GroupingText="Detalle de Tarjetas" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvCobranzaDetalles"
                        OnRowDataBound="gvCobranzaDetalles_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvCobranzaDetalles_PageIndexChanging">
                        <Columns>

                            <asp:TemplateField HeaderText="Tarjeta" SortExpression="Tarjeta">
                                <ItemTemplate>
                                    <%# Eval("TarjetaTransaccion.TarjetaDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Numero" SortExpression="Nro. TC">
                                <ItemTemplate>
                                    <%# Eval("TarjetaTransaccion.NumeroTarjetaCredito")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Fecha" SortExpression="Fecha">
                                <ItemTemplate>
                                    <%# Eval("FechaTransaccion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lote" SortExpression="Nro. Lote">
                                <ItemTemplate>
                                    <%# Eval("NumeroLote")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detalle" SortExpression="Detalle" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("Detalle")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:TemplateField HeaderText="Importe" SortExpression="Importe">
                            <ItemTemplate>
                                <%# Eval("TarjetaTransaccion.Importe")%>
                            </ItemTemplate>
                    </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Importe" SortExpression="TotalItem" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" Text='<%#Bind("TarjetaTransaccion.Importe", "{0:N2}") %>' runat="server" Enabled="false"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblTotalDetalles" runat="server" Text="0.00"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIncluir" Visible="true" Checked='<%#Eval("Checked") %>' runat="server" CommandName="Incluir" />
                                    </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <%-- ACA VA LA OTRA GRILLA --%>
    <%-- Insertar Grilla --%>
    <asp:UpdatePanel ID="upDeducciones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <asp:Button CssClass="botonesEvol" ID="btnAgregarDeduccion" runat="server" Text="Agregar Deduccion" Visible="false" OnClick="btnAgregarDeduccion_Click" />
            <br />
            <br />
            <div class="table-responsive">
            <asp:GridView ID="gvDeducciones" AllowPaging="true" AllowSorting="true"
                OnRowCommand="gvDeducciones_RowCommand" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="Grillaresponsive" AutoGenerateColumns="false" ShowFooter="true"
                OnRowDataBound="gvDeducciones_RowDataBound">
                <Columns>

                    <asp:TemplateField HeaderText="Deducciones" SortExpression="" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlDeducciones" runat="server" />
                            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvDeducciones" runat="server" ControlToValidate="ddlDeducciones" ErrorMessage="*" ValidationGroup="Aceptar"/>--%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Importe Deduccion" SortExpression="">
                        <ItemTemplate>
                                   <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteDeduccion" runat="server" Text='<%#Bind("ImporteDeduccion", "{0:N2}") %>'></Evol:CurrencyTextBox>
                
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteDeducciones" runat="server" Text="0.00" Style="text-align: right"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Eliminar" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
    <%-- HERE --%>
    <br />
    <asp:UpdatePanel ID="upTotal" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTotal" runat="server" Text="Total" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtTotal" Enabled="false" runat="server" /></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Banco Cuenta:"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlBancoCuenta" Enabled="false" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancoCuenta" runat="server" ControlToValidate="ddlBancoCuenta"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
    </div>
    <asp:UpdatePanel ID="upFechaConfirmacion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaConfirmacionBco" runat="server" Text="Fecha Confirmacion"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaConfirmacion" runat="server"></asp:TextBox>
                     <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaConfirmacion" runat="server" ControlToValidate="txtFechaConfirmacion"
                ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <center>
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <%--<asp:ImageButton ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>
