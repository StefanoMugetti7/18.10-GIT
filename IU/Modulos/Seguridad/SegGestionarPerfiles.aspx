<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SegGestionarPerfiles.aspx.cs" Inherits="IU.SegGestionarPerfiles" %>

<%@ Register Src="~/Modulos/Seguridad/SegGestionarPerfilesMenuesControlesPaginas.ascx" TagPrefix="ctr" TagName="SegGestionarMenuesControlesPagina" %>
<%@ Register Src="~/Modulos/Seguridad/SegGestionarPerfilesUsuarios.ascx" TagPrefix="ctr" TagName="SegGestionarPerfilUsuarios" %>
<%@ Register Src="~/Modulos/Seguridad/Controles/SegGestionarPerfilesReportes.ascx" TagPrefix="ctr" TagName="SegGestionarPerfilesReportes" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Seguridad/Controles/SegGestionarPerfilesProcesos.ascx" TagPrefix="ctr" TagName="SegGestionarPerfilesProcesos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../../Recursos/TreeView.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="SegGestionarPerfiles">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:MultiView ID="mvGestion" runat="server">
                    <asp:View ID="vDatos" runat="server">
                        <table width="100%">
                            <tr>
                                <td align="right" style="text-align: right">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" OnClick="btnAgregar_Click"
                                        Text="Agregar" Width="132px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
                                    <asp:GridView ID="gvDatos"
                                        OnRowCommand="gvDatos_RowCommand"
                                        OnRowDataBound="gvDatos_RowDataBound"
                                        DataKeyNames="IndiceColeccion"
                                        runat="server" SkinID="GrillaBasicaFormal"
                                        AutoGenerateColumns="false" AllowPaging="true" AllowSorting="true"
                                        OnPageIndexChanging="gvDatos_PageIndexChanging"
                                        OnSorting="gvDatos_Sorting">
                                        <PagerSettings Position="Bottom" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Codigo" DataField="IdPerfil" SortExpression="IdPerfil" />
                                            <asp:BoundField HeaderText="Perfil" DataField="Perfil" ItemStyle-Width="70%" SortExpression="Perfil" />
                                            <asp:BoundField HeaderText="Estado" DataField="BajaLogica" SortExpression="BajaLogica" />
                                            <asp:TemplateField HeaderText="Acciones">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                        AlternateText="Mostrar" ToolTip="Mostrar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                        AlternateText="Modificar" ToolTip="Modificar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/groups_f3.png" runat="server" CommandName="Copiar" ID="btnCopiar"
                                                        AlternateText="Copiar perfil" ToolTip="Copiar perfil" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="vCargarDatos" runat="server">
                        <div class="form-group row">

                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtPerfil" Width="350px" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" runat="server"
                                    ControlToValidate="txtPerfil" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                            <asp:CheckBox ID="chkBajaLogica" runat="server" Text="Estado" />
                            <asp:Button CssClass="botonesEvol" ID="btnUsuarios" runat="server" Text="Usuarios"
                                OnClick="btnUsuarios_Click" />
                        </div>
                        <div class="col-sm-6"></div>
                        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                        <ctr:SegGestionarPerfilUsuarios ID="SegGestionarPerfilUsuario" runat="server"></ctr:SegGestionarPerfilUsuarios>
                        <br />
                        <br />
                        <asp:TabContainer ID="tcPerfiles" runat="server" ActiveTabIndex="0"
                            Width="100%" SkinID="MyTab">
                            <asp:TabPanel runat="server" ID="tpMenues" HeaderText="Menues">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <th align="center">
                                                <asp:Label CssClass="labelEvol" ID="lblPaginas" runat="server" Text="Paginas"></asp:Label>
                                            </th>
                                            <th align="center">&nbsp;</th>
                                            <th align="center">
                                                <asp:Label CssClass="labelEvol" ID="lblPermisos" runat="server" Text="Permisos"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td valign="top" width="50%">
                                                <asp:TreeView ID="tvMenu" runat="server" ImageSet="BulletedList4"
                                                    OnSelectedNodeChanged="tvMenu_SelectedNodeChanged" ShowCheckBoxes="All">
                                                    <ParentNodeStyle Font-Bold="false" />
                                                    <HoverNodeStyle Font-Underline="true" ForeColor="#5555DD" />
                                                    <SelectedNodeStyle Font-Underline="true" ForeColor="#5555DD"
                                                        HorizontalPadding="0px" VerticalPadding="0px" />
                                                    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black"
                                                        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                </asp:TreeView>
                                            </td>
                                            <td width="10%"></td>
                                            <td valign="top">
                                                <asp:Button CssClass="botonesEvol" ID="btnAgregarControles" Visible="false" runat="server" Text="Agregar controles"
                                                    OnClick="btnAgregarControles_Click" /><br />
                                                <ctr:SegGestionarMenuesControlesPagina ID="MenuesControlesPagina" runat="server"></ctr:SegGestionarMenuesControlesPagina>
                                                <asp:CheckBoxList ID="chkControles" runat="server"
                                                    OnSelectedIndexChanged="chkControles_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpReportes" HeaderText="Reportes">
                                <ContentTemplate>
                                    <ctr:SegGestionarPerfilesReportes ID="ctrPerfilReportes" runat="server" />
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpProcesos" HeaderText="Procesos">
                                <ContentTemplate>
                                    <ctr:SegGestionarPerfilesProcesos ID="ctrPerfilProcesos" runat="server" />
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                        <br />
                        <center>
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                                onclick="btnAceptar_Click" />
                            &nbsp;<asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" CausesValidation="false"
                                onclick="btnCancelar_Click" Text="Volver" />
                        </center>
                    </asp:View>
                </asp:MultiView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
