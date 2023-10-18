<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="EmpresasListar.aspx.cs" Inherits="IU.Modulos.TGE.EmpresasListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ConsultarStock">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <div class="form-group row">     
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEmpresa" runat="server" Text="Empresa" />
          <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtEmpresa" runat="server" />
        </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
         <div class="col-lg-3 col-md-3 col-sm-9">    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
    </div>

        <div class="col-lg-3 col-md-3 col-sm-9">     <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
       
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
             </div></div>
            <div class="table-responsive">
        <asp:GridView ID="gvDatos"  AllowPaging="true" AllowSorting="true" OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                         <asp:BoundField  HeaderText="Empresa" DataField="Empresa" SortExpression="Empresa" />  
                         <asp:BoundField  HeaderText="Descripcion" DataField="Descripcion" SortExpression="Descripcion" />                       
                       <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
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