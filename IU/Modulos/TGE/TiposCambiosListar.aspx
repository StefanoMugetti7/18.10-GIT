<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposCambiosListar.aspx.cs" Inherits="IU.Modulos.TGE.TiposCambiosListar"  %>
<%@ Register Src="~/Modulos/TGE/Control/TiposCambiosDatosPopUp.ascx" TagPrefix="AUGE" TagName="TiposCambiosModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="TiposCambiosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <auge:TiposCambiosModificarDatos ID="popUpModificarDatos" runat="server"   />

    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                        runat="server" onclick="btnExportarExcel_Click" Visible="false" />                    
    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
        onclick="btnAgregar_Click" />
    <br />
    <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
        <Columns>
            <asp:TemplateField HeaderText="Country" >
                            <ItemTemplate>
                                <%# Eval("Pais.Pais")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <%# string.Concat(Eval("Moneda.Moneda"), " ", Eval("Moneda.Descripcion"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Date">
                            <ItemTemplate>
                                <%# Eval("FechaDesde", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Exchange">
                            <ItemTemplate>
                                <%# Eval("TipoCambio")%>
                            </ItemTemplate>
                        </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" 
                         AlternateText="View" ToolTip="View" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                         AlternateText="Edit" ToolTip="Edit" />  
                </ItemTemplate>
            </asp:TemplateField>--%>
            </Columns>
    </asp:GridView>
    </ContentTemplate>
    <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
</div> 
</asp:Content>