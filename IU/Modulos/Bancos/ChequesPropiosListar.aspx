<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ChequesPropiosListar.aspx.cs" Inherits="IU.Modulos.Bancos.ChequesPropiosListar" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ChequesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label CssClass="labelEvol" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaDesde" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaDesde" runat="server" Enabled="True" 
                    TargetControlID="txtFechaDesde" PopupButtonID="imgFechaDesde" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            
            <asp:Label CssClass="labelEvol" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaHasta" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaHasta" runat="server" Enabled="True" 
                    TargetControlID="txtFechaHasta" PopupButtonID="imgFechaHasta" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <br />
            <asp:Label CssClass="labelEvol" ID="lblFechaDiferidoDesde" runat="server" Text="Fecha Dif. Desde"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaDiferidoDesde" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaDiferidoDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaDiferidoDesde" runat="server" Enabled="True" 
                    TargetControlID="txtFechaDiferidoDesde" PopupButtonID="imgFechaDiferidoDesde" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <div class="EspacioValidador"></div>
            <asp:Label CssClass="labelEvol" ID="lblFechaDiferidoHasta" runat="server" Text="Fecha Dif. Hasta"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaDiferidoHasta" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaDiferidoHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="ceFechaDiferidoHasta" runat="server" Enabled="True" 
                    TargetControlID="txtFechaDiferidoHasta" PopupButtonID="imgFechaDiferidoHasta" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <br />
            
            <asp:Label CssClass="labelEvol" ID="lblNumeroCheque" runat="server" Text="Numero Cheque" />
            <asp:TextBox CssClass="textboxEvol" ID="txtNumeroCheque" runat="server" />
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblBanco" runat="server" Text="Banco" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlBancos" runat="server" />
            <br />
            
            <asp:Label CssClass="labelEvol" ID="lblFilial" runat="server" Text="Filial" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlFiliales" runat="server" />                        
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblEstado" runat="server" Text="Estado" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstado" runat="server" />
            <br />
            
            <asp:Label CssClass="labelEvol" ID="lblTitularCheque" runat="server" Text="Titular Cheque" />
            <asp:TextBox CssClass="textboxEvol" ID="txtTitularCheque" runat="server" />
            
            <br />
            <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
            <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="True" AllowSorting="True" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="False" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField ReadOnly="true" HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                        <asp:BoundField ReadOnly="true" HeaderText="Fecha Diferido" DataField="FechaDiferido" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaDiferido" />
                        <asp:BoundField ReadOnly="true" HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                        <asp:BoundField ReadOnly="true" HeaderText="Número Cheque" ItemStyle-HorizontalAlign="Right" DataField="NumeroCheque" SortExpression="NumeroCheque" />
                        <asp:TemplateField HeaderText="Banco" SortExpression="Banco.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Banco.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                            <ItemTemplate>
                                <%# Eval("Filial.Filial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" HeaderText="Importe" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" DataField="Importe" SortExpression="Importe" />
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>       
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblGrillaTotalRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
