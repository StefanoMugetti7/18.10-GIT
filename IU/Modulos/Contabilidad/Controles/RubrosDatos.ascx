<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RubrosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.RubrosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="RubrosDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRubro" runat="server" Text="Rubro" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtRubro" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvRubro" runat="server" ErrorMessage="*" ControlToValidate="txtRubro" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoRubro" runat="server" Text="Código Rubro" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoRubro" runat="server" MaxLength="1" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoRubro" runat="server" ErrorMessage="*" ControlToValidate="txtCodigoRubro" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <Columns>
                    <%--                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRow" runat="server" OnCheckedChanged="chkRow_CheckedChanged" AutoPostBack="true"/>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField HeaderText="IdSubRubro" DataField="IdSubRubro" Visible="false" />
                    <asp:BoundField HeaderText="Código SubRubro" DataField="CodigoSubRubro" SortExpression="CodigoSubRubro" />
                    <asp:BoundField HeaderText="SubRubro" DataField="SubRubro" SortExpression="SubRubro" />
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRow" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
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
