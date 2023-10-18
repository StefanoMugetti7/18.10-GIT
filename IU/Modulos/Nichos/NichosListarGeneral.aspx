<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="NichosListarGeneral.aspx.cs" Inherits="IU.Modulos.Nichos.NichosListarGeneral" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script lang="javascript" type="text/javascript">

        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
            SetTabIndexInput();

        });

        function SetTabIndexInput() {
            $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }

        function EjecutarFiltro(IdCementerio, IdEstado) {
            $('select[id$="ddlEstados"] option:selected').val(IdEstado);
            __doPostBack("<%=btnBuscar.UniqueID %>", "");
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="cards">
                <div class="card-group">
                    <asp:Literal ID="ltrCards" runat="server"></asp:Literal>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSocio" runat="server" Text="Socio"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control text-right" ID="txtSocio" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCementerio" runat="server" Text="Cementerio"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCementerio" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCementerio" ControlToValidate="ddlCementerio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                        OnClick="btnBuscar_Click" />
                </div>
            </div>

            

            <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdNicho, IdTipoCargoAfiliadoFormaCobro,IdAfiliado"
                runat="server" SkinID="GrillaBasicaFormalSticky"  AutoGenerateColumns="false" ShowFooter="true" 
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting">
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
                 <asp:TemplateField HeaderText="Fecha Confirmacion" ItemStyle-Wrap="false" SortExpression="FechaConfirmacion">
                        <ItemTemplate>
                            <%# Eval("FechaConfirmacion", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cementerio" SortExpression="Cementerio">
                        <ItemTemplate>
                            <%# Eval("Cementerio")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Codigo" SortExpression="Codigo">
                        <ItemTemplate>
                            <%# Eval("Codigo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo" SortExpression="TipoNicho">
                        <ItemTemplate>
                            <%# Eval("TipoNicho")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Capacidad" SortExpression="NichoCapacidad">
                        <ItemTemplate>
                            <%# Eval("NichoCapacidad")%>
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
                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" Visible ="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" Visible="false" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
