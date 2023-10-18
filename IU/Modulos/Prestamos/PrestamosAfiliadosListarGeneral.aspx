<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosListarGeneral.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosListarGeneral" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script lang="javascript" type="text/javascript">

        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
            SetTabIndexInput();

        });

        function SetTabIndexInput() {
            $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }

        function EjecutarFiltro(IdEstado) {
            $('select[id$="ddlEstados"] option:selected').val(IdEstado);
            __doPostBack("<%=btnBuscar.UniqueID %>", "");
        }
    </script>


        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
            <div class="cards" >
                <div class="card-group">
                    <asp:Literal ID="ltrCards" runat="server"></asp:Literal>
                </div>
            </div>

      </ContentTemplate>
      </asp:UpdatePanel>



    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Nro Socio"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control text-right" ID="txtNumeroSocio" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Nro Doc"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Generado Desde"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Generado Hasta"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblVendedor" runat="server" Text="Vendedor"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlVendedor" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma Cobro"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro" runat="server">
                    </asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellidoNombre" runat="server" Text="Apellido Nombre"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtApellidoNombre" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                        OnClick="btnBuscar_Click" />
                </div>
            </div>

            <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="false"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado, IdPrestamo, IdTipoOperacion"
                runat="server" SkinID="GrillaBasicaFormalSticky" onpageindexchanging="gvDatos_PageIndexChanging" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Número Socio" ItemStyle-Wrap="false" SortExpression="NumeroSocio">
                        <ItemTemplate>
                            <%# Eval("NumeroSocio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Número Documento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento">
                        <ItemTemplate>
                            <%# Eval("NumeroDocumento")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apellido Nombre" ItemStyle-Wrap="false" SortExpression="ApellidoNombre">
                        <ItemTemplate>
                            <%# Eval("ApellidoNombre")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false" SortExpression="FechaPrestamo">
                        <ItemTemplate>
                            <%# Eval("FechaPrestamo", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Autorizado" ItemStyle-Wrap="false" SortExpression="FechaAutorizado">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("FechaAutorizado")) > Convert.ToDateTime("1900/01/01") ?
                                        Eval("FechaAutorizado", "{0:dd/MM/yyyy}") : string.Empty %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="A partir de" ItemStyle-Wrap="false" SortExpression="FechaValidezAutorizado">
                                <ItemTemplate>
                                    <%# Convert.ToDateTime(Eval("FechaValidezAutorizado")) > Convert.ToDateTime("1900/01/01") ?
                                        Eval("FechaValidezAutorizado", "{0:dd/MM/yyyy}") : string.Empty %>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Confirmado" ItemStyle-Wrap="false" SortExpression="FechaConfirmacion">
                        <ItemTemplate>
                            <%# Convert.ToDateTime(Eval("FechaConfirmacion")) > Convert.ToDateTime("1900/01/01") ?
                                        Eval("FechaConfirmacion", "{0:dd/MM/yyyy}") : string.Empty %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="#Prestamo" ItemStyle-Wrap="false" DataField="NroDeIdentificacion" SortExpression="NroDeIdentificacion" />
                    <asp:TemplateField HeaderText="Importe" SortExpression="ImportePrestamo" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("ImportePrestamo", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cuota" SortExpression="ImporteCuota" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("ImporteCuota", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                    <asp:BoundField HeaderText="Cuotas" ItemStyle-Wrap="false" DataField="CantidadCuotas" SortExpression="CantidadCuotas" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <HeaderTemplate>
                            <asp:Label ID="lblCantidadCuotasPendientes" runat="server" Text="Pendientes" ToolTip="Cantidad de Cuotas Pendientes"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("CantidadCuotasPendientes")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Deuda" SortExpression="SaldoDeuda" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Moneda"), Eval("SaldoDeuda", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion">
                        <ItemTemplate>
                            <%# Eval("TipoOperacion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Forma de Cobro" SortExpression="FormaCobro">
                        <ItemTemplate>
                            <%# Eval("FormaCobro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inversor" SortExpression="Inversor">
                        <ItemTemplate>
                            <%# Eval("Inversor")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vendedor" SortExpression="Vendedor">
                        <ItemTemplate>
                            <%# Eval("Vendedor")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Leg.Conf." SortExpression="LegajoConformado">
                        <ItemTemplate>
                            <%# Eval("LegajoConformado")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                        <ItemTemplate>
                            <%# Eval("EstadoDescripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/9x.png" runat="server" CommandName="PreAutorizar" ID="btnPreAutorizar"
                                AlternateText="Pre Autorizar" ToolTip="Pre Autorizar" Visible="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" ID="btnAutorizar"
                                AlternateText="Autorizar" ToolTip="Autorizar" Visible="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Cancelar" ID="btnCancelar"
                                AlternateText="Cancelar" ToolTip="Cancelar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="AnularCancelar" ID="btnAnularCancelar"
                                AlternateText="Anular Cancelación Pendiente" ToolTip="Anular Cancelación Pendiente" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Anular" ID="btnAnular"
                                AlternateText="Anular" ToolTip="Anular" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" Visible="false" CommandName="AnularConfirmar" ID="btnAnularConfirmado"
                                AlternateText="Anular Prestamo Confirmado" ToolTip="Anular Prestamo Confirmado" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
