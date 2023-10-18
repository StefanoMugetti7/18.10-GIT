<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ExpedientesDerivacionesMultiples.aspx.cs" Inherits="IU.Modulos.Expedientes.ExpedientesDerivacionesMultiples" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="ExpedientesDerivacionesMultiples">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
       <div class="row justify-content-md-center">
            <div class="col-md-auto">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTitulo" runat="server" Text="Expedientes Pendientes de Aceptación"></asp:Label>
  </div></div>
            <div class="table-responsive">
    <asp:GridView ID="gvDatos" AllowPaging="true" AllowSorting="true" 
    DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
               <asp:TemplateField HeaderText="Fecha" >
                    <ItemTemplate>
                        <%# Eval("FechaExpediente", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
            <asp:TemplateField HeaderText="Numero" SortExpression="IdExpediente">
                    <ItemTemplate>
                        <%# Eval("IdExpediente")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo" SortExpression="ExpedienteTipo.Descripcion">
                    <ItemTemplate>
                        <%# Eval("ExpedienteTipo.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Titulo" DataField="Titulo" SortExpression="Titulo" />
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Filial -> Sector" SortExpression="ExpedienteTrackingSectorFilialFilial">
                <ItemTemplate>
                    <%# string.Concat(Eval("ExpedienteTracking.Sector.Filial.Filial"), " -> ", Eval("ExpedienteTracking.Sector.Sector"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Derivado a" SortExpression="ExpedienteTrackingSectorSector">
                <ItemTemplate>
                    <%# Convert.ToInt32( Eval("ExpedienteDerivado.Sector.IdSector")) == 0 ? string.Empty : string.Concat(Eval("ExpedienteDerivado.Sector.Filial.Filial"), " -> ", Eval("ExpedienteDerivado.Sector.Sector"))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Incluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <asp:CheckBox ID="chkIncluir" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
    </asp:GridView></div>
   
     <div class="row justify-content-md-center">
            <div class="col-md-auto">
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar Derivaciones" 
        onclick="btnAceptar_Click" />
</div></div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>

