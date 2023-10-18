<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Medicina.Controles.TurnosBuscarPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Turnos" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Prestador" SortExpression="Prestador.ApellidoNombre">
                    <ItemTemplate>
                        <%# Eval("Prestador.ApellidoNombre")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Especializacion" SortExpression="Especializacion.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Especializacion.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Paciente" SortExpression="Afiliado.ApellidoNombre">
                    <ItemTemplate>
                        <%# Eval("Afiliado.ApellidoNombre")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Turno" SortExpression="Turno.FechaHoraDesde">
                    <ItemTemplate>
                        <%# Eval("FechaHoraDesde", "{0:dd/MM/yyyy HH:mm}")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>   
            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
    </asp:GridView>
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" />
    </center>
</asp:Panel>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnVolver" >
</asp:ModalPopupExtender>