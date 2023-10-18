<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListasValoresListar.aspx.cs" Inherits="IU.Modulos.TGE.ListasValoresListar" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceEncabezado" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasValoresListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

                  <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblParametro" runat="server" Text="Parametro" />
            <div class="col-lg-3 col-md-3 col-sm-9">     <asp:TextBox CssClass="form-control" ID="txtParametro" runat="server"></asp:TextBox>
            </div> <div class="col-lg-3 col-md-3 col-sm-9">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
              </div></div>

            <div class="table-responsive">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Código" DataField="IdListaValor" ItemStyle-Wrap="false" SortExpression="IdListaValor" />
                        <asp:BoundField  HeaderText="Nombre Lista" DataField="ListaValor" SortExpression="ListaValor" />
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
    </div>
</asp:Content>