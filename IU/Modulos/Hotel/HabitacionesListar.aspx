<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="HabitacionesListar.aspx.cs" Inherits="IU.Modulos.Hotel.HabitacionesListar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>

    <script type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        SetTabIndexInput();
    });

        function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

        </script>
    <div class="ReservasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>       <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblHotel" runat="server" Text="Hotel"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">  
                    <asp:DropDownList CssClass="form-control select2" ID="ddlHoteles" runat="server">
                    </asp:DropDownList>
          
             </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNumero" runat="server" Text="Número"></asp:Label>
               <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtNumeroHabitacion" runat="server"></asp:TextBox>
            </div>
                 <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
                onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
            onclick="btnAgregar_Click" />
           </div></div>
                       <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombreHabitacion" runat="server" Text="Nombre"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtNombreHabitacion" runat="server" />
         </div></div>
                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                                runat="server" onclick="btnExportarExcel_Click" Visible="false" />
                <div class="table-responsive">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdHabitacion"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"  
            >
                <Columns>
                    <asp:TemplateField HeaderText="Hotel" SortExpression="Hotel">
                            <ItemTemplate>
                                <%# Eval("Hotel")%>
                            </ItemTemplate>
                    </asp:TemplateField>
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
            </asp:GridView></div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

