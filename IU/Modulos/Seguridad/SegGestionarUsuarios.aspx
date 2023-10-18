<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SegGestionarUsuarios.aspx.cs" Inherits="IU.SegGestionarUsuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Modulos/Seguridad/SegGestionarUsuariosContrasenia.ascx" TagPrefix="ctr" TagName="UsuariosCambiarContrasenia" %>
<%--<%@ Register Src="~/Modulos/Seguridad/SegGestionarUsuariosDatos.ascx" TagPrefix="ctr" TagName="UsuarioDatos" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <ctr:UsuariosCambiarContrasenia ID="UsuCambiarContrasenia" runat="server"></ctr:UsuariosCambiarContrasenia>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBuscar" runat="server" Text="Buscar"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtBuscar" runat="server"></asp:TextBox>
                </div>
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server"
                    OnClick="btnBuscar_Click" Text="Buscar" ValidationGroup="Buscar" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" CausesValidation="false"
                    OnClick="btnAgregar_Click" Text="Agregar" />
            </div>
            <br />
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <%--<pagersettings Position="Bottom" />--%>
                <Columns>
                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" SortExpression="Usuario" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" SortExpression="Apellido" />
                    <asp:BoundField DataField="BajaLogica" HeaderText="Estado" SortExpression="BajaLogica" />
                    <asp:TemplateField HeaderText="Contraseña">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="CambiarContrasenia" ID="btnCambiarContrasenia"
                                AlternateText="Change password" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Consultar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <%--</asp:View>
                <asp:View ID="vCargarDatos" runat="server">--%>
            <%--<ctr:UsuarioDatos ID="ctrUsuarioDatos" runat="server"></ctr:UsuarioDatos>--%>
            <%--</asp:View>
            </asp:MultiView>  --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
