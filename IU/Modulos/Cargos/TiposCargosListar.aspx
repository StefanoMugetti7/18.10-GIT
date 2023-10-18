<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposCargosListar.aspx.cs" Inherits="IU.Modulos.Cargos.TiposCargosListar" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo de Cargo" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtTipoCargo" runat="server" />
                </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
            </div>
                <div class="col-sm-3">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
        </div>
                </div>
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                <Columns>
                    <asp:BoundField  HeaderText="Código" DataField="CodigoCargo" ItemStyle-Wrap="false" SortExpression="Codigo" />
                    <asp:BoundField  HeaderText="Tipo de Cargo" DataField="TipoCargo" SortExpression="TipoCargo" />
                    <asp:BoundField  HeaderText="Importe" DataField="Importe" SortExpression="Importe" />
                    <asp:BoundField  HeaderText="Permite Cuotas" DataField="PermiteCuotasTexto" SortExpression="PermiteCuotasTexto" />
                    <asp:BoundField  HeaderText="Maximo Cuotas" DataField="CantidadMaximaCuotas" SortExpression="CantidadMaximaCuotas" />
                    <asp:TemplateField HeaderText="Tipo Proceso" SortExpression="TipoCargoProceso.Descripcion">
                        <ItemTemplate>
                            <%# Eval("TipoCargoProceso.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>       
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>            
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>