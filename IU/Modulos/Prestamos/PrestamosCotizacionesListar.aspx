<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestamosCotizacionesListar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosCotizacionesListar" Title="" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
</script>

    <div class="PrestamosCotizacionesListar">
        <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoUnidad" runat="server" Text="Tipo Unidad"></asp:Label>
            <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoUnidad" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-8"></div>
            <div class="col-sm-4">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
            </div>
        </div>
        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdCotizacionUnidad"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="#" SortExpression="IdCotizacionUnidad">
                    <ItemTemplate>
                        <%# Eval("IdCotizacionUnidad")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo Unidad" SortExpression="TipoUnidadDescripcion">
                    <ItemTemplate>
                        <%# Eval("TipoUnidadDescripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Coeficiente" SortExpression="Coeficiente">
                    <ItemTemplate>
                        <%# Eval("Coeficiente")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha Desde Que Aplica" SortExpression="FechaDesdeAplica">
                    <ItemTemplate>
                        <%# Eval("FechaDesdeAplica", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

