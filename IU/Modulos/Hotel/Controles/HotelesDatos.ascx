<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HotelesDatos.ascx.cs" Inherits="IU.Modulos.Hotel.Controles.HotelesDatos" %>

<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>

<script type="text/javascript">
    $(document).ready(function ()
    {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitSelect2);
        InitSelect2();
    });

    function InitSelect2()
    {
        var ddlHoraIngreso = $("select[name$='ddlHoraIngreso']");
        ddlHoraIngreso.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
        var ddlHoraEgreso = $("select[name$='ddlHoraEgreso']");
        ddlHoraEgreso.select2({ width: '100%', selectOnClose: true, theme: 'bootstrap4', language: 'es' });
    }
</script>


<div class="HotelesHabitaciones">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Nombre del Hotel"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
        </div>

        <%--<asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCondicionesFiscales" runat="server" Text="Condicion Fiscal"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" runat="server">
            </asp:DropDownList>
        </div>--%>

        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>

        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" ControlToValidate="ddlFilial" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
        </div>

        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraIngreso" runat="server" Text="Hora Ingreso"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlHoraIngreso" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfvHoraIngreso" ControlToValidate="ddlHoraIngreso" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHoraEgreso" runat="server" Text="Hora Egreso"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlHoraEgreso" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador2" ID="rfHoraEgreso" ControlToValidate="ddlHoraEgreso" ValidationGroup="Aceptar" runat="server"></asp:RequiredFieldValidator>
        </div>
    </div>
</div>
<div class="form-group row">
     <div class="col-1">
        <asp:Button CssClass="botonesEvol" ID="btnAgregarHabitacion" runat="server" Text="Agregar Habitacion" OnClick="btnAgregarHabitacion_Click" />
    </div>
</div> 
<div class="table-responsive">
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdHabitacion"
        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
        <Columns>
            <asp:TemplateField HeaderText="Producto" SortExpression="Producto">
                <ItemTemplate>
                    <%# Eval("Producto")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nro Habitacion" SortExpression="NumeroHabitacion">
                <ItemTemplate>
                    <%# Eval("NumeroHabitacion")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nombre Habitacion" SortExpression="NombreHabitacion">
                <ItemTemplate>
                    <%# Eval("NombreHabitacion")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Piso" SortExpression="NombreHabitacion">
                <ItemTemplate>
                    <%# Eval("Piso")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cant.Personas" SortExpression="NombreHabitacion">
                <ItemTemplate>
                    <%# Eval("Cantidad")%>
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
<contenttemplate>
    <div class="row justify-content-md-center">
        <div class="col-md-auto">
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </div>
    </div>
</contenttemplate>




    