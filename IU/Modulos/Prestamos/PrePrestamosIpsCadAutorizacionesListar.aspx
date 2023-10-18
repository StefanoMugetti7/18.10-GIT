<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PrePrestamosIpsCadAutorizacionesListar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrePrestamosIpsCadAutorizacionesListar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <asp:Label CssClass="labelEvol" ID="lblNumero" runat="server" Text="Número de CAD" />
    <Evol:CurrencyTextBox CssClass="textboxEvol" ID="txtNumero" Prefix="" NumberOfDecimals="0" runat="server" />
    <%--<div class="Espacio"></div>--%>
    <asp:Label CssClass="labelEvol" ID="lblPeriodo" runat="server" Text="Periodo" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlPeriodo" runat="server" />
    <%--<div class="Espacio"></div>--%>
    <asp:Label CssClass="labelEvol" ID="lblEstado" runat="server" Text="Estado" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlEstado" runat="server" />
    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
    <br />
    <br />
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
        OnRowDataBound="gvDatos_RowDataBound" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
        onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
            <Columns>
                <asp:BoundField  HeaderText="Tipo" DataField="Tipo" ItemStyle-Wrap="false" SortExpression="Tipo" />
                <asp:BoundField  HeaderText="Numero" DataField="Numero" SortExpression="Numero" />
                <asp:BoundField  HeaderText="Codigo" DataField="Codigo" SortExpression="Codigo" />
                <asp:BoundField  HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                <asp:BoundField  HeaderText="Importe" DataField="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Importe" />
                <asp:BoundField  HeaderText="Cant. Cuotas" DataField="CantidadCuotas" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" SortExpression="CantidadCuotas" />
                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                    <ItemTemplate>
                        <%# Eval("EstadoDescripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>            
                <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                    <ItemTemplate>
                        <asp:HiddenField ID="hdfSelloTiempo" runat="server" />
                        <asp:HiddenField ID="hdfNumeroCAD" runat="server" Value='<%#Eval("Numero") %>'/>
                        <asp:HiddenField ID="hdfIdPrestamoIpsCadAutorizacion" runat="server" Value='<%#Eval("IdPrestamoIpsCadAutorizacion") %>'/>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                            AlternateText="Generar Prestamo" ToolTip="Generar Prestamo" />
                    </ItemTemplate>
                </asp:TemplateField>
                </Columns>
    </asp:GridView>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
