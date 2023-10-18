<%@ Page Language="C#" MasterPageFile="~/Maestra2.Master" AutoEventWireup="true" CodeBehind="AfiliadosListar2.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosListar2" Title="" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div class="row">
        <div class="col-md-4">
            <asp:Label CssClass="labelEvol" ID="lblNumeroSocio" runat="server" Text="Número Socio"></asp:Label>
            <AUGE:NumericTextBox CssClass="form-control form-control-sm" ID="txtNumeroSocio" runat="server"></AUGE:NumericTextBox>
        </div>
        <div class="col-md-4">
            <asp:Label CssClass="labelEvol" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
            <asp:DropDownList CssClass="form-control form-control-sm" ID="ddlTipoDocumento" runat="server">
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:Label CssClass="labelEvol" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
            <AUGE:NumericTextBox CssClass="form-control form-control-sm" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
        </div>
    </div>
    <div class="row">
    <div class="form-group col-md-4">
        <asp:Label CssClass="labelEvol" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
        <asp:TextBox CssClass="form-control form-control-sm" ID="txtApellido" runat="server"></asp:TextBox>
    </div>
    <div class="form-group col-md-4">
        <asp:Label CssClass="labelEvol" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
        <asp:TextBox CssClass="form-control form-control-sm" ID="txtNombre"  runat="server"></asp:TextBox>
       </div>
    <div class="form-group col-md-4">
        <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
        <asp:DropDownList CssClass="form-control form-control-sm" ID="ddlEstados"  runat="server">
        </asp:DropDownList>
    </div>
    </div>
    <div class="row">
        <div class="form-group col-md-4">
            <asp:Label CssClass="labelEvol" ID="lblMatricula" runat="server" Text="Legajo"></asp:Label>
            <AUGE:NumericTextBox CssClass="form-control form-control-sm" ID="txtMatricula" runat="server"></AUGE:NumericTextBox>
        </div>
        <div class="form-group col-md-4">
            <asp:Label CssClass="labelEvol" ID="lblAlertas"  runat="server" Text="Alertas"></asp:Label>
            <asp:DropDownList CssClass="form-control form-control-sm" ID="ddlAlertas"  runat="server">
            </asp:DropDownList>
        </div>
        <div class="form-group col-md-4">
            <asp:Label CssClass="labelEvol" ID="Label1"  runat="server" Text="Incluir Familiares / Apoderado"></asp:Label>
            <asp:CheckBox CssClass="form-control form-control-sm form-check-input" ID="chkFamiliares" runat="server" />
        </div>
    </div>
    <div class="row">
    <div class="form-group col-md-4">
        <asp:Label CssClass="labelEvol" ID="lblCategoria" runat="server" Text="Categoria"></asp:Label>
        <asp:DropDownList CssClass="form-control form-control-sm" ID="ddlCategoria" runat="server"  TabIndex="0">
        </asp:DropDownList>
    </div>
    <div class="form-group col-md-4">
    </div>
    <div class="form-group col-md-4">
        <asp:Button CssClass="btn btn-primary btn-sm" ID="btnBuscar" runat="server" Text="Buscar" 
            onclick="btnBuscar_Click" />
        <asp:Button CssClass="btn btn-primary btn-sm" ID="btnAgregar" runat="server" Text="Agregar" 
        onclick="btnAgregar_Click" />
    </div>
    
    </div>
    <div class="row">
    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                        runat="server" onclick="btnExportarExcel_Click" Visible="false" />

    <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
    </div>
    <br />
    <div class="row">
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado,IdAfiliadoRef"
    runat="server" CssClass="table table-responsive table-striped" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    OnPreRender="gvDatos_PreRender"
    >
        <Columns>
            <asp:TemplateField HeaderText="Numero Socio" SortExpression="NumeroSocio">
                    <ItemTemplate>
                        <%# Eval("NumeroSocio")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Legajo" SortExpression="MatriculaIAF">
                    <ItemTemplate>
                        <%# Eval("MatriculaIAF")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumentoTipoDocumento">
                    <ItemTemplate>
                        <%# Eval("TipoDocumentoTipoDocumento")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
            <asp:BoundField  HeaderText="Apellido y Nombre" DataField="ApellidoNombre" SortExpression="ApellidoNombre" />
            <%--<asp:BoundField  HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" />--%>
            <asp:TemplateField HeaderText="Categoria" SortExpression="CategoriaCategoria">
                <ItemTemplate>
                    <%# Eval("CategoriaCategoria")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Participantes" DataField="CantidadParticipantes" SortExpression="CantidadParticipantes" />
            <asp:TemplateField HeaderText="Tipo Socio" SortExpression="AfiliadoTipo.AfiliadoTipo">
                <ItemTemplate>
                    <%# Eval("AfiliadoTipoAfiliadoTipo")%>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="Parentesco" SortExpression="Parentesco.Parentesco">
                <ItemTemplate>
                    <%# Eval("Parentesco.Parentesco")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("EstadoDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                         AlternateText="Ingresar a la cuenta" ToolTip="Ingresar a la cuenta" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
    </asp:GridView>
    </div>
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>