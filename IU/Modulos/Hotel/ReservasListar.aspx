<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ReservasListar.aspx.cs" Inherits="IU.Modulos.Hotel.ReservasListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            SetTabIndexInput();
        });
        function SetTabIndexInput() {
            $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }
    </script>
    <div class="ReservasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroReserva" runat="server" Text="Número de Reserva"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtNumeroReserva" runat="server" ThousandsSeparator="" Prefix="" NumberOfDecimals="0"></Evol:CurrencyTextBox>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblHoteles" runat="server" Text="Hotel"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlHoteles" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstados" runat="server" Text="Estado"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass=" form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                    </div>

                </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblApellido" runat="server" Text="Apellido Nombre"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server" />
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlReservaTipoDocumento" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtReservaNumeroDocumento" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFecha" runat="server" Text="Fecha Ingreso"></asp:Label>
                    <div class="col-sm-3">
                        <div class="form-group row">
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaIngreso" Placeholder="Desde" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">

                                <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaIngresoHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                    </div>
                    <div class="col-sm-3">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                                    OnClick="btnBuscar_Click" />
                                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                                    OnClick="btnAgregar_Click" />
                            </ContentTemplate>

                        </asp:UpdatePanel>

                    </div>
                </div>
                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                    runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdReserva"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="# Reserva" SortExpression="ApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("IdReserva")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cliente" SortExpression="ApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Numero Documento" SortExpression="NumeroDocumento">
                            <ItemTemplate>
                                <%# Eval("NumeroDocumento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Ingreso" ItemStyle-Wrap="false" SortExpression="FechaIngreso">
                            <ItemTemplate>
                                <%# Eval("FechaIngreso", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Egreso" ItemStyle-Wrap="false" SortExpression="FechaEgreso">
                            <ItemTemplate>
                                <%# Eval("FechaEgreso", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("PrecioTotal", "{0:C2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Usuario Alta" SortExpression="UsuarioAlta">
                            <ItemTemplate>
                                <%# Eval("UsuarioAlta")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
