<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConceptosContablesDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.ConceptosContablesDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesBuscar.ascx" TagName="buscarCuentasContables" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<div class="ConceptosContablesDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptoContable" runat="server" Text="Concepto Contable" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtConceptoContable" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvConceptoContable" runat="server" ErrorMessage="*"
                ControlToValidate="txtConceptoContable" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="upCuentaContable" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:buscarCuentasContables ID="buscarCuenta" runat="server" MostrarEliminar="false" MostrarEtiquetas="true" OnCuentasContablesBuscarSeleccionar="buscarCuenta_CuentasContablesBuscarSeleccionar" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server" />
                </div>
                <asp:RequiredFieldValidator ID="rfvTipoOperacion" ControlToValidate="ddlTipoOperacion" CssClass="Validador" ValidationGroup="AgregarOperacion" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" ValidationGroup="AgregarOperacion" OnClick="btnAgregar_Click" />
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField ID="hIdTipoOperacion" Value='<%# Eval("IdTipoOperacion") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tipo Operación" DataField="TipoOperacion" SortExpression="TipoOperacion" />
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
