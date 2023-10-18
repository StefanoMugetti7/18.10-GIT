<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposOperacionesComprobantesListar.aspx.cs" Inherits="IU.Modulos.TGE.TiposOperacionesComprobantesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ConsultarStock">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                <div class="col-sm-3">
                     
            <asp:DropDownList ID="ddlTipoOperacionOC" CssClass="form-control select2" runat="server"></asp:DropDownList></div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo de Factura" />
                <div class="col-sm-3">
                     
            <asp:DropDownList ID="ddlTipoFactura" CssClass="form-control select2" runat="server"></asp:DropDownList></div>
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                     
            <asp:DropDownList ID="ddlEstado" CssClass="form-control select2" runat="server"></asp:DropDownList></div>
          
</div>
            <div class="form-group row">
                <div class="col-sm-8"></div>
                  <div class="col-sm-4">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
           
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            </div></div>
        <asp:GridView ID="gvDatos"  AllowPaging="true" AllowSorting="true" OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTipoOperacionTipoFactura"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>

                         <asp:BoundField  HeaderText="Tipo Operacion" DataField="TipoOperacion" SortExpression="Tipo Operación" />  
                         <asp:BoundField  HeaderText="Descripcion" DataField="TipoFactura" SortExpression="Tipo Factura" /> 
                       <asp:BoundField  HeaderText="Signo" DataField="Signo" SortExpression="Signo" /> 
                            <asp:BoundField  HeaderText="Mostrar IVA" DataField="MostrarIVA" SortExpression="Mostrar IVA" />  
                       
                            <asp:TemplateField HeaderText="Estado" ItemStyle-Wrap="false" SortExpression="Estado.Descripcion">
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
                </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
