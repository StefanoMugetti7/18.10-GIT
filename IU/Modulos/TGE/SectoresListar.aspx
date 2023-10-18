<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SectoresListar.aspx.cs" Inherits="IU.Modulos.TGE.SectoresListar" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ConsultarStock">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <div class="form-group row"> 
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblSector" runat="server" Text="Sector" />
            <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtSector" runat="server" />
     </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
           <div class="col-lg-3 col-md-3 col-sm-9">  <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" />
      </div>

             <div class="col-lg-3 col-md-3 col-sm-9"><asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <div class="Espacio"></div>
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
</div></div>

            <div class="table-responsive">
        <asp:GridView ID="gvDatos"  AllowPaging="true" AllowSorting="true" OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                         <asp:BoundField  HeaderText="Sector" DataField="Sector" SortExpression="Sector" />  
                         
                         <asp:TemplateField HeaderText="Filial" SortExpression="Filial">
                            <ItemTemplate>
                                <%# Eval("Filial.Filial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         
                         <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                                              
                       <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        </Columns>
                </asp:GridView></div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>