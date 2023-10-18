<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CuentasContablesListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.CuentasContablesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="CuentasContablesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label CssClass="labelEvol" ID="lblDescripcion" runat="server" Text="Descripción" />
            <asp:TextBox CssClass="textboxEvol" ID="txtDescripción" runat="server" />
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblNumeroCuenta" runat="server" Text="Número Cuenta" />
            <asp:TextBox CssClass="textboxEvol" ID="txtNumeroCuenta" runat="server" />
            <div class="Espacio"></div>
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            <br />
            <asp:Label CssClass="labelEvol" ID="lblCapitulo" runat="server" Text="Capítulo" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlCapitulo" runat="server" />
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblRubro" runat="server" Text="Rubro" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlRubro" runat="server" />
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblEstado" runat="server" Text="Estado" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstado" runat="server" />
            <br />
            <asp:Label CssClass="labelEvol" ID="lblMoneda" runat="server" Text="Moneda" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlMoneda" runat="server" />
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblDepartamento" runat="server" Text="Departamento" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlDepartamento" runat="server" />
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblSubRubro" runat="server" Text="SubRubro" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlSubRubro" runat="server" />
            <br />
            <br />
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                <Columns>
                    <asp:TemplateField HeaderText="Capítulo" SortExpression="Capitulo.Capitulo">
                        <ItemTemplate>
                            <%# Eval("Capitulo.Capitulo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rubro" SortExpression="Rubro.Rubro">
                        <ItemTemplate>
                            <%# Eval("Rubro.Rubro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.Moneda">
                        <ItemTemplate>
                            <%# Eval("Moneda.Moneda")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Departamento" SortExpression="Departamento.Departamento">
                        <ItemTemplate>
                            <%# Eval("Departamento.Departamento")%>
                        </ItemTemplate>
                    </asp:TemplateField>             
                    <asp:TemplateField HeaderText="SubRubro" SortExpression="SubRubro.SubRubro">
                        <ItemTemplate>
                            <%# Eval("SubRubro.SubRubro")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField  HeaderText="Descripción" DataField="Descripcion" SortExpression="Descripcion" />
                    <asp:BoundField  HeaderText="Número Cuenta" DataField="NumeroCuenta" SortExpression="NumeroCuenta" />
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
            <br />
            <center>
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" Visible="false"  onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
