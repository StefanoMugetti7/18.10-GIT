<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FormasCobrosCodigosConceptosListar.aspx.cs" Inherits="IU.Modulos.TGE.FormasCobrosCodigosConceptosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ConsultarStock">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Forma Cobro" />
                 <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFormaCobro"  runat="server" />

                     </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo Cargo" />
                 <div class="col-sm-3">
                      <asp:TextBox CssClass="form-control" ID="txtTipoCargo" runat="server" />
                 </div>
                 <div class="col-sm-4"> <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
           
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" /></div>
                 </div>
           
<br />
<br />
        <asp:GridView ID="gvDatos"  AllowPaging="true" AllowSorting="true" OnRowCommand="gvDatos_RowCommand"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdFormaCobroCodigoConceptoTipoCargoCategoria"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Forma Cobro" DataField="FormaCobro"  />  
                        <asp:BoundField  HeaderText="Tipo Cargo" DataField="TipoCargo"  />  
                        <asp:BoundField  HeaderText="Categoria" DataField="Categoria"  />  
                        <asp:BoundField  HeaderText="Prestamo Plan" DataField="PrestamoPlan"  />
                        <asp:BoundField  HeaderText="Codigo Concepto" DataField="CodigoConcepto"  />  
                        <asp:BoundField  HeaderText="Codigo Sub Concepto" DataField="CodigoSubConcepto"  />  
                        <asp:BoundField  HeaderText="Codigo Concepto Prestamo Plan" DataField="CodigoConceptoPrestamoPlan"  /> 
                        <asp:BoundField  HeaderText="Importe Tope Cargo" DataField="ImporteTopeEnvioCargo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"/> <%--ImporteTopeEnvioCargo--%>
                        <asp:BoundField  HeaderText="Se Envia Como Prestamo" DataField="SeEnviaComoPrestamo"  /> 
                        <asp:BoundField  HeaderText="Se Envia En TXT" DataField="SeEnviaEnTXT"  /> 
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        </Columns>
                </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>