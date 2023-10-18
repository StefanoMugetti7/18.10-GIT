<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PanteonesListar.aspx.cs" Inherits="IU.Modulos.Nichos.PanteonesListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>

    <script type="text/javascript">

    </script>
    <div class="PanteonesListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCodigo" ValidationGroup="Aceptar" ControlToValidate="txtCodigo" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Panteon"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcion" ValidationGroup="Aceptar" ControlToValidate="txtDescripcion" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>     
                      <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="IdCementerio" runat="server" Text="Cementerio"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCementerio" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCementerio" ControlToValidate="ddlCementerio" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                 </div>
                    <div class="col-sm-1"></div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                    
                     
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                            
                       
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                            OnClick="btnAgregar_Click" />
                                        </div>  
                    </div>
                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                    runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdPanteon"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" SortExpression="IdPanteon">
                            <ItemTemplate>
                                <%# Eval("IdPanteon")%>
                            </ItemTemplate>
                            </asp:TemplateField> <asp:TemplateField HeaderText="Codigo" SortExpression="Codigo">
                            <ItemTemplate>                            
                                <%# Eval("Codigo")%>
                            </ItemTemplate>
                        </asp:TemplateField>  <asp:TemplateField HeaderText="Cementerio" SortExpression="Cementerio">
                            <ItemTemplate>
                                 <%# Eval("Cementerio")%>
                            </ItemTemplate>
                            </asp:TemplateField>  <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                            <ItemTemplate>
                                 <%# Eval("Descripcion")%>
                            </ItemTemplate>
                          </asp:TemplateField>  <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <ItemTemplate>
                                 <%# Eval("Estado")%>
                            </ItemTemplate>                      
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" SortExpression="Acciones">
                            <ItemTemplate>
                                <%# Eval("Acciones")%>
                            </ItemTemplate>     
                            <ItemTemplate>
                                 <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Consultar" ToolTip="Consultar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>

