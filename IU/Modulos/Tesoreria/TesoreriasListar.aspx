<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TesoreriasListar.aspx.cs" Inherits="IU.Modulos.Tesoreria.TesoreriasListar" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="TesoreriasCerrar">
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <asp:Label CssClass="labelEvol" ID="lblAgencias" runat="server" Text="Agencia"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlAgencias" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlAgencias_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Label CssClass="labelEvol" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFecha" runat="server"></asp:TextBox>
            <asp:Image ID="imgFecha" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
            <asp:CalendarExtender ID="ceFecha" runat="server" Enabled="true" 
                TargetControlID="txtFecha" PopupButtonID="imgFecha" Format="dd/MM/yyyy"></asp:CalendarExtender>
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
                onclick="btnBuscar_Click" ValidationGroup="Buscar" />
            <br />
            <br />
            <asp:GridView ID="gvDatosCabecera" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="false" >
                <Columns>
                    <asp:BoundField  HeaderText="Fecha Abrir" DataField="FechaAbrir" />
                    <asp:TemplateField HeaderText="Usuario Abrir" >
                        <ItemTemplate>
                            <%# string.Concat(Eval("Usuario.AbrirApellido"), Eval("Usuario.AbrirNombre") )%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField  HeaderText="Fecha Cerrar" DataField="FechaCerrar" />
                    <asp:TemplateField HeaderText="Usuario Cerrar" >
                        <ItemTemplate>
                            <%# string.Concat(Eval("Usuario.CerrarApellido"), Eval("Usuario.CerrarNombre") )%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" >
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="false" >
            <Columns>
                <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Moneda.Descripcion")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  HeaderText="Saldo Inicial" DataField="SaldoInicial" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="SaldoInicial" />
                <asp:BoundField  HeaderText="Ingresos" DataField="Ingreso" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Ingreso" />
                <asp:BoundField  HeaderText="Egresos" DataField="Egreso" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="Egreso" />
                <asp:BoundField  HeaderText="Saldo Final" DataField="SaldoFinal" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" SortExpression="SaldoFinal" />
            </Columns>
        </asp:GridView>
        <br />
        </ContentTemplate>
    </asp:UpdatePanel> 
    </div>
</asp:Content>