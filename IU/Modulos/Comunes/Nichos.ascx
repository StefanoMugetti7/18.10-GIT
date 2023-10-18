<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Nichos.ascx.cs" Inherits="IU.Modulos.Comunes.Nichos" %>


<style>
   .GridViewHeaderStyle {
    position: -webkit-sticky; /* this is for all Safari (Desktop & iOS), not for Chrome*/
    position: sticky;
    top: 0;
    z-index: 1; /* any positive value, layer order is global*/
}
</style>

<asp:UpdatePanel ID="pnlPrinc" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlBuscador" runat="server">
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCementerio" runat="server" Text="Cementerio"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCementerio" OnSelectedIndexChanged="ddlCementerio_OnSelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCementerio" ControlToValidate="ddlCementerio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblPanteon" runat="server" Text="Panteon"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlPanteon" Enabled="false" runat="server"></asp:DropDownList>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoNicho" runat="server" Text="Tipo Nicho"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoNicho" OnSelectedIndexChanged="ddlTipoNicho_OnSelectedIndexChanged" AutoPostBack="true" Enabled="true" runat="server"></asp:DropDownList>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCapacidadNicho" runat="server" Text="Capacidad"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlNichoCapacidad" Enabled="false" runat="server"></asp:DropDownList>
                </div>
                <div class="col-sm-5">
                </div>
                <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                        OnClick="btnBuscar_Click" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="pnlGrilla" Visible="false" runat="server" UpdateMode="Conditional">  
    <ContentTemplate>
        <div class="overflow-auto" style="max-height:300px">
        <asp:HiddenField ID="hdfIdNicho" runat="server" />
        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdNicho" style="border-collapse:inherit"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Cementerio" SortExpression="Cementerio">
                    <ItemTemplate>
                        <%# Eval("Cementerio")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="# Panteon" SortExpression="Panteon">
                    <ItemTemplate>
                        <%# Eval("Panteon")%>
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
                <asp:TemplateField HeaderText="Ubicacion" SortExpression="NichoUbicacion">
                    <ItemTemplate>
                        <%# Eval("NichoUbicacion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SubUbicacion" SortExpression="NichoSubUbicacion">
                    <ItemTemplate>
                        <%# Eval("NichoSubUbicacion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                            AlternateText="Consultar" ToolTip="Consultar" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" Visible="false" CommandName="Consultar" ID="btnImprimir"
                            AlternateText="Imprimir" ToolTip="Imprimir" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Cancelar" ID="btnCancelar"
                            AlternateText="Cancelar" ToolTip="Cancelar" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
            </div>
    </ContentTemplate>       
</asp:UpdatePanel>


