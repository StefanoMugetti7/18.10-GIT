<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlanesListar.aspx.cs" Inherits="IU.Modulos.Prestamos.PlanesListar" Title="" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

             <div class="col-sm-12">
                <div class="form-group row">
                    
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlan" runat="server" Text="Nombre Plan"></asp:Label>
    <div class="col-sm-3">
    <asp:TextBox CssClass="form-control" ID="txtPlan" runat="server"></asp:TextBox>
     </div>

                   <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" />
     </div>

                    <asp:Label CssClass="col-sm-1 col-form-label"  ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" /></div>
                        
                    </div>
                     </div>
            <div class="form-group row">
                    <div class="col-sm-8"></div>
    <div class="col-sm-4">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />

    </div>

            </div>
            
                
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Código" DataField="IdPrestamoPlan" ItemStyle-Wrap="false" SortExpression="IdPrestamoPlan" />
                        <asp:BoundField  HeaderText="Descripción" DataField="Descripcion" SortExpression="PlazoDias" />
                        <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Moneda.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>      
                        <asp:BoundField  HeaderText="Fecha Alta" DataField="FechaAlta" SortExpression="FechaAlta" />
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
                <br />
              
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
