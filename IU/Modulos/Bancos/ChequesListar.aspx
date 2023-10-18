<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ChequesListar.aspx.cs" Inherits="IU.Modulos.Bancos.ChequesListar"  %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ChequesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
         </div>
            
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
          <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
            
            </div>
<div class="col-sm-1">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            </div>
                </div>
   <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferidoDesde" runat="server" Text="Fecha Dif. Desde"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferidoDesde" runat="server"></asp:TextBox>
         </div>

    
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferidoHasta" runat="server" Text="Fecha Dif. Hasta"></asp:Label>
         <div class="col-sm-3">   
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferidoHasta" runat="server"></asp:TextBox>
           </div>
        
            </div>
               <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCheque" runat="server" Text="Numero Cheque" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" runat="server" />
           </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
           <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" />
           </div>
            </div>
               <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
          <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />                        
         </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
           <div class="col-sm-3">
              <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
         </div>
            </div>
               <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTitularCheque" runat="server" Text="Titular Cheque" />
          <div class="col-sm-3">
               <asp:TextBox CssClass="form-control" ID="txtTitularCheque" runat="server" />
            </div>
        </div>
            <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png" 
                        runat="server" onclick="btnExportarExcel_Click" Visible="false" />

            <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
            <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                        <asp:BoundField  HeaderText="Fecha Diferido" DataField="FechaDiferido" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaDiferido" />
                        <asp:BoundField  HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                        <asp:BoundField  HeaderText="Número Cheque" ItemStyle-HorizontalAlign="Right" DataField="NumeroCheque" SortExpression="NumeroCheque" />
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
                        <asp:BoundField  HeaderText="Importe" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right" DataField="Importe" SortExpression="Importe" />
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
</div>
</asp:Content>