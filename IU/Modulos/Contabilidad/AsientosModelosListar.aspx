<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosModelosListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosModelosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="AsientosModelosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
           <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" />
         </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
          <div class="col-sm-3">   <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" />
       </div> <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
           </div></div>
              <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoAsiento" Text="Tipo Asiento" runat="server" />
           <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlTipoAsiento" runat="server" />
        </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
         <div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" /></div>
</div>
            
            
            <div class="table-responsive">
          <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                <Columns>
                    <asp:BoundField  HeaderText="Detalle" DataField="Detalle" SortExpression="Detalle" />
                    <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion.TipoOperacion">
                        <ItemTemplate>
                            <%# Eval("TipoOperacion.TipoOperacion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Asiento" SortExpression="TipoAsiento.Descripcion">
                        <ItemTemplate>
                            <%# Eval("TipoAsiento.Descripcion")%>
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
         
            <center>
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false"  onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
