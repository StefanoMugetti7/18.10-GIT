<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChequesCambiarValoresDatos.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.ChequesCambiarValoresDatos" %>
<%--<%@ Register src="~/Modulos/Tesoreria/Controles/ChequesTercerosPopUp.ascx" tagname="popUpBuscarCheque" tagprefix="auge" %> --%>
<%@ Register Src="~/Modulos/Tesoreria/Controles/CajasIngresosValores.ascx" TagName="CajasIngresos" TagPrefix="AUGE" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>

<asp:UpdatePanel ID="upChequesTerceros" runat="server">
    <ContentTemplate>
        <auge:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Movimiento" runat="server" />
        <asp:Panel ID="Panel1" GroupingText="Egreso de Cheques" runat="server">
            <%--    <AUGE:popUpBuscarCheque ID="ctrBuscarCheque" runat="server" />--%>
            <%--<asp:Button CssClass="botonesEvol" ID="btnAgregarCheque" Visible="true" runat="server" Text="Buscar Cheques terceros" 
            onclick="btnBuscarCheque_Click" />--%>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblChequesTerceros" runat="server" Text="Cheque de Terceros"></asp:Label>
                <div class="col-sm-6">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlChequesTerceros" runat="server">
                    </asp:DropDownList>
                </div>
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarChequesTerceros" runat="server" Text="Agregar" OnClick="btnAgregarCheque_Click" />
                    </div>
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvChequesTerceros" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvChequesTerceros_RowDataBound" OnRowCommand="gvChequesTerceros_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" DataField="Fecha" />
                        <asp:BoundField HeaderText="Fecha Dif" DataFormatString="{0:dd/MM/yyyy}" DataField="FechaDiferido" />
                        <asp:BoundField HeaderText="Concepto" DataField="Concepto" />
                        <asp:BoundField HeaderText="Numero Cheque" DataField="NumeroCheque" />
                        <asp:TemplateField HeaderText="Banco">
                            <ItemTemplate>
                                <%# Eval("Banco.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="CUIT" DataField="CUIT" />
                        <asp:BoundField HeaderText="Titular" DataField="TitularCheque" />
                        <asp:BoundField HeaderText="Importe" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>


<auge:CajasIngresos ID="ctrIngresosValores" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <center>
            <auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />
            <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
            <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Confirmar" 
                onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                onclick="btnCancelar_Click" />
            </center>
    </ContentTemplate>
</asp:UpdatePanel>



