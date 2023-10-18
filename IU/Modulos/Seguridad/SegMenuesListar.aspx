<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SegMenuesListar.aspx.cs" Inherits="IU.Modulos.Seguridad.SegMenuesListar"  %>
<%@ Register src="~/Modulos/Seguridad/Controles/SegMenuesModificarDatosPopUp.ascx" tagname="SegMenuesModificarDatosPopUp" tagprefix="AUGE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
         <AUGE:SegMenuesModificarDatosPopUp ID="ctrPopUpMenu" Visible="false" runat="server" />
        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar Menu" 
             onclick="btnAgregar_Click" />
         <br />
        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
        runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
        onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
            <Columns>
                <asp:BoundField  HeaderText="Menu" DataField="Menu" ItemStyle-Wrap="false" SortExpression="Menu" />
                <asp:BoundField  HeaderText="Estado Baja" DataField="BajaLogica" SortExpression="BajaLogica" />
                <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                    <ItemTemplate>
                        <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                            AlternateText="Mostrar" ToolTip="Mostrar" />--%>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                            AlternateText="Modificar" ToolTip="Modificar" />
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>