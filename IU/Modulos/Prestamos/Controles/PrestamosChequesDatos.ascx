<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestamosChequesDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrestamosChequesDatos" %>


<asp:UpdatePanel ID="upPrestamosCheques" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" OnClick="btnAgregarItem_Click" Enabled="false" Visible="false" runat="server" Text="Agregar item"
            CausesValidation="false" />
        <asp:GridView ID="gvDatos" ShowFooter="true" OnRowCommand="gvDatos_RowCommand"
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Número de Cheque">
                    <ItemTemplate>
                        <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" Enabled="false" Text='<%#Eval("NumeroCheque") %>' runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Banco">
                    <ItemTemplate>
                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" Enabled="false" runat="server"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="Banco">
                    <ItemTemplate>
                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" Enabled="false" Text='<%#Eval("Banco") %>' runat="server"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Fecha Diferido">
                    <ItemTemplate>
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferido" Enabled="false" Text='<%#Eval("FechaDiferido", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Importe">
                    <ItemTemplate>
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server" Text='<%#Eval("Importe", "{0:C2}")%>'></Evol:CurrencyTextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CUIT">
                    <ItemTemplate>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCUIT" Enabled="false" Text='<%#Eval("Cuit")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Titular">
                    <ItemTemplate>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtTitularCheque" Enabled="false" Text='<%#Eval("TitularCheque")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cod.Postal">
                    <ItemTemplate>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtCodigoPostal" Enabled="false" Text='<%#Eval("CodigoPostal")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nro.Sucursal">
                    <ItemTemplate>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtNumeroSucursal" Enabled="false" Text='<%#Eval("NumeroSucursal")%>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cant.Dias">
                    <ItemTemplate>
                        <%#Eval("CantidadDias")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                            AlternateText="Elminiar" ToolTip="Eliminar" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>

