<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlantillasListar.aspx.cs" Inherits="IU.Modulos.Plantillas.PlantillasListar" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ConsultarStock">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlantillas" runat="server" Text="Plantilla" />
                <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtPlantilla" runat="server" />
                    </div>
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado" />
        <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
        </div>
 <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
      <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"  onclick="btnAgregar_Click" />
     </div></div>
        <asp:GridView ID="gvDatos"  AllowPaging="true" AllowSorting="true" OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Codigo" DataField="Codigo" SortExpression="Codigo" />   
                        <asp:BoundField  HeaderText="Plantilla" DataField="NombrePlantilla" SortExpression="NombrePlantilla" />  
                         <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                                              
                       <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        </Columns>
                </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>