<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SubsidiosListar.aspx.cs" Inherits="IU.Modulos.Subsidios.SubsidiosListar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="SubsidiosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubsidios" runat="server" Text="Subsidio" />
                 <div class="col-sm-3"> <asp:TextBox CssClass="form-control" ID="txtSubsidio" runat="server" /></div>
        
              <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                     
            <asp:DropDownList ID="ddlEstado" CssClass="form-control select2" runat="server"></asp:DropDownList></div>

                <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" Visible="true" onclick="btnAgregar_Click" />
         </div></div>
            <div class="data-table">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Descripcion" DataField="Descripcion" SortExpression="Descripcion" />
                        <asp:TemplateField HeaderText="Tipo Subsidio" SortExpression="TipodeSubsidio.Descripcion">
                            <ItemTemplate>
                                <%# Eval("SubsidioTipo.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField  HeaderText="Meses Carencia" DataField="MesesCarencia" SortExpression="MesesCarencia" />--%>
                        <asp:BoundField  HeaderText="Cantidad Maxima" DataField="CantidadMaxima" SortExpression="CantidadMaxima" />
                        <asp:BoundField  HeaderText="Frecuencia Mensual" DataField="FrecuenciaAnual" SortExpression="FrecuenciaAnual" />
                        <asp:TemplateField HeaderText="Modifica Importe" >
                            <ItemTemplate>
                                <%# Eval("ModificaImporte").ToString().ToLower() == "true" ? "Si" : "No" %>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                </asp:GridView></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
