<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="GestionarReportes.aspx.cs" Inherits="IU.Modulos.Reportes.GestionarReportes" %>
<%@ Register Src="~/Modulos/Reportes/GestionarReportesDatos.ascx" TagPrefix="AUGE" TagName="ReportesDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="GestionarReportes" >
            <asp:MultiView ID="mvGestion" runat="server">
                <asp:View ID="vBuscar" runat="server">

                    <div class="form-group row">

                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblBuscarPor" runat="server" Text="Reportes"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                  </div>
                  <div class="col-lg-3 col-md-3 col-sm-9">       
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" CausesValidation="false"
                        onclick="btnBuscar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
                        CausesValidation="false" onclick="btnAgregar_Click" />
                </div></div>
                    <div class="table-responsive">
                    <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="false"  
                        onrowcommand="gvDatos_RowCommand" DataKeyNames="IndiceColeccion" SkinID="GrillaResponsive" 
                                    style="text-align: center" AllowPaging="true" AllowSorting="true" 
                                    onpageindexchanging="gvDatos_PageIndexChanging" 
                                    onsorting="gvDatos_Sorting">
                        <Columns>
                            <asp:BoundField DataField="IdReporte" HeaderText="Codigo" SortExpression="IdReporte"  />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" SortExpression="Descripcion" />
                            <asp:TemplateField HeaderText="Modulo Sistema" SortExpression="ModulosSistema.Descripcion">
                                <ItemTemplate><%# Eval("ModuloSistema.ModuloSistema")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado">
                                    <ItemTemplate>
                                        <asp:Image ID="estado" ImageUrl='<%# string.Format("~/Imagenes/{0}.png", Eval("Estado.IdEstado")) %>' AlternateText='<%# Eval("Estado.Descripcion")%>' ToolTip='<%# Eval("Estado.Descripcion")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                         AlternateText="Mostrar" ToolTip="Mostrar"/>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                         AlternateText="Modificar" ToolTip="Modificar"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
            </asp:GridView></div>
                </asp:View>
                <asp:View ID="vDatos" runat="server">
                    <AUGE:ReportesDatos ID="RepDatos" runat="server" />
                </asp:View>
            </asp:MultiView>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

