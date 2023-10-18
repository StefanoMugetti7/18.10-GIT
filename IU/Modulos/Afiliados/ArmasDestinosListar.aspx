<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ArmasDestinosListar.aspx.cs" Inherits="IU.Modulos.Afiliados.ArmasDestinosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="ArmasDestinosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

    <ContentTemplate>
          <div class="form-group row">

   <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigoArma" runat="server" Text="Codigo Arma"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9"> <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoArma" runat="server" />
 </div>
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDestino" runat="server" Text="Destino"></asp:Label>
    <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtDestino" runat="server"></asp:TextBox>
    </div>
  <div class="col-lg-3 col-md-3 col-sm-9">
    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
        <div class="Espacio"></div>
    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
    onclick="btnAgregar_Click" />
</div></div>
    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                        runat="server" onclick="btnExportarExcel_Click" Visible="false" />
    <br />


        <div class="table-responsive">
     <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
    >
        <Columns>
            <asp:TemplateField HeaderText="Arma" SortExpression="Descripcion">
                    <ItemTemplate>
                        <%# Eval("Arma.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Destino" SortExpression="Destino">
                    <ItemTemplate>
                        <%# Eval("Destino")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Localidad" SortExpression="Localidad">
                <ItemTemplate>
                    <%# Eval("Localidad.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
             <asp:TemplateField HeaderText="Acciones">
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>

</div>

</asp:Content>
